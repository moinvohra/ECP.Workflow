using ECP.Messaging.Abstraction;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECP.Messages
{
    public class WorkflowActivated : IntegrationEvent
    {
        public int SourceWorkflowId { get; set; }
        public string TenantId { get; set; }
        public string ApplicationId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public dynamic Definitionjson { get; set; }
        public dynamic Previewjson { get; set; }
        public int Status { get; set; } = 1;
        public string CreatedBy { get; set; }

        public WorkflowActivated(int SourceWorkflowId,
            string TenantId,
            string ApplicationId,
            string Name,
            string Code,
            dynamic Definitionjson,
            dynamic Previewjson,
            string CreatedBy
            )
        {
            this.SourceWorkflowId = SourceWorkflowId;
            this.TenantId = TenantId;
            this.ApplicationId = ApplicationId;
            this.Name = Name;
            this.Code = Code;
            this.Definitionjson = Definitionjson;
            this.Previewjson = Previewjson;
            this.CreatedBy = CreatedBy;
        }
    }
}
