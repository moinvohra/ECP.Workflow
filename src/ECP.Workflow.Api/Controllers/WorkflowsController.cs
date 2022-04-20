using ECP.Workflow.Common;
using ECP.Workflow.Model;
using ECP.Workflow.Model.Utility;
using ECP.Workflow.Service;
using ECP.Workflow.Service.Validation;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ECP.KendoGridFilter;
using static ECP.Workflow.Model.Utility.General;
using ECP.Workflow.Repository.Query;

namespace ECP.Workflow.Api.Controllers
{
    [Route("tenants/{tenantId}/applications/{applicationid}/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = IdentityServerAuthenticationDefaults.AuthenticationScheme, Policy = "ApiReader")]
    public class WorkflowsController : ControllerBase
    {
        private readonly IWorkflowService _service;

        private readonly IWorkflowSearchRepository _repository;
        public WorkflowsController(IWorkflowService service,
            IWorkflowSearchRepository repository)
        {
            _service = service;
            _repository = repository;
        }

        /// <summary>
        /// Get Workflow List
        /// </summary>
        /// <param name="req"></param>
        /// <param name="tenantId"></param>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("search")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(BaseResponse<List<WorkflowListRecord>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [TypeFilter(type: typeof(CheckPermissionFilterAttribute), Arguments = new object[] { Feature.WorkflowList })]
        public async Task<ActionResult> GetData(
            [FromBody] DataSourceRequest req,
            [FromRoute] string tenantId,
            [FromRoute] string applicationId
            )
        {
            BaseResponse<List<WorkflowListRecord>> response = new BaseResponse<List<WorkflowListRecord>>();

            DataSourceResult workflowData = await _repository.search(tenantId, applicationId, req);

            response.ResponseCode = (int)HttpStatusCode.OK;
            response.TotalRecords = workflowData.Total;
            response.ResponseData = workflowData.Total == 0 ? new List<WorkflowListRecord>() : workflowData.Data.OfType<WorkflowListRecord>().ToList();
            return StatusCode(response.ResponseCode, response);
        }


        /// <summary>
        /// Get Workflow by Id
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="applicationId"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetById/{Id}")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(BaseResponse<WorkflowDetail>), (int)HttpStatusCode.OK)]
        [TypeFilter(type: typeof(CheckPermissionFilterAttribute), Arguments = new object[] { Feature.WorkflowList })]
        public async Task<ActionResult> GetById([FromRoute] string tenantId, [FromRoute] string applicationId, [FromRoute] int Id)
        {
            BaseResponse<WorkflowDetail> response = new BaseResponse<WorkflowDetail>();

            var retval = await _service.GetById(tenantId, applicationId, Id);

            response.ResponseCode = (int)HttpStatusCode.OK;
            response.ResponseData = retval;
            return StatusCode(response.ResponseCode, response);
        }

        /// <summary>
        /// Create Workflow
        /// </summary>
        /// <param name="req"></param>
        /// <param name="tenantId"></param>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.Conflict)]
        [ProducesResponseType(typeof(BaseResponse<WorkflowDetail>), (int)HttpStatusCode.OK)]
        [TypeFilter(type: typeof(CheckPermissionFilterAttribute), Arguments = new object[] { Feature.WorkflowCreate })]
        public async Task<ActionResult> Post([FromBody]AddWorkflowReq req, [FromRoute] string tenantId, [FromRoute] string applicationId)
        {
            BaseResponse<WorkflowDetail> response = new BaseResponse<WorkflowDetail>();

            response.ResponseCode = (int)HttpStatusCode.Created;
            response.ResponseData = await _service.Create(req, tenantId, applicationId);
            response.ResponseMessage = string.Format(WorkflowCreated, req.Name);
            return StatusCode(response.ResponseCode, response);
        }

        /// <summary>
        /// Update Workflow
        /// </summary>
        /// <param name="req"></param>
        /// <param name="tenantId"></param>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        [HttpPut]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.Conflict)]
        [ProducesResponseType(typeof(BaseResponse<WorkflowDetail>), (int)HttpStatusCode.OK)]
        [TypeFilter(type: typeof(CheckPermissionFilterAttribute), Arguments = new object[] { Feature.WorkflowUpdate })]
        public async Task<ActionResult> Put([FromBody]EditWorkflowReq req, [FromRoute] string tenantId, [FromRoute] string applicationId)
        {
            BaseResponse<WorkflowDetail> response = new BaseResponse<WorkflowDetail>();

            response.ResponseCode = (int)HttpStatusCode.OK;
            response.ResponseMessage = string.Format(WorkflowUpdated, req.Name);
            response.ResponseData = await _service.Update(req, tenantId, applicationId);
            return StatusCode(response.ResponseCode, response);
        }

        /// <summary>
        /// Clone Workflow
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="applicationId"></param>
        /// <param name="Id"></param>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("{Id}/Clone")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.Conflict)]
        [ProducesResponseType(typeof(BaseResponse<WorkflowDetail>), (int)HttpStatusCode.Created)]
        [TypeFilter(type: typeof(CheckPermissionFilterAttribute), Arguments = new object[] { Feature.WorkflowCreate })]
        public async Task<ActionResult> Clone([FromRoute] string tenantId, 
            [FromRoute] string applicationId, 
            [FromRoute] int Id, 
            [FromBody] CloneWorkflowReq req)
        {
            BaseResponse<WorkflowDetail> response = new BaseResponse<WorkflowDetail>();

            response.ResponseCode = (int)HttpStatusCode.Created;
            response.ResponseData = await _service.Clone(req, tenantId, applicationId, Id);
            response.ResponseMessage = string.Format(General.WorkflowCopy, req.Name);
            return StatusCode(response.ResponseCode, response);
        }

        /// <summary>
        /// Activate Workflow
        /// </summary>
        /// <param name="req"></param>
        /// <param name="tenantId"></param>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("Active")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(BaseResponse<WorkflowDetail>), (int)HttpStatusCode.OK)]
        [TypeFilter(type: typeof(CheckPermissionFilterAttribute), Arguments = new object[] { Feature.WorkflowActivate })]
        public async Task<ActionResult> Active([FromBody]EditWorkflowReq req, [FromRoute] string tenantId, [FromRoute] string applicationId)
        {
            BaseResponse<WorkflowDetail> response = new BaseResponse<WorkflowDetail>();

            List<Model.Utility.Error> validate = WorkflowDefinitionParser.Validate(req.DefinitionJson);

            if (validate.Any())
            {
                response.ResponseCode = (int)HttpStatusCode.BadRequest;
                response.ResponseMessage = string.Format(WorkflowInvalid, req.Name);


                foreach (var item in validate)
                {
                    response.Error.Add(new Workflow.Common.Error
                    {
                        ErrorMessage = item.ErrorMessage,
                        StatusCode = (int)HttpStatusCode.BadRequest
                    });
                }
                return StatusCode(response.ResponseCode, response);
            }

            response.ResponseCode = (int)HttpStatusCode.OK;
            response.ResponseMessage = string.Format(WorkflowUpdatedActivated, req.Name);
            response.ResponseData = await _service.Activate(req, tenantId, applicationId);
            return StatusCode(response.ResponseCode, response);
        }

        /// <summary>
        /// Deactivate Workflow
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="applicationId"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("InActive/{Id}")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(BaseResponse<WorkflowDetail>), (int)HttpStatusCode.OK)]
        [TypeFilter(type: typeof(CheckPermissionFilterAttribute), Arguments = new object[] { Feature.WorkflowInActivate })]
        public async Task<ActionResult> InActive([FromRoute] string tenantId, [FromRoute] string applicationId, [FromRoute] int Id)
        {
            BaseResponse<WorkflowDetail> response = new BaseResponse<WorkflowDetail>();

            response.ResponseCode = (int)HttpStatusCode.OK;
            response.ResponseData = await _service.DeActivate(tenantId, applicationId, Id);
            response.ResponseMessage = General.WorkflowInActivated;
            return StatusCode(response.ResponseCode, response);
        }


        /// <summary>
        /// Delete Workflow
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="applicationId"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("Delete/{Id}")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.Conflict)]
        [TypeFilter(type: typeof(CheckPermissionFilterAttribute), Arguments = new object[] { Feature.WorkflowDelete })]
        public async Task<ActionResult> Delete([FromRoute] string tenantId, [FromRoute] string applicationId, [FromRoute] int Id)
        {
            BaseResponse<WorkflowDetail> response = new BaseResponse<WorkflowDetail>();

            await _service.Delete(tenantId, applicationId, Id);
            response.ResponseCode = (int)HttpStatusCode.NoContent;
            response.ResponseMessage = General.WorkflowDeleted;
            return StatusCode(response.ResponseCode);
        }
    }
}