namespace ConferenceAPI.Models
{
    public class Notification
    {
        public int Id { get; set; }

        public string Message { get; set; } = null!;

        public DateTime SentDate { get; set; }

        public string ParticipantTemplate = "Hello, {0}! You have been enrolled to the course {1} held by {2} on {3} at {4}, hosted at {5}.";

        public string SpeakerTemplate = "Dear {0}, you are scheduled to speak at the conference {1} on {2} at {3}, hosted at {4}.";
    }
}
