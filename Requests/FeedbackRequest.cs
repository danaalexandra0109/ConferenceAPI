using System;
using System.Collections.Generic;

namespace ConferenceAPI.Models;

public partial class FeedbackRequest
{
    public string? AttendeeEmail { get; set; }

    public int ConferenceId { get; set; }

    public int SpeakerId { get; set; }

    public decimal? Rating { get; set; }

    public string? Message { get; set; }
}
