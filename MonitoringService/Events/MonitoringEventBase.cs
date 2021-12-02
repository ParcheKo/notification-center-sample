using System;
using MediatR;

namespace MonitoringService.Events
{
    public abstract class MonitoringEventBase : Entity, INotification
    {
        protected MonitoringEventBase(
            string who,
            MonitoringEventType what)
        {
            Id = Guid.NewGuid();
            When = DateTimeOffset.UtcNow;
            Who = who ?? throw new ArgumentNullException(nameof(who));
            What = what;
        }

        private MonitoringEventType What { get; }

        private DateTimeOffset When { get; }
        private string Who { get; }
    }
}