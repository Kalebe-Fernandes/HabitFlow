using FluentAssertions;
using HabitFlow.Domain.Common.Exceptions;
using HabitFlow.Domain.Habits;
using HabitFlow.Domain.Habits.Enums;
using HabitFlow.Domain.Habits.Events;
using HabitFlow.Domain.Habits.ValueObjects;

namespace HabitFlow.UnitTests.Domain.Habits
{
    [TestFixture]
    public class HabitTests
    {
        private static readonly Guid _userId = Guid.NewGuid();

        private static Habit CreateValidHabit(
            string name = "Meditar",
            string iconName = "meditation",
            string colorHex = "#4CAF50",
            HabitFrequency? frequency = null,
            HabitTarget? target = null) =>
            Habit.Create(
                _userId,
                name,
                "Meditacao diaria",
                iconName,
                colorHex,
                frequency ?? HabitFrequency.Daily(),
                target ?? HabitTarget.Binary());

        // ── Create ────────────────────────────────────────────────────────────────

        [Test]
        public void Create_ValidInputs_ReturnsHabit()
        {
            var habit = CreateValidHabit();

            habit.Should().NotBeNull();
            habit.UserId.Should().Be(_userId);
            habit.Name.Should().Be("Meditar");
            habit.Status.Should().Be(HabitStatus.Active);
            habit.CurrentStreak.Should().Be(0);
            habit.LongestStreak.Should().Be(0);
            habit.TotalCompletions.Should().Be(0);
        }

        [Test]
        public void Create_ValidInputs_RaisesHabitCreatedEvent()
        {
            var habit = CreateValidHabit();

            habit.DomainEvents.Should().ContainSingle(e => e is HabitCreatedEvent);
            var evt = (HabitCreatedEvent)habit.DomainEvents.Single();
            evt.HabitId.Should().Be(habit.Id);
            evt.UserId.Should().Be(_userId);
            evt.HabitName.Should().Be("Meditar");
        }

        [Test]
        public void Create_TrimsNameAndIconAndColor()
        {
            var habit = Habit.Create(
                _userId, "  Correr  ", null, "  run  ", "#FF0000",
                HabitFrequency.Daily(), HabitTarget.Binary());

            habit.Name.Should().Be("Correr");
            habit.IconName.Should().Be("run");
            habit.ColorHex.Should().Be("#FF0000");
        }

        [TestCase("")]
        [TestCase("   ")]
        public void Create_EmptyName_ThrowsValidationException(string name)
        {
            var act = () => Habit.Create(_userId, name, null, "icon", "#000000",
                HabitFrequency.Daily(), HabitTarget.Binary());

            act.Should().Throw<ValidationException>()
                .WithMessage("Habit name is required");
        }

        [Test]
        public void Create_NameExceedsMaxLength_ThrowsValidationException()
        {
            var name = new string('A', 201);
            var act = () => Habit.Create(_userId, name, null, "icon", "#000000",
                HabitFrequency.Daily(), HabitTarget.Binary());

            act.Should().Throw<ValidationException>();
        }

        [Test]
        public void Create_InvalidColorHex_ThrowsValidationException()
        {
            var act = () => Habit.Create(_userId, "Correr", null, "run", "red",
                HabitFrequency.Daily(), HabitTarget.Binary());

            act.Should().Throw<ValidationException>()
                .WithMessage("Color must be in hex format (#RRGGBB)");
        }

        [TestCase(4)]
        [TestCase(51)]
        public void Create_InvalidXpPerCompletion_ThrowsValidationException(int xp)
        {
            var act = () => Habit.Create(_userId, "Correr", null, "run", "#FF0000",
                HabitFrequency.Daily(), HabitTarget.Binary(), xpPerCompletion: xp);

            act.Should().Throw<ValidationException>();
        }

        [Test]
        public void Create_EndDateBeforeStartDate_ThrowsValidationException()
        {
            var start = DateTime.UtcNow;
            var end = start.AddDays(-1);

            var act = () => Habit.Create(_userId, "Correr", null, "run", "#FF0000",
                HabitFrequency.Daily(), HabitTarget.Binary(), start, end);

            act.Should().Throw<ValidationException>();
        }

        // ── Complete ──────────────────────────────────────────────────────────────

        [Test]
        public void Complete_ValidDate_AddCompletion()
        {
            var habit = CreateValidHabit();
            var date = DateOnly.FromDateTime(DateTime.Today);

            habit.Complete(date);

            habit.Completions.Should().HaveCount(1);
            habit.TotalCompletions.Should().Be(1);
        }

        [Test]
        public void Complete_ValidDate_RaisesHabitCompletedEvent()
        {
            var habit = CreateValidHabit();
            habit.ClearDomainEvents();
            var date = DateOnly.FromDateTime(DateTime.Today);

            habit.Complete(date);

            habit.DomainEvents.Should().Contain(e => e is HabitCompletedEvent);
        }

        [Test]
        public void Complete_SameDateTwice_ThrowsDomainException()
        {
            var habit = CreateValidHabit();
            var date = DateOnly.FromDateTime(DateTime.Today);
            habit.Complete(date);

            var act = () => habit.Complete(date);

            act.Should().Throw<DomainException>()
                .WithMessage("Habit already completed for this date");
        }

        [Test]
        public void Complete_ArchivedHabit_ThrowsDomainException()
        {
            var habit = CreateValidHabit();
            habit.Archive();
            var date = DateOnly.FromDateTime(DateTime.Today);

            var act = () => habit.Complete(date);

            act.Should().Throw<DomainException>()
                .WithMessage("Cannot complete an inactive habit");
        }

        // ── Streak calculation ────────────────────────────────────────────────────

        [Test]
        public void Complete_ConsecutiveDays_IncrementsStreak()
        {
            var habit = CreateValidHabit();
            var today = DateOnly.FromDateTime(DateTime.Today);

            habit.Complete(today.AddDays(-2));
            habit.Complete(today.AddDays(-1));
            habit.Complete(today);

            habit.CurrentStreak.Should().Be(3);
        }

        [Test]
        public void Complete_NonConsecutiveDays_RestartsStreak()
        {
            var habit = CreateValidHabit();
            var today = DateOnly.FromDateTime(DateTime.Today);

            habit.Complete(today.AddDays(-5));
            habit.Complete(today);

            habit.CurrentStreak.Should().Be(1);
            habit.LongestStreak.Should().Be(1);
        }

        [Test]
        public void Complete_LongestStreakUpdated_WhenCurrentExceedsPrevious()
        {
            var habit = CreateValidHabit();
            var today = DateOnly.FromDateTime(DateTime.Today);

            habit.Complete(today.AddDays(-2));
            habit.Complete(today.AddDays(-1));
            habit.Complete(today);

            habit.LongestStreak.Should().Be(3);
        }

        [Test]
        public void Complete_StreakMilestone_RaisesStreakAchievedEvent()
        {
            var habit = CreateValidHabit();
            var today = DateOnly.FromDateTime(DateTime.Today);

            for (int i = 6; i >= 0; i--)
                habit.Complete(today.AddDays(-i));

            habit.CurrentStreak.Should().Be(7);
            habit.DomainEvents.Should().Contain(e => e is StreakAchievedEvent);
        }

        // ── Status transitions ────────────────────────────────────────────────────

        [Test]
        public void Archive_ActiveHabit_SetsStatusToArchived()
        {
            var habit = CreateValidHabit();
            habit.Archive();

            habit.Status.Should().Be(HabitStatus.Archived);
        }

        [Test]
        public void Archive_AlreadyArchived_IsIdempotent()
        {
            var habit = CreateValidHabit();
            habit.Archive();
            habit.ClearDomainEvents();

            habit.Archive();

            habit.Status.Should().Be(HabitStatus.Archived);
            habit.DomainEvents.Should().BeEmpty();
        }

        [Test]
        public void Restore_ArchivedHabit_SetsStatusToActive()
        {
            var habit = CreateValidHabit();
            habit.Archive();
            habit.Restore();

            habit.Status.Should().Be(HabitStatus.Active);
        }

        [Test]
        public void Pause_ActiveHabit_SetsStatusToPaused()
        {
            var habit = CreateValidHabit();
            habit.Pause();

            habit.Status.Should().Be(HabitStatus.Paused);
        }

        [Test]
        public void Resume_PausedHabit_SetsStatusToActive()
        {
            var habit = CreateValidHabit();
            habit.Pause();
            habit.Resume();

            habit.Status.Should().Be(HabitStatus.Active);
        }

        // ── UpdateDetails ─────────────────────────────────────────────────────────

        [Test]
        public void UpdateDetails_ValidInputs_UpdatesFields()
        {
            var habit = CreateValidHabit();
            habit.UpdateDetails("Correr", "Corrida diaria", "run", "#FF5722");

            habit.Name.Should().Be("Correr");
            habit.Description.Should().Be("Corrida diaria");
            habit.IconName.Should().Be("run");
            habit.ColorHex.Should().Be("#FF5722");
        }

        [Test]
        public void UpdateDetails_NullIconAndColor_KeepsPreviousValues()
        {
            var habit = CreateValidHabit("Meditar", "meditation", "#4CAF50");
            habit.UpdateDetails("Meditacao", null, null, null);

            habit.IconName.Should().Be("meditation");
            habit.ColorHex.Should().Be("#4CAF50");
        }

        [Test]
        public void UpdateDetails_InvalidColor_ThrowsValidationException()
        {
            var habit = CreateValidHabit();

            var act = () => habit.UpdateDetails("Correr", null, null, "red");

            act.Should().Throw<ValidationException>();
        }

        // ── CalculateSuccessRate ──────────────────────────────────────────────────

        [Test]
        public void CalculateSuccessRate_NoCompletions_ReturnsZero()
        {
            var habit = CreateValidHabit();

            habit.CalculateSuccessRate().Should().Be(0m);
        }

        [Test]
        public void CalculateSuccessRate_WithCompletions_ReturnsPositiveRate()
        {
            var habit = CreateValidHabit();
            habit.Complete(DateOnly.FromDateTime(DateTime.Today));

            var rate = habit.CalculateSuccessRate();

            rate.Should().BeGreaterThan(0m);
            rate.Should().BeLessThanOrEqualTo(100m);
        }
    }
}
