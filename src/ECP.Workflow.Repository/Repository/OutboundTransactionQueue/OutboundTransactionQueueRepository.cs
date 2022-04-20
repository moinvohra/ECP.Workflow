using Dapper;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using ECP.Workflow.Repository.ConnectionProvider;
using ECP.Workflow.Model;
using ECP.workflow.Repository;
using System.Collections.Generic;
using System.Linq;

namespace ECP.Workflow.Repository
{
    public class OutboundTransactionQueueRepository : RepositoryBase, IOutboundTransactionQueueRepository
    {

        public OutboundTransactionQueueRepository(IOptions<ConnectionSettings> connectionOptions,
            IConnectionProvider connectionProvider
           ) : base(connectionOptions: connectionOptions,
                connectionProvider: connectionProvider)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transactionQueueOutbound"></param>
        /// <returns></returns>

        public async Task<int> CreateOutboundTransactionQueueAsync(TransactionQueueOutbound transactionQueueOutbound)
        {

            int retval = -1;

            retval = await _db.ExecuteScalarAsync<int>(
                 @"INSERT INTO workflow.transactionqueueoutbound 
                (
                 eventtype,
                 payload, 
                 servicename,
                 senttoexchange)
                 VALUES(
                 @eventtype,
                 @payload,  
                 @servicename,
                 @senttoexchange
                 )returning transactionqueueoutboundid", transactionQueueOutbound);

            return retval;

        }

        public async Task<List<TransactionQueueOutbound>> GetDetails()
        {
            var retval = await _db.QueryAsync<TransactionQueueOutbound>(@"SELECT 
                 transactionqueueoutboundid,
                 eventtype,
                 payload
                 from    
                 workflow.transactionqueueoutbound
                WHERE senttoexchange = false").ConfigureAwait(false);
            return retval.ToList();
        }

        public async Task<int> UpdateQueueOutBoundStatus(int transactionqueueoutboundid)
        {
            int retval = await _db.ExecuteAsync(@"
                          UPDATE workflow.transactionqueueoutbound
                          SET
                           senttoexchange = true
                          WHERE
                          transactionqueueoutboundid = @transactionqueueoutboundid", transactionqueueoutboundid).ConfigureAwait(false);

            return retval;
        }
    }
}
