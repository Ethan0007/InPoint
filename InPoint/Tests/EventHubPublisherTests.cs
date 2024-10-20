using Azure.Messaging.EventHubs.Producer;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace InPoint.Tests
{
    public class EventHubPublisherTests
    {
        [Fact]
        public async Task Publish_ShouldSendEventToEventHub()
        {
            // Arrange
            var mockClient = new Mock<EventHubProducerClient>();
            var publisher = new EventHubPublisher(mockClient.Object);

            // Act
            await publisher.Publish("Test Message");

            // Assert
            mockClient.Verify(client => client.SendAsync(It.IsAny<EventDataBatch>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
