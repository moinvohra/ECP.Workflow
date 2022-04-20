using ECP.Messaging.Abstraction;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECP.Messages
{
    public class NewUserCreatedEvent : IntegrationEvent
    {
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string ActivationUrl { get; set; }

        public NewUserCreatedEvent(string UserName
            , string FullName
            , string Displayname
            , string Email
            , string ActivarionUrl)
        {
            this.UserName = UserName;
            this.FullName = FullName;
            this.DisplayName = DisplayName;
            this.Email = Email;
            this.ActivationUrl = ActivationUrl;
        }
    }
}
