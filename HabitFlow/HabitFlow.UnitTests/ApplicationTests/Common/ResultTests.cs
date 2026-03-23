using FluentAssertions;
using HabitFlow.Aplicacao.Common.Models;

namespace HabitFlow.UnitTests.ApplicationTests.Common
{
    [TestFixture]
    public class ResultTests
    {
        // ── Non-generic Result ────────────────────────────────────────────────────

        [Test]
        public void Success_SetsIsSuccessTrue()
        {
            var result = Result.Success();

            result.IsSuccess.Should().BeTrue();
            result.IsFailure.Should().BeFalse();
            result.Error.Should().BeEmpty();
        }

        [Test]
        public void Failure_SetsIsSuccessFalse()
        {
            var result = Result.Failure("algo correu mal");

            result.IsSuccess.Should().BeFalse();
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be("algo correu mal");
        }

        [Test]
        public void Failure_NullMessage_ErrorIsEmpty()
        {
            var result = Result.Failure(null!);
            result.Error.Should().BeEmpty();
        }

        // ── Generic Result<T> ─────────────────────────────────────────────────────

        [Test]
        public void SuccessT_SetsValueAndIsSuccess()
        {
            var result = Result.Success(42);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(42);
            result.Error.Should().BeEmpty();
        }

        [Test]
        public void FailureT_SetsErrorAndDefaultValue()
        {
            var result = Result.Failure<int>("nao encontrado");

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be("nao encontrado");
            result.Value.Should().Be(default);
        }

        [Test]
        public void SuccessT_WithReferenceType_SetsValue()
        {
            var obj = new { Nome = "Joao" };
            var result = Result.Success(obj);

            result.Value.Should().BeSameAs(obj);
        }

        [Test]
        public void FailureT_WithReferenceType_ValueIsNull()
        {
            var result = Result.Failure<string>("erro");

            result.Value.Should().BeNull();
        }

        // ── IsFailure mirrors IsSuccess ───────────────────────────────────────────

        [Test]
        public void IsFailure_AlwaysOppositeOfIsSuccess_ForSuccess()
        {
            var result = Result.Success("ok");

            result.IsSuccess.Should().BeTrue();
            result.IsFailure.Should().BeFalse();
        }

        [Test]
        public void IsFailure_AlwaysOppositeOfIsSuccess_ForFailure()
        {
            var result = Result.Failure<string>("erro");

            result.IsSuccess.Should().BeFalse();
            result.IsFailure.Should().BeTrue();
        }
    }
}
