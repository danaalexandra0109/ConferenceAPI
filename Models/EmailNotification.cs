using System;

namespace ConferenceAPI.Models
{
    public partial class EmailNotification : Notification
    {
        public string To { get; set; } = null!;
        public string Cc { get; set; } = null!;
        public string Subject { get; set; } = null!;

        public EmailNotification(string participantName, string courseName, DateTime sentDate, string to, string cc, string subject)
        {
            Message = string.Format(ParticipantTemplate, participantName, courseName);
            SentDate = sentDate;

            To = to;
            Cc = cc;
            Subject = subject;
        }

        public EmailNotification(string speakerName, string conferenceName, DateTime conferenceDate, DateTime sentDate, string to, string cc, string subject)
        {
            Message = string.Format(SpeakerTemplate, speakerName, conferenceName, conferenceDate.ToShortDateString());
            SentDate = sentDate;

            To = to;
            Cc = cc;
            Subject = subject;
        }

    }
}
