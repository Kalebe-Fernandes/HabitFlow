using FluentAssertions;
using HabitFlow.Domain.Common.Exceptions;
using HabitFlow.Domain.Habits.Enums;
using HabitFlow.Domain.Habits.ValueObjects;

namespace HabitFlow.UnitTests.Domain.Habits
{
    [TestFixture]
    public class HabitFrequencyTests
    {
        // ── Daily ─────────────────────────────────────────────────────────────────

        [Test]
        public void Daily_SetsAllDaysBitFlag()
        {
            var frequency = HabitFrequency.Daily();

            frequency.Type.Should().Be(FrequencyType.Daily);
            frequency.DaysOfWeek.Should().Be(127);
        }

        [Test]
        public void Daily_IncludesAllDaysOfWeek()
        {
            var frequency = HabitFrequency.Daily();

            foreach (DayOfWeek day in Enum.GetValues<DayOfWeek>())
                frequency.IncludesDay(day).Should().BeTrue();
        }

        [Test]
        public void Daily_DaysPerWeek_ReturnsSeven()
        {
            HabitFrequency.Daily().DaysPerWeek().Should().Be(7);
        }

        // ── Weekly ────────────────────────────────────────────────────────────────

        [Test]
        public void Weekly_ValidDaysOfWeek_SetsFrequency()
        {
            // Monday (2) + Wednesday (8) + Friday (32) = 42
            var frequency = HabitFrequency.Weekly(42);

            frequency.Type.Should().Be(FrequencyType.Weekly);
            frequency.DaysOfWeek.Should().Be(42);
        }

        [TestCase(0)]
        [TestCase(128)]
        [TestCase(-1)]
        public void Weekly_InvalidDaysOfWeek_ThrowsValidationException(int daysOfWeek)
        {
            var act = () => HabitFrequency.Weekly(daysOfWeek);

            act.Should().Throw<ValidationException>();
        }

        [Test]
        public void Weekly_IncludesDay_ReturnsTrueForIncludedDays()
        {
            // Monday=2, Wednesday=8 → 10
            var frequency = HabitFrequency.Weekly(10);

            frequency.IncludesDay(DayOfWeek.Monday).Should().BeTrue();
            frequency.IncludesDay(DayOfWeek.Wednesday).Should().BeTrue();
            frequency.IncludesDay(DayOfWeek.Tuesday).Should().BeFalse();
        }

        [Test]
        public void Weekly_DaysPerWeek_CountsSetBits()
        {
            // Monday(2) + Friday(32) = 34 → 2 days
            var frequency = HabitFrequency.Weekly(34);

            frequency.DaysPerWeek().Should().Be(2);
        }

        // ── Custom ────────────────────────────────────────────────────────────────

        [Test]
        public void Custom_ValidDaysOfWeek_SetsTypeToCustom()
        {
            var frequency = HabitFrequency.Custom(10);

            frequency.Type.Should().Be(FrequencyType.Custom);
        }

        [TestCase(0)]
        [TestCase(128)]
        public void Custom_InvalidDaysOfWeek_ThrowsValidationException(int daysOfWeek)
        {
            var act = () => HabitFrequency.Custom(daysOfWeek);

            act.Should().Throw<ValidationException>();
        }

        // ── Equality ──────────────────────────────────────────────────────────────

        [Test]
        public void Equals_SameTypeAndDays_ReturnsTrue()
        {
            var a = HabitFrequency.Weekly(42);
            var b = HabitFrequency.Weekly(42);

            a.Equals(b).Should().BeTrue();
            (a == b).Should().BeTrue();
        }

        [Test]
        public void Equals_DifferentDays_ReturnsFalse()
        {
            var a = HabitFrequency.Weekly(42);
            var b = HabitFrequency.Weekly(10);

            a.Equals(b).Should().BeFalse();
            (a != b).Should().BeTrue();
        }

        [Test]
        public void Equals_DailyInstances_AreEqual()
        {
            var a = HabitFrequency.Daily();
            var b = HabitFrequency.Daily();

            a.Equals(b).Should().BeTrue();
        }
    }
}
