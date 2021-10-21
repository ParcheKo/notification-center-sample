using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MonitoringService.Configurations;
using MonitoringService.Stream;
using static MonitoringService.Configurations.AppRoutes.Api.Notifications;

namespace MonitoringService.Api
{
    [Route(Collection)]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly IHubContext<NotificationHub, INotificationHubClient> _notificationHubContext;

        public NotificationController(Settings settings,
            IHubContext<NotificationHub, INotificationHubClient> hubContext)
        {
            _notificationHubContext = hubContext;
        }

        [HttpGet]
        [Route(GetAll)]
        public async Task<string> GetNotificationHistory()
        {
            await _notificationHubContext.Clients.All.ReceiveSimpleMessage("someone reached the get-all route!");
            //todo: get a paged list of notifications.
            return await Task.FromResult("I am the notifications!");
        }
        // NOTE: To Bypass Route attribute applied at Controller level in cases like defining child resources routes etc 
        // [Route("~/" + UserControllerRoutes.Base + UserControllerRoutes.GetOne)]
        // [HttpGet]
        // public async Task Test()
        // {
        //     await Task.FromResult("test");
        // }
    }
}