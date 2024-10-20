using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*
 * OWNER: JOEVER MONCEDA
 */
namespace InPoint
{
    public static class EventHubServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureEventHub(this IServiceCollection services, IConfiguration eventHubConfig)
        {
            var eventHubOptions = new EventHubOptions();
            eventHubConfig.Bind(eventHubOptions);

            services.AddSingleton(new EventHubProducerClient(eventHubOptions.ConnectionString, eventHubOptions.EventHubName));
            services.AddSingleton<InPointMessagePublisher, EventHubPublisher>();

            return services;
        }
    }

    public class EventHubOptions
    {
        public string ConnectionString { get; set; }
        public string EventHubName { get; set; }
    }

    public class EventHubPublisher : IEventHubPublisher
    {
        private readonly EventHubProducerClient _producerClient;

        public EventHubPublisher(EventHubProducerClient producerClient)
        {
            _producerClient = producerClient;
        }

        public async Task Publish(string message, 
            string? subject = null,
            string? eventType = null,
            string? dataVersion = null)
        {
            using EventDataBatch eventBatch = await _producerClient.CreateBatchAsync();
            eventBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes(message)));

            await _producerClient.SendAsync(eventBatch);
        }
    }
}
