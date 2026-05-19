using System.ComponentModel;
using TechTrack.MaintenanceService.Background.Processors;

namespace TechTrack.MaintenanceService.Background
{
    public class MaintenanceWorker(IServiceProvider serviceProvider) : BackgroundService
    {
        private readonly TimeSpan _interval = TimeSpan.FromMinutes(10);

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = serviceProvider.CreateScope();

                var processors = scope.ServiceProvider
                    .GetServices<IBackgroundProcessor>()
                    .ToList();

                foreach (var processor in processors)
                {
                    await processor.ProcessAsync(stoppingToken);
                }

                await Task.Delay(_interval, stoppingToken);
            }
        }
    }
}
