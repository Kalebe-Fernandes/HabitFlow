using FluentAssertions;
using HabitFlow.Domain.Common.Exceptions;
using HabitFlow.Domain.Habits.Enums;
using HabitFlow.Domain.Habits.ValueObjects;

namespace HabitFlow.UnitTests.Domain.Habits
{
    [TestFixture]
    public class HabitTargetTests
    {
        // ── Binary ────────────────────────────────────────────────────────────────

        [Test]
        public void Binary_SetsTypeCorrectly()
        {
            var target = HabitTarget.Binary();

            target.Type.Should().Be(TargetType.Binary);
            target.Value.Should().BeNull();
            target.Unit.Should().BeNull();
        }

        [Test]
        public void Binary_IsMet_ReturnsTrueForPositiveValue()
        {
            var target = HabitTarget.Binary();

            target.IsMet(1m).Should().BeTrue();
            target.IsMet(0.1m).Should().BeTrue();
        }

        [Test]
        public void Binary_IsMet_ReturnsFalseForZero()
        {
            var target = HabitTarget.Binary();

            target.IsMet(0m).Should().BeFalse();
        }

        [Test]
        public void Binary_CalculatePercentage_ReturnsHundredForPositive()
        {
            var target = HabitTarget.Binary();

            target.CalculatePercentage(1m).Should().Be(100m);
        }

        [Test]
        public void Binary_CalculatePercentage_ReturnsZeroForZero()
        {
            var target = HabitTarget.Binary();

            target.CalculatePercentage(0m).Should().Be(0m);
        }

        // ── Numeric ───────────────────────────────────────────────────────────────

        [Test]
        public void Numeric_ValidInputs_SetsFields()
        {
            var target = HabitTarget.Numeric(30m, "minutos");

            target.Type.Should().Be(TargetType.Numeric);
            target.Value.Should().Be(30m);
            target.Unit.Should().Be("minutos");
        }

        [Test]
        public void Numeric_TrimsUnit()
        {
            var target = HabitTarget.Numeric(10m, "  paginas  ");

            target.Unit.Should().Be("paginas");
        }

        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-100)]
        public void Numeric_ZeroOrNegativeValue_ThrowsValidationException(decimal value)
        {
            var act = () => HabitTarget.Numeric(value, "ml");

            act.Should().Throw<ValidationException>()
                .WithMessage("Target value must be greater than zero");
        }

        [TestCase("")]
        [TestCase("   ")]
        public void Numeric_EmptyUnit_ThrowsValidationException(string unit)
        {
            var act = () => HabitTarget.Numeric(10m, unit);

            act.Should().Throw<ValidationException>()
                .WithMessage("Unit is required for numeric targets");
        }

        [Test]
        public void Numeric_UnitExceedsMaxLength_ThrowsValidationException()
        {
            var longUnit = new string('x', 21);
            var act = () => HabitTarget.Numeric(10m, longUnit);

            act.Should().Throw<ValidationException>();
        }

        [Test]
        public void Numeric_IsMet_TrueWhenCompletedValueMeetsTarget()
        {
            var target = HabitTarget.Numeric(30m, "minutos");

            target.IsMet(30m).Should().BeTrue();
            target.IsMet(31m).Should().BeTrue();
            target.IsMet(29m).Should().BeFalse();
        }

        [Test]
        public void Numeric_CalculatePercentage_ReturnsCorrectRatio()
        {
            var target = HabitTarget.Numeric(100m, "ml");

            target.CalculatePercentage(50m).Should().Be(50m);
            target.CalculatePercentage(100m).Should().Be(100m);
        }

        [Test]
        public void Numeric_CalculatePercentage_CapsAtHundred()
        {
            var target = HabitTarget.Numeric(10m, "ml");

            target.CalculatePercentage(999m).Should().Be(100m);
        }

        [Test]
        public void Numeric_CalculatePercentage_NeverNegative()
        {
            var target = HabitTarget.Numeric(10m, "ml");

            target.CalculatePercentage(-5m).Should().Be(0m);
        }

        // ── Equality ──────────────────────────────────────────────────────────────

        [Test]
        public void Equals_TwoBinaryTargets_AreEqual()
        {
            var a = HabitTarget.Binary();
            var b = HabitTarget.Binary();

            (a == b).Should().BeTrue();
        }

        [Test]
        public void Equals_NumericWithSameValueAndUnit_AreEqual()
        {
            var a = HabitTarget.Numeric(30m, "minutos");
            var b = HabitTarget.Numeric(30m, "minutos");

            (a == b).Should().BeTrue();
        }

        [Test]
        public void Equals_NumericWithDifferentValues_AreNotEqual()
        {
            var a = HabitTarget.Numeric(30m, "minutos");
            var b = HabitTarget.Numeric(60m, "minutos");

            (a != b).Should().BeTrue();
        }

        [Test]
        public void Equals_BinaryAndNumeric_AreNotEqual()
        {
            var a = HabitTarget.Binary();
            var b = HabitTarget.Numeric(10m, "ml");

            (a != b).Should().BeTrue();
        }
    }
}
