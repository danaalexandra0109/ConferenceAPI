using ConferenceAPI.Data;
using ConferenceAPI.Models;
using ConferenceAPI.Requests;
using ConferenceAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConferenceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        DanaBazaDeDateContext _context;
        NotificationManager _manager;

        public NotificationController(DanaBazaDeDateContext context)
        {
            _context = context;
            _manager = new NotificationManager();
        }

        [HttpPost("SendParticipantEmailNotification")]
        public IActionResult SendParticipantEmailNotification([FromBody] NotificationRequest request)
        {
            var participant = _context.ConferenceXattendees.FirstOrDefault(a => a.Id == request.ReciverId);
            var conference = _context.Conferences.Include(c=> c.Location).Include(cs=> cs.ConferenceXspeakers).ThenInclude(s=> s.Speaker).FirstOrDefault(c => c.Id == request.ConferenceId);
            var mainSpeaker = _context.ConferenceXspeakers
                                .Where(cxs => cxs.ConferenceId == request.ConferenceId && cxs.IsMainSpeaker)
                                .Select(cxs => cxs.Speaker)
                                .FirstOrDefault();

            if (participant == null || conference == null || mainSpeaker == null)
            {
                return BadRequest("Invalid participant, conference, or speaker information.");
            }

            var emailNotification = new EmailNotification(participant, conference);

            _manager.SendNotification(emailNotification);
            return Ok("Participant email notification sent successfully.");
        }

        [HttpPost("SendSpeakerEmailNotification")]
        public IActionResult SendSpeakerEmailNotification([FromBody] NotificationRequest request)
        {
            var speaker = _context.Speakers.FirstOrDefault(s => s.Id == request.ReciverId);
            var conference = _context.Conferences.Include(c => c.Location).FirstOrDefault(c => c.Id == request.ConferenceId);

            if (speaker == null || conference == null)
            {
                return BadRequest("Invalid speaker or conference information.");
            }

            var emailNotification = new EmailNotification(speaker, conference);

            _manager.SendNotification(emailNotification);
            return Ok("Speaker email notification sent successfully.");
        }


    }
}
