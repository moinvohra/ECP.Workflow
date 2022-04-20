using System;
using System.Collections.Generic;
using System.Text;

namespace ECP.Workflow.Model.Workflow
{
    public class ActiveWorkflowData
    {
        public int SourceWorkflowId { get; set; }
        public string TenantId { get; set; }
        public string ApplicationId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public DateTime StartDate { get; set; }
        public dynamic DefinitionJson { get; set; }
        public dynamic PreviewJson { get; set; }
        public int Status { get; set; } 
    }
}
