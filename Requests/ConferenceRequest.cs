using System;
using System.Collections.Generic;

namespace ConferenceAPI.Models;

public partial class ConferenceRequest
{
    public int ConferenceTypeId { get; set; }

    public int LocationId { get; set; }

    public string OrganizerEmail { get; set; } = null!;

    public int CategoryId { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public string Name { get; set; } = null!;
}
