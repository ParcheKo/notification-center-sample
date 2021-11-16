using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MonitoringService.Configurations;
using MonitoringService.Stream;

namespace MonitoringService.Api
{
    [Route("[controller]")]
    [ApiController]
    public class EchoController : ControllerBase
    {
        private readonly IHubContext<NotificationHub, INotificationHubClient> _notificationHubContext;
        private readonly Settings _settings;

        public EchoController(Settings settings,
            IHubContext<NotificationHub, INotificationHubClient> hubContext)
        {
            _settings = settings;
            _notificationHubContext = hubContext;
        }

        [HttpGet]
        [Route("settings")]
        public async Task<JsonResult> GetSettings()
        {
            return await Task.FromResult(new JsonResult(_settings));
        }
    }
}