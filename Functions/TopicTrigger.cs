using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace metalgear.Functions
{
    public class TopicTrigger
    {
        private readonly ILogger<TopicTrigger> _logger;

        public TopicTrigger(ILogger<TopicTrigger> logger)
        {
            _logger = logger;
        }

        //[Function(nameof(TopicTrigger))]
        //public async Task Run(
        //    [ServiceBusTrigger("mytopic", "mysubscription", Connection = "")]
        //    ServiceBusReceivedMessage message,
        //    ServiceBusMessageActions messageActions)
        //{
        //    _logger.LogInformation("Message ID: {id}", message.MessageId);
        //    _logger.LogInformation("Message Body: {body}", message.Body);
        //    _logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);

        //    // Complete the message
        //    await messageActions.CompleteMessageAsync(message);
        //}
    }
}
