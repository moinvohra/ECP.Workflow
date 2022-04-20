using ECP.KendoGridFilter;
using ECP.Workflow.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECP.Workflow.Repository.Query
{
    public interface IWorkflowSearchRepository
    {
        Task<DataSourceResult> search(
         string tenantId,
         string applicationId,
         DataSourceRequest req);
    }
}
