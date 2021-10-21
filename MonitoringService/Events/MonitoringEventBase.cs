using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace MonitoringService.Events
{
    public abstract class MonitoringEventBase : Entity, INotification
    {
        protected MonitoringEventBase(string who, MonitoringEventType what)
        {
            Id = Guid.NewGuid();
            When = DateTimeOffset.UtcNow;
            Who = who ?? throw new ArgumentNullException(nameof(who));
            What = what;
        }

        public DateTimeOffset When { get; }
        public string Who { get; }

        public MonitoringEventType What { get; }
    }

    public abstract class Entity
    {
        public Guid Id { get; init; }
        public DateTimeOffset CreationMoment { get; init; }
        public DateTimeOffset ModificationMoment { get; init; }
        public string CreationUser { get; init; }
        public string ModificationUser { get; init; }
    }

    public interface IReadOnlyRepository<T>
        where T : Entity
    {
        Task<T> GetById(Guid id);
        Task<IEnumerable<T>> GetAll();
    }

    public interface IRepository<T> : IReadOnlyRepository<T>
        where T : Entity
    {
        Task<T> Add(T entity);
    }

    public class UserProvider : IUserProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string User => _httpContextAccessor?.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
        // public string User => _httpContextAccessor?.HttpContext?.User?.Identity?.Name ?? string.Empty;
        // this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
    }

    public interface IUserProvider
    {
        public string User { get; }
    }
}