using System;
using System.Collections.Generic;
using ConferenceAPI.Models;

namespace ConferenceAPI.Services
{
    public class NotificationManager
    {
        // Dictionary to map notification types to their respective services
        public readonly Dictionary<Type, INotificationService> _services;

        // Constructor to initialize the dictionary with service instances
        public NotificationManager()
        {
            _services = new Dictionary<Type, INotificationService>
            {
                { typeof(EmailNotification), new EmailService() }, // Correct service instance
                { typeof(SmsNotification), new SMSService() }      // Correct service instance
            };
        }

        // Method to send the notification using the appropriate service
        public void SendNotification(Notification notification)
        {
            var type = notification.GetType();

            if (_services.ContainsKey(type))
            {
                // Use the service to send the notification
                _services[type].Send(notification);
            }
            else
            {
                throw new NotSupportedException("Notification type not supported.");
            }
        }
    }
}
