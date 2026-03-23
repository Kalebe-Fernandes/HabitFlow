using FluentAssertions;
using HabitFlow.Domain.Common.Exceptions;
using HabitFlow.Domain.Goals;
using HabitFlow.Domain.Goals.Events;

namespace HabitFlow.UnitTests.Domain.Goals
{
    [TestFixture]
    public class GoalTests
    {
        private static readonly Guid _userId = Guid.NewGuid();
        private static readonly DateTime _start = DateTime.UtcNow.Date;
        private static readonly DateTime _target = _start.AddMonths(3);

        private static Goal CreateValidGoal(
            string name = "Ler 12 livros",
            decimal targetValue = 12m,
            string targetUnit = "livros") =>
            Goal.Create(_userId, name, "Meta anual de leitura", targetValue, targetUnit, _start, _target);

        // ── Create ────────────────────────────────────────────────────────────────

        [Test]
        public void Create_ValidInputs_ReturnsGoal()
        {
            var goal = CreateValidGoal();

            goal.Should().NotBeNull();
            goal.Id.Should().NotBeEmpty();
            goal.UserId.Should().Be(_userId);
            goal.Name.Should().Be("Ler 12 livros");
            goal.TargetValue.Should().Be(12m);
            goal.CurrentValue.Should().Be(0m);
            goal.Status.Should().Be(GoalStatus.Active);
        }

        [Test]
        public void Create_TrimsName()
        {
            var goal = Goal.Create(_userId, "  Meta  ", null, 10m, "itens", _start, _target);

            goal.Name.Should().Be("Meta");
        }

        [Test]
        public void Create_RaisesGoalCreatedEvent()
        {
            var goal = CreateValidGoal();

            goal.DomainEvents.Should().ContainSingle(e => e is GoalCreatedEvent);
            var evt = (GoalCreatedEvent)goal.DomainEvents.Single();
            evt.GoalId.Should().Be(goal.Id);
            evt.UserId.Should().Be(_userId);
            evt.GoalName.Should().Be("Ler 12 livros");
        }

        [TestCase("")]
        [TestCase("   ")]
        public void Create_EmptyName_ThrowsValidationException(string name)
        {
            var act = () => Goal.Create(_userId, name, null, 10m, "itens", _start, _target);

            act.Should().Throw<ValidationException>()
                .WithMessage("Goal name is required");
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void Create_ZeroOrNegativeTargetValue_ThrowsValidationException(decimal value)
        {
            var act = () => Goal.Create(_userId, "Meta", null, value, "itens", _start, _target);

            act.Should().Throw<ValidationException>()
                .WithMessage("Target value must be positive");
        }

        [Test]
        public void Create_TargetDateBeforeStartDate_ThrowsValidationException()
        {
            var act = () => Goal.Create(_userId, "Meta", null, 10m, "itens", _target, _start);

            act.Should().Throw<ValidationException>()
                .WithMessage("Target date must be after start date");
        }

        [Test]
        public void Create_TargetDateEqualsStartDate_ThrowsValidationException()
        {
            var act = () => Goal.Create(_userId, "Meta", null, 10m, "itens", _start, _start);

            act.Should().Throw<ValidationException>();
        }

        // ── ProgressPercentage ────────────────────────────────────────────────────

        [Test]
        public void ProgressPercentage_Initially_IsZero()
        {
            var goal = CreateValidGoal();

            goal.ProgressPercentage.Should().Be(0m);
        }

        [Test]
        public void ProgressPercentage_HalfwayThrough_IsFiftyPercent()
        {
            var goal = CreateValidGoal(targetValue: 10m);
            goal.UpdateProgress(5m);

            goal.ProgressPercentage.Should().Be(50m);
        }

        [Test]
        public void ProgressPercentage_ExceedingTarget_CapsAtHundred()
        {
            var goal = CreateValidGoal(targetValue: 10m);
            goal.UpdateProgress(999m);

            goal.ProgressPercentage.Should().Be(100m);
        }

        // ── UpdateProgress ────────────────────────────────────────────────────────

        [Test]
        public void UpdateProgress_BelowTarget_KeepsStatusActive()
        {
            var goal = CreateValidGoal(targetValue: 12m);

            goal.UpdateProgress(6m);

            goal.Status.Should().Be(GoalStatus.Active);
            goal.CurrentValue.Should().Be(6m);
        }

        [Test]
        public void UpdateProgress_MeetsTarget_SetsStatusToCompleted()
        {
            var goal = CreateValidGoal(targetValue: 12m);

            goal.UpdateProgress(12m);

            goal.Status.Should().Be(GoalStatus.Completed);
        }

        [Test]
        public void UpdateProgress_ExceedsTarget_SetsStatusToCompleted()
        {
            var goal = CreateValidGoal(targetValue: 10m);

            goal.UpdateProgress(15m);

            goal.Status.Should().Be(GoalStatus.Completed);
        }

        [Test]
        public void UpdateProgress_RaisesGoalProgressUpdatedEvent()
        {
            var goal = CreateValidGoal();
            goal.ClearDomainEvents();

            goal.UpdateProgress(5m);

            goal.DomainEvents.Should().ContainSingle(e => e is GoalProgressUpdatedEvent);
            var evt = (GoalProgressUpdatedEvent)goal.DomainEvents.Single();
            evt.GoalId.Should().Be(goal.Id);
            evt.CurrentValue.Should().Be(5m);
            evt.TargetValue.Should().Be(12m);
        }

        // ── AddHabit ──────────────────────────────────────────────────────────────

        [Test]
        public void AddHabit_NewHabit_AddsToCollection()
        {
            var goal = CreateValidGoal();
            var habitId = Guid.NewGuid();

            goal.AddHabit(habitId, 1.0m);

            goal.GoalHabits.Should().HaveCount(1);
            goal.GoalHabits.First().HabitId.Should().Be(habitId);
        }

        [Test]
        public void AddHabit_DuplicateHabit_IsIdempotent()
        {
            var goal = CreateValidGoal();
            var habitId = Guid.NewGuid();
            goal.AddHabit(habitId, 1.0m);

            goal.AddHabit(habitId, 0.5m);

            goal.GoalHabits.Should().HaveCount(1);
        }

        [Test]
        public void AddHabit_MultipleDistinctHabits_AddsAll()
        {
            var goal = CreateValidGoal();

            goal.AddHabit(Guid.NewGuid(), 0.5m);
            goal.AddHabit(Guid.NewGuid(), 0.5m);

            goal.GoalHabits.Should().HaveCount(2);
        }
    }
}
