using ECP.Workflow.Repository.ProvisionTenantApplications;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ECP.Workflow.Service.DeProvisionTenantApplication
{
    public class DeProvisionTenantApplicationsService : IDeProvisionTenantApplicationsService
    {
        private readonly IDeProvisionTenantApplicationsRepository _repository;
        private readonly ILogger<DeProvisionTenantApplicationsService> _logger;
        
        public DeProvisionTenantApplicationsService(IDeProvisionTenantApplicationsRepository repository, 
            ILogger<DeProvisionTenantApplicationsService> logger
            )
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<int> DeProvisionTenantApplication(string tenantId, string[] applicationId)
        {
            _logger.LogInformation($"Workflow tenant application de-assigned : {tenantId} => {string.Join(",", applicationId)}");

            return await _repository.DeProvisionTenantApplication(tenantId, applicationId);
        }
    }
}
