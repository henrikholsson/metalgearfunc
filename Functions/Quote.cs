using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask;
using Microsoft.DurableTask.Client;
using System.Dynamic;
using System.Net;

namespace metalgear.Functions;
public class Quote
{

    [Function(nameof(RequestQuote))]
    public async Task<HttpResponseData> RequestQuote(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req,
        [DurableClient] DurableTaskClient client,
        FunctionContext context)
    {
        string instanceId = await client.ScheduleNewOrchestrationInstanceAsync(nameof(RunQuotePipeline));
        return await client.CreateCheckStatusResponseAsync(req, instanceId);
        // return response

    }

    [Function(nameof(RunQuotePipeline))]
    public async Task<string> RunQuotePipeline(
        [OrchestrationTrigger] TaskOrchestrationContext context)
    {
        var input = context.GetInput<string>();
        var productAvailability = context.CallActivityAsync<string>(nameof(GetProductAvailability), 5);
        var total = context.CallActivityAsync<string>(nameof(GetTotal), 5);


        context.SetCustomStatus("Initiated quote process");
        var result = await Task.WhenAll(productAvailability, total);

        context.SetCustomStatus($"Retrieved quote data {productAvailability}, {total}");
        var requestQuoteResult = context.CallActivityAsync<string>(nameof(RequestQuoteAppproval), "request approval");

        
        using var timeoutCts = new CancellationTokenSource();
        DateTime dueTime = context.CurrentUtcDateTime.AddMinutes(10);
        var timeoutTask = context.CreateTimer(dueTime, timeoutCts.Token);

        var approvalTask = context.WaitForExternalEvent<string>("ApprovalReceived");
        context.SetCustomStatus("Waiting for approval");

        var completedTask = await Task.WhenAny(approvalTask, timeoutTask);
        if (completedTask == approvalTask)
        {
            context.SetCustomStatus("Approval received");
            timeoutCts.Cancel();
            return $"Approved: {approvalTask.Result}";
        }
        else
        {
            context.SetCustomStatus("Approval timed out");
            return "Approval timed out.";
        }
    }

    [Function(nameof(RequestQuoteAppproval))]
    public async Task<string> RequestQuoteAppproval([ActivityTrigger] string input)
    {
        await Task.Delay(1000);
        //sending email to sales representative
        return $"sent email to rep {input}";
    }

    [Function(nameof(GetProductAvailability))]
    public async Task<string> GetProductAvailability([ActivityTrigger] int seconds)
    {
        await Task.Delay(seconds * 1000);
        return $"GetProductAvailability completed after {seconds} seconds.";
    }

    [Function(nameof(GetTotal))]
    public async Task<string> GetTotal([ActivityTrigger] int input)
    {
        await Task.Delay(1000);
        return $"GetTotal completed after 1 second.";
    }


    [Function(nameof(ApproveQuote))]
    public async Task<HttpResponseData> ApproveQuote(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "approve/{instanceId}")] HttpRequestData req,
        [DurableClient] DurableTaskClient client,
        string instanceId)
    {
        string body = await new StreamReader(req.Body).ReadToEndAsync();
        await client.RaiseEventAsync(instanceId, "ApprovalReceived", body);

        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteStringAsync("Approval received.");
        return response;
    }
}
