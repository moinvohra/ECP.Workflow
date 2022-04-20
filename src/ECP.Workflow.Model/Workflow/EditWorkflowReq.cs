using System.ComponentModel.DataAnnotations.Schema;

namespace ECP.Workflow.Model
{
    public class EditWorkflowReq
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Code { get; set; }

        public dynamic DefinitionJson { get; set; }

        public dynamic PreviewJson { get; set; }
        
    }
}
