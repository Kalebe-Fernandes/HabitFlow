using FluentAssertions;
using HabitFlow.Domain.Habits.ValueObjects;

namespace HabitFlow.UnitTests.Domain.Common
{
    /// <summary>
    /// Tests for the ValueObject base class equality contract,
    /// using HabitFrequency and HabitTarget as concrete implementations.
    /// </summary>
    [TestFixture]
    public class ValueObjectTests
    {
        [Test]
        public void Equals_TwoEqualInstances_ReturnsTrue()
        {
            var a = HabitFrequency.Daily();
            var b = HabitFrequency.Daily();

            a.Equals(b).Should().BeTrue();
        }

        [Test]
        public void Equals_NullOther_ReturnsFalse()
        {
            var a = HabitFrequency.Daily();

            a.Equals(null).Should().BeFalse();
        }

        [Test]
        public void Equals_DifferentTypes_ReturnsFalse()
        {
            var frequency = HabitFrequency.Daily();
            var target = HabitTarget.Binary();

            frequency.Equals(target).Should().BeFalse();
        }

        [Test]
        public void OperatorEqual_BothNull_ReturnsTrue()
        {
            HabitFrequency? a = null;
            HabitFrequency? b = null;

            (a == b).Should().BeTrue();
        }

        [Test]
        public void OperatorEqual_OneNull_ReturnsFalse()
        {
            HabitFrequency? a = HabitFrequency.Daily();
            HabitFrequency? b = null;

            (a == b).Should().BeFalse();
            (b == a).Should().BeFalse();
        }

        [Test]
        public void OperatorNotEqual_DifferentValues_ReturnsTrue()
        {
            var a = HabitFrequency.Weekly(10);
            var b = HabitFrequency.Weekly(42);

            (a != b).Should().BeTrue();
        }

        [Test]
        public void GetHashCode_EqualInstances_ReturnSameHash()
        {
            var a = HabitFrequency.Daily();
            var b = HabitFrequency.Daily();

            a.GetHashCode().Should().Be(b.GetHashCode());
        }

        [Test]
        public void GetHashCode_DifferentInstances_ReturnDifferentHash()
        {
            var a = HabitTarget.Numeric(10m, "ml");
            var b = HabitTarget.Numeric(20m, "ml");

            // Hashes can collide but should differ for clearly distinct values
            a.GetHashCode().Should().NotBe(b.GetHashCode());
        }
    }
}
