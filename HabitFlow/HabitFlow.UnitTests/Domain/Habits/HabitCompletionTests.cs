using FluentAssertions;
using HabitFlow.Domain.Common.Exceptions;
using HabitFlow.Domain.Habits;

namespace HabitFlow.UnitTests.Domain.Habits
{
    [TestFixture]
    public class HabitCompletionTests
    {
        private static readonly Guid _habitId = Guid.NewGuid();
        private static readonly DateOnly _date = DateOnly.FromDateTime(DateTime.Today);

        // ── Create ────────────────────────────────────────────────────────────────

        [Test]
        public void Create_MinimalInputs_ReturnsCompletion()
        {
            var completion = HabitCompletion.Create(_habitId, _date);

            completion.HabitId.Should().Be(_habitId);
            completion.CompletionDate.Should().Be(_date);
            completion.CompletedValue.Should().BeNull();
            completion.Notes.Should().BeNull();
            completion.MoodLevel.Should().BeNull();
            completion.EnergyLevel.Should().BeNull();
        }

        [Test]
        public void Create_AllInputs_SetsAllFields()
        {
            var completion = HabitCompletion.Create(_habitId, _date, 30m, "Otimo treino", 4, 5);

            completion.CompletedValue.Should().Be(30m);
            completion.Notes.Should().Be("Otimo treino");
            completion.MoodLevel.Should().Be(4);
            completion.EnergyLevel.Should().Be(5);
        }

        [Test]
        public void Create_TrimsNotes()
        {
            var completion = HabitCompletion.Create(_habitId, _date, notes: "  nota  ");

            completion.Notes.Should().Be("nota");
        }

        [Test]
        public void Create_NegativeCompletedValue_ThrowsValidationException()
        {
            var act = () => HabitCompletion.Create(_habitId, _date, completedValue: -1m);

            act.Should().Throw<ValidationException>()
                .WithMessage("Completed value cannot be negative");
        }

        [TestCase(0)]
        [TestCase(6)]
        [TestCase(-1)]
        public void Create_InvalidMoodLevel_ThrowsValidationException(int mood)
        {
            var act = () => HabitCompletion.Create(_habitId, _date, moodLevel: mood);

            act.Should().Throw<ValidationException>()
                .WithMessage("Mood level must be between 1 and 5");
        }

        [TestCase(0)]
        [TestCase(6)]
        public void Create_InvalidEnergyLevel_ThrowsValidationException(int energy)
        {
            var act = () => HabitCompletion.Create(_habitId, _date, energyLevel: energy);

            act.Should().Throw<ValidationException>()
                .WithMessage("Energy level must be between 1 and 5");
        }

        [Test]
        public void Create_NotesExceedMaxLength_ThrowsValidationException()
        {
            var longNotes = new string('x', 501);
            var act = () => HabitCompletion.Create(_habitId, _date, notes: longNotes);

            act.Should().Throw<ValidationException>();
        }

        [TestCase(1)]
        [TestCase(3)]
        [TestCase(5)]
        public void Create_ValidMoodLevels_DoNotThrow(int mood)
        {
            var act = () => HabitCompletion.Create(_habitId, _date, moodLevel: mood);

            act.Should().NotThrow();
        }

        // ── Update ────────────────────────────────────────────────────────────────

        [Test]
        public void Update_ValidInputs_OverwritesFields()
        {
            var completion = HabitCompletion.Create(_habitId, _date, 10m, "nota original", 3, 3);

            completion.Update(20m, "nota nova", 5, 4);

            completion.CompletedValue.Should().Be(20m);
            completion.Notes.Should().Be("nota nova");
            completion.MoodLevel.Should().Be(5);
            completion.EnergyLevel.Should().Be(4);
        }

        [Test]
        public void Update_NullInputs_ClearsOptionalFields()
        {
            var completion = HabitCompletion.Create(_habitId, _date, 10m, "nota", 3, 3);

            completion.Update(null, null, null, null);

            completion.CompletedValue.Should().BeNull();
            completion.Notes.Should().BeNull();
            completion.MoodLevel.Should().BeNull();
            completion.EnergyLevel.Should().BeNull();
        }

        [Test]
        public void Update_InvalidMoodLevel_ThrowsValidationException()
        {
            var completion = HabitCompletion.Create(_habitId, _date);

            var act = () => completion.Update(moodLevel: 6);

            act.Should().Throw<ValidationException>();
        }
    }
}
