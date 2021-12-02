using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace MonitoringService.Events
{
    public class UserProvider : IUserProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserProvider(
            IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string User => _httpContextAccessor?.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
        // public string User => _httpContextAccessor?.HttpContext?.User?.Identity?.Name ?? string.Empty;
        // this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
    }
}