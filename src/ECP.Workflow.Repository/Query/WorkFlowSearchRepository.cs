using ECP.KendoGridFilter;
using ECP.Workflow.Model;
using ECP.Workflow.Repository.DBContext;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECP.Workflow.Repository.Query
{
    public class WorkFlowSearchRepository : IWorkflowSearchRepository
    {
        private readonly WorkflowDbContext _workflowDbContext;
        public WorkFlowSearchRepository(WorkflowDbContext workflowDbContext)
        {
            _workflowDbContext = workflowDbContext;
        }
        public async Task<DataSourceResult> search(string tenantId, string applicationId, DataSourceRequest req)
        {
            NpgsqlParameter tenantIdParameter = new NpgsqlParameter("@tenantid", NpgsqlTypes.NpgsqlDbType.Varchar);
            tenantIdParameter.Value = tenantId;

            NpgsqlParameter applicationIdParameter = new NpgsqlParameter("@applicationid", NpgsqlTypes.NpgsqlDbType.Varchar);
            applicationIdParameter.Value = applicationId;

            IQueryable<WorkflowListRecord> workflowQuery = _workflowDbContext.workflowListView
                                                 .FromSqlRaw(@"
                                                   SELECT
                                                   workflowid,
                                                   status,
                                                   tenantid,
                                                   applicationid,
                                                   workflowcode,
                                                   workflowname,
                                                   startdate,
                                                   enddate,
                                                   previewjson
                                                   FROM
                                                   workflow.workflowdefinition
                                                   WHERE
                                                   tenantid = @tenantid
                                                   AND applicationid = @applicationid",
                                                   new NpgsqlParameter[] { tenantIdParameter, applicationIdParameter });

            return await Task.FromResult(QueryableExtensions.ToDataSourceResult(workflowQuery, req.Take, req.Skip, req.Sort, req.Filter));
        }
    }
}
