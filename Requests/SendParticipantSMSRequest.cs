namespace ConferenceAPI.Requests
{
    public class SendParticipantSMSRequest
    {
        public string ParticipantPhoneNumber { get; set; } = null!;
        public string CourseName { get; set; } = null!;
    }
}
