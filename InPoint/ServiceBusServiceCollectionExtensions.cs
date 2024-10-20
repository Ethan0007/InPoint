using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InPoint
{
    public static class ServiceBusServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureServiceBus(this IServiceCollection services, IConfiguration serviceBusConfig)
        {
            var serviceBusOptions = new ServiceBusOptions();
            serviceBusConfig.Bind(serviceBusOptions);

            services.AddSingleton(new ServiceBusClient(serviceBusOptions.ConnectionString));
            services.AddSingleton<InPointMessagePublisher, ServiceBusPublisher>();

            return services;
        }
    }

    public class ServiceBusOptions
    {
        public string ConnectionString { get; set; }
        public string QueueName { get; set; }
    }

    public class ServiceBusPublisher : IServiceBusPublisher
    {
        private readonly ServiceBusClient _client;
        private readonly ServiceBusOptions _options;

        public ServiceBusPublisher(ServiceBusClient client, IOptions<ServiceBusOptions> options)
        {
            _client = client;
            _options = options.Value;
        }

        public async Task Publish(string message, 
            string? subject = null,
            string? eventType = null,
            string? dataVersion = null)
        {
            ServiceBusSender sender = _client.CreateSender(_options.QueueName);
            ServiceBusMessage busMessage = new ServiceBusMessage(message);

            await sender.SendMessageAsync(busMessage);
        }
    }
}
