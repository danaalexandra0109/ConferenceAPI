namespace ConferenceAPI.Requests
{
    public class ConferenceRequest
    {
        public int ConferenceTypeId { get; set; }

        public int LocationId { get; set; }

        public string OrganizerEmail { get; set; } = null!;

        public int CategoryId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Name { get; set; } = null!;
    }
}
