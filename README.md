# InPoint 
### _NuGet Library: Simplifying Messaging Providers in .NET_

[![PubSub](https://github.com/Ethan0007/InPoint/blob/main/publish-subscribe.png?raw=true)](https://github.com/Ethan0007/InPoint/blob/main/publish-subscribe.png)

#### Introduction
In today's microservices architecture, messaging systems play a crucial role in ensuring seamless communication between services. However, managing multiple messaging providers can be a daunting task. This article addresses a common challenge developers face when integrating various messaging systems in .NET applications and provides a clear solution to streamline this process using the InPoint NuGet library.

#### The Problem
When working with multiple messaging providers like RabbitMQ, Event Hub, Service Bus, and Event Grid, developers often run into configuration issues, especially when the required providers are not specified in the configuration file. This oversight can lead to runtime errors, making it challenging to maintain and extend your application.

To avoid such pitfalls, it’s essential to ensure that your application is configured correctly to recognize and utilize the specified messaging providers.

#### The Solution: InPoint NuGet Library
To effectively manage different messaging providers, we can create a modular setup in .NET that allows for easy configuration and integration of various messaging systems. The InPoint NuGet library provides a straightforward way to achieve this, allowing you to register multiple messaging providers easily.

#### Libraries to Install
To implement this solution, you will need to install the following libraries:
- dotnet add package RabbitMQ.Client
- dotnet add package Microsoft.Azure.EventHubs
- dotnet add package Microsoft.Azure.ServiceBus
- dotnet add package Microsoft.Azure.EventGrid

#### JSON Configuration
Here's an example of the appsettings.json file that outlines the configuration for multiple messaging providers:
```
{
  "MessagingProvider": [ "RabbitMQ", "EventHub", "ServiceBus", "EventGrid" ],
  "RabbitMQ": {
    "Host": "localhost",
    "UserName": "guest",
    "Password": "guest"
  },
  "EventHub": {
    "ConnectionString": "YourEventHubConnectionString",
    "EventHubName": "YourEventHubName"
  },
  "ServiceBus": {
    "ConnectionString": "YourServiceBusConnectionString",
    "QueueName": "YourQueueName"
  },
  "EventGrid": {
    "TopicEndpoint": "https://yourtopic.westeurope-1.eventgrid.azure.net/api/events",
    "SasKey": "YourSasKey"
  }
}
```


#### Using the InPoint Library in Your Application
##### Step 1: Register the InPoint Library
In your Program.cs file, you can register the InPoint messaging services with the following line:
```
builder.Services.AddInPointMessaging(builder.Configuration);
```
This will configure your application to utilize the specified messaging providers.
##### Step 2: Implement Messaging in Your Controller
Here’s how to implement a controller that uses the messaging service to publish messages:

```
[ApiController]
[Route("api/[controller]")]
public class MessageController : ControllerBase
{
    private readonly MessagePublisherFactory _publisherFactory;
    private readonly IConfiguration _configuration;

    public MessageController(MessagePublisherFactory publisherFactory, IConfiguration configuration)
    {
        _publisherFactory = publisherFactory;
        _configuration = configuration;
    }

    [HttpPost("publish")]
    public async Task<IActionResult> PublishMessage([FromBody] string message)
    {
        var providers = _configuration.GetSection("MessagingProvider").Get<string[]>();
        var selectedProvider = providers.First(); // Choose the provider logic here

        var publisher = _publisherFactory.GetPublisher(selectedProvider);
        await publisher.Publish(message);

        return Ok("Message published successfully!");
    }
}
```
##### Step 3: Implement a Notification Service
You can also create a service that utilizes the messaging system to send notifications:
```
public class NotificationService
{
    private readonly MessagePublisherFactory _publisherFactory;
    private readonly IConfiguration _configuration;

    public NotificationService(MessagePublisherFactory publisherFactory, IConfiguration configuration)
    {
        _publisherFactory = publisherFactory;
        _configuration = configuration;
    }

    public async Task SendNotification(string message)
    {
        var providers = _configuration.GetSection("MessagingProvider").Get<string[]>();
        var selectedProvider = providers.First(); // Choose the provider logic here

        var publisher = _publisherFactory.GetPublisher(selectedProvider);
        await publisher.Publish(message);
    }
}
```

#### Additional Resources
- [RabbitMQ Documentation](https://www.rabbitmq.com/) 
- [Azure Event Hubs Documentation](https://azure.microsoft.com/en-us/products/event-hubs)
- [Azure Service Bus Documentation](https://azure.microsoft.com/en-us/products/service-bus/?ef_id=_k_Cj0KCQjwsc24BhDPARIsAFXqAB3E5BjZWmnkqTID22quT_mEgNOILfymHDd0CKXqV9seMModR1JyOvQaAkvSEALw_wcB_k_&OCID=AIDcmm76som1hh_SEM__k_Cj0KCQjwsc24BhDPARIsAFXqAB3E5BjZWmnkqTID22quT_mEgNOILfymHDd0CKXqV9seMModR1JyOvQaAkvSEALw_wcB_k_&gad_source=1&gclid=Cj0KCQjwsc24BhDPARIsAFXqAB3E5BjZWmnkqTID22quT_mEgNOILfymHDd0CKXqV9seMModR1JyOvQaAkvSEALw_wcB)
- [Azure Event Grid Documentation](https://learn.microsoft.com/en-us/azure/event-grid/overview)

#### License 
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)  
  Copyright (c) 2024 [Joever Monceda](https://github.com/Ethan0007)

Linkedin: [Joever Monceda](https://www.linkedin.com/in/joever-monceda-55242779/)  
  Medium: [Joever Monceda](https://medium.com/@joever.monceda/new-net-core-vuejs-vuex-router-webpack-starter-kit-e94b6fdb7481)  
  Twitter [@_EthanHunt07](https://twitter.com/_EthanHunt07)  
  Facebook: [Ethan Hunt](https://www.facebook.com/nethan.hound.3/)