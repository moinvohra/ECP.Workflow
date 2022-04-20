namespace ECP.Messages
{
    using ECP.Messaging.Abstraction;
    public class SendSMSCommand : IntegrationEvent
    {
        /// <summary>
        /// Application Identifier
        /// </summary>
        public string ApplicationId { get; set; }

        /// <summary>
        /// Tenant Identifier
        /// </summary>
        public string TenantId { get; set; }

        /// <summary>
        /// Code of the template to be setnt
        /// </summary>
        public string TemplateCode { get; set; }

        /// <summary>
        /// Message Validity time (in minutes)
        /// </summary>
        public ushort ValidityPeriod { get; set; }

        /// <summary>
        /// Receiver contact number
        /// </summary>
        public string To { get; set; }

        /// <summary>
        /// Message Content
        /// </summary>
        public string TemplateContent { get; set; }

        public SendSMSCommand(string ApplicationId,
            string TenantId,
            string TemplateCode,
            string To,
            string TemplateContent,
            ushort ValidityPeriod
            )
        {
            this.ApplicationId = ApplicationId;
            this.TenantId = TenantId;
            this.TemplateCode = TemplateCode;
            this.To = To;
            this.TemplateContent = TemplateContent;
            this.ValidityPeriod = ValidityPeriod;
        }
    }
}