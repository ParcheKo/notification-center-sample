using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using ExceptionContext = Microsoft.AspNetCore.Mvc.Filters.ExceptionContext;

namespace MonitoringService
{
    public class NotificationCenterHub: Hub<INotificationClientApp>
    {
        public async Task NotifyAll(string message)
        {
            await Clients.All.ReceiveSimpleMessage(message);
        }
        
        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
    }
}