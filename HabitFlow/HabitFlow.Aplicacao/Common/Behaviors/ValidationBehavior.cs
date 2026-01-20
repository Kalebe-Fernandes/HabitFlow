using FluentValidation;
using HabitFlow.Aplicacao.Common.Models;
using MediatR;

namespace HabitFlow.Aplicacao.Common.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : Result
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators = validators;

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (!_validators.Any()) return await next();

            var context = new ValidationContext<TRequest>(request);
            var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
            var failures = validationResults.Where(r => !r.IsValid).SelectMany(r => r.Errors).ToList();

            if (failures.Any())
            {
                var errors = string.Join("; ", failures.Select(f => f.ErrorMessage));
                return (TResponse)Result.Failure(errors);
            }

            return await next();
        }
    }
}
