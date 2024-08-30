namespace ConferenceAPI.Requests
{
    public class ConfXSpekXLocRequest
    {
        public int ConferenceTypeId { get; set; }

        public int LocationId { get; set; }

        public string OrganizerEmail { get; set; } = null!;

        public int CategoryId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string ConferenceName { get; set; } = null!;



        public string? LocationName { get; set; }

        public string? Code { get; set; }

        public int CountryId { get; set; }

        public string? Address { get; set; }

        public int CountyId { get; set; }

        public int CityId { get; set; }

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }


        public int ConferenceId { get; set; }
        public List<ConferenceXspeakerRequest> speakersforConference { get; set; } = new List<ConferenceXspeakerRequest>();

    }
}
