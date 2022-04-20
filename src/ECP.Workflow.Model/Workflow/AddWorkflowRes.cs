namespace ECP.Workflow.Model
{
    public class AddWorkflowRes
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string DefinitionJson  {get;set;}
        public string PreviewJson { get; set; }
        public int Status { get; set; }
        public string CreatedDate { get; set; }
    }
}
