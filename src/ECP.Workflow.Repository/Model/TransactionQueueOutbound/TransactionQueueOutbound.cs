using System;
using System.Collections.Generic;
using System.Text;

namespace ECP.Workflow.Repository.Model
{
    public class TransactionQueueOutbound
    {
        public int transactionqueueoutboundid { get; set; }
        public string EventType { get; set; }
        public string Payload { get; set; }
        public string ServiceName { get; set; }
        public bool SentToExchange { get; set; } = true;
    }
}
