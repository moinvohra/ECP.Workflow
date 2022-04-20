using System;
using System.Collections.Generic;
using System.Text;

namespace ECP.Workflow.Model
{
    public class TransactionQueueOutbound
    {
        public int TransactionQueueOutboundId { get; set; }
        public string EventType { get; set; }
        public string Payload { get; set; }
        public string ServiceName { get; set; } = string.Empty;
        public bool SentToExchange { get; set; } = false;
    }
}
