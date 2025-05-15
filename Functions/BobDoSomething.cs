using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace metalgear.Functions
{
    public class BobDoSomething
    {
        private readonly ILogger _logger;

        public BobDoSomething(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<BobDoSomething>();
        }

        [Function(nameof(TriggerBob))]
        public void TriggerBob([TimerTrigger("0 */5 * * * *")] TimerInfo timerInfo)
        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            if (timerInfo.ScheduleStatus is not null)
            {
                _logger.LogInformation($"BOB DO SOMETHING!" +
                    $"Ultimate available next time at: {timerInfo.ScheduleStatus.Next}");
            }
        }
    }
}
