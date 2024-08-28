using ConferenceAPI.Models;
using ConferenceAPI.Requests;
using Microsoft.AspNetCore.Mvc;

namespace ConferenceAPI.Services
{
    public class SMSService : INotificationService
    {
        public void Send(Notification notification)
        {
            // Ensure the notification is of type SmsNotification
            if (notification is SmsNotification smsNotification)
            {
                // Here, you would implement the logic to send the SMS
                // For now, we'll just simulate it with a console output or log

                Console.WriteLine($"Sending SMS to: {smsNotification.PhoneNumber}");
                Console.WriteLine($"Message: {smsNotification.Message}");
            }
            else
            {
                throw new InvalidOperationException("Notification must be of type SmsNotification.");
            }
        }
    }
}

