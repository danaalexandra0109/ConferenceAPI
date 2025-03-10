﻿using ConferenceAPI.Data;
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

            if (participant == null) 
            {
                return BadRequest("Invalid participant.");
            }

            if(conference == null)
            {
                return BadRequest("Invalid conference.");
            }

            var conferenceExists = _context.Conferences.Any(c => c.Id == request.ConferenceId);
            if (!conferenceExists)
            {
                return BadRequest("Invalid conference ID.");
            }

            if (mainSpeaker == null)
            {
                return BadRequest("No speaker for conference");
            }

            var emailNotification = new EmailNotification(participant, conference);

            _context.EmailNotifications.Add(emailNotification);
            _context.SaveChanges();

            _manager.SendNotification(emailNotification);
            return Ok("Participant email notification sent successfully.");


            
        }

        [HttpPost("SendSpeakerEmailNotification")]
        public IActionResult SendSpeakerEmailNotification([FromBody] NotificationRequest request)
        {
            var speaker = _context.Speakers.FirstOrDefault(s => s.Id == request.ReciverId);
            var conference = _context.Conferences.Include(c => c.Location).FirstOrDefault(c => c.Id == request.ConferenceId);

            if (conference == null)
            {
                return BadRequest("Invalid conference information.");
            }

            // Check if speaker is null or has not attended the conference
            if (speaker == null)
            {
                return BadRequest("Invalid speaker.");
            }

            var hasAttended = _context.ConferenceXspeakers.Any(a => a.ConferenceId == conference.Id && a.SpeakerId == speaker.Id);

            if (!hasAttended)
            {
                return NotFound("Speaker has't attended that conference!");
            }

            //if (!hasAttended)
            //{
            //    var newSpeaker = new ConferenceXspeaker
            //    {
            //        SpeakerId = speaker.Id,
            //        ConferenceId = conference.Id
            //    };

            //    _context.ConferenceXspeakers.Add(newSpeaker);
            //    _context.SaveChanges(); // Save changes to the database
            //}


            var emailNotification = new EmailNotification(speaker, conference);


            _context.EmailNotifications.Add(emailNotification);
            _context.SaveChanges();

            _manager.SendNotification(emailNotification);
            return Ok("Speaker email notification sent successfully.");
        }


        [HttpPost("SendParticipantSmsNotification")]
        public IActionResult SendParticipantSmsNotification([FromBody] NotificationRequest request)
        {
            var participant = _context.ConferenceXattendees.FirstOrDefault(a => a.Id == request.ReciverId);
            var conference = _context.Conferences.Include(c => c.Location).Include(cs => cs.ConferenceXspeakers).ThenInclude(s => s.Speaker).FirstOrDefault(c => c.Id == request.ConferenceId);
            var mainSpeaker = _context.ConferenceXspeakers
                                .Where(cxs => cxs.ConferenceId == request.ConferenceId && cxs.IsMainSpeaker)
                                .Select(cxs => cxs.Speaker)
                                .FirstOrDefault();

            if (participant == null)
            {
                return BadRequest("Invalid participant.");
            }

            if (conference == null)
            {
                return BadRequest("Invalid conference.");
            }

            var conferenceExists = _context.Conferences.Any(c => c.Id == request.ConferenceId);
            if (!conferenceExists)
            {
                return BadRequest("Invalid conference ID.");
            }

            if (mainSpeaker == null)
            {
                return BadRequest("No speaker for conference");
            }

            var smsNotification = new SmsNotification(participant, conference);

            _context.Smsnotifications.Add(smsNotification);
            _context.SaveChanges();

            _manager.SendNotification(smsNotification);
            return Ok("Participant sms notification sent successfully.");

            //adauga metoda de adaugare in tabela EmailNotification
        }

        [HttpPost("SendSpeakerSmsNotification")]
        public IActionResult SendSpeakerSmsNotification([FromBody] NotificationRequest request)
        {
            var speaker = _context.Speakers.FirstOrDefault(s => s.Id == request.ReciverId);
            var conference = _context.Conferences.Include(c => c.Location).FirstOrDefault(c => c.Id == request.ConferenceId);

            if (conference == null)
            {
                return BadRequest("Invalid conference information.");
            }

            // Check if speaker is null or has not attended the conference
            if (speaker == null)
            {
                return BadRequest("Invalid speaker.");
            }

            var hasAttended = _context.ConferenceXspeakers.Any(a => a.ConferenceId == conference.Id && a.SpeakerId == speaker.Id);

            if (!hasAttended)
            {
                return NotFound("Speaker has't attended that conference!");
            }

            //if (!hasAttended)
            //{
            //    var newSpeaker = new ConferenceXspeaker
            //    {
            //        SpeakerId = speaker.Id,
            //        ConferenceId = conference.Id
            //    };

            //    _context.ConferenceXspeakers.Add(newSpeaker);
            //    _context.SaveChanges(); // Save changes to the database
            //}


            var smsNotification = new SmsNotification(speaker, conference);

            _context.Smsnotifications.Add(smsNotification);
            _context.SaveChanges();


            _manager.SendNotification(smsNotification);
            return Ok("Speaker sms notification sent successfully.");
        }
        //adauga metoda de adaugare in EmailNotification

    }
}
