using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;

namespace MonitoringService.Stream
{
    public class SignalRUserProvider : IUserIdProvider
    {
        public string? GetUserId(
            HubConnectionContext connection)
        {
            // return connection.User?.Identity?.Name;
            return connection.User?.FindFirst(ClaimTypes.Email)?.Value;
        }
    }
}