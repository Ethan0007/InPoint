using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
/*
 * OWNER: JOEVER MONCEDA
 */
namespace InPoint.Tests
{
    public class ServiceBusPublisherTests
    {
        [Fact]
        public async Task Publish_ShouldSendMessageToServiceBusQueue()
        {
            // Arrange
            var mockClient = new Mock<ServiceBusClient>();
            var mockSender = new Mock<ServiceBusSender>();
            mockClient.Setup(client => client.CreateSender(It.IsAny<string>())).Returns(mockSender.Object);

            var publisher = new ServiceBusPublisher(mockClient.Object, Options.Create(new ServiceBusOptions { QueueName = "testQueue" }));

            // Act
            await publisher.Publish("Test Message");

            // Assert
            mockSender.Verify(sender => sender.SendMessageAsync(It.IsAny<ServiceBusMessage>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
