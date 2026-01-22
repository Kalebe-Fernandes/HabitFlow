namespace HabitFlow.Contracts.Habits
{
    public record HabitCalendarDto(
        Guid HabitId,
        string HabitName,
        Dictionary<DateOnly, CompletionStatusDto> CompletionsByDate
    );
}
