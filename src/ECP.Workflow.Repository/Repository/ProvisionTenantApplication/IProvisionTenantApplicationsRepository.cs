using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ECP.Workflow.Repository.ProvisionTenantApplications
{
    public interface IProvisionTenantApplicationsRepository
    {
        Task<int> ProvisionTenantApplication(
            string PrimaryTenantId,
            string TenantId, 
            string[] ApplicationIds);
    }
}
