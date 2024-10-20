using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InPoint
{
    public interface InPointMessagePublisher
    {
        Task Publish(string message, 
            string? subject = null, 
            string? eventType = null, 
            string? dataVersion = null);
    }


    public interface IRabbitMQPublisher : InPointMessagePublisher { }
    public interface IEventHubPublisher : InPointMessagePublisher { }
    public interface IServiceBusPublisher : InPointMessagePublisher { }
    public interface IEventGridPublisher : InPointMessagePublisher { }
}
