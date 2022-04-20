using ECP.Messaging.Abstraction;

namespace ECP.Messages
{
    public class ApplicationUpdated : IntegrationEvent
    {
        public string ApplicationId { get; set; }
        public string ApplicationName { get; set; }

        public ApplicationUpdated(string applicationId, string applicationName)
        {
            this.ApplicationId = applicationId;
            this.ApplicationName = applicationName;
        }
    }
}
