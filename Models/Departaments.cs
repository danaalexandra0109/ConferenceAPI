using System;
using System.Collections.Generic;

namespace ConferenceAPI.Models;

public partial class Departaments
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Code { get; set; }

    public string? Description { get; set; }

    public int? Employees { get; set; }
}
