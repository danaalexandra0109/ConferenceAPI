using ConferenceAPI.Models;

namespace ConferenceAPI.Services
{
    public interface INotificationService
    {
        public void Send(Notification notification);
    }
}
