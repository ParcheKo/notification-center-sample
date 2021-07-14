using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MonitoringService
{
    [Route("api")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        public NotificationController(Settings settings)
        {
            
        }

        [HttpGet]
        [Route(NotificationControllerRoutes.NotificationsHistory)]
        public async Task<string> GetNotificationHistory()
        {
            //todo: get a paged list of notifications.
            return await Task.FromResult("World!");
        }
    }
}