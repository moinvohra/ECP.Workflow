using ECP.Messages;
using ECP.Messaging.RabbitMQ.Abstractions;
using ECP.Messaging.RabbitMQ.MessageQueue;
using ECP.Workflow.Service.ProvisionWorkflowService;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ECP.Workflow.EventReceivers.EventHandlers
{
    public class WorkflowActivatedEventHandler : IEventHandler<WorkflowActivated>
    {
        private readonly ILogger<WorkflowActivatedEventHandler> _logger;
        private readonly IProvisionWorkflowService _service;
        public WorkflowActivatedEventHandler(ILogger<WorkflowActivatedEventHandler> logger,
            IProvisionWorkflowService service)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _service = service;
        }
        public async Task<int> HandleAsync(WorkflowActivated integrationEvent)
        {
            _logger.LogInformation($"Receiving workflow activated event : { JsonConvert.SerializeObject(integrationEvent)}");

            try
            {
                int retval = await _service.ActivateWorkflow(
                       sourceWorkflowId: integrationEvent.SourceWorkflowId,
                       tenantId: integrationEvent.TenantId,
                       applicationId: integrationEvent.ApplicationId,
                        workflowName: integrationEvent.Name,
                       workflowCode: integrationEvent.Code,
                       definitionJson: JsonConvert.SerializeObject(integrationEvent.Definitionjson),
                        previewJson: JsonConvert.SerializeObject(integrationEvent.Previewjson),
                       status: integrationEvent.Status,
                       createdBy: integrationEvent.CreatedBy
                        );

                _logger.LogInformation($"Retrieval status for message: { JsonConvert.SerializeObject(integrationEvent)} => {retval}");

                return (int)BrokerAction.ConfirmAndAcknowledge;
            }
            catch (Exception ex)
            {
                _logger.LogError($"message {JsonConvert.SerializeObject(integrationEvent)} can not be copied because of : {ex.Message} " +
                    $"Inner Exception : {JsonConvert.SerializeObject(ex.InnerException)}");
                
                return (int)BrokerAction.ConfirmAndAcknowledge;
            }
        }
    }
}