using ECP.Messaging.Abstraction;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECP.Messages
{
    public class NotificationTemplateUpdated : IntegrationEvent
    {
        public int NotificationTypeId { get; set; }
        public string TenantId { get; set; }
        public string ApplicationId { get; set; }
        public string TemplateName { get; set; }
        public string CodeName { get; set; }
        public string TemplateBody { get; set; }
        public string Description { get; set; }
        public string NotificationSubject { get; set; }
        public string UpdatedBy { get; set; }


        public NotificationTemplateUpdated(int NotificationTypeId,
            string TenantId,
            string ApplicationId,
            string TemplateName,
            string CodeName,
                string TemplateBody,
            string Description,
            string NotificationSubject,
            string UpdatedBy)
        {
            this.NotificationTypeId = NotificationTypeId;
            this.TenantId = TenantId;
            this.ApplicationId = ApplicationId;
            this.TemplateName = TemplateName;
            this.CodeName = CodeName;
            this.TemplateBody = TemplateBody;
            this.Description = Description;
            this.NotificationSubject = NotificationSubject;
            this.UpdatedBy = UpdatedBy;
        }

    }
}
