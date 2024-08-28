using System;

namespace ConferenceAPI.Models
{
    public partial class EmailNotification : Notification
    {
        public string To { get; set; } = null!;
        public string Cc { get; set; } = null!;
        public string Subject { get; set; } = null!;

        public EmailNotification()
        {
        }
        public EmailNotification(ConferenceXattendee attendee, Conference conference)
        {
            To = attendee.AttendeeEmail;
            Subject = $"Enrollment Confirmation for {conference.Name}";
            var mainSpeaker = conference.ConferenceXspeakers
                               .Where(cs => cs.IsMainSpeaker)
                               .Select(cs => cs.Speaker)
                               .FirstOrDefault();
            if  (mainSpeaker == null)
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

        public EmailNotification(Speaker speaker, Conference conference)
        {
            To = speaker.Email;
            Subject = $"Speaking Engagement at {conference.Name}";
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
