using ECP.Messaging.Abstraction;

namespace ECP.Messages
{
    public class TenantCreated: IntegrationEvent
    {
        public string TenantId { get; set; }
        public string TenantName { get; set; }

        public TenantCreated(string tenantId, string tenantName)
        {
            this.TenantId = tenantId;
            this.TenantName = tenantName;
        }
    }
}
