using System;
using System.Collections.Generic;
using System.Text;

namespace ECP.Workflow.Service.Model
{
    public class WorkflowDetail
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Status { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string DefinitionJson { get; set; }
        public string PreviewJson { get; set; }
    }
}
