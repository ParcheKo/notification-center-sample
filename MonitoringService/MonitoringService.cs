using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MonitoringService.Configurations;
using MonitoringService.Events;

namespace MonitoringService
{
    public class MonitoringService : IHostedService
    {
        private readonly ILogger<MonitoringService> _logger;
        private readonly IMediator _mediator;
        private Settings _settings;
        private readonly IList<FileSystemWatcher> _fileWatchers;
        private readonly Dictionary<FileSystemWatcher, App> _fileSystemWatcherAppMap;

        public MonitoringService(ILogger<MonitoringService> logger, IServiceProvider provider, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
            _fileWatchers = new List<FileSystemWatcher>();
            InitializeMonitoringSettings(provider);
            _fileSystemWatcherAppMap = new Dictionary<FileSystemWatcher, App>();
        }

        private void InitializeMonitoringSettings(IServiceProvider provider)
        {
            using var scope = provider.CreateScope();
            var scopedProvider = scope.ServiceProvider;
            _settings = scopedProvider.GetRequiredService<IOptionsSnapshot<Settings>>().Value;
        }

        private async Task ConfigureFileWatchers(IList<FileSystemWatcher> fileWatchers)
        {
            foreach (var app in _settings.MonitoredApps)
            {
                var fileWatcher = new FileSystemWatcher(app.RootPath)
                {
                    IncludeSubdirectories = app.IncludeSubdirectories,
                    EnableRaisingEvents = app.IsMonitored,
                    NotifyFilter = app.ChangesToMonitor,
                    Filter = app.FileNameFilter
                };
                _fileSystemWatcherAppMap.Add(fileWatcher, app);
                fileWatcher.Changed += OnChanged;
                fileWatcher.Created += OnCreated;
                fileWatcher.Deleted += OnDeleted;
                fileWatcher.Renamed += OnRenamed;
                fileWatcher.Error += OnError;

                fileWatchers.Add(fileWatcher);
            }
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed)
            {
                return;
            }

            _logger.LogInformation("Changed: {FullPath}", e.FullPath);
        }

        private void OnCreated(object sender, FileSystemEventArgs e)
        {
            Thread.Sleep(1000);
            _fileSystemWatcherAppMap.TryGetValue((sender as FileSystemWatcher)!, out var app);
            var version = e.FullPath.GetAssemblyVersion();
            _mediator.Publish(new AppPublished("Amir", app!.Name, version));
            _logger.LogInformation("Created: {FullPath}", e.FullPath);
        }

        private void OnDeleted(object sender, FileSystemEventArgs e)
        {
            _logger.LogInformation("Deleted: {FullPath}", e.FullPath);
        }

        private void OnRenamed(object sender, RenamedEventArgs e)
        {
            _logger.LogInformation("Renamed: From \"{OldFullPath}\" To \"{NewFullPath}\"", e.OldFullPath, e.FullPath);
        }

        private void OnError(object sender, ErrorEventArgs e)
        {
            PrintException(e.GetException());
        }

        private void PrintException(Exception? ex)
        {
            if (ex != null)
            {
                _logger.LogInformation("Error Message: {ExceptionMessage}", ex.Message);
                _logger.LogInformation("StackTrace: {StackTrace}", ex.StackTrace);
                PrintException(ex.InnerException);
            }
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await ConfigureFileWatchers(_fileWatchers);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }
    }
}