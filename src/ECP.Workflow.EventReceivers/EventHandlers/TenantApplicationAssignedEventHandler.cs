using ECP.Messages;
using ECP.Messaging.RabbitMQ.Abstractions;
using ECP.Messaging.RabbitMQ.MessageQueue;
using ECP.Workflow.Service.ProvisionTenantApplication;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ECP.Workflow.EventReceivers.EventHandlers
{
    public class TenantApplicationAssignedEventHandler : IEventHandler<TenantApplicationAssigned>
    {
        private readonly ILogger<TenantApplicationAssignedEventHandler> _logger;
        private readonly IProvisionTenantApplicationsService _service;

        public TenantApplicationAssignedEventHandler(ILogger<TenantApplicationAssignedEventHandler> logger,
            IProvisionTenantApplicationsService service)
        {
            _logger = logger;
            _service = service;
        }
        public async Task<int> HandleAsync(TenantApplicationAssigned integrationEvent)
        {
            _logger.LogInformation($"Workflow tenant application assigned : {integrationEvent.TenantId} => {string.Join(",", integrationEvent.ApplicationIds)}");

            try
            {
                int retval = await _service.ProvisionTenantApplication(
                       tenantId: integrationEvent.TenantId,
                       applicationId: integrationEvent.ApplicationIds);

                _logger.LogInformation($"Retrieval status for event: {integrationEvent.TenantId} => {string.Join(",", integrationEvent.ApplicationIds)} =>  {retval}");

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
