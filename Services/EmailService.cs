using System;
using System.Net;
using System.Net.Mail;
using ConferenceAPI.Models;

namespace ConferenceAPI.Services
{
    public class EmailService : INotificationService
    {
        public SmtpClient _smtpClient;
        public string _senderEmail;

        public EmailService()
        {
            // You should ideally get these from a configuration file
            _smtpClient = new SmtpClient("localhost")
            {
                Port = 25, // SMTP port number
                Credentials = new NetworkCredential("notification.system@conferences.com", "#Cookie0109"),
                EnableSsl = false,
            };

            _senderEmail = "notification.system@conferences.com"; // Your email address used as sender
        }

        public void Send(Notification notification)
        {
            // Ensure the notification is of type EmailNotification
            if (notification is EmailNotification emailNotification)
            {
                try
                {
                    // Create the MailMessage object
                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(_senderEmail),
                        Subject = emailNotification.Subject,
                        Body = emailNotification.Message,
                        IsBodyHtml = true, // Set to true if the email body contains HTML
                    };

                    // Add recipient(s)
                    mailMessage.To.Add(emailNotification.To);

                    // Add CC(s) if provided
                    if (!string.IsNullOrEmpty(emailNotification.Cc))
                    {
                        mailMessage.CC.Add(emailNotification.Cc);
                    }

                    // Send the email
                    _smtpClient.Send(mailMessage);

                    Console.WriteLine($"Email sent to: {emailNotification.To}");
                }
                catch (Exception ex)
                {
                    // Handle errors (e.g., log the exception)
                    Console.WriteLine($"Failed to send email: {ex.Message}");
                    throw;
                }
            }
            else
            {
                throw new InvalidOperationException("Notification must be of type EmailNotification.");
            }
        }
    }
}
