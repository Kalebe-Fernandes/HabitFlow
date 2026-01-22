using HabitFlow.Domain.Common.Exceptions;
using HabitFlow.Domain.Common.Models;
using HabitFlow.Domain.Gamification.Enums;

namespace HabitFlow.Domain.Gamification.ValueObjects
{
    /// <summary>
    /// Value object representing the criteria for earning a badge.
    /// Defines what conditions must be met for a user to receive a badge.
    /// </summary>
    public sealed class BadgeCriteria : ValueObject
    {
        /// <summary>
        /// Gets the type of criteria (Streak, TotalCompletions, ConsecutiveDays, etc.).
        /// </summary>
        public BadgeCriteriaType Type { get; private init; }

        /// <summary>
        /// Gets the target value that must be reached.
        /// </summary>
        public int TargetValue { get; private init; }

        /// <summary>
        /// Gets the specific habit ID if this badge is habit-specific (null for general badges).
        /// </summary>
        public Guid? SpecificHabitId { get; private init; }

        /// <summary>
        /// Gets the category ID if this badge is category-specific (null for general badges).
        /// </summary>
        public int? SpecificCategoryId { get; private init; }

        private BadgeCriteria() { }

        /// <summary>
        /// Creates streak-based criteria (e.g., "Maintain a 7-day streak").
        /// </summary>
        /// <param name="targetDays">Number of consecutive days required.</param>
        /// <param name="habitId">Optional specific habit ID.</param>
        /// <returns>A BadgeCriteria for streaks.</returns>
        public static BadgeCriteria ForStreak(int targetDays, Guid? habitId = null)
        {
            ValidateTargetValue(targetDays);

            return new BadgeCriteria
            {
                Type = BadgeCriteriaType.Streak,
                TargetValue = targetDays,
                SpecificHabitId = habitId
            };
        }

        /// <summary>
        /// Creates total completions criteria (e.g., "Complete 100 habits").
        /// </summary>
        /// <param name="targetCompletions">Number of total completions required.</param>
        /// <param name="habitId">Optional specific habit ID.</param>
        /// <param name="categoryId">Optional specific category ID.</param>
        /// <returns>A BadgeCriteria for total completions.</returns>
        public static BadgeCriteria ForTotalCompletions(int targetCompletions, Guid? habitId = null, int? categoryId = null)
        {
            ValidateTargetValue(targetCompletions);

            return new BadgeCriteria
            {
                Type = BadgeCriteriaType.TotalCompletions,
                TargetValue = targetCompletions,
                SpecificHabitId = habitId,
                SpecificCategoryId = categoryId
            };
        }

        /// <summary>
        /// Creates consecutive days criteria (e.g., "Complete habits for 30 consecutive days").
        /// </summary>
        /// <param name="targetDays">Number of consecutive days required.</param>
        /// <param name="categoryId">Optional specific category ID.</param>
        /// <returns>A BadgeCriteria for consecutive days.</returns>
        public static BadgeCriteria ForConsecutiveDays(int targetDays, int? categoryId = null)
        {
            ValidateTargetValue(targetDays);

            return new BadgeCriteria
            {
                Type = BadgeCriteriaType.ConsecutiveDays,
                TargetValue = targetDays,
                SpecificCategoryId = categoryId
            };
        }

        /// <summary>
        /// Creates level-based criteria (e.g., "Reach level 10").
        /// </summary>
        /// <param name="targetLevel">The level that must be reached.</param>
        /// <returns>A BadgeCriteria for levels.</returns>
        public static BadgeCriteria ForLevel(int targetLevel)
        {
            ValidateTargetValue(targetLevel);

            return new BadgeCriteria
            {
                Type = BadgeCriteriaType.Level,
                TargetValue = targetLevel
            };
        }

        /// <summary>
        /// Creates perfect week criteria (e.g., "Complete all habits for 7 days straight").
        /// </summary>
        /// <param name="targetWeeks">Number of perfect weeks required.</param>
        /// <returns>A BadgeCriteria for perfect weeks.</returns>
        public static BadgeCriteria ForPerfectWeeks(int targetWeeks)
        {
            ValidateTargetValue(targetWeeks);

            return new BadgeCriteria
            {
                Type = BadgeCriteriaType.PerfectWeek,
                TargetValue = targetWeeks
            };
        }

        /// <summary>
        /// Creates goal completion criteria (e.g., "Complete 5 goals").
        /// </summary>
        /// <param name="targetGoals">Number of goals that must be completed.</param>
        /// <returns>A BadgeCriteria for goal completions.</returns>
        public static BadgeCriteria ForGoalCompletions(int targetGoals)
        {
            ValidateTargetValue(targetGoals);

            return new BadgeCriteria
            {
                Type = BadgeCriteriaType.GoalCompletion,
                TargetValue = targetGoals
            };
        }

        /// <summary>
        /// Gets a user-friendly description of the criteria.
        /// </summary>
        /// <returns>A string describing what is required to earn the badge.</returns>
        public string GetDescription()
        {
            return Type switch
            {
                BadgeCriteriaType.Streak => $"Maintain a {TargetValue}-day streak{GetHabitSpecificText()}",
                BadgeCriteriaType.TotalCompletions => $"Complete {TargetValue} habits{GetHabitOrCategoryText()}",
                BadgeCriteriaType.ConsecutiveDays => $"Complete habits for {TargetValue} consecutive days{GetCategoryText()}",
                BadgeCriteriaType.Level => $"Reach level {TargetValue}",
                BadgeCriteriaType.PerfectWeek => $"Complete {TargetValue} perfect week{(TargetValue > 1 ? "s" : "")}",
                BadgeCriteriaType.GoalCompletion => $"Complete {TargetValue} goal{(TargetValue > 1 ? "s" : "")}",
                _ => "Unknown criteria"
            };
        }

        private string GetHabitSpecificText() => SpecificHabitId.HasValue ? " on a specific habit" : "";
        private string GetCategoryText() => SpecificCategoryId.HasValue ? " in a specific category" : "";
        private string GetHabitOrCategoryText()
        {
            if (SpecificHabitId.HasValue) return " of a specific habit";
            if (SpecificCategoryId.HasValue) return " in a specific category";
            return "";
        }

        private static void ValidateTargetValue(int targetValue)
        {
            if (targetValue <= 0)
                throw new ValidationException(nameof(targetValue), "Target value must be positive");
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Type;
            yield return TargetValue;
            yield return SpecificHabitId;
            yield return SpecificCategoryId;
        }
    }
}
