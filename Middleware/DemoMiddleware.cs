using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Logging;

namespace metalgear.Middleware;
public class DemoMiddleware : IFunctionsWorkerMiddleware
{
    public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
    {
        var logger = context.GetLogger<DemoMiddleware>();

        logger.LogInformation("👋 Middleware: Forespørsel mottatt!");

        // Du kan også lese detaljer fra requesten hvis ønskelig
        var request = await context.GetHttpRequestDataAsync();
        if (request != null)
        {
            logger.LogInformation($"📝 HTTP {request.Method} til {request.Url}");
        }

        // Kjør neste i pipeline
        await next(context);

        // Legg til en header i response
        var response = context.GetHttpResponseData();
        if (response != null)
        {
            response.Headers.Add("X-Middleware-Demo", "Virker!");
            logger.LogInformation("🎉 Header lagt til i respons.");
        }
    }
}
