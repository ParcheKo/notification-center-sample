using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MonitoringService.Configurations;
using MonitoringService.Events;
using Timer = System.Timers.Timer;

namespace MonitoringService
{
    public class MonitoringService : IHostedService
    {
        private const int DebouncingTimeInMilliseconds = 10000;
        private readonly Dictionary<FileSystemWatcher, App> _fileSystemWatcherAppMappings;
        private readonly IList<FileSystemWatcher> _fileWatchers;
        private readonly ILogger<MonitoringService> _logger;
        private readonly IMediator _mediator;
        private readonly Timer _restorer = new(DebouncingTimeInMilliseconds);
        private bool _isDebouncing;
        private Settings _settings;

        public MonitoringService(ILogger<MonitoringService> logger,
            IServiceProvider provider,
            IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
            _fileWatchers = new List<FileSystemWatcher>();
            using var scope = provider.CreateScope();
            var scopedProvider = scope.ServiceProvider;
            _settings = scopedProvider.GetRequiredService<IOptionsSnapshot<Settings>>().Value;
            scopedProvider.GetRequiredService<IOptionsMonitor<Settings>>().OnChange(settings =>
            {
                UpdateSettings(settings);
                ConfigureFileWatchers(_fileWatchers).GetAwaiter().GetResult();
            });
            _fileSystemWatcherAppMappings = new Dictionary<FileSystemWatcher, App>();
            _restorer.Elapsed += Restore;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await ConfigureFileWatchers(_fileWatchers);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }

        private void UpdateSettings(Settings settings)
        {
            _settings = settings;
        }

        private void Restore(object sender,
            ElapsedEventArgs e)
        {
            _isDebouncing = false;
            _restorer.Stop();
        }

        private async Task ConfigureFileWatchers(IList<FileSystemWatcher> fileWatchers)
        {
            if (fileWatchers.Any())
            {
                foreach (var app in _settings.MonitoredApps)
                {
                    var fileWatcher = _fileSystemWatcherAppMappings.First(p => p.Value.Name == app.Name).Key;
                    fileWatcher.IncludeSubdirectories = app.IncludeSubdirectories;
                    fileWatcher.EnableRaisingEvents = app.IsMonitored;
                    fileWatcher.NotifyFilter = app.ChangesToMonitor;
                    fileWatcher.Filter = app.FileNameFilter;
                }

                return;
            }

            foreach (var app in _settings.MonitoredApps)
            {
                var fileWatcher = new FileSystemWatcher(app.RootPath)
                {
                    IncludeSubdirectories = app.IncludeSubdirectories,
                    EnableRaisingEvents = app.IsMonitored,
                    NotifyFilter = app.ChangesToMonitor,
                    Filter = app.FileNameFilter
                };
                _fileSystemWatcherAppMappings.Add(fileWatcher, app);
                fileWatcher.Changed += OnChanged;
                fileWatcher.Created += OnCreated;
                fileWatcher.Deleted += OnDeleted;
                fileWatcher.Renamed += OnRenamed;
                fileWatcher.Error += OnError;

                fileWatchers.Add(fileWatcher);
            }
        }

        private void OnChanged(object sender,
            FileSystemEventArgs e)
        {
            PublishAppPublishingMessage(sender, e);
            _logger.LogInformation("Changed: {FullPath}", e.FullPath);
            Debounce();
        }

        private void OnCreated(object sender,
            FileSystemEventArgs e)
        {
            PublishAppPublishingMessage(sender, e);
            _logger.LogInformation("Created: {FullPath}", e.FullPath);
            Debounce();
        }

        private void WaitForStorage()
        {
            Thread.Sleep(2000);
        }

        private void Debounce()
        {
            _isDebouncing = true;
            _restorer.Start();
        }

        private void PublishAppPublishingMessage(object sender,
            FileSystemEventArgs e)
        {
            if (_isDebouncing)
            {
                return;
            }

            _fileSystemWatcherAppMappings.TryGetValue((sender as FileSystemWatcher)!, out var app);
            WaitForStorage();
            var version = e.FullPath.GetAssemblyVersion();
            _mediator.Publish(new AppPublished("USERNAME", app!.Name, version));
        }

        private void OnDeleted(object sender,
            FileSystemEventArgs e)
        {
            _logger.LogInformation("Deleted: {FullPath}", e.FullPath);
        }

        private void OnRenamed(object sender,
            RenamedEventArgs e)
        {
            _logger.LogInformation("Renamed: From \"{OldFullPath}\" To \"{NewFullPath}\"", e.OldFullPath, e.FullPath);
        }

        private void OnError(object sender,
            ErrorEventArgs e)
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
    }
}