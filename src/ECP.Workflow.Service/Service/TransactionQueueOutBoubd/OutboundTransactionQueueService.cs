using ECP.workflow.Repository;
using ECP.Workflow.Model;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECP.Workflow.Service
{
    public class OutboundTransactionQueueService : IOutboundTransactionQueueService
    {
        private readonly ILogger _logger;
        private readonly IOutboundTransactionQueueRepository _outboundTrnsactionQueueRepository;

        public OutboundTransactionQueueService(ILoggerFactory loggerFactory,
            IOutboundTransactionQueueRepository outboundTrnsactionQueueRepository)
        {
            _logger = loggerFactory.CreateLogger<OutboundTransactionQueueService>();
            _outboundTrnsactionQueueRepository = outboundTrnsactionQueueRepository;
        }
        public async Task<List<TransactionQueueOutbound>> GetDetails()
        {
            
           return await _outboundTrnsactionQueueRepository.GetDetails();

        }

        public async Task<int> UpdateQueueOutBoundStatus(int transactionqueueoutboundid)
        {
            _logger.LogInformation("Updating queue status");
            return await _outboundTrnsactionQueueRepository.UpdateQueueOutBoundStatus(transactionqueueoutboundid);
        }
    }
}
