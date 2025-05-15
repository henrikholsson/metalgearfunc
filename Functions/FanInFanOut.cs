using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace metalgear.Functions;
public class FanInFanOut
{
    private readonly ILogger<FanInFanOut> _logger;
    private static List<int> Numbers = new List<int>();
    public FanInFanOut(ILogger<FanInFanOut> logger)
    {
        _logger = logger;
    }
    [Function(nameof(RunFanInFanOut))]
    public  async Task<HttpResponseData> RunFanInFanOut(
       [HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req,
       [DurableClient] DurableTaskClient client)
    {
        string instanceId = await client.ScheduleNewOrchestrationInstanceAsync(nameof(RunOrchestrator), null);
        return await client.CreateCheckStatusResponseAsync(req, instanceId);
    }

    [Function(nameof(RunOrchestrator))]
    public  async Task<string> RunOrchestrator(
        [OrchestrationTrigger] TaskOrchestrationContext context)
    {
        const int processCount = 5; //antall paralelle aktiviteter
        var fanOutTasks = new List<Task<IEnumerable<int>>>();

        for (int i = 0; i < processCount; i++)
        {
            fanOutTasks.Add(context.CallActivityAsync<IEnumerable<int>>(nameof(GetNumbers), $"GetNumbers_{i}"));
        }
        var allNumberLists = await Task.WhenAll(fanOutTasks);
        var allNumbers = allNumberLists.SelectMany(x => x).ToList();

        var sum = await context.CallActivityAsync<int>(nameof(GetSum), allNumbers);
        return $"Orchestration completed: {sum}";
    }

    [Function(nameof(GetNumbers))]
    public async Task<IEnumerable<int>> GetNumbers([ActivityTrigger] string name)
    {
        var random = new Random();
        int delayMs = random.Next(500, 3000);
        await Task.Delay(delayMs);


        var numbers = Enumerable.Range(1, 24).Select(_ => random.Next(1, 100));
        _logger.LogInformation($"Generated numbers from {name}: {string.Join(", ", numbers)}");
        return numbers;
    }

    [Function(nameof(GetSum))]
    public async Task<int> GetSum([ActivityTrigger] IEnumerable<int> numbers)
    {
        var random = new Random();
        int delayMs = random.Next(500, 3000);
        await Task.Delay(delayMs);
        var sum = numbers.Sum();
        _logger.LogInformation($"Sum of numbers: {sum}");
        return sum;
    }


}
