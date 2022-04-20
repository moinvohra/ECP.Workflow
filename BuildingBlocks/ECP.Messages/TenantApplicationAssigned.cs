using ECP.Messaging.Abstraction;

namespace ECP.Messages
{
    public class TenantApplicationAssigned : IntegrationEvent
    {
        public string TenantId { get; set; }
        public string[] ApplicationIds { get; set; }
        public TenantApplicationAssigned(string TenantId,string[] ApplicationIds)
        {
            this.TenantId = TenantId;
            this.ApplicationIds = ApplicationIds;
        }
    }
}

