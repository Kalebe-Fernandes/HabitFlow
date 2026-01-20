using HabitFlow.Domain.Common.Exceptions;
using HabitFlow.Domain.Common.Models;
using HabitFlow.Domain.Habits.Enums;
using HabitFlow.Domain.Habits.Events;
using HabitFlow.Domain.Habits.ValueObjects;

namespace HabitFlow.Domain.Habits
{
    /// <summary>
    /// Aggregate root representing a user's habit.
    /// Contains all business rules related to habits, completions, and streaks.
    /// </summary>
    public sealed class Habit : AggregateRoot<Guid>
    {
        private const int MaxNameLength = 200;
        private const int MaxDescriptionLength = 1000;
        private const int MaxIconNameLength = 50;
        private const int MaxColorHexLength = 7;
        private readonly List<HabitCompletion> _completions = [];

        /// <summary>
        /// Gets the user identifier who owns this habit.
        /// </summary>
        public Guid UserId { get; private set; }

        /// <summary>
        /// Gets the habit name.
        /// </summary>
        public string Name { get; private set; } = string.Empty;

        /// <summary>
        /// Gets the habit description.
        /// </summary>
        public string? Description { get; private set; }

        /// <summary>
        /// Gets the icon name for visual representation.
        /// </summary>
        public string IconName { get; private set; } = string.Empty;

        /// <summary>
        /// Gets the color in hex format (e.g., "#6366F1").
        /// </summary>
        public string ColorHex { get; private set; } = string.Empty;

        /// <summary>
        /// Gets the frequency configuration.
        /// </summary>
        public HabitFrequency Frequency { get; private set; } = null!;

        /// <summary>
        /// Gets the target configuration.
        /// </summary>
        public HabitTarget Target { get; private set; } = null!;

        /// <summary>
        /// Gets the start date of the habit.
        /// </summary>
        public DateTime? StartDate { get; private set; }

        /// <summary>
        /// Gets the end date of the habit.
        /// </summary>
        public DateTime? EndDate { get; private set; }

        /// <summary>
        /// Gets the current status of the habit.
        /// </summary>
        public HabitStatus Status { get; private set; }

        /// <summary>
        /// Gets the category identifier.
        /// </summary>
        public int? CategoryId { get; private set; }

        /// <summary>
        /// Gets the current streak (consecutive days completed).
        /// </summary>
        public int CurrentStreak { get; private set; }

        /// <summary>
        /// Gets the longest streak achieved.
        /// </summary>
        public int LongestStreak { get; private set; }

        /// <summary>
        /// Gets the total number of completions.
        /// </summary>
        public int TotalCompletions { get; private set; }

        /// <summary>
        /// Gets the XP points awarded per completion.
        /// </summary>
        public int XPPerCompletion { get; private set; }

        /// <summary>
        /// Gets the collection of completions.
        /// </summary>
        public IReadOnlyCollection<HabitCompletion> Completions => _completions.AsReadOnly();

        private Habit() { }

        private Habit(Guid userId, string name, string? description, string iconName, string colorHex, HabitFrequency frequency, HabitTarget target, DateTime? startDate, DateTime? endDate, int? categoryId, int xpPerCompletion)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            Name = name;
            Description = description;
            IconName = iconName;
            ColorHex = colorHex;
            Frequency = frequency;
            Target = target;
            StartDate = startDate;
            EndDate = endDate;
            Status = HabitStatus.Active;
            CategoryId = categoryId;
            CurrentStreak = 0;
            LongestStreak = 0;
            TotalCompletions = 0;
            XPPerCompletion = xpPerCompletion;

            AddDomainEvent(new HabitCreatedEvent(Id, UserId, name));
        }

        /// <summary>
        /// Creates a new habit.
        /// </summary>
        public static Habit Create(Guid userId, string name, string? description, string iconName, string colorHex, HabitFrequency frequency, HabitTarget target, DateTime? startDate = null, DateTime? endDate = null, int? categoryId = null, int xpPerCompletion = 10)
        {
            ValidateHabitInputs(name, description, iconName, colorHex, startDate, endDate, xpPerCompletion);

            return new Habit(
                userId,
                name.Trim(),
                description?.Trim(),
                iconName.Trim(),
                colorHex.Trim(),
                frequency,
                target,
                startDate,
                endDate,
                categoryId,
                xpPerCompletion);
        }

        /// <summary>
        /// Completes the habit for a specific date.
        /// </summary>
        /// <param name="completionDate">The date of completion</param>
        /// <param name="completedValue">The value completed (for numeric targets)</param>
        /// <param name="notes">Optional notes</param>
        /// <param name="moodLevel">Optional mood level</param>
        /// <param name="energyLevel">Optional energy level</param>
        public void Complete(DateOnly completionDate, decimal? completedValue = null, string? notes = null, int? moodLevel = null, int? energyLevel = null)
        {
            if (Status != HabitStatus.Active)
            {
                throw new DomainException("Cannot complete an inactive habit", "HABIT_INACTIVE");
            }

            if (_completions.Any(c => c.CompletionDate == completionDate))
            {
                throw new DomainException("Habit already completed for this date", "DUPLICATE_COMPLETION");
            }

            var completion = HabitCompletion.Create(
                Id,
                completionDate,
                completedValue,
                notes,
                moodLevel,
                energyLevel);

            _completions.Add(completion);
            TotalCompletions++;
            UpdatedAt = DateTime.UtcNow;

            RecalculateStreak(completionDate);

            AddDomainEvent(new HabitCompletedEvent(
                Id,
                UserId,
                completionDate,
                completedValue,
                XPPerCompletion));

            if (CurrentStreak > 0 && IsStreakMilestone(CurrentStreak))
            {
                AddDomainEvent(new StreakAchievedEvent(Id, UserId, CurrentStreak));
            }
        }

        /// <summary>
        /// Updates the habit information.
        /// </summary>
        public void Update(string name, string? description, string iconName, string colorHex, HabitFrequency frequency, HabitTarget target, DateTime? startDate, DateTime? endDate, int? categoryId, int xpPerCompletion)
        {
            ValidateHabitInputs(name, description, iconName, colorHex, startDate, endDate, xpPerCompletion);

            Name = name.Trim();
            Description = description?.Trim();
            IconName = iconName.Trim();
            ColorHex = colorHex.Trim();
            Frequency = frequency;
            Target = target;
            StartDate = startDate;
            EndDate = endDate;
            CategoryId = categoryId;
            XPPerCompletion = xpPerCompletion;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Archives the habit.
        /// </summary>
        public void Archive()
        {
            if (Status == HabitStatus.Archived)
            {
                return;
            }

            Status = HabitStatus.Archived;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Restores an archived habit.
        /// </summary>
        public void Restore()
        {
            if (Status != HabitStatus.Archived)
            {
                return;
            }

            Status = HabitStatus.Active;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Pauses the habit (e.g., vacation mode).
        /// </summary>
        public void Pause()
        {
            if (Status == HabitStatus.Paused)
            {
                return;
            }

            Status = HabitStatus.Paused;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Resumes a paused habit.
        /// </summary>
        public void Resume()
        {
            if (Status != HabitStatus.Paused)
            {
                return;
            }

            Status = HabitStatus.Active;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Calculates the success rate (percentage of expected completions that were completed).
        /// </summary>
        public decimal CalculateSuccessRate()
        {
            var expectedCompletions = CalculateExpectedCompletions();

            if (expectedCompletions == 0)
            {
                return 0m;
            }

            return (decimal)TotalCompletions / expectedCompletions * 100m;
        }

        private int CalculateExpectedCompletions()
        {
            var startDate = StartDate ?? CreatedAt;
            var today = DateTime.UtcNow;
            var daysSinceStart = (today - startDate).Days + 1;

            if (daysSinceStart <= 0)
            {
                return 0;
            }

            var weeks = daysSinceStart / 7;
            var remainingDays = daysSinceStart % 7;
            var daysPerWeek = Frequency.DaysPerWeek();

            return (weeks * daysPerWeek) + Math.Min(remainingDays, daysPerWeek);
        }

        private void RecalculateStreak(DateOnly completionDate)
        {
            var orderedCompletions = _completions
                .OrderByDescending(c => c.CompletionDate)
                .ToList();

            CurrentStreak = CalculateCurrentStreak(orderedCompletions, completionDate);

            if (CurrentStreak > LongestStreak)
            {
                LongestStreak = CurrentStreak;
            }
        }

        private int CalculateCurrentStreak(List<HabitCompletion> orderedCompletions, DateOnly latestDate)
        {
            if (orderedCompletions.Count == 0)
            {
                return 0;
            }

            var streak = 1;
            var currentDate = latestDate;

            for (int i = 1; i < orderedCompletions.Count; i++)
            {
                var previousDate = orderedCompletions[i].CompletionDate;
                var expectedPreviousDate = GetPreviousExpectedDate(currentDate);

                if (previousDate == expectedPreviousDate)
                {
                    streak++;
                    currentDate = previousDate;
                }
                else if (previousDate < expectedPreviousDate)
                {
                    break;
                }
            }

            return streak;
        }

        private DateOnly GetPreviousExpectedDate(DateOnly currentDate)
        {
            var previousDate = currentDate.AddDays(-1);

            while (!Frequency.IncludesDay(previousDate.DayOfWeek))
            {
                previousDate = previousDate.AddDays(-1);
            }

            return previousDate;
        }

        private static bool IsStreakMilestone(int streak)
        {
            return streak switch
            {
                7 => true,
                14 => true,
                21 => true,
                30 => true,
                60 => true,
                90 => true,
                100 => true,
                365 => true,
                _ => streak % 100 == 0
            };
        }

        private static void ValidateHabitInputs(string name, string? description, string iconName, string colorHex, DateTime? startDate, DateTime? endDate, int xpPerCompletion)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ValidationException(nameof(name), "Habit name is required");
            }

            if (name.Length > MaxNameLength)
            {
                throw new ValidationException(nameof(name), $"Habit name cannot exceed {MaxNameLength} characters");
            }

            if (description?.Length > MaxDescriptionLength)
            {
                throw new ValidationException(nameof(description), $"Description cannot exceed {MaxDescriptionLength} characters");
            }

            if (string.IsNullOrWhiteSpace(iconName))
            {
                throw new ValidationException(nameof(iconName), "Icon name is required");
            }

            if (iconName.Length > MaxIconNameLength)
            {
                throw new ValidationException(nameof(iconName), $"Icon name cannot exceed {MaxIconNameLength} characters");
            }

            if (string.IsNullOrWhiteSpace(colorHex))
            {
                throw new ValidationException(nameof(colorHex), "Color is required");
            }

            if (!colorHex.StartsWith('#') || colorHex.Length != 7)
            {
                throw new ValidationException(nameof(colorHex), "Color must be in hex format (#RRGGBB)");
            }

            if (startDate.HasValue && endDate.HasValue && endDate.Value <= startDate.Value)
            {
                throw new ValidationException(nameof(endDate), "End date must be after start date");
            }

            if (xpPerCompletion < 5 || xpPerCompletion > 50)
            {
                throw new ValidationException(nameof(xpPerCompletion), "XP per completion must be between 5 and 50");
            }
        }
    }
}
