using Microsoft.Extensions.Options;
using Moq;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace InPoint.Tests
{
    public class RabbitMqPublisherTests
    {
        [Fact]
        public void Publish_ShouldSendMessageToQueue()
        {
            // Arrange
            var mockChannel = new Mock<IModel>();
            var options = Options.Create(new RabbitMqOptions { QueueName = "testQueue" });
            var publisher = new RabbitMQPublisher(mockChannel.Object, options);

            // Act
            publisher.Publish("Test Message");

            // Assert
            mockChannel.Verify(ch => ch.BasicPublish(
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IBasicProperties>(), It.IsAny<byte[]>()
            ), Times.Once);
        }
    }
}
