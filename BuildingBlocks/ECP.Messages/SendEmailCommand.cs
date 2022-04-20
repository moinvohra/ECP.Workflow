namespace ECP.Messages
{
    using ECP.Messaging.Abstraction;
    public class SendEmailCommand : IntegrationEvent
    {
        public string ApplicationId { get; set; }
        public string TenantId { get; set; }
        public string Subject { get; set; }
        public string SenderAddress { get; set; }
        public string ReceiverAddress { get; set; }
        public string TemplateCode { get; set; }
        public string TemplateContent { get; set; }

        public SendEmailCommand(string ApplicationId,
            string TenantId,
            string Subject,
            string SenderAddress,
            string ReceiverAddress,
            string TemplateCode,
            string TemplateContent)
        {
            this.ApplicationId = ApplicationId;
            this.Subject = Subject;
            this.SenderAddress = SenderAddress;
            this.ReceiverAddress = ReceiverAddress;
            this.TemplateCode = TemplateCode;
            this.TemplateContent = TemplateContent;
            this.TenantId = TenantId;
        }
    }
}