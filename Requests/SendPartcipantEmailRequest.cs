namespace ConferenceAPI.Requests
{
    public class SendParticipantEmailRequest
    {
        public string ParticipantEmail { get; set; } = null!;
        public string CourseName { get; set; } = null!;
        public string Subject { get; set; } = null!;
        public required string Cc { get; set; }
    }
}
