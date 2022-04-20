using ECP.Messaging.Abstraction;
using System;

namespace ECP.Messages
{
    public class ExportTemplateCreated : IntegrationEvent
    {
        public string TenantId { get; set; }
        public string ApplicationId { get; set; }
        public int TemplateId { get; set; }
        public string TemplateCode { get; set; }
        public string TemplateName { get; set; }
        public string Tags { get; set; }
        public int? HeaderTemplateId { get; set; }
        public int? FooterTemplateId { get; set; }
        public string TemplateHtmlContents { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        public ExportTemplateCreated(string TenantId,string ApplicationId,int TemplateId,string TemplateCode,string TemplateName,string Tags,
                                     int? HeaderTemplateId,int? FooterTemplateId,string TemplateHtmlContents,string CreatedBy,DateTime CreatedDate
                                    )
        {
            this.TenantId = TenantId;
            this.ApplicationId = ApplicationId;
            this.TemplateId = TemplateId;
            this.TemplateCode = TemplateCode;
            this.TemplateName = TemplateName;
            this.Tags = Tags;
            this.HeaderTemplateId = HeaderTemplateId;
            this.FooterTemplateId = FooterTemplateId;
            this.TemplateHtmlContents = TemplateHtmlContents;
            this.CreatedBy = CreatedBy;
            this.CreatedDate = CreatedDate;
        }
    }
}
