namespace HabitFlow.Contracts.Reports
{
    /// <summary>
    /// Data point for pattern visualization.
    /// </summary>
    public record PatternDataPointDto(
        string Label, // e.g., "Monday", "8 AM", "After Exercise"
        decimal Value,
        int Count
    );
}
