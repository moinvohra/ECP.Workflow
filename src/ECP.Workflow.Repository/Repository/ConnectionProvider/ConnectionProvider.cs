using System.Data;

namespace ECP.Workflow.Repository.ConnectionProvider
{
    public class ConnectionProvider : IConnectionProvider
    {
       readonly DatabaseType _databaseType;

        public ConnectionProvider()
        {
            _databaseType = DatabaseType.PostgreSql;
        }
        public IDbConnection CreateConnection(string connectionString)
        {
            IDbConnection dbConnection = null;
            switch (_databaseType)
            {
                case DatabaseType.PostgreSql:
                    dbConnection = new Npgsql.NpgsqlConnection(connectionString);
                    break;
            }
            return dbConnection;
        }
    }
}
