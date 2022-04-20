using ECP.Messaging.Abstraction;

namespace ECP.Messages
{
    public class ApplicationDeleted : IntegrationEvent
    {
        public string ApplicationId { get; set; }

        public ApplicationDeleted(string applicationId)
        {
            this.ApplicationId = applicationId;
        }
    }
}
