namespace ECP.Workflow.Model
{
    public class AddWorkflowReq
    {
        public int WorkflowId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public dynamic Definitionjson { get; set; }
        public dynamic Previewjson { get; set; }
        public int Status { get; set; } = -1;
    }
}