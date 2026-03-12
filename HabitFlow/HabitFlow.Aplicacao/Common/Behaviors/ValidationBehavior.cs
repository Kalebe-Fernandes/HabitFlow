using FluentValidation;
using HabitFlow.Aplicacao.Common.Models;
using MediatR;

namespace HabitFlow.Aplicacao.Common.Behaviors
{
    /// <summary>
    /// MediatR pipeline behavior that runs FluentValidation validators before the handler.
    /// Supports both Result and Result&lt;T&gt; response types without invalid casts.
    /// </summary>
    public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : Result
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators = validators;

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (!_validators.Any())
                return await next();

            var context = new ValidationContext<TRequest>(request);
            var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
            var failures = validationResults
                .Where(r => !r.IsValid)
                .SelectMany(r => r.Errors)
                .ToList();

            if (failures.Count == 0)
                return await next(cancellationToken);

            var errors = string.Join("; ", failures.Select(f => f.ErrorMessage));

            // Result (non-generic): returned by ICommand : IRequest<Result>
            if (typeof(TResponse) == typeof(Result))
                return (TResponse)(object)Result.Failure(errors);

            // Result<T>: returned by ICommand<TResponse> : IRequest<Result<TResponse>>
            var valueType = typeof(TResponse).GetGenericArguments()[0];

            var failureResult = typeof(Result)
                .GetMethods()
                .First(m => m.Name == nameof(Result.Failure) && m.IsGenericMethod && m.GetParameters().Length == 1)
                .MakeGenericMethod(valueType)
                .Invoke(null, [errors])!;

            return (TResponse)failureResult;
        }
    }
}
