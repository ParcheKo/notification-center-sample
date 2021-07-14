using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using ExceptionContext = Microsoft.AspNetCore.Mvc.Filters.ExceptionContext;

namespace MonitoringService
{
    public interface INotificationClient
    {
        Task ReceiveMessage(string message);
    }
    
    public class NotificationHub: Hub<INotificationClient>
    {
        public async Task Notify(string message)
        {
            await Clients.All.ReceiveMessage(message);
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