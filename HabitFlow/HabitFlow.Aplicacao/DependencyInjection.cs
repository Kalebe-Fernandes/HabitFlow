using FluentValidation;
using HabitFlow.Aplicacao.Common.Behaviors;
using HabitFlow.Aplicacao.Common.Mappings;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace HabitFlow.Aplicacao
{
    /// <summary>
    /// Extension methods for configuring Application layer services.
    /// </summary>
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            // MediatR with ordered pipeline behaviors
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(assembly);
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            });

            // FluentValidation - registers all validators in the assembly automatically
            services.AddValidatorsFromAssembly(assembly);

            // Mapster
            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(typeof(MappingConfig).Assembly);
            services.AddSingleton<IMapper>(new Mapper(config));

            return services;
        }
    }
}
