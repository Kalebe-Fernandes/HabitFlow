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

        public Guid UserId { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string? Description { get; private set; }
        public string IconName { get; private set; } = string.Empty;
        public string ColorHex { get; private set; } = string.Empty;
        public HabitFrequency Frequency { get; private set; } = null!;
        public HabitTarget Target { get; private set; } = null!;
        public DateTime? StartDate { get; private set; }
        public DateTime? EndDate { get; private set; }
        public HabitStatus Status { get; private set; }
        public int? CategoryId { get; private set; }
        public int CurrentStreak { get; private set; }
        public int LongestStreak { get; private set; }
        public int TotalCompletions { get; private set; }
        public int XPPerCompletion { get; private set; }

        public IReadOnlyCollection<HabitCompletion> Completions => _completions.AsReadOnly();

        private Habit() { }

        private Habit(
            Guid userId, string name, string? description, string iconName, string colorHex,
            HabitFrequency frequency, HabitTarget target, DateTime? startDate, DateTime? endDate,
            int? categoryId, int xpPerCompletion)
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

        public static Habit Create(
            Guid userId, string name, string? description, string iconName, string colorHex,
            HabitFrequency frequency, HabitTarget target, DateTime? startDate = null,
            DateTime? endDate = null, int? categoryId = null, int xpPerCompletion = 10)
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
        /// Recalculates streak and raises domain events.
        /// </summary>
        public void Complete(
            DateOnly completionDate,
            decimal? completedValue = null,
            string? notes = null,
            int? moodLevel = null,
            int? energyLevel = null)
        {
            if (Status != HabitStatus.Active)
                throw new DomainException("Cannot complete an inactive habit", "HABIT_INACTIVE");

            if (_completions.Any(c => c.CompletionDate == completionDate))
                throw new DomainException("Habit already completed for this date", "DUPLICATE_COMPLETION");

            var completion = HabitCompletion.Create(Id, completionDate, completedValue, notes, moodLevel, energyLevel);

            _completions.Add(completion);
            TotalCompletions++;
            UpdatedAt = DateTime.UtcNow;

            RecalculateStreak(completionDate);

            AddDomainEvent(new HabitCompletedEvent(Id, UserId, completionDate, completedValue, XPPerCompletion));

            if (CurrentStreak > 0 && IsStreakMilestone(CurrentStreak))
                AddDomainEvent(new StreakAchievedEvent(Id, UserId, CurrentStreak));
        }

        /// <summary>
        /// Fully replaces all mutable fields of the habit.
        /// Use when all configuration data is available (e.g., from a dedicated settings screen).
        /// </summary>
        public void Update(
            string name, string? description, string iconName, string colorHex,
            HabitFrequency frequency, HabitTarget target, DateTime? startDate, DateTime? endDate,
            int? categoryId, int xpPerCompletion)
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
        /// Updates only the display details of the habit (name, description, icon, color).
        /// Fields passed as null are left unchanged.
        /// </summary>
        public void UpdateDetails(string name, string? description, string? iconName, string? colorHex)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ValidationException(nameof(name), "Habit name is required");

            if (name.Length > MaxNameLength)
                throw new ValidationException(nameof(name), $"Habit name cannot exceed {MaxNameLength} characters");

            if (description?.Length > MaxDescriptionLength)
                throw new ValidationException(nameof(description), $"Description cannot exceed {MaxDescriptionLength} characters");

            if (iconName is not null && iconName.Length > MaxIconNameLength)
                throw new ValidationException(nameof(iconName), $"Icon name cannot exceed {MaxIconNameLength} characters");

            if (colorHex is not null && (!colorHex.StartsWith('#') || colorHex.Length != MaxColorHexLength))
                throw new ValidationException(nameof(colorHex), "Color must be in hex format (#RRGGBB)");

            Name = name.Trim();
            Description = description?.Trim() ?? Description;
            IconName = iconName?.Trim() ?? IconName;
            ColorHex = colorHex?.Trim() ?? ColorHex;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Archive()
        {
            if (Status == HabitStatus.Archived) return;
            Status = HabitStatus.Archived;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Restore()
        {
            if (Status != HabitStatus.Archived) return;
            Status = HabitStatus.Active;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Pause()
        {
            if (Status == HabitStatus.Paused) return;
            Status = HabitStatus.Paused;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Resume()
        {
            if (Status != HabitStatus.Paused) return;
            Status = HabitStatus.Active;
            UpdatedAt = DateTime.UtcNow;
        }

        public decimal CalculateSuccessRate()
        {
            var expectedCompletions = CalculateExpectedCompletions();
            return expectedCompletions == 0
                ? 0m
                : (decimal)TotalCompletions / expectedCompletions * 100m;
        }

        private int CalculateExpectedCompletions()
        {
            var startDate = StartDate ?? CreatedAt;
            var daysSinceStart = (DateTime.UtcNow - startDate).Days + 1;

            if (daysSinceStart <= 0) return 0;

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
                LongestStreak = CurrentStreak;
        }

        private int CalculateCurrentStreak(List<HabitCompletion> orderedCompletions, DateOnly latestDate)
        {
            if (orderedCompletions.Count == 0) return 0;

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
                previousDate = previousDate.AddDays(-1);

            return previousDate;
        }

        private static bool IsStreakMilestone(int streak) => streak switch
        {
            7 or 14 or 21 or 30 or 60 or 90 or 100 or 365 => true,
            _ => streak % 100 == 0
        };

        private static void ValidateHabitInputs(
            string name, string? description, string iconName, string colorHex,
            DateTime? startDate, DateTime? endDate, int xpPerCompletion)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ValidationException(nameof(name), "Habit name is required");

            if (name.Length > MaxNameLength)
                throw new ValidationException(nameof(name), $"Habit name cannot exceed {MaxNameLength} characters");

            if (description?.Length > MaxDescriptionLength)
                throw new ValidationException(nameof(description), $"Description cannot exceed {MaxDescriptionLength} characters");

            if (string.IsNullOrWhiteSpace(iconName))
                throw new ValidationException(nameof(iconName), "Icon name is required");

            if (iconName.Length > MaxIconNameLength)
                throw new ValidationException(nameof(iconName), $"Icon name cannot exceed {MaxIconNameLength} characters");

            if (string.IsNullOrWhiteSpace(colorHex))
                throw new ValidationException(nameof(colorHex), "Color is required");

            if (!colorHex.StartsWith('#') || colorHex.Length != MaxColorHexLength)
                throw new ValidationException(nameof(colorHex), "Color must be in hex format (#RRGGBB)");

            if (startDate.HasValue && endDate.HasValue && endDate.Value <= startDate.Value)
                throw new ValidationException(nameof(endDate), "End date must be after start date");

            if (xpPerCompletion < 5 || xpPerCompletion > 50)
                throw new ValidationException(nameof(xpPerCompletion), "XP per completion must be between 5 and 50");
        }
    }
}
