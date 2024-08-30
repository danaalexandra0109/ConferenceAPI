using Microsoft.AspNetCore.Mvc;
using ConferenceAPI.Models;
using ConferenceAPI.Requests;
using System.Linq;
using ConferenceAPI.Data;
using Azure.Core;

namespace ConferenceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConferenceController : ControllerBase
    {
        public DanaBazaDeDateContext _context;

        public ConferenceController(DanaBazaDeDateContext context)
        {
            _context = context;
        }

        [HttpPost("PostConference")]
        public ActionResult AddConferenceandSpeakersLocation([FromBody] ConfXSpekXLocRequest request)
        {
            if (request == null)
            {
                return BadRequest("Please input information!");
            }

            if (request.ConferenceTypeId != 1 || request.ConferenceId != 2)
            {
                return BadRequest("Invalid Conference Type");
            }

            if (string.IsNullOrEmpty(request.OrganizerEmail))
            {
                return BadRequest("Enter email!");
            }

            if (string.IsNullOrEmpty(request.ConferenceName))
            {
                return BadRequest("Enter Conference Name");
            }

            if (string.IsNullOrEmpty(request.LocationName))
            {
                return BadRequest("Enter Location Name");
            }

            if (string.IsNullOrEmpty(request.Code))
            {
                return BadRequest("Enter Location Code");
            }

            if (string.IsNullOrEmpty(request.Address))
            {
                return BadRequest("Enter Address");
            }

            // Check if the location exists
            bool locationExists = _context.Locations.Any(l => l.Id == request.LocationId);

            if (!locationExists)
            {
                // Validate country, county, and city existence
                bool countryExists = _context.DictionaryCountries.Any(c => c.Id == request.CountryId);
                if (!countryExists)
                {
                    return BadRequest("Invalid Country!");
                }

                bool countyExists = _context.DictionaryCounties.Any(c => c.Id == request.CountyId);
                if (!countyExists)
                {
                    return BadRequest("Invalid County!");
                }

                bool cityExists = _context.DictionaryCities.Any(c => c.Id == request.CityId);
                if (!cityExists)
                {
                    return BadRequest("Invalid City!");
                }

                // Add new location
                var location = new Location
                {
                    Name = request.LocationName,
                    Code = request.Code,
                    CountryId = request.CountryId,
                    Address = request.Address,
                    CountyId = request.CountyId,
                    CityId = request.CityId,
                    Latitude = request.Latitude,
                    Longitude = request.Longitude
                };

                _context.Locations.Add(location);
                _context.SaveChanges();

                request.LocationId = location.Id; // Update the request with the newly created LocationId
            }

            // Check if a conference at the same location already exists within the same date range
            bool conflictingConferenceExists = _context.Conferences.Any(c =>
                c.LocationId == request.LocationId &&
                (
                    (request.StartDate >= c.StartDate && request.StartDate <= c.EndDate) ||
                    (request.EndDate >= c.StartDate && request.EndDate <= c.EndDate) ||
                    (request.StartDate <= c.StartDate && request.EndDate >= c.EndDate)
                ));

            if (conflictingConferenceExists)
            {
                return Conflict("A conference is already scheduled at the same location during the selected dates.");
            }

            // Check if the conference already exists
            bool conferenceExists = _context.Conferences.Any(c =>
                c.Name == request.ConferenceName &&
                c.StartDate == request.StartDate &&
                c.EndDate == request.EndDate &&
                c.CategoryId == request.CategoryId &&
                c.OrganizerEmail == request.OrganizerEmail &&
                c.ConferenceTypeId == request.ConferenceTypeId &&
                c.LocationId == request.LocationId);

            if (conferenceExists)
            {
                return Conflict("Conference already exists!");
            }

            // Validate conference type and category existence
            bool typeExists = _context.DictionaryConferenceTypes.Any(c => c.Id == request.ConferenceTypeId);
            if (!typeExists)
            {
                return BadRequest("Invalid type!");
            }

            bool categoryExists = _context.DictionaryCategories.Any(c => c.Id == request.CategoryId);
            if (!categoryExists)
            {
                return BadRequest("Invalid category!");
            }

            // Add new conference
            var conference = new Conference
            {
                ConferenceTypeId = request.ConferenceTypeId,
                LocationId = request.LocationId,
                OrganizerEmail = request.OrganizerEmail,
                CategoryId = request.CategoryId,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Name = request.ConferenceName
            };

            _context.Conferences.Add(conference);
            _context.SaveChanges();

            // Add speakers to the conference
            foreach (ConferenceXspeakerRequest speakerRequest in request.speakersforConference)
            {
                bool speakerExists = _context.Speakers.Any(s => s.Id == speakerRequest.SpeakerId);
                if (!speakerExists)
                {
                    return NotFound($"Speaker with ID {speakerRequest.SpeakerId} not found!");
                }

                // Check if the speaker is already scheduled for another conference during the same dates
                bool speakerConflictExists = _context.ConferenceXspeakers.Any(cs =>
                    cs.SpeakerId == speakerRequest.SpeakerId &&
                    _context.Conferences.Any(c =>
                        c.Id == cs.ConferenceId &&
                        (
                            (request.StartDate >= c.StartDate && request.StartDate <= c.EndDate) ||
                            (request.EndDate >= c.StartDate && request.EndDate <= c.EndDate) ||
                            (request.StartDate <= c.StartDate && request.EndDate >= c.EndDate)
                        )));

                if (speakerConflictExists)
                {
                    return Conflict($"Speaker with ID {speakerRequest.SpeakerId} is already scheduled for another conference during the selected dates.");
                }

                var conferenceXSpeaker = new ConferenceXspeaker
                {
                    ConferenceId = conference.Id,
                    SpeakerId = speakerRequest.SpeakerId,
                    IsMainSpeaker = speakerRequest.IsMainSpeaker
                };

                _context.ConferenceXspeakers.Add(conferenceXSpeaker);
            }

            _context.SaveChanges();

            return Ok("Conference and speakers added successfully!");
        }




        [HttpPost("Attend")]
        public ActionResult Attend([FromBody] ConferenceXAttendeeRequest attendeeRequest)
        {
            if (attendeeRequest == null)
            {
                return BadRequest("Please input!");
            }

            int StatusId = 1;

            if(attendeeRequest.AttendeeEmail == null)
            {
                return BadRequest("Please, input email!");
            }

            bool conferenceExists = _context.Conferences.Any(c => c.Id == attendeeRequest.ConferenceId);
            if (!conferenceExists)
            {
                return BadRequest("The specified ConferenceId does not exist.");
            }

            Conference conference = _context.Conferences.FirstOrDefault(c => c.Id == attendeeRequest.ConferenceId);
            if (conference == null)
            {
                return BadRequest("Conference not found.");
            }

            // Check if the conference has already started
            if (conference.StartDate < DateTime.Today)
            {
                return BadRequest("Conference has already started.");
            }

            // Check if the conference has already ended
            if (conference.EndDate < DateTime.Today)
            {
                return BadRequest("Conference has already ended.");
            }

            if (attendeeRequest.Name == null)
            {
                return BadRequest("Please input name!");
            }

            if(attendeeRequest.PhoneNumber == null)
            {
                return BadRequest("Please input phone number!");
            }

            var existingAttendee = _context.ConferenceXattendees
        .FirstOrDefault(a => a.AttendeeEmail == attendeeRequest.AttendeeEmail
                          && a.ConferenceId == attendeeRequest.ConferenceId
                          && a.Name == attendeeRequest.Name
                          && a.PhoneNumber == attendeeRequest.PhoneNumber);


            if (existingAttendee != null)
            {
                // Check if StatusId is not 1, and update it if needed
                if (existingAttendee.StatusId != 1)
                {
                    existingAttendee.StatusId = StatusId;
                    _context.SaveChanges();
                    return Ok("Status updated!");
                }

                // If StatusId is already 1, return a message indicating no change was necessary
                return Ok("Attendee already exists with Attended Status.");
            }


            var attendee = new ConferenceXattendee
            {
                AttendeeEmail = attendeeRequest.AttendeeEmail,
                ConferenceId = attendeeRequest.ConferenceId,
                StatusId = StatusId,
                Name = attendeeRequest.Name,
                PhoneNumber = attendeeRequest.PhoneNumber
            };
            _context.ConferenceXattendees.Add(attendee);
            _context.SaveChanges();
            return Ok(attendee);
        }

        [HttpPost("Withdraw")]
        public ActionResult Withdraw([FromBody] ConferenceXAttendeeRequest attendeeRequest)
        {
            if (attendeeRequest == null)
            {
                return BadRequest("Please input!");
            }

            int StatusId = 2;

            if (attendeeRequest.AttendeeEmail == null)
            {
                return BadRequest("Please, input email!");
            }

            bool conferenceExists = _context.Conferences.Any(c => c.Id == attendeeRequest.ConferenceId);
            if (!conferenceExists)
            {
                return BadRequest("The specified ConferenceId does not exist.");
            }

            var conference = _context.Conferences.FirstOrDefault(c => c.Id == attendeeRequest.ConferenceId);
            if (conference == null)
            {
                return BadRequest("Conference not found.");
            }

            // Check if the conference has already ended
            if (conference.EndDate < DateTime.Today)
            {
                return BadRequest("Conference has already ended.");
            }

            if (attendeeRequest.Name == null)
            {
                return BadRequest("Please input name!");
            }

            if (attendeeRequest.PhoneNumber == null)
            {
                return BadRequest("Please input phone number!");
            }

            var existingAttendee = _context.ConferenceXattendees
        .FirstOrDefault(a => a.AttendeeEmail == attendeeRequest.AttendeeEmail
                          && a.ConferenceId == attendeeRequest.ConferenceId
                          && a.Name == attendeeRequest.Name
                          && a.PhoneNumber == attendeeRequest.PhoneNumber);


            if (existingAttendee != null)
            {
                // Check if StatusId is not 1, and update it if needed
                if (existingAttendee.StatusId != 1)
                {
                    existingAttendee.StatusId = StatusId;
                    _context.SaveChanges();
                    return Ok("Status updated!");
                }

                // If StatusId is already 1, return a message indicating no change was necessary
                return Ok("Attendee already exists with Withdrawn Status.");
            }


            var attendee = new ConferenceXattendee
            {
                AttendeeEmail = attendeeRequest.AttendeeEmail,
                ConferenceId = attendeeRequest.ConferenceId,
                StatusId = StatusId,
                Name = attendeeRequest.Name,
                PhoneNumber = attendeeRequest.PhoneNumber
            };
            _context.ConferenceXattendees.Add(attendee);
            _context.SaveChanges();
            return Ok(attendee);
        }

        [HttpPost("Join")]
        public ActionResult Join([FromBody] ConferenceXAttendeeRequest attendeeRequest)
        {
            if (attendeeRequest == null)
            {
                return BadRequest("Please input!");
            }

            int StatusId = 3;
            int attend = 1;

            if (attendeeRequest.AttendeeEmail == null)
            {
                return BadRequest("Please, input email!");
            }

            bool conferenceExists = _context.Conferences.Any(c => c.Id == attendeeRequest.ConferenceId);
            if (!conferenceExists)
            {
                return BadRequest("The specified ConferenceId does not exist.");
            }

            var conference = _context.Conferences.FirstOrDefault(c => c.Id == attendeeRequest.ConferenceId);
            if (conference == null)
            {
                return BadRequest("The specified ConferenceId does not exist.");
            }

            // Check if the current date is before the conference's start date
            if (DateTime.Now < conference.StartDate)
            {
                return BadRequest("Conference is pending.");
            }

            // Check if the current date is after the conference's end date
            if (DateTime.Now > conference.EndDate)
            {
                return BadRequest("Conference has already ended.");
            }

            if (attendeeRequest.Name == null)
            {
                return BadRequest("Please input name!");
            }

            if (attendeeRequest.PhoneNumber == null)
            {
                return BadRequest("Please input phone number!");
            }

            var existingAttendee = _context.ConferenceXattendees
        .FirstOrDefault(a => a.AttendeeEmail == attendeeRequest.AttendeeEmail
                          && a.ConferenceId == attendeeRequest.ConferenceId
                          && a.Name == attendeeRequest.Name
                          && a.PhoneNumber == attendeeRequest.PhoneNumber);


            if (existingAttendee != null)
            {
                // Check if StatusId is not 1, and update it if needed
                if (existingAttendee.StatusId == attend)
                {
                    existingAttendee.StatusId = StatusId;
                    _context.SaveChanges();
                    return Ok("Status updated!");
                }

                // If StatusId is already 1, return a message indicating no change was necessary
                return Ok("Attendee already exists with Joined Status.");
            }


            var attendee = new ConferenceXattendee
            {
                AttendeeEmail = attendeeRequest.AttendeeEmail,
                ConferenceId = attendeeRequest.ConferenceId,
                StatusId = StatusId,
                Name = attendeeRequest.Name,
                PhoneNumber = attendeeRequest.PhoneNumber
            };
            _context.ConferenceXattendees.Add(attendee);
            _context.SaveChanges();
            return Ok(attendee);
        }


    }
}