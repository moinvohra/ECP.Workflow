using ECP.Messages;
using ECP.Messaging.RabbitMQ.Abstractions;
using ECP.Messaging.RabbitMQ.MessageQueue;
using ECP.Workflow.Service.DeProvisionTenantApplication;
using ECP.Workflow.Service.ProvisionWorkflowService;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ECP.Workflow.EventReceivers.EventHandlers
{
    public class TenantApplicationDeAssignedEventHandler : IEventHandler<TenantApplicationDeAssigned>
    {
        private readonly ILogger<TenantApplicationDeAssignedEventHandler> _logger;
        private readonly IDeProvisionTenantApplicationsService _service;
        public TenantApplicationDeAssignedEventHandler(ILogger<TenantApplicationDeAssignedEventHandler> logger,
            IDeProvisionTenantApplicationsService service)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _service = service;
        }
        public async Task<int> HandleAsync(TenantApplicationDeAssigned integrationEvent)
        {
            _logger.LogInformation($"Workflow tenant application de-assigned : {integrationEvent.TenantId} => {string.Join(",", integrationEvent.ApplicationIds)}");

            try
            {

                int retval = await _service.DeProvisionTenantApplication(tenantId: integrationEvent.TenantId, applicationId: integrationEvent.ApplicationIds);

                _logger.LogInformation($"Retrieval status for event: {integrationEvent.TenantId} => {string.Join(",", integrationEvent.ApplicationIds)} =>  {retval}");

                return (int)BrokerAction.ConfirmAndAcknowledge;
            }
            catch (Exception ex)
            {
                _logger.LogError($"message {JsonConvert.SerializeObject(integrationEvent)} can not be done because of : {ex.Message} " +
                    $"Inner Exception : {JsonConvert.SerializeObject(ex.InnerException)}");

                return (int)BrokerAction.ConfirmAndAcknowledge;
            }
        }
    }
}