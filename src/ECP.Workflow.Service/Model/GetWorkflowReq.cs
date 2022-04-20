namespace ECP.Workflow.Service.Model
{
    public class GetWorkflowReq
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public int Status { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
    }
}
