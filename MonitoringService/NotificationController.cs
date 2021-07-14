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
        private readonly IHubContext<NotificationCenterHub, INotificationClientApp> _hubContext;

        public NotificationController(Settings settings, IHubContext<NotificationCenterHub, INotificationClientApp> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpGet]
        [Route(NotificationControllerRoutes.NotificationsHistory)]
        public async Task<string> GetNotificationHistory()
        {
            await _hubContext.Clients.All.ReceiveSimpleMessage("someone reached the get-all route!");
            //todo: get a paged list of notifications.
            return await Task.FromResult("I am the notifications!");
        }
    }
}