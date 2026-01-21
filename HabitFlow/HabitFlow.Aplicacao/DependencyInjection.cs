using FluentValidation;
using HabitFlow.Aplicacao.Common.Behaviors;
using HabitFlow.Aplicacao.Common.Mappings;
using HabitFlow.Aplicacao.Features.Users.Commands.Register;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace HabitFlow.Aplicacao
{
    /// <summary>
    /// Extension methods for configuring Application layer services.
    /// UPDATED: Added PerformanceBehavior and Mapster configuration.
    /// </summary>
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            // MediatR with pipeline behaviors
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(assembly);
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            });

            // FluentValidation - Requer: FluentValidation.DependencyInjectionExtensions
            services.AddScoped<IValidator<RegisterUserCommand>, RegisterUserCommandValidator>();

            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(typeof(MappingConfig).Assembly);
            var mapperConfig = new Mapper(config);
            services.AddSingleton<IMapper>(mapperConfig);

            return services;
        }
    }

}
