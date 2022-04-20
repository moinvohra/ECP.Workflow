using ECP.Workflow.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ECP.Workflow.Service
{
    public interface IOutboundTransactionQueueService
    {
        Task<List<TransactionQueueOutbound>> GetDetails();

        Task<int> UpdateQueueOutBoundStatus(int transactionqueueoutboundid);
    }
}
