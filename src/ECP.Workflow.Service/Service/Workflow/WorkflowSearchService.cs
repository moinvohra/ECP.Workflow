using ECP.KendoGridFilter;
using ECP.Workflow.Repository.Query;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ECP.Workflow.Service
{
    public class WorkflowSearchService : IWorkflowSearchService
    {
        private readonly ILogger _logger;
        private readonly IWorkflowSearchRepository _repository;

        public WorkflowSearchService(ILoggerFactory loggerFactory,
            IWorkflowSearchRepository repository)
        {
            _logger = loggerFactory.CreateLogger<WorkflowSearchService>();
            _repository = repository;
        }

        public async Task<DataSourceResult> search(string tenantId, string applicationId, DataSourceRequest req)
        {
            _logger.LogInformation($"Workflow Search TenantId:  {tenantId}, ApplicationId : {applicationId} and Data: {JsonConvert.SerializeObject(req)}");

            return await _repository.search(tenantId, applicationId, req);
        }
    }
}
