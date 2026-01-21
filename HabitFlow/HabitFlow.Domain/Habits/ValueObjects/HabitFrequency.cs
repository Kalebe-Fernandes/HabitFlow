using HabitFlow.Domain.Common.Exceptions;
using HabitFlow.Domain.Common.Models;
using HabitFlow.Domain.Habits.Enums;

namespace HabitFlow.Domain.Habits.ValueObjects
{
    /// <summary>
    /// Value object representing the frequency configuration of a habit.
    /// </summary>
    public sealed class HabitFrequency : ValueObject
    {
        /// <summary>
        /// Gets the frequency type.
        /// </summary>
        public FrequencyType Type { get; private set; }

        /// <summary>
        /// Gets the specific days of the week (for Weekly or Custom types).
        /// Bit flags: Sunday=1, Monday=2, Tuesday=4, Wednesday=8, Thursday=16, Friday=32, Saturday=64
        /// </summary>
        public int DaysOfWeek { get; private set; }

        private HabitFrequency(FrequencyType type, int daysOfWeek)
        {
            Type = type;
            DaysOfWeek = daysOfWeek;
        }

        /// <summary>
        /// Creates a daily frequency (every day of the week).
        /// </summary>
        public static HabitFrequency Daily()
        {
            return new HabitFrequency(FrequencyType.Daily, 127); // All days (1+2+4+8+16+32+64)
        }

        /// <summary>
        /// Creates a weekly frequency with specific days.
        /// </summary>
        /// <param name="daysOfWeek">Bit flags for days of the week</param>
        public static HabitFrequency Weekly(int daysOfWeek)
        {
            if (daysOfWeek < 1 || daysOfWeek > 127)
            {
                throw new ValidationException(nameof(daysOfWeek), "Days of week must be between 1 and 127");
            }

            return new HabitFrequency(FrequencyType.Weekly, daysOfWeek);
        }

        /// <summary>
        /// Creates a custom frequency with specific days.
        /// </summary>
        /// <param name="daysOfWeek">Bit flags for days of the week</param>
        public static HabitFrequency Custom(int daysOfWeek)
        {
            if (daysOfWeek < 1 || daysOfWeek > 127)
            {
                throw new ValidationException(nameof(daysOfWeek), "Days of week must be between 1 and 127");
            }

            return new HabitFrequency(FrequencyType.Custom, daysOfWeek);
        }

        /// <summary>
        /// Checks if a specific day of week is included in this frequency.
        /// </summary>
        /// <param name="dayOfWeek">The day of week to check (0=Sunday, 6=Saturday)</param>
        public bool IncludesDay(DayOfWeek dayOfWeek)
        {
            var dayFlag = 1 << (int)dayOfWeek;
            return (DaysOfWeek & dayFlag) == dayFlag;
        }

        /// <summary>
        /// Gets the number of days per week for this frequency.
        /// </summary>
        public int DaysPerWeek()
        {
            var count = 0;
            var days = DaysOfWeek;

            while (days > 0)
            {
                count += days & 1;
                days >>= 1;
            }

            return count;
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Type;
            yield return DaysOfWeek;
        }
    }
}
