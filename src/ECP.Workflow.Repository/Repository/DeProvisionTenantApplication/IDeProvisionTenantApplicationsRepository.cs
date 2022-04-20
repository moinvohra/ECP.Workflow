using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ECP.Workflow.Repository.ProvisionTenantApplications
{
    public interface IDeProvisionTenantApplicationsRepository
    {
        Task<int> DeProvisionTenantApplication(
            string TenantId, 
            string[] ApplicationIds);
    }
}
