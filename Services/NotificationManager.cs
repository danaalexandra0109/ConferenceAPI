using System;
using System.Collections.Generic;
using ConferenceAPI.Models;

namespace ConferenceAPI.Services
{
    public class NotificationManager
    {
        // A readonly dictionary to store the notification services
        private readonly Dictionary<Type, INotificationService> _services;

        // Constructor to initialize the dictionary with available services
        public NotificationManager()
        {
            _services = new Dictionary<Type, INotificationService>
    {
        { typeof(EmailNotification), new EmailService() },
        { typeof(SmsNotification), new SMSService() }
    };
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<INotificationService, EmailService>();
            services.AddTransient<INotificationService, SMSService>();
            services.AddSingleton<NotificationManager>();
        }


        // Method to send a notification based on its type
        public void SendNotification(Notification notification)
        {
            var type = notification.GetType();
            if (_services.ContainsKey(type))
            {
                _services[type].Send(notification);
            }
            else
            {
                throw new NotSupportedException("Notification type not supported.");
            }
        }
    }
}
