using ECP.Workflow.Repository.ProvisionTenantApplications;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ECP.Workflow.Service.ProvisionTenantApplication
{
    public class ProvisionTenantApplicationsService : IProvisionTenantApplicationsService
    {
        private readonly IProvisionTenantApplicationsRepository _repository;
        private readonly ILogger<ProvisionTenantApplicationsService> _logger;
        private readonly IConfiguration _configuration;
        public ProvisionTenantApplicationsService(IProvisionTenantApplicationsRepository repository, 
            ILogger<ProvisionTenantApplicationsService> logger,
             IConfiguration configuration
            )
        {
            _repository = repository;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<int> ProvisionTenantApplication(string tenantId, string[] applicationId)
        {
            _logger.LogInformation($"Workflow tenant application assigned : {tenantId} => {string.Join(",", applicationId)}");

            int retval = await _repository.ProvisionTenantApplication(_configuration["TenantConfiguration:PrimaryTenantId"].ToString(), tenantId, applicationId);

            return retval;
        }
    }
}
