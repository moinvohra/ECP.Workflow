using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ECP.Workflow.Service.DeProvisionTenantApplication
{
    public interface IDeProvisionTenantApplicationsService
    {
        Task<int> DeProvisionTenantApplication(string tenantId, 
            string[] applicationId);
    }
}
