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

    public record CompleteHabitResponse(Guid CompletionId, int CurrentStreak, int XPAwarded);
}
