using System;

namespace ConferenceAPI.Models
{
    public partial class SmsNotification : Notification
    {
        public string PhoneNumber { get; set; } = null!;

        public SmsNotification()
        {
        }
        public SmsNotification(ConferenceXattendee attendee, Conference conference)
        {
            PhoneNumber = attendee.PhoneNumber;
            var mainSpeaker = conference.ConferenceXspeakers
                               .Where(cs => cs.IsMainSpeaker)
                               .Select(cs => cs.Speaker)
                               .FirstOrDefault();
            if (mainSpeaker == null)
            {
                mainSpeaker = conference.ConferenceXspeakers
                    .Select(cs => cs.Speaker)
                    .OrderByDescending(s => s.Rating)
                    .FirstOrDefault();
            }

            Message = string.Format(ParticipantTemplate,
                                   attendee.Name,
                                   conference.Name,
                                   mainSpeaker.Name,
                                   conference.StartDate.ToShortDateString(),
                                   conference.StartDate.ToShortTimeString(),
                                   conference.Location.Name);
            SentDate = DateTime.Now;
        }

        public SmsNotification(Speaker speaker, Conference conference)
        {
            PhoneNumber = speaker.PhoneNumber;
            Message = string.Format(SpeakerTemplate,
                                    speaker.Name,
                                    conference.Name,
                                    conference.StartDate.ToShortDateString(),
                                    conference.StartDate.ToShortTimeString(),
                                    conference.Location.Name);
            SentDate = DateTime.Now;
        }
    }
}