using ECP.Messaging.Abstraction;

namespace ECP.Messages
{
    public class TenantUpdated : IntegrationEvent
    {
        public string TenantId { get; set; }
        public string TenantName { get; set; }

        public TenantUpdated(string tenantId, string tenantName)
        {
            this.TenantId = tenantId;
            this.TenantName = tenantName;
        }
    }
}
