using FluentAssertions;
using FluentValidation;
using HabitFlow.Aplicacao.Common.Behaviors;
using HabitFlow.Aplicacao.Common.Models;
using MediatR;
using NSubstitute;

namespace HabitFlow.UnitTests.ApplicationTests.Common
{
    [TestFixture]
    public class ValidationBehaviorTests
    {
        // ── Test doubles ──────────────────────────────────────────────────────────

        private record NoValidatorCommand(string Value) : IRequest<Result>;
        private record ValidatedCommand(string Value) : IRequest<Result>;
        private record ValidatedGenericCommand(string Value) : IRequest<Result<string>>;

        private class NoopValidator : AbstractValidator<ValidatedCommand>
        {
            public NoopValidator() => RuleFor(x => x.Value).NotEmpty();
        }

        private class AlwaysFailValidator : AbstractValidator<ValidatedCommand>
        {
            public AlwaysFailValidator() =>
                RuleFor(x => x.Value).Must(_ => false).WithMessage("Erro de teste");
        }

        private class GenericFailValidator : AbstractValidator<ValidatedGenericCommand>
        {
            public GenericFailValidator() =>
                RuleFor(x => x.Value).Must(_ => false).WithMessage("Erro generico");
        }

        // ── No validators ─────────────────────────────────────────────────────────

        [Test]
        public async Task Handle_NoValidators_CallsNext()
        {
            var behavior = new ValidationBehavior<NoValidatorCommand, Result>(
                Enumerable.Empty<IValidator<NoValidatorCommand>>());

            var nextCalled = false;
            Task<Result> Next(CancellationToken _)
            {
                nextCalled = true;
                return Task.FromResult(Result.Success());
            }

            var result = await behavior.Handle(
                new NoValidatorCommand("x"), Next, CancellationToken.None);

            nextCalled.Should().BeTrue();
            result.IsSuccess.Should().BeTrue();
        }

        // ── Passing validation ────────────────────────────────────────────────────

        [Test]
        public async Task Handle_ValidRequest_CallsNext()
        {
            var behavior = new ValidationBehavior<ValidatedCommand, Result>(
                [new NoopValidator()]);

            var nextCalled = false;
            Task<Result> Next(CancellationToken _)
            {
                nextCalled = true;
                return Task.FromResult(Result.Success());
            }

            var result = await behavior.Handle(
                new ValidatedCommand("valor"), Next, CancellationToken.None);

            nextCalled.Should().BeTrue();
            result.IsSuccess.Should().BeTrue();
        }

        // ── Failing validation – non-generic Result ───────────────────────────────

        [Test]
        public async Task Handle_InvalidRequest_ReturnsFailureWithoutCallingNext()
        {
            var behavior = new ValidationBehavior<ValidatedCommand, Result>(
                [new AlwaysFailValidator()]);

            var nextCalled = false;
            Task<Result> Next(CancellationToken _)
            {
                nextCalled = true;
                return Task.FromResult(Result.Success());
            }

            var result = await behavior.Handle(
                new ValidatedCommand("x"), Next, CancellationToken.None);

            nextCalled.Should().BeFalse();
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Contain("Erro de teste");
        }

        // ── Failing validation – generic Result<T> ────────────────────────────────

        [Test]
        public async Task Handle_InvalidGenericRequest_ReturnsGenericFailure()
        {
            var behavior = new ValidationBehavior<ValidatedGenericCommand, Result<string>>(
                [new GenericFailValidator()]);

            Task<Result<string>> Next(CancellationToken _) =>
                Task.FromResult(Result.Success("ok"));

            var result = await behavior.Handle(
                new ValidatedGenericCommand("x"), Next, CancellationToken.None);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Contain("Erro generico");
            result.Value.Should().BeNull();
        }

        // ── Multiple validation errors ────────────────────────────────────────────

        [Test]
        public async Task Handle_MultipleValidationErrors_JoinsAllMessages()
        {
            var validator1 = Substitute.For<IValidator<ValidatedCommand>>();
            validator1.ValidateAsync(Arg.Any<ValidationContext<ValidatedCommand>>(), Arg.Any<CancellationToken>())
                .Returns(new FluentValidation.Results.ValidationResult(
                    [new FluentValidation.Results.ValidationFailure("Value", "Erro 1")]));

            var validator2 = Substitute.For<IValidator<ValidatedCommand>>();
            validator2.ValidateAsync(
                    Arg.Any<ValidationContext<ValidatedCommand>>(), Arg.Any<CancellationToken>())
                .Returns(new FluentValidation.Results.ValidationResult(
                    [new FluentValidation.Results.ValidationFailure("Value", "Erro 2")]));

            var behavior = new ValidationBehavior<ValidatedCommand, Result>([validator1, validator2]);

            Task<Result> Next(CancellationToken _) => Task.FromResult(Result.Success());
            var outcome = await behavior.Handle(new ValidatedCommand("x"), Next, CancellationToken.None);

            outcome.IsFailure.Should().BeTrue();
            outcome.Error.Should().Contain("Erro 1");
            outcome.Error.Should().Contain("Erro 2");
        }
    }
}
