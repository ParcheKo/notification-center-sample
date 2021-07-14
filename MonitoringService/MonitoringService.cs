using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MonitoringService
{
    public class MonitoringService : IHostedService
    {
        private readonly ILogger<MonitoringService> _logger;
        private Settings _settings;
        private readonly IList<FileSystemWatcher> _fileWatchers;

        public MonitoringService(ILogger<MonitoringService> logger, IServiceProvider provider)
        {
            _logger = logger;
            InitializeMonitoringSettings(provider);
            _fileWatchers = new List<FileSystemWatcher>();
            InitializeFileWatchers();
        }

        private void InitializeMonitoringSettings(IServiceProvider provider)
        {
            using (var scope = provider.CreateScope())
            {
                var scopedProvider = scope.ServiceProvider;
                _settings = scopedProvider.GetRequiredService<IOptionsSnapshot<Settings>>().Value;
            }
        }

        private void InitializeFileWatchers()
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
                fileWatcher.Changed += OnChanged;
                fileWatcher.Created += OnCreated;
                fileWatcher.Deleted += OnDeleted;
                fileWatcher.Renamed += OnRenamed;
                fileWatcher.Error += OnError;

                _fileWatchers.Add(fileWatcher);
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
            await Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }
    }
}