using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ECP.Workflow.Service.ProvisionWorkflowService
{
    public interface IProvisionWorkflowService
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
