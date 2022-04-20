using ECP.Messaging.Abstraction;
using System.Collections.Generic;

namespace ECP.Messages
{
    public class TenantApplicationDeAssigned : IntegrationEvent
    {
        public string TenantId { get; set; }
        public string[] ApplicationIds { get; set; }
        public string TenantTitle { get; set; }

        public TenantApplicationDeAssigned(string TenantId, string TenantTitle, string[] ApplicationIds)
        {
            this.TenantId = TenantId;
            this.ApplicationIds = ApplicationIds;
            this.TenantTitle = TenantTitle;
        }
    }
}
