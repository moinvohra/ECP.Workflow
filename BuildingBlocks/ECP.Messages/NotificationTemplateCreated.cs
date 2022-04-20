using ECP.Messaging.Abstraction;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECP.Messages
{
    public class NotificationTemplateCreated : IntegrationEvent
    {
        public int NotificationTemplateId { get; set; }
        public int NotificationTypeId { get; set; }
        public string TenantId { get; set; }
        public string NotificationSubject { get; set; }
        public string ApplicationId { get; set; }
        public string TemplateName { get; set; }
        public string CodeName { get; set; }

        public string TemplateBody { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }

        public NotificationTemplateCreated(
           int NotificationTemplateId,
           int NotificationTypeId,
           string TenantId,
           string ApplicationId,
           string NotificationSubject,
           string TemplateName,
           string CodeName,
           string TemplateBody,
           string Description,
           string CreatedBy)
        {
            this.NotificationTemplateId = NotificationTemplateId;
            this.NotificationTypeId = NotificationTypeId;
            this.TenantId = TenantId;
            this.ApplicationId = ApplicationId;
            this.NotificationSubject = NotificationSubject;
            this.TemplateName = TemplateName;
            this.CodeName = CodeName;
            this.TemplateBody = TemplateBody;
            this.Description = Description;
            this.CreatedBy = CreatedBy;
        }
    }
}
