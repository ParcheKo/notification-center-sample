using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace MonitoringService
{
    public class AppPublishedHandler : INotificationHandler<AppPublished>
    {
        private readonly IHubContext<NotificationHub, INotificationHubClient> _notificationHubContext;

        public AppPublishedHandler(IHubContext<NotificationHub, INotificationHubClient> notificationHubContext)
        {
            _notificationHubContext = notificationHubContext;
        }
        public async Task Handle(AppPublished notification, CancellationToken cancellationToken)
        {
            await _notificationHubContext.Clients.All.ReceiveAppPublishedMessage(notification);
        }
    }
}