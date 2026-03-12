using HabitFlow.Domain.Common.Exceptions;
using HabitFlow.Domain.Common.Models;
using HabitFlow.Domain.Goals.Events;

namespace HabitFlow.Domain.Goals
{
    public sealed class Goal : AggregateRoot<Guid>
    {
        private readonly List<GoalHabit> _goalHabits = [];

        public Guid UserId { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string? Description { get; private set; }
        public decimal TargetValue { get; private set; }
        public string TargetUnit { get; private set; } = string.Empty;
        public decimal CurrentValue { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime TargetDate { get; private set; }
        public GoalStatus Status { get; private set; }

        public IReadOnlyCollection<GoalHabit> GoalHabits => _goalHabits.AsReadOnly();

        /// <summary>
        /// Percentage of the target achieved. Derived from CurrentValue and TargetValue.
        /// Capped at 100 % to represent a completed goal.
        /// </summary>
        public decimal ProgressPercentage => TargetValue > 0
            ? Math.Min(100m, CurrentValue / TargetValue * 100m)
            : 0m;

        private Goal() { }

        public static Goal Create(
            Guid userId,
            string name,
            string? description,
            decimal targetValue,
            string targetUnit,
            DateTime startDate,
            DateTime targetDate)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ValidationException(nameof(name), "Goal name is required");

            if (targetValue <= 0)
                throw new ValidationException(nameof(targetValue), "Target value must be positive");

            if (targetDate <= startDate)
                throw new ValidationException(nameof(targetDate), "Target date must be after start date");

            var goal = new Goal
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Name = name.Trim(),
                Description = description,
                TargetValue = targetValue,
                TargetUnit = targetUnit.Trim(),
                CurrentValue = 0,
                StartDate = startDate,
                TargetDate = targetDate,
                Status = GoalStatus.Active
            };

            goal.AddDomainEvent(new GoalCreatedEvent(goal.Id, userId, name));
            return goal;
        }

        public void AddHabit(Guid habitId, decimal contributionWeight)
        {
            if (_goalHabits.Any(gh => gh.HabitId == habitId)) return;
            _goalHabits.Add(new GoalHabit { GoalId = Id, HabitId = habitId, ContributionWeight = contributionWeight });
        }

        public void AddHabit(GoalHabit goalHabit)
        {
            if (_goalHabits.Any(gh => gh.HabitId == goalHabit.HabitId)) return;
            _goalHabits.Add(goalHabit);
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateProgress(decimal newValue)
        {
            CurrentValue = newValue;

            if (CurrentValue >= TargetValue)
                Status = GoalStatus.Completed;

            UpdatedAt = DateTime.UtcNow;
            AddDomainEvent(new GoalProgressUpdatedEvent(Id, UserId, CurrentValue, TargetValue));
        }
    }
}
