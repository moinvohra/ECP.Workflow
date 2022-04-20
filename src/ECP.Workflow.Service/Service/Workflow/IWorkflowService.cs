using ECP.Workflow.Model;
using System.Threading.Tasks;

namespace ECP.Workflow.Service
{
    public interface IWorkflowService
    {
        Task<WorkflowDetail> GetById(string tenantId, string applicationId, int Id);

        Task<WorkflowDetail> Create(AddWorkflowReq req, string tenantId, string applicationId);

        Task<WorkflowDetail> Update(EditWorkflowReq req, string tenantId, string applicationId);

        Task<WorkflowDetail> Activate(EditWorkflowReq req, string tenantId, string applicationId);

        Task<WorkflowDetail> Clone(CloneWorkflowReq req, string tenantId, string applicationId,int SourceWorkflowId);

        Task<WorkflowDetail> DeActivate(string tenantId, string applicationId, int Id);

        Task<int> Delete(string tenantId, string applicationId, int Id);
    }
}