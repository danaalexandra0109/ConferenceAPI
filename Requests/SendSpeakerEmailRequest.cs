namespace ConferenceAPI.Requests
{
    public class SendSpeakerEmailRequest
    {
        public string SpeakerEmail { get; set; } = null!;
        public string ConferenceName { get; set; } = null!;
        public string Subject { get; set; } = null!;
        public required string Cc { get; set; }
    }
}
