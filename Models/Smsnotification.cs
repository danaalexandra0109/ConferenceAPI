using System;

namespace ConferenceAPI.Models
{
    public partial class SmsNotification : Notification
    {
        public string PhoneNumber { get; set; } = null!;

        public SmsNotification(string participantName, string courseName, DateTime sentDate, string phoneNumber)
        {
            Message = string.Format(ParticipantTemplate, participantName, courseName);
            SentDate = sentDate;

            PhoneNumber = phoneNumber;
        }

        public SmsNotification(string speakerName, string conferenceName, DateTime conferenceDate, DateTime sentDate, string phoneNumber)
        {
            Message = string.Format(SpeakerTemplate, speakerName, conferenceName, conferenceDate.ToShortDateString());
            SentDate = sentDate;

            PhoneNumber = phoneNumber;
        }
    }
}
