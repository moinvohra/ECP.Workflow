using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ECP.Workflow.Repository.ConnectionProvider
{
    public interface IConnectionProvider
    {
        IDbConnection CreateConnection(string connectionString);
    }
}
