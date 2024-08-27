using ConferenceAPI.Data;
using ConferenceAPI.Models;
using ConferenceAPI.Requests;
using ConferenceAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ConferenceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        public DanaBazaDeDateContext _context = new DanaBazaDeDateContext();
        public NotificationManager _manager = new NotificationManager();

        public NotificationController(DanaBazaDeDateContext context, NotificationManager manager)
        {
            _context = context;
            _manager = manager;
        }
       

       

    }
}
