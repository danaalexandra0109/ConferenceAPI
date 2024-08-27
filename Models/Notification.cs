namespace ConferenceAPI.Models
{
    public class Notification
    {
        public int Id { get; set; }

        public string Message { get; set; } = null!;

        public DateTime SentDate { get; set; }

        public string ParticipantTemplate { get; set; } = "Hello, {0}! You have been enrolled to the course {1}.";

        public string SpeakerTemplate { get; set; } = "Dear {0}, you are scheduled to speak at the conference {1} on {2}.";
    }
}
