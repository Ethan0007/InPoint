using Azure.Messaging.EventGrid;
using Azure;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
/*
 * OWNER: JOEVER MONCEDA
 */
namespace InPoint
{
    public static class EventGridServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureEventGrid(this IServiceCollection services, IConfiguration eventGridConfig)
        {
            var eventGridOptions = new EventGridOptions();
            eventGridConfig.Bind(eventGridOptions);

            services.AddSingleton(new EventGridPublisherClient(new Uri(eventGridOptions.Endpoint), new AzureKeyCredential(eventGridOptions.AccessKey)));
            services.AddSingleton<InPointMessagePublisher, EventGridPublisher>();

            return services;
        }
    }

    public class EventGridOptions
    {
        public string Endpoint { get; set; }
        public string AccessKey { get; set; }
    }

    public class EventGridPublisher : IEventGridPublisher
    {
        private readonly EventGridPublisherClient _client;

        public EventGridPublisher(EventGridPublisherClient client)
        {
            _client = client;
        }

        public async Task Publish(string message,
            string? subject = null, 
            string? eventType = null, 
            string? dataVersion = null)
        {
            EventGridEvent eventGridEvent = new EventGridEvent(
                subject: subject,
                eventType: eventType,
                dataVersion: dataVersion,
                data: message
            );

            await _client.SendEventAsync(eventGridEvent);
        }
    }

}
