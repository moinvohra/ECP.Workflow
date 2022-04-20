using System.Threading.Tasks;

namespace ECP.Messaging.RabbitMQ.Abstractions
{
    public interface IDynamicIntegrationEventHandler
    {
        Task HandleAsync(dynamic eventData);

    }
}
