namespace ECP.Workflow.Service.Model
{
    public class EditWorkflowReq
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string DefinitionJson { get; set; }
        public string PreviewJson { get; set; }
    }
}
