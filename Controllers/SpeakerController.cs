using ConferenceAPI.Data;
using ConferenceAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConferenceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpeakerController : ControllerBase
    {
        public DanaBazaDeDateContext _context = new DanaBazaDeDateContext();
        public SpeakerController( DanaBazaDeDateContext context)
        {
            _context = context;
        }

        [HttpGet("GetSpeaker")] 
        public ActionResult GetAllItems()
        {
            var speakers = _context.Speakers.ToList();
            return Ok(speakers);
        }

        [HttpPost("PostSpeaker")]
        public ActionResult CreateSpeaker([FromBody] SpeakerRequest speakerReq)
        {
            if (speakerReq == null)
            {
                return BadRequest();
            }
            if (speakerReq.Name == null)
            {
                return BadRequest("Input Name!");
            }
            if(speakerReq.PhoneNumber == null)
            {
                return BadRequest("Input Phone Number!");
            }
            var existingSpeaker = _context.Speakers.FirstOrDefault(s => s.PhoneNumber == speakerReq.PhoneNumber);
            if (existingSpeaker != null)
            {
                return Conflict("Phone number already in use!");
            }
            existingSpeaker = _context.Speakers.FirstOrDefault(s => s.Email == speakerReq.Email);
            if (existingSpeaker != null)
            {
                return Conflict("Email already in use!");
            }
            Speaker speaker = new Speaker
            {
                Name = speakerReq.Name,
                Nationality = speakerReq.Nationality,
                Rating = speakerReq.Rating,
                PhoneNumber = speakerReq.PhoneNumber,
                Email = speakerReq.Email
            };
            _context.Speakers.Add(speaker);
            _context.SaveChanges();
            return Created(Url.ToString(),speaker);
        }

        [HttpPut("{Id}")]
        public ActionResult UpdateSpeaker(int id,  decimal rating)
        {
            if (id == null)
            {
                return BadRequest("Speaker not found");
            }
            if(rating == null)
            {
                return BadRequest("Input Rating!");
            }
            if(rating<1 || rating > 5)
            {
                return BadRequest("Input a valid value!");
            }
            var existingSpeaker = _context.Speakers.FirstOrDefault(i => i.Id == id);
            if (existingSpeaker == null)
            {
                return NotFound();
            }
            existingSpeaker.Rating = rating;
            _context.SaveChanges();
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteSpeaker(int id)
        {
            var speaker = _context.Speakers.Include(i=>i.Feedbacks).Include(i=>i.ConferenceXspeakers).FirstOrDefault(i => i.Id == id);
            if (speaker == null)
            {
                return NotFound();
            }
            if (speaker.Feedbacks.Any())
            {
                foreach (var feedback in _context.Feedbacks)
                {
                    _context.Feedbacks.Remove(feedback);
                }
            }
            if (speaker.ConferenceXspeakers.Any())
            {
                foreach (var conferencexspeakers in _context.ConferenceXspeakers)
                {
                    _context.ConferenceXspeakers.Remove(conferencexspeakers);
                }
            }
            _context.Speakers.Remove(speaker);
            _context.SaveChanges();
            return Ok();
        }

        [HttpGet("GetSpeakerRating")]
        public ActionResult <decimal> GetRating(int id)
        {
            var speaker = _context.Speakers.FirstOrDefault(i => i.Id == id);
            if (speaker == null)
            {
                return BadRequest("Input a valid ID!");
            }
            return Ok(speaker.Rating);
        }
    }
}
