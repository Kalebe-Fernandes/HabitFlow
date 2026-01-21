using HabitFlow.Domain.Common.Exceptions;
using HabitFlow.Domain.Common.Models;

namespace HabitFlow.Domain.Habits
{
    /// <summary>
    /// Entity representing a completion record for a habit on a specific date.
    /// </summary>
    public sealed class HabitCompletion : Entity<long>
    {
        private const int MaxNotesLength = 500;

        /// <summary>
        /// Gets the habit identifier this completion belongs to.
        /// </summary>
        public Guid HabitId { get; private set; }

        /// <summary>
        /// Gets the date when the habit was completed (date only, no time).
        /// </summary>
        public DateOnly CompletionDate { get; private set; }

        /// <summary>
        /// Gets the completed value (for numeric targets).
        /// Null for binary targets or 1 if completed.
        /// </summary>
        public decimal? CompletedValue { get; private set; }

        /// <summary>
        /// Gets optional notes about this completion.
        /// </summary>
        public string? Notes { get; private set; }

        /// <summary>
        /// Gets the mood level (1-5) at the time of completion.
        /// </summary>
        public int? MoodLevel { get; private set; }

        /// <summary>
        /// Gets the energy level (1-5) at the time of completion.
        /// </summary>
        public int? EnergyLevel { get; private set; }

        private HabitCompletion() { }

        private HabitCompletion(Guid habitId, DateOnly completionDate, decimal? completedValue, string? notes, int? moodLevel, int? energyLevel)
        {
            HabitId = habitId;
            CompletionDate = completionDate;
            CompletedValue = completedValue;
            Notes = notes?.Trim();
            MoodLevel = moodLevel;
            EnergyLevel = energyLevel;
        }

        /// <summary>
        /// Creates a new habit completion record.
        /// </summary>
        /// <param name="habitId">The habit identifier</param>
        /// <param name="completionDate">The date of completion</param>
        /// <param name="completedValue">The value completed (for numeric targets)</param>
        /// <param name="notes">Optional notes</param>
        /// <param name="moodLevel">Optional mood level (1-5)</param>
        /// <param name="energyLevel">Optional energy level (1-5)</param>
        public static HabitCompletion Create(Guid habitId, DateOnly completionDate, decimal? completedValue = null, string? notes = null, int? moodLevel = null, int? energyLevel = null)
        {
            ValidateCompletionInputs(completedValue, notes, moodLevel, energyLevel);

            return new HabitCompletion(
                habitId,
                completionDate,
                completedValue,
                notes,
                moodLevel,
                energyLevel);
        }

        /// <summary>
        /// Updates the completion with new values.
        /// </summary>
        public void Update(decimal? completedValue = null, string? notes = null, int? moodLevel = null, int? energyLevel = null)
        {
            ValidateCompletionInputs(completedValue, notes, moodLevel, energyLevel);

            CompletedValue = completedValue;
            Notes = notes?.Trim();
            MoodLevel = moodLevel;
            EnergyLevel = energyLevel;
            UpdatedAt = DateTime.UtcNow;
        }

        private static void ValidateCompletionInputs(decimal? completedValue, string? notes, int? moodLevel, int? energyLevel)
        {
            if (completedValue.HasValue && completedValue.Value < 0)
            {
                throw new ValidationException(nameof(completedValue), "Completed value cannot be negative");
            }

            if (!string.IsNullOrWhiteSpace(notes) && notes.Length > MaxNotesLength)
            {
                throw new ValidationException(nameof(notes), $"Notes cannot exceed {MaxNotesLength} characters");
            }

            if (moodLevel.HasValue && (moodLevel.Value < 1 || moodLevel.Value > 5))
            {
                throw new ValidationException(nameof(moodLevel), "Mood level must be between 1 and 5");
            }

            if (energyLevel.HasValue && (energyLevel.Value < 1 || energyLevel.Value > 5))
            {
                throw new ValidationException(nameof(energyLevel), "Energy level must be between 1 and 5");
            }
        }
    }
}
