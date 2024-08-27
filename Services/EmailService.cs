using ConferenceAPI.Models;
using ConferenceAPI.Requests;
using Microsoft.AspNetCore.Mvc;

namespace ConferenceAPI.Services
{
    public class EmailService : INotificationService
    {
        public void Send(Notification notification)
        {
            [HttpPost("SendParticipantEmailNotification")]
            public IActionResult SendParticipantEmailNotification([FromBody] SendParticipantEmailRequest request)
            {
                var notification = new EmailNotification
                {
                    To = request.ParticipantEmail,
                    Cc = request.Cc,
                    Subject = request.Subject,
                    Message = string.Format(Notification.ParticipantTemplate, request.ParticipantEmail, request.CourseName),
                    SentDate = DateTime.Now
                };

                _manager.Send(notification);

                return Ok("Participant email notification sent.");
            }

            [HttpPost("SendSpeakerEmailNotification")]
            public IActionResult SendSpeakerEmailNotification([FromBody] SendSpeakerEmailRequest request)
            {
                var notification = new EmailNotification
                {
                    To = request.SpeakerEmail,
                    Cc = request.Cc,
                    Subject = request.Subject,
                    Message = string.Format(Notification.SpeakerTemplate, request.SpeakerEmail, request.ConferenceName),
                    SentDate = DateTime.Now
                };

                _manager.Send(notification);

                return Ok("Speaker email notification sent.");
            }
        }
    }
}
