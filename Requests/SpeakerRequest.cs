using System;
using System.Collections.Generic;

namespace ConferenceAPI.Models;

public partial class SpeakerRequest
{


    public string? Name { get; set; }

    public string? Nationality { get; set; }

    public decimal? Rating { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

}
