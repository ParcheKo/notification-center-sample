using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using ExceptionContext = Microsoft.AspNetCore.Mvc.Filters.ExceptionContext;

namespace MonitoringService
{
    public class NotificationHub: Hub<INotificationHubClient>
    {
        public async Task NotifyOthers(string message)
        {
            await Clients.Others.ReceiveSimpleMessage(message);
        }
        
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