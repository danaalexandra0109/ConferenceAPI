﻿using System;
using System.Collections.Generic;

namespace ConferenceAPI.Models;

public partial class VwConferenceXattendeeEmailXmainSpeaker
{
    public string ConferenceName { get; set; } = null!;

    public string AttendeeEmail { get; set; } = null!;

    public string SpeakerName { get; set; } = null!;
}
