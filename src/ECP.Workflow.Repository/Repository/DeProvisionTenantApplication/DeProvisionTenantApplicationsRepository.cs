using ECP.Workflow.Repository.ConnectionProvider;
using Microsoft.Extensions.Options;
using Npgsql;
using NpgsqlTypes;
using System.Data;
using System.Threading.Tasks;

namespace ECP.Workflow.Repository.ProvisionTenantApplications
{
    public class DeProvisionTenantApplicationsRepository : RepositoryBase, IDeProvisionTenantApplicationsRepository
    {
        public DeProvisionTenantApplicationsRepository(IOptions<ConnectionSettings> connectionOptions,
            IConnectionProvider connectionProvider
           ) : base(connectionOptions: connectionOptions,
                connectionProvider: connectionProvider)
        {
        }

        public async Task<int> DeProvisionTenantApplication(string TenantId, string[] ApplicationIds)
        {
            int retval = -1;

            _db.Open();


            try
            {
                var command = new NpgsqlCommand("CALL workflow.deprovisionworkflowfortenant(@tenantid,@applications)", (NpgsqlConnection)_db);

                command.Parameters.AddWithValue("@tenantid", NpgsqlDbType.Varchar, TenantId);
                command.Parameters.AddWithValue("@applications", NpgsqlDbType.Array | NpgsqlDbType.Text, ApplicationIds);

                await command.ExecuteNonQueryAsync();
                retval = 1;
            }
            finally
            {
                if (_db.State == ConnectionState.Open)
                    _db.Close();
            }
            return retval;
        }
    }
}