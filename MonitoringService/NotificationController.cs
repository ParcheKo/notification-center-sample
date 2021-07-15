using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace MonitoringService
{
    [Route("api")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly IHubContext<NotificationHub, INotificationHubClient> _notificationHubContext;

        public NotificationController(Settings settings, IHubContext<NotificationHub, INotificationHubClient> hubContext)
        {
            _notificationHubContext = hubContext;
        }

        [HttpGet]
        [Route(NotificationControllerRoutes.NotificationsHistory)]
        public async Task<string> GetNotificationHistory()
        {
            await _notificationHubContext.Clients.All.ReceiveSimpleMessage("someone reached the get-all route!");
            //todo: get a paged list of notifications.
            return await Task.FromResult("I am the notifications!");
        }
    }
}