using ConferenceAPI.Models;
using ConferenceAPI.Requests;
using Microsoft.AspNetCore.Mvc;

namespace ConferenceAPI.Services
{
    public class SMSService : INotificationService
    {
        public void Send(Notification notification)
        {
            [HttpPost("SendParticipantSmsNotification")]
            public IActionResult SendParticipantSmsNotification([FromBody] SendParticipantSmsRequest request)
            {
                var notification = new SmsNotification
                {
                    PhoneNumber = request.ParticipantPhoneNumber,
                    Message = string.Format(Notification.ParticipantTemplate, request.ParticipantPhoneNumber, request.CourseName),
                    SentDate = DateTime.Now
                };

                _manager.Send(notification);

                return Ok("Participant SMS notification sent.");
            }

            [HttpPost("SendSpeakerSmsNotification")]
            public IActionResult SendSpeakerSmsNotification([FromBody] SendSpeakerSmsRequest request)
            {
                var notification = new SmsNotification
                {
                    PhoneNumber = request.SpeakerPhoneNumber,
                    Message = string.Format(Notification.SpeakerTemplate, request.SpeakerPhoneNumber, request.ConferenceName),
                    SentDate = DateTime.Now
                };

                _manager.Send(notification);

                return Ok("Speaker SMS notification sent.");
            }
        }
    }
}
