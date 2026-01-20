namespace HabitFlow.Aplicacao.Features.Habits.Queries.GetHabits
{
    public record HabitDto(
        Guid Id,
        string Name,
        string? Description,
        string IconName,
        string ColorHex,
        string FrequencyType,
        string TargetType,
        decimal? TargetValue,
        string? TargetUnit,
        int CurrentStreak,
        int LongestStreak,
        int TotalCompletions,
        string Status
    );
}
