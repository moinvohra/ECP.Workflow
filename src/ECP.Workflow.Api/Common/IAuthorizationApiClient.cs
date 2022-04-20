using System.Threading.Tasks;

namespace ECP.Workflow.Common
{
    public interface IAuthorizationApiClient
    {
        Task<bool> HasPermissionAsync(string token, string tenantId, string applicationid, string featurecode);
    }
}
