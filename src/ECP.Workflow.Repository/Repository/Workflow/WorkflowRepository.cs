using Dapper;
using ECP.workflow.Repository;
using ECP.Workflow.Model;
using ECP.Workflow.Model.Utility;
using ECP.Workflow.Model.Workflow;
using ECP.Workflow.Repository.ConnectionProvider;
using ECP.Workflow.Repository.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static ECP.Workflow.Model.Utility.General;

namespace ECP.Workflow.Repository.WorkflowRepository
{
    public class WorkflowRepository : RepositoryBase, IWorkflowRepository
    {
        private readonly IOutboundTransactionQueueRepository _outboundTransactionQueueRepository;

        public WorkflowRepository(IOptions<ConnectionSettings> connectionOptions,
             IConnectionProvider connectionProvider,
              IOutboundTransactionQueueRepository outboundTransactionQueueRepository

            ) : base(connectionOptions: connectionOptions,
                 connectionProvider: connectionProvider)
        {
            _outboundTransactionQueueRepository = outboundTransactionQueueRepository;

        }

        public async Task<int> Clone(CloneWorkflowReq req, string tenantId, string applicationId, int sourceWorkflowId)
        {

            string sql = @"
               INSERT INTO workflow.workflowdefinition (tenantid, applicationid, workflowname, workflowcode, definitionjson, previewjson,sourceworkflowid)
               SELECT
                   tenantid,
                   applicationid,
                   @workflowname,
                   @workflowcode,
                   definitionjson,
                   previewjson,
                   @sourceworkflowid
               FROM
                   workflow.workflowdefinition
               WHERE
                   tenantid = @tenantId
                   AND applicationid = @applicationId
                   AND workflowid = @sourceWorkflowId
               RETURNING
                   workflowid;";

            int id = await _db.ExecuteScalarAsync<int>(sql,
                new
                {
                    @workflowname = req.Name,
                    @workflowcode = req.Code,
                    tenantid = tenantId,
                    applicationid = applicationId,
                    sourceWorkflowId = sourceWorkflowId,
                });

            return id;
        }

        public async Task<int> Create(AddWorkflowReq req, string tenantId, string applicationId, string createdBy)
        {
            int retval = 0;

            retval = await _db.ExecuteScalarAsync<int>(
          @"INSERT INTO workflow.workflowdefinition 
                (
                 tenantId,
                 applicationId, 
                 workflowname,
                 workflowcode,
                 definitionjson,
                 previewjson,
                 status,
                 createdby
                 )
                  VALUES 
                 (
                 @tenantId,   
                 @applicationId, 
                 @workflowname,
                 @workflowcode,
                 CAST(@definitionJson AS jsonb),
                 CAST(@previewjson AS jsonb),
                 @status,
                 @createdby   
                 )returning workflowid", new
          {
              tenantId,
              applicationId,
              workflowname = req.Name,
              workflowcode = req.Code,
              definitionjson = JsonConvert.SerializeObject(req.Definitionjson),
              previewjson = JsonConvert.SerializeObject(req.Previewjson),
              req.Status,
              createdBy
          }).ConfigureAwait(false);

            return retval;
        }

        public async Task<int> Delete(string tenantId, string applicationId, int id)
        {
            await _db.ExecuteAsync(@"
                Delete from workflow.workflowdefinition where
                workflowid= @workflowid", new
            {
                workflowid = id
            }).ConfigureAwait(false);

            return (int)ReturnType.Success;
        }

        public async Task<WorkflowDetail> GetById(string tenantId, string applicationId, int id)
        {
            var result = await _db.QuerySingleOrDefaultAsync<WorkflowDetail>(
                           @"SELECT
                              workflowid as Id,
                              status,
                              workflowname as Name,
                              workflowcode as Code,
                              definitionjson,
                              previewjson,
                              status,
                              startdate,
                              enddate,
                              sourceworkflowid
                              FROM
                              workflow.workflowdefinition
                              WHERE workflowid = @workflowid 
                              and tenantid = @tenantId 
                              and applicationid = @applicationid", new
                           {
                               workflowid = id,
                               tenantId,
                               applicationId
                           }).ConfigureAwait(false);
            return result;

        }

        public async Task<int> GetTotalCount(GetWorkflowReq req, string tenantId, string applicationId)
        {

            var retval = await _db.ExecuteScalarAsync<int>(@"SELECT count(*) as TotalCount 
                                                  FROM workflow.workflowdefinition
                                                  WHERE tenantid = @tenantid
                                                  AND applicationid = @applicationid
                                                  AND (@workflowname is null OR workflowname = @workflowname)
                                                  AND (@workflowcode is null OR workflowcode = @workflowcode)
                                                  AND (@status is null OR status = @status)",
                                                new
                                                {
                                                    tenantId,
                                                    applicationId,
                                                    @workflowname = req.Name,
                                                    @workflowcode = req.Code,
                                                    req.Status
                                                }).ConfigureAwait(false);

            return retval;
        }

        public async Task<int> DeActivate(string tenantId, string applicationId, int id, string updatedBy)
        {
            var exists = await _db.ExecuteScalarAsync<bool>(
             @"SELECT EXISTS(
                        SELECT 1 from 
                         workflow.workflowdefinition
                       WHERE
                        tenantid = @tenantid
                    AND applicationid = @applicationid 
                    AND  workflowid = @workflowid
                    )", param: new
             {
                 tenantId,
                 applicationId,
                 workflowid = id
             }).ConfigureAwait(false);

            if (!exists)
                return (int)ReturnType.NotFound;

            _db.Open();
            using (var transaction = _db.BeginTransaction())
            {
                var retval = await _db.ExecuteAsync(
                   @"UPDATE workflow.workflowdefinition
                  SET status =  @status,
                      enddate = @enddate,
                      modifiedby = @modifiedby
                  WHERE 
                      tenantId = @tenantId
                  AND applicationId = @applicationId
                  AND workflowId = @WorkflowId",
                  new
                  {
                      status = (int)WorkflowStatus.InActive,
                      enddate = DateTime.Now,
                      modifiedby = updatedBy,
                      tenantid = tenantId,
                      applicationid = applicationId,
                      workflowid = id
                  });


                TransactionQueueOutbound queueOutbound = new TransactionQueueOutbound();
                queueOutbound.Payload = JsonConvert.SerializeObject(id);
                queueOutbound.EventType = General.WORKFLOWINACTIVATED;
                queueOutbound.ServiceName = General.WORKFLOWINACTIVATED;
                queueOutbound.SentToExchange = false;

                int ret = await _outboundTransactionQueueRepository.CreateOutboundTransactionQueueAsync(queueOutbound);

                if (ret != -1)
                {
                    transaction.Commit();
                }
                else
                {
                    transaction.Rollback();
                }


                return retval;
            }
        }

        public async Task<int> Update(EditWorkflowReq req, string tenantId, string applicationId, string updatedBy)
        {

            var retval = await _db.ExecuteAsync(
           @"UPDATE workflow.workflowdefinition
                  SET workflowname =  @workflowname,
                      workflowcode =  @workflowcode,
                      definitionjson = CAST(@definitionJson AS jsonb),
                      previewjson = CAST(@previewjson AS jsonb),
                      status = @status,
                      modifiedby = @modifiedby,
                      modifieddate = @modifieddate
                  WHERE 
                    WorkflowId = @WorkflowId", new
           {
               workflowname = req.Name,
               workflowcode = req.Code,
               definitionJson = JsonConvert.SerializeObject(req.DefinitionJson),
               previewjson = JsonConvert.SerializeObject(req.PreviewJson),
               status = (int)General.WorkflowStatus.Pending,
               modifiedby = updatedBy,
               modifieddate = DateTime.Now,
               workflowid = req.Id
           });

            return retval;
        }

        public async Task<int> Activate(EditWorkflowReq req, string tenantId, string applicationId, string updatedBy)
        {
            var exists = await _db.ExecuteScalarAsync<bool>(
             @"SELECT EXISTS(
                       SELECT 1  FROM  
                      workflow.workflowdefinition 
                      WHERE 
                        workflowid <> @workflowid
                       AND workflowcode = @workflowcode
                       AND applicationid = @applicationid
                       AND tenantid = @tenantid
                    )", new
             {
                 workflowid = req.Id,
                 workflowcode = req.Code,
                 tenantid = tenantId,
                 applicationid = applicationId
             });

            if (exists)
                return (int)ReturnType.Duplicate;


            _db.Open();
            using (var transaction = _db.BeginTransaction())
            {
                var WorkflowStartDate = DateTime.Now;
                var retval = await _db.ExecuteAsync(
                @"UPDATE workflow.workflowdefinition
                           SET workflowname =  @workflowname,
                               workflowcode =  @workflowcode,
                               definitionjson = CAST(@definitionJson AS jsonb),
                               previewjson = CAST(@previewjson AS jsonb),
                               status = @status,
                               startdate = @startdate,  
                               modifiedby = @modifiedby,
                               modifieddate = @modifieddate
                           WHERE 
                             WorkflowId = @WorkflowId", new
                {
                    workflowname = req.Name,
                    workflowcode = req.Code,
                    definitionJson = JsonConvert.SerializeObject(req.DefinitionJson),
                    previewjson = JsonConvert.SerializeObject(req.PreviewJson),
                    status = (int)General.WorkflowStatus.Active,
                    startdate = WorkflowStartDate,
                    modifiedby = updatedBy,
                    modifieddate = WorkflowStartDate,
                    workflowid = req.Id
                });

                TransactionQueueOutbound transactionQueue = new TransactionQueueOutbound();

                transactionQueue.Payload = JsonConvert.SerializeObject(new ActiveWorkflowData()
                {
                    SourceWorkflowId = req.Id,
                    Name = req.Name,
                    TenantId = tenantId,
                    ApplicationId = applicationId,
                    Code = req.Code,
                    DefinitionJson = req.DefinitionJson,
                    PreviewJson = req.PreviewJson,
                    StartDate = WorkflowStartDate,
                    Status = (int)General.WorkflowStatus.Active,
                });

                transactionQueue.EventType = General.WORKFLOWACTIVATED;
                transactionQueue.SentToExchange = false;

                int retv = await _outboundTransactionQueueRepository.CreateOutboundTransactionQueueAsync(transactionQueue);

                if (retv != -1)
                {
                    transaction.Commit();
                }
                else
                {
                    transaction.Rollback();
                }
                return retval;
            }
        }

        public async Task<bool> CheckCodeExist(string tenantId, string applicationId, string codeName)
        {
            var exists = await _db.ExecuteScalarAsync<bool>(
                   @"SELECT EXISTS(
                            SELECT 1 from 
                            workflow.workflowdefinition
                            WHERE applicationid = @applicationid
                            and tenantId = @tenantId
                            and LOWER(workflowcode) = LOWER(@workflowcode)
                      )",
                   new
                   {
                       applicationId,
                       tenantId,
                       workflowcode = codeName
                   }
                   ).ConfigureAwait(false);

            return exists;
        }

        public async Task<bool> CheckCodeExist(string tenantId, string applicationId, string codeName, int id)
        {
            var exists = await _db.ExecuteScalarAsync<bool>(
           @"SELECT EXISTS(
                       SELECT 1  FROM  
                      workflow.workflowdefinition 
                      WHERE 
                        workflowid <> @workflowid
                       AND LOWER(workflowcode) = LOWER(@workflowcode)
                       AND applicationid = @applicationid
                       AND tenantid = @tenantid
                    )", new
           {
               workflowid = id,
               workflowcode = codeName,
               tenantid = tenantId,
               applicationid = applicationId
           });

            return exists;
        }

        public async Task<bool> IsActivated(string tenantId, string applicationId, int id)
        {
            var activated = await _db.ExecuteScalarAsync<bool>(
             @"SELECT EXISTS(
                        SELECT 1 from 
                         workflow.workflowdefinition
                       WHERE
                        tenantid = @tenantid
                    AND applicationid = @applicationid 
                    AND  workflowid = @workflowid AND status in (0,1)
                    )", param: new
             {
                 tenantId,
                 applicationId,
                 workflowid = id
             }).ConfigureAwait(false);

            return activated;
        }
    }
}
