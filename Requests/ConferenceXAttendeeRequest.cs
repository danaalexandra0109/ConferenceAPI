namespace ConferenceAPI.Requests
{
    public class ConferenceXAttendeeRequest
    {
        public string AttendeeEmail { get; set; } = null!;

        public int ConferenceId { get; set; }

        public int StatusId { get; set; }

        public string? Name { get; set; }

        public string? PhoneNumber { get; set; }
    }
}
