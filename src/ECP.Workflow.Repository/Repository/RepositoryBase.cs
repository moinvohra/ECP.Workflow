﻿
using ECP.Workflow.Repository.ConnectionProvider;
using Microsoft.Extensions.Options;
using System.Data;

namespace ECP.Workflow.Repository
{
    public class RepositoryBase
    {
        protected readonly IDbConnection _db = null;
        protected readonly ConnectionSettings _connectionOptions;

        public RepositoryBase(IOptions<ConnectionSettings> connectionOptions,
            IConnectionProvider connectionProvider)
        {
            _connectionOptions = connectionOptions.Value;
            _db = connectionProvider.CreateConnection(_connectionOptions.ConnectionString);
        }
    }
}
