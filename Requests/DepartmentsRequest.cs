using System;
using System.Collections.Generic;

namespace ConferenceAPI.Models;

public partial class DepartamentsRequest
{

    public string? Name { get; set; }

    public string? Code { get; set; }

    public string? Description { get; set; }

    public int? Employees { get; set; }
}
