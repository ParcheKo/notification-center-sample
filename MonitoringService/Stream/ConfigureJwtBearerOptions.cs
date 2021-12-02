﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using MonitoringService.Configurations;

namespace MonitoringService.Stream
{
    public class ConfigureJwtBearerOptions : IPostConfigureOptions<JwtBearerOptions>
    {
        public void PostConfigure(
            string name,
            JwtBearerOptions options)
        {
            var originalOnMessageReceived = options.Events.OnMessageReceived;
            options.Events.OnMessageReceived = async context =>
            {
                await originalOnMessageReceived(context);

                if (string.IsNullOrEmpty(context.Token))
                {
                    var accessToken = context.Request.Query["access_token"];
                    var path = context.HttpContext.Request.Path;

                    if (!string.IsNullOrEmpty(accessToken) &&
                        path.StartsWithSegments(AppRoutes.Stream.Notifications))
                        context.Token = accessToken;
                }
            };
        }
    }
}