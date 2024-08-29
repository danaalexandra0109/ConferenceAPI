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




//public void Send(Notification notification)
//{
//    // Ensure the notification is of type EmailNotification
//    if (notification is EmailNotification emailNotification)
//    {
//        try
//        {
//            // Create the MailMessage object
//            var mailMessage = new MailMessage
//            {
//                From = new MailAddress(_senderEmail),
//                Subject = emailNotification.Subject,
//                Body = emailNotification.Message,
//                IsBodyHtml = true, // Set to true if the email body contains HTML
//            };

//            // Add recipient(s)
//            mailMessage.To.Add(emailNotification.To);

//            // Add CC(s) if provided
//            if (!string.IsNullOrEmpty(emailNotification.Cc))
//            {
//                mailMessage.CC.Add(emailNotification.Cc);
//            }

//            // Send the email
//            _smtpClient.Send(mailMessage);

//            Console.WriteLine($"Email sent to: {emailNotification.To}");
//        }
//        catch (Exception ex)
//        {
//            // Handle errors (e.g., log the exception)
//            Console.WriteLine($"Failed to send email: {ex.Message}");
//            throw;
//        }
//    }
//    else
//    {
//        throw new InvalidOperationException("Notification must be of type EmailNotification.");
//    }
//}