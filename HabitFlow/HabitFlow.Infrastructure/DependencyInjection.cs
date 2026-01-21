using HabitFlow.Aplicacao.Common.Interfaces;
using HabitFlow.Domain.Repositories;
using HabitFlow.Infrastructure.BackgroundJobs;
using HabitFlow.Infrastructure.Data;
using HabitFlow.Infrastructure.Repositories;
using HabitFlow.Infrastructure.Services;
using HabitFlow.Infrastructure.Services.Cache;
using HabitFlow.Infrastructure.Services.Email;
using HabitFlow.Infrastructure.Services.Notifications;
using HabitFlow.Infrastructure.Services.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HabitFlow.Infrastructure
{
    /// <summary>
    /// Extension methods for configuring Infrastructure layer services.
    /// </summary>
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Database
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                options.UseSqlServer(connectionString, sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 3,
                        maxRetryDelay: TimeSpan.FromSeconds(5),
                        errorNumbersToAdd: null);
                    sqlOptions.CommandTimeout(30);
                });

                // Enable detailed errors only in development
                if (configuration.GetValue<bool>("DetailedErrors"))
                {
                    options.EnableDetailedErrors();
                    options.EnableSensitiveDataLogging();
                }
            });

            // Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IHabitRepository, HabitRepository>();
            services.AddScoped<IGoalRepository, GoalRepository>();
            services.AddScoped<IUserLevelRepository, UserLevelRepository>();

            // External Services
            services.AddScoped<IEmailService, MockEmailService>();
            services.AddScoped<IStorageService, MockStorageService>();
            services.AddScoped<INotificationService, MockNotificationService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();

            // Cache
            services.AddMemoryCache();
            services.AddScoped<ICacheService, MemoryCacheService>();

            // Background Jobs
            services.AddHostedService<HabitReminderJob>();
            services.AddHostedService<BadgeEvaluationJob>();
            services.AddHostedService<DailyMetricsAggregationJob>();
            services.AddHostedService<EmailQueueProcessorJob>();

            return services;
        }
    }
}