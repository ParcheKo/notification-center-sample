namespace MonitoringService.Configurations
{
    public static class Constants
    {
        public static class Configuration
        {
            public static class Cors
            {
                public const string NotificationsApiPolicy = nameof(NotificationsApiPolicy);
                public const string NotificationsStreamPolicy = nameof(NotificationsStreamPolicy);
                public const string HealthChecksPolicy = nameof(HealthChecksPolicy);
            }

            public static class OpenApi
            {
                public const string NotificationsApiTitle = "Notifications API Title";
                public const string NotificationsApiName = "Notifications API OpenApi Document Name In UI";
            }

            public static class HealthCheck
            {
                public const string ApiName = "Notifications Service HealCheck API ";
                public const string ApiUrl = "/hc";
                public const string UIPath = "/hc-ui";
                public const string Redis = nameof(Redis);
            }
        }
    }
}