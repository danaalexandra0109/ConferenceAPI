using Microsoft.AspNetCore.Mvc;
using ConferenceAPI.Models;
using ConferenceAPI.Requests;
using System.Linq;
using ConferenceAPI.Data;

namespace ConferenceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConferenceController : ControllerBase
    {
        private readonly DanaBazaDeDateContext _context;

        public ConferenceController(DanaBazaDeDateContext context)
        {
            _context = context;
        }

        const int Attended = 1;
        const int Joined = 3;
        const int Withdrawn = 2;

        [HttpPost("attend")]
        public IActionResult AttendConference([FromBody] ConferenceXAttendeeRequest request)
        {
            if (request == null)
            {
                return BadRequest("Request cannot be null.");
            }

            // Validate the conference exists
            bool conferenceExists = _context.Conferences.Any(c => c.Id == request.ConferenceId);
            if (!conferenceExists)
            {
                return NotFound("Invalid conference ID.");
            }

            // Validate the status exists
            var status = _context.DictionaryStatuses.Find(request.StatusId);
            if (status == null)
            {
                return BadRequest("Invalid status ID.");
            }

            // Check if an attendance record already exists for the given attendee and conference
            var existingRecord = _context.ConferenceXattendees
                .FirstOrDefault(ca => ca.ConferenceId == request.ConferenceId && ca.AttendeeEmail == request.AttendeeEmail);

            if (existingRecord != null)
            {
                // Handle the existing record
                if (existingRecord.StatusId == Attended)
                {
                    // If status is 'Attended', return a message indicating the attendee is already marked as attended
                    return BadRequest("The attendee is already marked as attended.");
                }

                // Update the status if it is different from 'Attended'
                if (request.StatusId != Attended)
                {
                    existingRecord.StatusId = request.StatusId;
                    existingRecord.Name = request.Name;
                    existingRecord.PhoneNumber = request.PhoneNumber;
                    _context.ConferenceXattendees.Update(existingRecord);
                    _context.SaveChanges();
                    return Ok("Attendance status updated.");
                }
                else
                {
                    // Set the status to 'Attended'
                    existingRecord.StatusId = Attended;
                    existingRecord.Name = request.Name;
                    existingRecord.PhoneNumber = request.PhoneNumber;
                    _context.ConferenceXattendees.Update(existingRecord);
                    _context.SaveChanges();
                    return Ok("Attendance status updated to attended.");
                }
            }
            else
            {
                // Create a new attendance record
                var newRecord = new ConferenceXattendee
                {
                    AttendeeEmail = request.AttendeeEmail,
                    ConferenceId = request.ConferenceId,
                    StatusId = request.StatusId,
                    Name = request.Name,
                    PhoneNumber = request.PhoneNumber
                };

                _context.ConferenceXattendees.Add(newRecord);
                _context.SaveChanges();
                return Ok("Attendance recorded successfully.");
            }
        }

    }
}
//verifica data conferintei, pt attended ai nev sa se fi terminat