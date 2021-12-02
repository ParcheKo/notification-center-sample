using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using MonitoringService.Stream;

namespace MonitoringService.Events
{
    public class AppPublished : MonitoringEventBase
    {
        public AppPublished(
            string who,
            string appName,
            string version)
            : base(
                who,
                MonitoringEventType.AppPublished)
        {
            AppName = appName ?? throw new ArgumentNullException(nameof(appName));
            Version = version ?? throw new ArgumentNullException(nameof(version));
        }

        public string AppName { get; set; }
        public string Version { get; set; }

        public class AppPublishedHandler : INotificationHandler<AppPublished>
        {
            private readonly IHubContext<NotificationHub, INotificationHubClient> _notificationHubContext;

            public AppPublishedHandler(
                IHubContext<NotificationHub, INotificationHubClient> notificationHubContext)
            {
                _notificationHubContext = notificationHubContext;
            }

            public async Task Handle(
                AppPublished notification,
                CancellationToken cancellationToken)
            {
                await _notificationHubContext.Clients.All.ReceiveAppPublishedMessage(notification);
            }
        }
    }

    public enum MonitoringEventType
    {
        AppPublished
    }
}