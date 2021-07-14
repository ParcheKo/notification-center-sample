using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;

namespace MonitoringService
{
    public class NameUserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            // return connection.User?.Identity?.Name;
            return connection.User?.FindFirst(ClaimTypes.Email)?.Value;
        }
    }
}