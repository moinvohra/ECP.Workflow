

using ECP.Workflow.Repository.ConnectionProvider;
using Microsoft.Extensions.Options;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Data;
using System.Threading.Tasks;

namespace ECP.Workflow.Repository.ProvisionWorkflowRepository
{
    public class ProvisionWorkflowRepository : RepositoryBase, IProvisionWorkflowRepository
    {

        public ProvisionWorkflowRepository(IOptions<ConnectionSettings> connectionOptions,
            IConnectionProvider connectionProvider) : base(connectionOptions: connectionOptions,
                connectionProvider: connectionProvider)
        {

        }

        public async Task<int> ActivateWorkflow(int sourceWorkflowId,
            string tenantId,
            string applicationId,
            string workflowName,
            string workflowCode,
            dynamic definitionJson,
            dynamic previewJson,
            int status,
            string createdBy
            )
        {
            try
            {
                _db.Open();

                var command = new NpgsqlCommand($"CALL workflow.cloneactiveworkflow(@SourceWorkflowId,@WorkflowName,@WorkflowCode,@TenantId,@ApplicationId,@DefinitionJson,@PreviewJson,@Status)", (NpgsqlConnection)_db);

                command.Parameters.AddWithValue("@SourceWorkflowId", NpgsqlDbType.Integer, sourceWorkflowId);
                command.Parameters.AddWithValue("@WorkflowName", NpgsqlDbType.Varchar, workflowName);
                command.Parameters.AddWithValue("@WorkflowCode", NpgsqlDbType.Varchar, workflowCode);
                command.Parameters.AddWithValue("@TenantId", NpgsqlDbType.Varchar, tenantId);
                command.Parameters.AddWithValue("@ApplicationId", NpgsqlDbType.Varchar, applicationId);
                command.Parameters.AddWithValue("@DefinitionJson", NpgsqlDbType.Jsonb, definitionJson);
                command.Parameters.AddWithValue("@PreviewJson", NpgsqlDbType.Jsonb, previewJson);
                command.Parameters.AddWithValue("@Status", NpgsqlDbType.Integer, status);

                await command.ExecuteNonQueryAsync();
            }
            finally
            {
                if (_db.State == ConnectionState.Open)
                    _db.Close();
            }
            return 1;
        }
    }
}
