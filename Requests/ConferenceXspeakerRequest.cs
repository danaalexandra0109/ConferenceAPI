using System;
using System.Collections.Generic;

namespace ConferenceAPI.Models;

public partial class ConferenceXspeakerRequest
{
    public int ConferenceId { get; set; }

    public int SpeakerId { get; set; }

}
