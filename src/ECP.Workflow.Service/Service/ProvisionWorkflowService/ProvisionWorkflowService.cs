using ECP.Workflow.Repository.ProvisionWorkflowRepository;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ECP.Workflow.Service.ProvisionWorkflowService
{
    public class ProvisionWorkflowService : IProvisionWorkflowService
    {
        private readonly IProvisionWorkflowRepository _repository;
        private readonly ILogger _logger;
        public ProvisionWorkflowService(IProvisionWorkflowRepository repository,
             ILoggerFactory loggerFactory)
        {
            _repository = repository;
            _logger = loggerFactory.CreateLogger<ProvisionWorkflowService>();
        }

        public async Task<int> ActivateWorkflow(int sourceWorkflowId,
            string tenantId,
            string applicationId,
            string workflowName,
            string workflowCode,
            dynamic definitionJson,
            dynamic previewJson,
            int status,
            string createdBy)
        {
            _logger.LogInformation($"Activate workflow for child tenants: SourceWorkflowId: {sourceWorkflowId} " +
                $"TenantId: {tenantId}" +
                $"ApplicationId:{applicationId} " +
                $"WorkflowName:{workflowName} " +
                $"WorkflowCode:{workflowCode} " +
                $"DefinitionJson:{definitionJson} " +
                $"PreviewJson :{previewJson} Status:{status} CreatedBy :{createdBy}");

            var retval = await _repository.ActivateWorkflow(sourceWorkflowId,
             tenantId,
             applicationId,
             workflowName,
             workflowCode,
             definitionJson,
             previewJson,
             status,
             createdBy);

            return retval;
        }
    }
}
