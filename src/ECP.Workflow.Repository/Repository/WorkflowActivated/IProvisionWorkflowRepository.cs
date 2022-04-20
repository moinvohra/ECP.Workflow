using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ECP.Workflow.Repository.ProvisionWorkflowRepository
{
    public interface IProvisionWorkflowRepository
    {
        Task<int> ActivateWorkflow(
          int sourceWorkflowId,
            string tenantId,
            string applicationId,
          string workflowName,
          string workflowCode,
          dynamic definitionJson,
          dynamic previewJson,
          int status,
          string createdBy);
    }
}
