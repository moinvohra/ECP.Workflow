using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECP.Messaging.RabbitMQ.Connection
{
    public interface IMqConnection :IDisposable
    {
        bool IsConnected { get; }
        IModel CreateModel();
    }
}
