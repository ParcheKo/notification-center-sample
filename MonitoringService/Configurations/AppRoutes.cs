namespace MonitoringService.Configurations
{
    public static class AppRoutes
    {
        public static class Api
        {
            private const string Base = "api";

            public static class Notifications
            {
                public const string Collection = Base + "/" + "notifications";
                public const string GetAll = "";
                public const string GetOne = "{id:guid}";
                public const string Create = "";
                public const string Update = "{id:guid}";
                public const string Delete = "{id:guid}";
            }

            public static class Users
            {
                public const string Collection = Base + "/" + "users";
                public const string GetAll = "";
                public const string GetOne = "{id:guid}";
                public const string Create = "";
                public const string Update = "{id:guid}";
                public const string Delete = "{id:guid}";
            }
        }

        public static class Stream
        {
            private const string Base = "stream";
            public const string Notifications = Base + "/" + "notifications";
        }
    }
}