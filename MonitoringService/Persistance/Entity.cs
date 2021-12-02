using System;

namespace MonitoringService.Persistance
{
    public abstract class Entity
    {
        public DateTimeOffset CreationMoment { get; init; } = DateTimeOffset.UtcNow;
        public string CreationUser { get; init; } = string.Empty;
        protected Guid Id { get; init; } = Guid.NewGuid();
        public DateTimeOffset ModificationMoment { get; init; } = DateTimeOffset.UtcNow;
        public string ModificationUser { get; protected set; } = string.Empty;
    }
}