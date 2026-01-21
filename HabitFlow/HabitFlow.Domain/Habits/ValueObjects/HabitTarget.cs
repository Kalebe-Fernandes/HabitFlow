using HabitFlow.Domain.Common.Exceptions;
using HabitFlow.Domain.Common.Models;
using HabitFlow.Domain.Habits.Enums;

namespace HabitFlow.Domain.Habits.ValueObjects
{
    /// <summary>
    /// Value object representing the target configuration of a habit.
    /// </summary>
    public sealed class HabitTarget : ValueObject
    {
        private const int MaxValueLength = 50;
        private const int MaxUnitLength = 20;

        /// <summary>
        /// Gets the target type (Binary or Numeric).
        /// </summary>
        public TargetType Type { get; private set; }

        /// <summary>
        /// Gets the target value (for Numeric type).
        /// Null for Binary type.
        /// </summary>
        public decimal? Value { get; private set; }

        /// <summary>
        /// Gets the unit of measurement (e.g., "minutos", "páginas", "copos").
        /// Null for Binary type.
        /// </summary>
        public string? Unit { get; private set; }

        private HabitTarget(TargetType type, decimal? value, string? unit)
        {
            Type = type;
            Value = value;
            Unit = unit;
        }

        /// <summary>
        /// Creates a binary target (yes/no completion).
        /// </summary>
        public static HabitTarget Binary()
        {
            return new HabitTarget(TargetType.Binary, null, null);
        }

        /// <summary>
        /// Creates a numeric target with a specific value and unit.
        /// </summary>
        /// <param name="value">The target value to achieve</param>
        /// <param name="unit">The unit of measurement</param>
        public static HabitTarget Numeric(decimal value, string unit)
        {
            if (value <= 0)
            {
                throw new ValidationException(nameof(value), "Target value must be greater than zero");
            }

            if (string.IsNullOrWhiteSpace(unit))
            {
                throw new ValidationException(nameof(unit), "Unit is required for numeric targets");
            }

            if (unit.Length > MaxUnitLength)
            {
                throw new ValidationException(nameof(unit), $"Unit cannot exceed {MaxUnitLength} characters");
            }

            return new HabitTarget(TargetType.Numeric, value, unit.Trim());
        }

        /// <summary>
        /// Checks if a given value meets this target.
        /// </summary>
        /// <param name="completedValue">The value to check</param>
        public bool IsMet(decimal completedValue)
        {
            if (Type == TargetType.Binary)
            {
                return completedValue > 0;
            }

            return completedValue >= Value!.Value;
        }

        /// <summary>
        /// Calculates the percentage of completion for a given value.
        /// </summary>
        /// <param name="completedValue">The completed value</param>
        /// <returns>Percentage between 0 and 100</returns>
        public decimal CalculatePercentage(decimal completedValue)
        {
            if (Type == TargetType.Binary)
            {
                return completedValue > 0 ? 100m : 0m;
            }

            if (Value!.Value == 0)
            {
                return 0m;
            }

            var percentage = (completedValue / Value.Value) * 100m;
            return Math.Min(100m, Math.Max(0m, percentage));
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Type;
            yield return Value;
            yield return Unit;
        }
    }
}
