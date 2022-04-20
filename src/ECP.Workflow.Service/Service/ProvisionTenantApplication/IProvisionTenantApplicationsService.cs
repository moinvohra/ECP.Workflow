using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ECP.Workflow.Service.ProvisionTenantApplication
{
    public interface IProvisionTenantApplicationsService
    {
        Task<int> ProvisionTenantApplication(string tenantId, 
            string[] applicationId);
    }
}
