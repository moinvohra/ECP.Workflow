using ECP.Messages;
using ECP.Workflow.Model;
using ECP.Workflow.Model.Utility;
using ECP.Workflow.Service;
using ECP.Workflow.Service.MessageBroker;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ECP.Workflow.EventPublisher
{
    public class WorkflowEventPublisher : BackgroundService
    {
        
        /// <summary>
        /// logger object
        /// </summary>
        private readonly ILogger<WorkflowEventPublisher> _logger;

        
        /// <summary>
        /// Message broker Object
        /// </summary>
        private readonly IMessageBroker _messageBroker;


        /// <summary>
        /// Transaction queue object
        /// </summary>
       private readonly IOutboundTransactionQueueService _outboundTransactionQueueService;



        public WorkflowEventPublisher(ILogger<WorkflowEventPublisher> logger
             , IOutboundTransactionQueueService outboundTransactionQueueService
            , IMessageBroker messageBroker
             )
        {
            _logger = logger;
            _messageBroker = messageBroker;
            _outboundTransactionQueueService = outboundTransactionQueueService;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Workflow background task is starting.");
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Workflow background task is stopping.");
            return base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Workflow Activated background task has started execution.");

            while (!stoppingToken.IsCancellationRequested)
            {
                

                List<TransactionQueueOutbound> retval  = await _outboundTransactionQueueService.GetDetails();

                foreach (var item in retval)
                {
                    
                    if (item.EventType.ToLower().Equals(General.WORKFLOWACTIVATED.ToString().ToLower()))
                    {
                        _logger.LogInformation($"Getting activated event {item.TransactionQueueOutboundId}");
                        var messageData = JsonConvert.DeserializeObject<WorkflowActivated>(item.Payload);

                        //  Send the message to events
                        _messageBroker.SendWorkflowActivatedMessage(messageData);
                    }

                    //Update the status of the queue
                   await _outboundTransactionQueueService.UpdateQueueOutBoundStatus(item.TransactionQueueOutboundId);
                  
                }
                await Task.Delay(1500);
            }
        }
    }
}