using System;
using MediatR;

namespace MonitoringService
{
    public class MonitoringEventBase : INotification
    {
        public MonitoringEventBase(string who)
        {
            Id = Guid.NewGuid();
            When = DateTimeOffset.UtcNow;
            Who = who ?? throw new ArgumentNullException(nameof(who));
        }

        public Guid Id { get; private set; }
        public DateTimeOffset When { get; private set; }
        public string Who { get; private set; }
    }
}