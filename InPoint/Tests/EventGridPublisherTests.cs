using Azure.Messaging.EventGrid;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace InPoint.Tests
{
    public class EventGridPublisherTests
    {
        [Fact]
        public async Task Publish_ShouldSendEventToEventGrid()
        {
            // Arrange
            var mockClient = new Mock<EventGridPublisherClient>();
            var publisher = new EventGridPublisher(mockClient.Object);

            // Act
            await publisher.Publish("Test Message");

            // Assert
            mockClient.Verify(client => client.SendEventAsync((EventGridEvent)
                It.IsAny<IEnumerable<EventGridEvent>>(), 
                It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
