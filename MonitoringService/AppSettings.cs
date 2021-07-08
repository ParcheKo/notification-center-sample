using System;
using System.Collections.Generic;
using System.IO;

namespace MonitoringService
{
    public class AppSettings
    {
        public MonitoringSettings MonitoringSettings { get; set; }
    }

    public class MonitoringSettings
    {
        public IEnumerable<App> Apps { get; set; }
    }

    public class App
    {
        public string Name { get; set; }
        public string RootPath { get; set; }
        public bool IncludeSubdirectories { get; set; }
        public bool IsMonitored { get; set; }
        public string FileNameFilter { get; set; }
        public NotifyFilters ChangesToMonitor { get; set; }
    }
}