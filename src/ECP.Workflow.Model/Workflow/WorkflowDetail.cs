using System;
using System.Collections.Generic;
using System.Text;

namespace ECP.Workflow.Model
{
    public class WorkflowDetail
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int Status { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public dynamic DefinitionJson { get; set; }
        public dynamic PreviewJson { get; set; }

        public int SourceWorkflowId { get; set; }
    }
}
