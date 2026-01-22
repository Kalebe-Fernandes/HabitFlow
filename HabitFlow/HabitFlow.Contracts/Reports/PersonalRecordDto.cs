namespace HabitFlow.Contracts.Reports
{
    /// <summary>
    /// Response DTO for personal records (RF-044).
    /// </summary>
    public record PersonalRecordDto(
        string RecordType, // LongestStreak, HighestValue, MostCompletionsInWeek, etc.
        string HabitName,
        Guid HabitId,
        decimal Value,
        string Unit,
        DateTime AchievedDate,
        bool IsCurrentRecord,
        decimal? PreviousRecord
    );
}
