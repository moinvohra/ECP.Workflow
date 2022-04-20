using ECP.Messaging.Abstraction;
using System;

namespace ECP.Messages
{
    public class AuditLogMessage:IntegrationEvent
    {
        public string ClientId { get; set; }
        public string ClientName { get; set; }
        public string TenantId { get; set; }
        public string ApplicationId { get; set; }
        public string ApplicationName { get; set; }
        public string Module { get; set; }
        public string EntityName { get; set; }
        public string Feature { get; set; }
        public string RecordType { get; set; }
        public string RecordId { get; set; }
        public string IpAddress { get; set; }
        public string LogonUser { get; set; }
        public string LogonUserName { get; set; }
        public DateTime LoggedDate { get; set; }
        public string EntityObject { get; set; }
        public string RecordTitle { get; set; }
		public string LogonTenantId { get; set; }

        public AuditLogMessage(string ClientId, string ClientName, string TenantId,
                               string ApplicationId, string ApplicationName, string Module,
                               string EntityName, string Feature, string RecordType,
                               string RecordId, string IpAddress, string LogonUser,
                               string LogonUserName, DateTime LoggedDate, string EntityObject, string LogonTenantId, string RecordTitle
                              )
        {
            this.ClientId = ClientId;
            this.ClientName = ClientName;
            this.TenantId = TenantId;
            this.ApplicationId = ApplicationId;
            this.ApplicationName = ApplicationName;
            this.Module = Module;
            this.EntityName = EntityName;
            this.Feature = Feature;
            this.RecordType = RecordType;
            this.RecordId = RecordId;
            this.IpAddress = IpAddress;
            this.LogonUser = LogonUser;
            this.LogonUserName = LogonUserName;
            this.LoggedDate = LoggedDate;
            this.EntityObject = EntityObject;
            this.LogonTenantId = LogonTenantId;
            this.RecordTitle = RecordTitle;
        }
    }
}
