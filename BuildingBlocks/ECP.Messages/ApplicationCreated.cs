using ECP.Messaging.Abstraction;

namespace ECP.Messages
{
    public class ApplicationCreated : IntegrationEvent
    {
        public string ApplicationId { get; set; }
        public string ApplicationName { get; set; }

        public ApplicationCreated(string applicationId, string applicationName)
        {
            this.ApplicationId = applicationId;
            this.ApplicationName = applicationName;
        }
    }
}
