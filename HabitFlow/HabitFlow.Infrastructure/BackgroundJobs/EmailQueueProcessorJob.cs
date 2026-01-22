using HabitFlow.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HabitFlow.Infrastructure.BackgroundJobs
{
    public class EmailQueueProcessorJob(IServiceProvider serviceProvider, ILogger<EmailQueueProcessorJob> logger) : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;
        private readonly ILogger<EmailQueueProcessorJob> _logger = logger;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("EmailQueueProcessorJob is starting");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await ProcessEmailQueueAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing email queue");
                }

                await Task.Delay(TimeSpan.FromMinutes(2), stoppingToken);
            }
        }

        private async Task ProcessEmailQueueAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            _logger.LogDebug("Processing email queue");
            // TODO: Implement email queue processing when Notification entity is added
        }
    }
}
