using ECP.Messaging.Abstraction;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECP.Messages
{
    public class SendPushNotificationCommand : IntegrationEvent
    {
        public string TenantId { get; set; }
        public string ApplicationId { get; set; }
        public string MessageTitle { get; set; }
        public string MessageText { get; set; }
        public string TemplateCode { get; set; }
        public string[] DeviceIds { get; set; }

        public SendPushNotificationCommand(string TenantId,
            string ApplicationId,
            string MessageTitle,
            string MessageText,
            string TemplateCode,
            string[] DeviceIds)
        {
            this.TenantId = TenantId;
            this.ApplicationId = ApplicationId;
            this.MessageTitle = MessageTitle;
            this.MessageText = MessageText;
            this.TemplateCode = TemplateCode;
            this.DeviceIds = DeviceIds;
        }
    }
}
