using HabitFlow.Aplicacao.Common.Interfaces;

namespace HabitFlow.Aplicacao.Features.Habits.Commands.CompleteHabit
{
    public record CompleteHabitCommand(
            Guid HabitId,
            Guid UserId,
            DateTime CompletionDate,
            decimal? CompletedValue,
            string? Notes,
            int? MoodLevel,
            int? EnergyLevel
        ) : ICommand<CompleteHabitResponse>;

    /// <summary>
    /// CompletionId is long because HabitCompletion uses Entity&lt;long&gt;
    /// with a database-generated IDENTITY value.
    /// </summary>
    public record CompleteHabitResponse(long CompletionId, int CurrentStreak, int XPAwarded);

    public record CompleteHabitRequest(
            DateTime CompletionDate,
            decimal? CompletedValue,
            string? Notes,
            int? MoodLevel,
            int? EnergyLevel);
}
