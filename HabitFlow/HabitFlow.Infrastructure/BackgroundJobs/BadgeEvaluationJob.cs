using HabitFlow.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HabitFlow.Infrastructure.BackgroundJobs
{
    public class BadgeEvaluationJob(IServiceProvider serviceProvider, ILogger<BadgeEvaluationJob> logger) : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;
        private readonly ILogger<BadgeEvaluationJob> _logger = logger;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("BadgeEvaluationJob is starting");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await EvaluateBadgesAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error evaluating badges");
                }

                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }

        private async Task EvaluateBadgesAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            _logger.LogDebug("Evaluating badge criteria for all users");
            // TODO: Implement badge evaluation logic when Badge system is fully implemented
        }
    }
}
