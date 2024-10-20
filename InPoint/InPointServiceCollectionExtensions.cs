using InPoint.Factory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
/*
 * OWNER: JOEVER MONCEDA
 */
namespace InPoint
{
    public static class InPointServiceCollectionExtensions
    {
        public static IServiceCollection AddInPointMessaging(this IServiceCollection services, IConfiguration config)
        {
            var providers = config.GetSection("MessagingProvider").Get<string[]>();

            if (!providers.Any()) throw new ArgumentException("No messaging provider!");

            foreach (var provider in providers)
            {
                switch (provider)
                {
                    case "RabbitMQ":
                        services.ConfigureRabbitMQ(config.GetSection("RabbitMQ"));
                        services.AddScoped<IRabbitMQPublisher, RabbitMQPublisher>();
                        break;
                    case "EventHub":
                        services.ConfigureEventHub(config.GetSection("EventHub"));
                        services.AddScoped<IEventHubPublisher, EventHubPublisher>();
                        break;
                    case "ServiceBus":
                        services.ConfigureServiceBus(config.GetSection("ServiceBus"));
                        services.AddScoped<IServiceBusPublisher, ServiceBusPublisher>();
                        break;
                    case "EventGrid":
                        services.ConfigureEventGrid(config.GetSection("EventGrid"));
                        services.AddScoped<IEventGridPublisher, EventGridPublisher>();
                        break;
                    default:
                        throw new ArgumentException($"Unsupported messaging provider: {provider}");
                }
            }

            services.AddScoped<InPointPublisherFactory>();

            return services;
        }
    }
}
