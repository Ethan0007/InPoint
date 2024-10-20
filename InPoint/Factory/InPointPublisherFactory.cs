using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*
 * OWNER: JOEVER MONCEDA
 */
namespace InPoint.Factory
{
    public class InPointPublisherFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public InPointPublisherFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public InPointMessagePublisher GetPublisher(string provider)
        {
            return provider switch
            {
                "RabbitMQ" => _serviceProvider.GetRequiredService<IRabbitMQPublisher>(),
                "EventHub" => _serviceProvider.GetRequiredService<IEventHubPublisher>(),
                "ServiceBus" => _serviceProvider.GetRequiredService<IServiceBusPublisher>(),
                "EventGrid" => _serviceProvider.GetRequiredService<IEventGridPublisher>(),
                _ => throw new ArgumentException($"Unsupported messaging provider: {provider}"),
            };
        }
    }
}
