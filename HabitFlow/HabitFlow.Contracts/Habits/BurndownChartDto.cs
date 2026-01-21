namespace HabitFlow.Contracts.Habits
{
    public record BurndownChartDto(
        Guid HabitId,
        string HabitName,
        DateTime StartDate,
        DateTime EndDate,
        List<BurndownPointDto> IdealLine,
        List<BurndownPointDto> ActualLine,
        DateTime? ProjectedCompletionDate
    );
}
