using System;

namespace MonitoringService
{
    public class AppPublished : MonitoringEventBase
    {
        public AppPublished(string who, string appName, string version)
            : base(who)
        {
            AppName = appName ?? throw new ArgumentNullException(nameof(appName));
            Version = version ?? throw new ArgumentNullException(nameof(version));
        }

        public string AppName { get; set; }
        public string Version { get; set; }
    }
}