using System;
using System.Collections.Generic;
using System.IO;

namespace MonitoringService.Configurations
{
    public class AppSettings
    {
        public Settings Settings { get; set; }
    }

    public class Settings
    {
        public IEnumerable<App> MonitoredApps { get; set; }
        public string NotificationCenterAppUrl { get; set; }
        public HealthChecks HealthChecks { get; set; }
    }

    public class Database
    {
        public string Name { get; set; }
        public string ConnectionString { get; set; }
    }
    
    public class HealthChecks
    {
        public List<Database> Databases { get; set; }
        public string RedisConnectionString { get; set; }
        public string RabbitMqConnectionString { get; set; }
        public string SeqUri { get; set; }
        public string ElasticSearchUri { get; set; }
        public string ElasticSearchUsername { get; set; }
        public string ElasticSearchPassword { get; set; }
        public string NotificationsStreamUrl { get; set; }
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