using ECP.Messages;
using ECP.Messaging.RabbitMQ.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECP.Workflow.Service.MessageBroker
{
    public class MessageBroker : IMessageBroker
    {
        private readonly IPublishEventBus _eventBus;
        private readonly ILogger<MessageBroker> _logger;
        private readonly IConfiguration _configuration;
        public MessageBroker(IPublishEventBus eventBus,
            ILogger<MessageBroker> logger,
            IConfiguration configuration
            )
        {
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration;
        }

        public void SendLogMessage(AuditLogMessage @event)
        {
            _logger.LogInformation("----- Publishing integration event");

            _eventBus.Publish(@event,
             exchangeName: _configuration["RabbitMQ:AuditLogQueue:ExchangeName"],
             exchangeType: ExchangeType.Direct,
             queueName: _configuration["RabbitMQ:AuditLogQueue:QueueName"],
             routingKey: _configuration["RabbitMQ:AuditLogQueue:RouteName"],
             deliveryMode: 2,
             queueProperties: null,
             exchangeProperties: null);
        }

        public void SendWorkflowActivatedMessage(WorkflowActivated @event)
        {
            _logger.LogInformation("----- Publishing integration event");

            _eventBus.Publish(@event,
             exchangeName: _configuration["RabbitMQ:WorkFlowActivatedQueue:ExchangeName"],
             exchangeType: ExchangeType.Direct,
             queueName: _configuration["RabbitMQ:WorkFlowActivatedQueue:QueueName"],
             routingKey: _configuration["RabbitMQ:WorkFlowActivatedQueue:RouteName"],
             deliveryMode: 2,
             queueProperties: null,
             exchangeProperties: null);
        }
    }
}