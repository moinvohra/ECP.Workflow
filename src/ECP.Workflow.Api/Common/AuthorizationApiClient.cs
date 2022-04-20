using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ECP.Workflow.Common
{
    /// <summary>
    /// Check permission with authorization API
    /// </summary>
    public class AuthorizationApiClient : IAuthorizationApiClient
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly string _authorizeUrl;
       

        public AuthorizationApiClient(IHttpClientFactory clientFactory,
            IConfiguration config)
        {
            _clientFactory = clientFactory;
            _authorizeUrl = config["AuthorizeServer:AuthorizeURL"];
        }

        public async Task<bool> HasPermissionAsync(string token, string tenantId, string applicationid, string featurecode)
        {
            string url = $" {_authorizeUrl}/tenants/{tenantId}/applications/{applicationid}/permissions/Check/{featurecode}";
            var client = _clientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("authorization", token);
            HttpResponseMessage authresponse = await client.GetAsync(url);
            if (authresponse.IsSuccessStatusCode)
                return true;

            return false;
        }
    }
}
