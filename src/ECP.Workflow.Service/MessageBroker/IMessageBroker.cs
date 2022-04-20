using ECP.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECP.Workflow.Service.MessageBroker
{
    public interface IMessageBroker
    {
        void SendLogMessage(AuditLogMessage @event);

        void SendWorkflowActivatedMessage(WorkflowActivated @event);
    }
}
