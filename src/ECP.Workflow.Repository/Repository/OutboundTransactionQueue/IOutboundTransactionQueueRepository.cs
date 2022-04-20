using ECP.Workflow.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECP.workflow.Repository
{
    public interface IOutboundTransactionQueueRepository
    {
        /// <summary>
        /// Add Transaction Queue
        /// </summary>
        /// <param name="transactionQueueOutbound"></param>
        /// <returns></returns>
        Task<int> CreateOutboundTransactionQueueAsync(TransactionQueueOutbound transactionQueueOutbound);

        Task<List<TransactionQueueOutbound>> GetDetails();

        Task<int> UpdateQueueOutBoundStatus(int transactionqueueoutboundid);
    }
}
