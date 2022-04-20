using ECP.Workflow.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECP.Workflow.Repository.WorkflowRepository
{
    public interface IWorkflowRepository
    {


        Task<WorkflowDetail> GetById(string tenantId, string applicationId, int id);

        Task<int> Create(AddWorkflowReq req, string tenantId, string applicationId, string createdBy);

        Task<int> Update(EditWorkflowReq req, string tenantId, string applicationId, string updatedBy);

        Task<int> Activate(EditWorkflowReq req, string tenantId, string applicationId, string updatedBy);

        Task<int> Clone(CloneWorkflowReq req, string tenantId, string applicationId, int sourceWorkflowId);

        Task<int> DeActivate(string tenantId, string applicationId, int id, string updatedBy);

        Task<int> Delete(string tenantId, string applicationId, int id);

        Task<bool> CheckCodeExist(string tenantId, string applicationId, string codeName);

        Task<bool> CheckCodeExist(string tenantId, string applicationId, string codeName, int id);

        Task<bool> IsActivated(string tenantId, string applicationId, int id);
    }
}
