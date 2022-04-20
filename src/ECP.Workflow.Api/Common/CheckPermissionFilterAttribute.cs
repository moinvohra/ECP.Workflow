using ECP.Workflow.Service.Model.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading.Tasks;

namespace ECP.Workflow.Common
{
    public class CheckPermissionFilterAttribute : ActionFilterAttribute
    {
        private readonly string _featureCode;
        private readonly IAuthorizationApiClient _authClient;

        public CheckPermissionFilterAttribute(string featurecode,
             IAuthorizationApiClient authClient)
        {
            _featureCode = featurecode;
            _authClient = authClient;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, 
            ActionExecutionDelegate next)
        {
            if (!await _authClient.
                   HasPermissionAsync(Convert.ToString(context.HttpContext.Request.Headers["Authorization"]),
                    context.ActionArguments["tenantid"].ToString(),
                    context.ActionArguments["applicationid"].ToString(),
                    _featureCode).ConfigureAwait(false))
            {
                context.Result = new ForbidResult();
                return;
            }
            await next();
        }
    }
}
