using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace ECP.Workflow.Service.Model.Utility
{

    public static class IdentityExtensions
    {
        public static string GetUserId(this HttpContext context)
        {
            var identity = context.User.Identity as ClaimsIdentity;

            if (identity != null)
                return identity.FindFirst("sub")?.Value;
            return string.Empty;
        }

        /// <summary>
        /// Get Login user fistName
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetFirstname(this HttpContext context)
        {
            var identity = context.User.Identity as ClaimsIdentity;
            if (identity != null)
                return identity.FindFirst("firstName")?.Value;
            return string.Empty;
        }

        /// <summary>
        /// Get Login user lastName
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetLastname(this HttpContext context)
        {
            var identity = context.User.Identity as ClaimsIdentity;
            if (identity != null)
                return identity.FindFirst("lastName")?.Value;
            return string.Empty;
        }

        /// <summary>
        /// Get Login user lastName
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetPhoneNumber(this HttpContext context)
        {
            var identity = context.User.Identity as ClaimsIdentity;
            if (identity != null)
                return identity.FindFirst("logo")?.Value;
            return string.Empty;
        }


        public static string GetClientId(this HttpContext context)
        {
            var identity = context.User.Identity as ClaimsIdentity;
            if (identity != null)
                return identity.FindFirst("client_id")?.Value;
            return string.Empty;
        }

        public static string GetUsername(this HttpContext context)
        {
            var identity = context.User.Identity as ClaimsIdentity;
            if (identity != null)
                return identity.FindFirst("username")?.Value;
            return string.Empty;
        }


        public static string GetTenantId(this HttpContext context)
        {
            var identity = context.User.Identity as ClaimsIdentity;
            if (identity != null)
                return identity.FindFirst("tenantId")?.Value;
            return string.Empty;
        }
    }
}
