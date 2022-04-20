using ECP.Messages;
using ECP.Workflow.Model;
using ECP.Workflow.Model.Utility;
using ECP.Workflow.Repository.WorkflowRepository;
using ECP.Workflow.Service.MessageBroker;
using ECP.Workflow.Service.Model.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using static ECP.Workflow.Model.Utility.General;

namespace ECP.Workflow.Service
{
    public class WorkflowService : IWorkflowService
    {
        private readonly ILogger _logger;
        private readonly IGetIpAddress _getIpAddress;
        private readonly IMessageBroker _messageBroker;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWorkflowRepository _repository;

        public WorkflowService(ILoggerFactory loggerFactory,
               IGetIpAddress getIpAddress,
               IHttpContextAccessor httpContextAccessor,
               IWorkflowRepository repository,
               IMessageBroker messageBroker)
        {
            _logger = loggerFactory.CreateLogger<WorkflowService>();
            _getIpAddress = getIpAddress;
            _httpContextAccessor = httpContextAccessor;
            _repository = repository;
            _messageBroker = messageBroker;
        }

        public async Task<WorkflowDetail> Clone(CloneWorkflowReq req, string tenantId, string applicationId, int SourceWorkflowId)
        {
            if (await _repository.CheckCodeExist(tenantId, applicationId, req.Code))
                throw new DuplicateFoundException("Workflow", req.Code, Errors.WorkflowDuplicate, General.Duplicate);

            _logger.LogInformation($"Copy workflow from Id:  {SourceWorkflowId} and Data: {JsonConvert.SerializeObject(req)}");

            int retval = await _repository.Clone(req, tenantId, applicationId, SourceWorkflowId);

            _messageBroker.SendLogMessage(new AuditLogMessage(
                                  ApplicationId: applicationId,
                                  TenantId: tenantId,
                                  ClientId: _httpContextAccessor.HttpContext.GetClientId(),
                                  ClientName: string.Empty,
                                  ApplicationName: applicationId,
                                  IpAddress: _getIpAddress.GetCurrentIpAddress(),
                                  LogonUser: _httpContextAccessor.HttpContext.GetUserId(),
                                  LogonUserName: _httpContextAccessor.HttpContext.GetFirstname() + " " + _httpContextAccessor.HttpContext.GetLastname(),
                                  Module: "Workflow",
                                  EntityName: "Workflow",
                                  RecordId: retval.ToString(),
                                  RecordType: ModuleConstants.RecordTypeCreate,
                                  Feature: ModuleConstants.WorkflowClone,
                                  EntityObject: JsonConvert.SerializeObject(await _repository.GetById(tenantId,applicationId,retval)),
                                  LoggedDate: DateTime.UtcNow,
                                  RecordTitle: req.Name,
                                  LogonTenantId: _httpContextAccessor.HttpContext.GetTenantId()
                       ));

            return await _repository.GetById(tenantId, applicationId, retval);
        }

        public async Task<WorkflowDetail> Create(AddWorkflowReq req, string tenantId, string applicationId)
        {
            if (await _repository.CheckCodeExist(tenantId, applicationId, req.Code))
                throw new DuplicateFoundException("Workflow", req.Code, Errors.WorkflowDuplicate, General.Duplicate);

            _logger.LogInformation($"Create a new workflow: {JsonConvert.SerializeObject(req)}");

            var retval = await _repository.Create(req, tenantId, applicationId, _httpContextAccessor.HttpContext.GetUserId());

            _messageBroker.SendLogMessage(new AuditLogMessage(
                                 ApplicationId: applicationId,
                                 TenantId: tenantId,
                                 ClientId: _httpContextAccessor.HttpContext.GetClientId(),
                                 ClientName: string.Empty,
                                 ApplicationName: applicationId,
                                 IpAddress: _getIpAddress.GetCurrentIpAddress(),
                                 LogonUser: _httpContextAccessor.HttpContext.GetUserId(),
                                 LogonUserName: _httpContextAccessor.HttpContext.GetFirstname() + " " + _httpContextAccessor.HttpContext.GetLastname(),
                                 Module: "Workflow",
                                 EntityName: "Workflow",
                                 RecordId: retval.ToString(),
                                 RecordType: ModuleConstants.RecordTypeCreate,
                                 Feature: ModuleConstants.WorkflowCreate,
                                 EntityObject: JsonConvert.SerializeObject(await _repository.GetById(tenantId, applicationId, retval)),
                                 LoggedDate: DateTime.UtcNow,
                                  RecordTitle: req.Name,
                                 LogonTenantId: _httpContextAccessor.HttpContext.GetTenantId()
                      ));

            return await _repository.GetById(tenantId, applicationId, retval);
        }

        public async Task<int> Delete(string tenantId, string applicationId, int Id)
        {
            _logger.LogInformation($"Delete workflow Id: {Id}");

            WorkflowDetail workflowDetail = await GetById(tenantId, applicationId, Id);

            if (workflowDetail == null)
                throw new DuplicateFoundException("Workflow", Id.ToString(), Errors.WorkflowDetailsNotFound, General.NoDataFound);

            if (await _repository.IsActivated(tenantId, applicationId, Id))
            {
                throw new EntityCanNotDeletedException("Workflow", Id.ToString(), Errors.WorkflowCanNotDeleted, General.WorkflowCanNotDeleted);
            }

            await _repository.Delete(tenantId, applicationId, Id);
            _messageBroker.SendLogMessage(new AuditLogMessage(
                                 ApplicationId: applicationId,
                                 TenantId: tenantId,
                                 ClientId: _httpContextAccessor.HttpContext.GetClientId(),
                                 ClientName: string.Empty,
                                 ApplicationName: applicationId,
                                 IpAddress: _getIpAddress.GetCurrentIpAddress(),
                                 LogonUser: _httpContextAccessor.HttpContext.GetUserId(),
                                 LogonUserName: _httpContextAccessor.HttpContext.GetFirstname() + " " + _httpContextAccessor.HttpContext.GetLastname(),
                                 Module: "Workflow",
                                 EntityName: "Workflow",
                                 RecordId: Id.ToString(),
                                 RecordType: ModuleConstants.RecordTypeDelete,
                                 Feature: ModuleConstants.WorkflowDelete,
                                 EntityObject: JsonConvert.SerializeObject(workflowDetail),
                                 LoggedDate: DateTime.UtcNow,
                                 RecordTitle: workflowDetail.Name,
                                 LogonTenantId: _httpContextAccessor.HttpContext.GetTenantId()
                                 ));
            return (int)ReturnType.Success;
        }

        public async Task<WorkflowDetail> GetById(string tenantId, string applicationId, int Id)
        {
            _logger.LogInformation($"Get Workflow by Id: {Id} ");

            var result = await _repository.GetById(tenantId, applicationId, Id);

            if (result == null)
                throw new EntityNotFoundException("Workflow", Id.ToString(), Errors.WorkflowDetailsNotFound, General.NoDataFound);

            return result;
        }

        public async Task<WorkflowDetail> DeActivate(string tenantId, string applicationId, int Id)
        {
            _logger.LogInformation($"Inactive workflow Id: {Id}");

            WorkflowDetail workflowDetail = await GetById(tenantId, applicationId, Id);

            if (workflowDetail == null)
                throw new EntityNotFoundException("Workflow", Id.ToString(), General.NoDataFound, System.Net.HttpStatusCode.NotFound);

            await _repository.DeActivate(tenantId, applicationId, Id, _httpContextAccessor.HttpContext.GetUserId());

            _messageBroker.SendLogMessage(new AuditLogMessage(
                               ApplicationId: applicationId,
                               TenantId: tenantId,
                               ClientId: _httpContextAccessor.HttpContext.GetClientId(),
                               ClientName: string.Empty,
                               ApplicationName: applicationId,
                               IpAddress: _getIpAddress.GetCurrentIpAddress(),
                               LogonUser: _httpContextAccessor.HttpContext.GetUserId(),
                               LogonUserName: _httpContextAccessor.HttpContext.GetFirstname() + " " + _httpContextAccessor.HttpContext.GetLastname(),
                               Module: "Workflow",
                               EntityName: "Workflow",
                               RecordId: Id.ToString(),
                               RecordType: ModuleConstants.RecordTypeInActive,
                               Feature: ModuleConstants.WorkflowInActive,
                               EntityObject: JsonConvert.SerializeObject(await _repository.GetById(tenantId, applicationId, Id)),
                               LoggedDate: DateTime.UtcNow,
                               RecordTitle: workflowDetail.Name,
                               LogonTenantId: _httpContextAccessor.HttpContext.GetTenantId()

                    ));

            return workflowDetail;
        }

        public async Task<WorkflowDetail> Update(EditWorkflowReq req, string tenantId, string applicationId)
        {

            if (await _repository.CheckCodeExist(tenantId, applicationId, req.Code, req.Id))
                throw new DuplicateFoundException("Workflow", req.Code, Errors.WorkflowDuplicate, General.Duplicate);

            _logger.LogInformation($"Update workflow req: {JsonConvert.SerializeObject(req)}");

            await _repository.Update(req, tenantId, applicationId, _httpContextAccessor.HttpContext.GetUserId());
            _messageBroker.SendLogMessage(new AuditLogMessage(
                                  ApplicationId: applicationId,
                                  TenantId: tenantId,
                                  ClientId: _httpContextAccessor.HttpContext.GetClientId(),
                                  ClientName: string.Empty,
                                  ApplicationName: applicationId,
                                  IpAddress: _getIpAddress.GetCurrentIpAddress(),
                                  LogonUser: _httpContextAccessor.HttpContext.GetUserId(),
                                  LogonUserName: _httpContextAccessor.HttpContext.GetFirstname() + " " + _httpContextAccessor.HttpContext.GetLastname(),
                                  Module: "Workflow",
                                  EntityName: "Workflow",
                                  RecordId: req.Id.ToString(),
                                  RecordType: ModuleConstants.RecordTypeUpdate,
                                  Feature: ModuleConstants.WorkflowUpdate,
                                  EntityObject: JsonConvert.SerializeObject(await _repository.GetById(tenantId, applicationId, req.Id)),
                                  LoggedDate: DateTime.UtcNow,
                                  RecordTitle: req.Name,
                                  LogonTenantId: _httpContextAccessor.HttpContext.GetTenantId()

                       ));

            return await _repository.GetById(tenantId, applicationId, req.Id);
        }

        public async Task<WorkflowDetail> Activate(EditWorkflowReq req, string tenantId, string applicationId)
        {
            if (await _repository.CheckCodeExist(tenantId, applicationId, req.Code, req.Id))
                throw new DuplicateFoundException("Workflow", req.Code, Errors.WorkflowDuplicate, General.Duplicate);

            _logger.LogInformation($"Update workflow req: {JsonConvert.SerializeObject(req)}");

            await _repository.Activate(req, tenantId, applicationId, _httpContextAccessor.HttpContext.GetUserId());

            _messageBroker.SendLogMessage(new AuditLogMessage(
                                  ApplicationId: applicationId,
                                  TenantId: tenantId,
                                  ClientId: _httpContextAccessor.HttpContext.GetClientId(),
                                  ClientName: string.Empty,
                                  ApplicationName: applicationId,
                                  IpAddress: _getIpAddress.GetCurrentIpAddress(),
                                  LogonUser: _httpContextAccessor.HttpContext.GetUserId(),
                                  LogonUserName: _httpContextAccessor.HttpContext.GetFirstname() + " " + _httpContextAccessor.HttpContext.GetLastname(),
                                  Module: "Workflow",
                                  EntityName: "Workflow",
                                  RecordId: req.Id.ToString(),
                                  RecordType: ModuleConstants.RecordTypeUpdate,
                                  Feature: ModuleConstants.WorkflowActive,
                                  EntityObject: JsonConvert.SerializeObject(await _repository.GetById(tenantId, applicationId, req.Id)),
                                  LoggedDate: DateTime.UtcNow,
                                  RecordTitle: req.Name,
                                  LogonTenantId: _httpContextAccessor.HttpContext.GetTenantId()
                       ));

            return await _repository.GetById(tenantId, applicationId, req.Id);
        }
    }
}
