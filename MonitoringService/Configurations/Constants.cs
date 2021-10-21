namespace MonitoringService.Configurations
{
    public static class Constants
    {
        public static class Configuration
        {
            public static class Cors
            {
                public const string NotificationsCorsPolicy = nameof(NotificationsCorsPolicy);
                public const string HealthChecksCorsPolicy = nameof(HealthChecksCorsPolicy);
            }

            public static class OpenApi
            {
                public const string NotificationsApiTitle = "Notifications API";
                public const string NotificationsApiName = "Notifications API";
            }

            public static class HealthCheck
            {
                public const string ApiName = "Notifications Service HealCheck API ";
                public const string ApiUrl = "/hc";
                public const string UIPath = "/hc-ui";
            }
        }
    }
}