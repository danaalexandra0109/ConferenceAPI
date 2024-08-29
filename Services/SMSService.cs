using System;
using ConferenceAPI.Models;
using Vonage;
using Vonage.Request;
using Vonage.Messaging;

namespace ConferenceAPI.Services
{
    public class SMSService : INotificationService
    {
        public void Send(Notification notification)
        {
            // Ensure the notification is of type SmsNotification
            if (notification is SmsNotification smsNotification)
            {
                var credentials = Credentials.FromApiKeyAndSecret("f1c52e7f", "qSEgfwvVVF9jrLsB");

                var vonageClient = new VonageClient(credentials);
                var response =vonageClient.SmsClient.SendAnSmsAsync(new Vonage.Messaging.SendSmsRequest()

                {

                    To = smsNotification.PhoneNumber,

                    From = "Conferences",

                    Text = smsNotification.Message

                });
            }

            else
            {
                throw new InvalidOperationException("Notification must be of type SmsNotification.");

            }
        }
    }
}

