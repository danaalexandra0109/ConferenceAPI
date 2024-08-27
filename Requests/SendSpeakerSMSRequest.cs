namespace ConferenceAPI.Requests
{
    public class SendSpeakerSMSRequest
    {
        public string SpeakerPhoneNumber { get; set; } = null!;
        public string ConferenceName { get; set; } = null!;
    }
}
