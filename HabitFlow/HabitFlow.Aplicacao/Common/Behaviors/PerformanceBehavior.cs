using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace HabitFlow.Aplicacao.Common.Behaviors
{
    /// <summary>
    /// Pipeline behavior that logs performance warnings for requests taking longer than threshold.
    /// </summary>
    public sealed class PerformanceBehavior<TRequest, TResponse>(ILogger<PerformanceBehavior<TRequest, TResponse>> logger) : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<PerformanceBehavior<TRequest, TResponse>> _logger = logger;
        private const int PerformanceThresholdMs = 500;

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var stopwatch = Stopwatch.StartNew();
            var response = await next(cancellationToken);

            stopwatch.Stop();

            if (stopwatch.ElapsedMilliseconds > PerformanceThresholdMs)
            {
                var requestName = typeof(TRequest).Name;

                _logger.LogWarning(
                    "Long Running Request: {RequestName} took {ElapsedMilliseconds}ms (threshold: {Threshold}ms)",
                    requestName,
                    stopwatch.ElapsedMilliseconds,
                    PerformanceThresholdMs);
            }

            return response;
        }
    }
}
