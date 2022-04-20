using ECP.KendoGridFilter;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ECP.Workflow.Service
{
    public interface IWorkflowSearchService
    {
        Task<DataSourceResult> search(
        string tenantId,
        string applicationId,
        DataSourceRequest req);
    }
}
