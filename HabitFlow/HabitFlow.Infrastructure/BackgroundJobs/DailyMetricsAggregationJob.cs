using HabitFlow.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HabitFlow.Infrastructure.BackgroundJobs
{
    public class DailyMetricsAggregationJob(IServiceProvider serviceProvider, ILogger<DailyMetricsAggregationJob> logger) : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;
        private readonly ILogger<DailyMetricsAggregationJob> _logger = logger;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("DailyMetricsAggregationJob is starting");

            while (!stoppingToken.IsCancellationRequested)
            {
                var now = DateTime.UtcNow;
                var nextRun = DateTime.Today.AddDays(1).AddMinutes(5); // Run at 00:05 UTC
                var delay = nextRun - now;

                if (delay.TotalMilliseconds > 0)
                {
                    _logger.LogInformation("Next daily metrics aggregation at {NextRun}", nextRun);
                    await Task.Delay(delay, stoppingToken);
                }

                try
                {
                    await AggregateDailyMetricsAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error aggregating daily metrics");
                }

                await Task.Delay(TimeSpan.FromHours(23), stoppingToken);
            }
        }

        private async Task AggregateDailyMetricsAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var yesterday = DateTime.UtcNow.Date.AddDays(-1);
            _logger.LogInformation("Aggregating metrics for {Date}", yesterday);
            // TODO: Implement daily metrics aggregation when Analytics schema is added
        }
    }
}
