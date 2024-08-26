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
            var items = _context.Speakers.ToList();
            return Ok(items);
        }

        [HttpPost("PostSpeaker")]
        public ActionResult CreateSpeaker([FromBody] SpeakerRequest item)
        {
            if (item == null)
            {
                return BadRequest();
            }
            if (item.Name == null)
            {
                return BadRequest("Input Name!");
            }
            if(item.PhoneNumber == null)
            {
                return BadRequest("Input Phone Number!");
            }
            Speaker speaker = new Speaker
            {
                Name = item.Name,
                Nationality = item.Nationality,
                Rating = item.Rating,
                PhoneNumber = item.PhoneNumber,
                Email = item.Email
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
            var existingItem = _context.Speakers.FirstOrDefault(i => i.Id == id);
            if (existingItem == null)
            {
                return NotFound();
            }
            existingItem.Rating = rating;
            _context.SaveChanges();
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteSpeaker(int id)
        {
            var item = _context.Speakers.Include(i=>i.Feedbacks).Include(i=>i.ConferenceXspeakers).FirstOrDefault(i => i.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            if (item.Feedbacks.Any())
            {
// pt fiecare feedback din lista asta sterge feedback-ul
            }
            _context.Speakers.Remove(item);
            _context.SaveChanges();
            return Ok();
        }

        [HttpGet("GetSpeakerRating")]
        public ActionResult <decimal> GetRating(int id)
        {
            var item = _context.Speakers.FirstOrDefault(i => i.Id == id);
            if (item == null)
            {
                return BadRequest("Input a valid ID!");
            }
            var items = _context.Speakers.ToList(Rating);
            return Ok();
        }


    }
}
