using ECP.Messaging.Abstraction;
using System;

namespace ECP.Messages
{
    public class ExportTemplateUpdated : IntegrationEvent
    {
        public string TenantId { get; set; }
        public string ApplicationId { get; set; }
        public int TemplateId { get; set; }
        public string TemplateCode { get; set; }
        public string TemplateName { get; set; }
        public int? HeaderTemplateId { get; set; }
        public int? FooterTemplateId { get; set; }
        public string TemplateHtmlContents { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public ExportTemplateUpdated(string TenantId, string ApplicationId, int TemplateId,string TemplateCode,
            string TemplateName,int? HeaderTemplateId, int? FooterTemplateId, string TemplateHtmlContents,
            string UpdatedBy, DateTime? UpdatedDate)
        { 
            this.TenantId=TenantId;
            this.ApplicationId = ApplicationId;
            this.TemplateId = TemplateId;
            this.TemplateCode = TemplateCode;
            this.TemplateName = TemplateName;
            this.HeaderTemplateId = HeaderTemplateId;
            this.FooterTemplateId = FooterTemplateId;
            this.TemplateHtmlContents = TemplateHtmlContents;
            this.UpdatedBy = UpdatedBy;
            this.UpdatedDate = UpdatedDate;
        }

    }
}
