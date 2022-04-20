using ECP.Messaging.RabbitMQ.Abstractions;
using System;
using System.Collections.Generic;
using static ECP.Messaging.RabbitMQ.EventBusSubscriptionsManager;
using ECP.Messaging.Abstraction;

namespace ECP.Messaging.RabbitMQ
{
    public interface IEventBusSubscriptionsManager
    {
        void AddSubscription<T,TH>(string routeKey) 
           where T : IntegrationEvent
           where TH : IEventHandler;

        IEnumerable<SubscriptionInfo> GetHandlersForEvent(string eventName);

        Type GetEventTypeByName(string eventName);
    }
}