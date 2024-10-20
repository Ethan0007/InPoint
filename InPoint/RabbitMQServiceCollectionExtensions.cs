using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InPoint
{
    public static class RabbitMQServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureRabbitMQ(this IServiceCollection services, IConfiguration rabbitMQConfig)
        {
            var rabbitMqOptions = new RabbitMqOptions();
            rabbitMQConfig.Bind(rabbitMqOptions);

            var connectionFactory = new ConnectionFactory
            {
                HostName = rabbitMqOptions.Host,
                UserName = rabbitMqOptions.Username,
                Password = rabbitMqOptions.Password
            };

            services.AddSingleton(connectionFactory);
            services.AddSingleton<IConnection>(sp => connectionFactory.CreateConnection());
            services.AddSingleton<IModel>(sp => sp.GetRequiredService<IConnection>().CreateModel());

            services.AddSingleton<InPointMessagePublisher, RabbitMQPublisher>();

            return services;
        }
    }

    public class RabbitMqOptions
    {
        public string Host { get; set; }
        public string QueueName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class RabbitMQPublisher : IRabbitMQPublisher
    {
        private readonly IModel _channel;
        private readonly RabbitMqOptions _options;

        public RabbitMQPublisher(IModel channel, IOptions<RabbitMqOptions> options)
        {
            _channel = channel;
            _options = options.Value;
        }

        public Task Publish(string message, 
            string? subject = null,
            string? eventType = null,
            string? dataVersion = null)
        {
            return Task.Run(() =>
            {
                _channel.QueueDeclare(queue: _options.QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
                var body = Encoding.UTF8.GetBytes(message);
                _channel.BasicPublish(exchange: "", routingKey: _options.QueueName, basicProperties: null, body: body);
            });
        }
    }
}
