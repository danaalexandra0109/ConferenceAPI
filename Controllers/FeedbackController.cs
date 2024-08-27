using ConferenceAPI.Data;
using ConferenceAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ConferenceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly DanaBazaDeDateContext _context;

        public FeedbackController(DanaBazaDeDateContext context)
        {
            _context = context;
        }

        [HttpGet("GetFeedback")]
        public ActionResult GetAllItems()
        {
            List<Feedback> feedbacks = _context.Feedbacks.ToList();
            return Ok(feedbacks);
        }

        [HttpPost("PostFeedback")]
        public ActionResult CreateFeedback([FromBody] FeedbackRequest feedbackReq)
        {
            if (feedbackReq == null)
            {
                return BadRequest("Please enter feedback!");
            }
            if (string.IsNullOrWhiteSpace(feedbackReq.AttendeeEmail))
            {
                return BadRequest("Email required!");
            }
            if (string.IsNullOrWhiteSpace(feedbackReq.SpeakerId.ToString()))
            {
                return BadRequest("Please input a Speaker!");
            }
            if (string.IsNullOrWhiteSpace(feedbackReq.ConferenceId.ToString()))
            {
                return BadRequest("Please input a Conference!");
            }
            if (feedbackReq.Rating == null)
            {
                return BadRequest("Please input your feedback rating for our speaker!");
            }
            if (string.IsNullOrWhiteSpace(feedbackReq.Message))
            {
                return BadRequest("Please help us improve!");
            }
            var conferenceExists = _context.Conferences.Any(c => c.Id == feedbackReq.ConferenceId);
            if (!conferenceExists)
            {
                return BadRequest("The selected conference does not exist.");
            }
            var speakerExists = _context.Speakers.Any(s => s.Id == feedbackReq.SpeakerId);
            if (!speakerExists)
            {
                return BadRequest("The selected speaker does not exist.");
            }
            var existingFeedback = _context.Feedbacks.FirstOrDefault(f =>
                f.AttendeeEmail == feedbackReq.AttendeeEmail &&
                f.ConferenceId == feedbackReq.ConferenceId &&
                f.SpeakerId == feedbackReq.SpeakerId);
            if (existingFeedback != null)
            {
                return Conflict("Your feedback was already registered, thank you!");
            }
            Feedback feedback = new Feedback
            {
                AttendeeEmail = feedbackReq.AttendeeEmail,
                ConferenceId = feedbackReq.ConferenceId,
                SpeakerId = feedbackReq.SpeakerId,
                Rating = feedbackReq.Rating,
                Message = feedbackReq.Message
            };
            _context.Feedbacks.Add(feedback);
            _context.SaveChanges();
            var speakerFeedbacks = _context.Feedbacks.Where(f => f.SpeakerId == feedbackReq.SpeakerId).ToList();
            if (speakerFeedbacks.Any())
            {
                var newAverageRating = speakerFeedbacks.Average(f => f.Rating);

                var speaker = _context.Speakers.FirstOrDefault(s => s.Id == feedbackReq.SpeakerId);
                if (speaker != null)
                {
                    speaker.Rating = newAverageRating;
                    _context.SaveChanges();
                }
            }
            return Created(Url.ToString(), feedback);
        }

        [HttpGet("GetFeedbackbyRating")]
        public ActionResult GetAllFeedback()
        {
         var conferenceRatings = _context.Conferences.Select(c => new
        {
            ConferenceId = c.Id,
            ConferenceName = c.Name,
            AverageRating = _context.Feedbacks
                .Where(f => f.ConferenceId == c.Id)
                .Average(f => (double?)f.Rating) ?? 0, 
            NumberOfFeedbacks = _context.Feedbacks
                .Count(f => f.ConferenceId == c.Id)  
        })
        .OrderByDescending(c => c.AverageRating)
        .ThenByDescending(c => c.NumberOfFeedbacks) 
        .ToList();

            return Ok(conferenceRatings);
        }

        [HttpGet("GetFeedbackofAttendee")]
        public ActionResult GetAllFeedbacksofAttendee(string attendeeEmail)
        {
            var feedbacks = _context.Feedbacks.Where(f => f.AttendeeEmail == attendeeEmail).ToList(); 
            if (feedbacks == null || !feedbacks.Any())
            {
                return NotFound("No feedbacks found for the specified attendee.");
            }
            return Ok(feedbacks);
        }
    }
}
