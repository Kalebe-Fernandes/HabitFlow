using HabitFlow.Aplicacao.Common.Interfaces;

namespace HabitFlow.Aplicacao.Features.Habits.Commands.CreateHabit
{
    public record CreateHabitCommand(
            Guid UserId,
            string Name,
            string? Description,
            string IconName,
            string ColorHex,
            string FrequencyType, // "Daily", "Weekly", "Custom"
            string TargetType, // "Binary", "Numeric"
            decimal? TargetValue,
            string? TargetUnit,
            int? DaysOfWeekFrequency,
            DateTime? StartDate,
            DateTime? EndDate,
            int CategoryId
        ) : ICommand<CreateHabitResponse>;

    public record CreateHabitResponse(Guid HabitId, string Name);

    public record CreateHabitRequest(
            string Name,
            string? Description,
            string IconName,
            string ColorHex,
            string FrequencyType,
            string TargetType,
            decimal? TargetValue,
            string? TargetUnit,
            int? DaysOfWeekFrequency,
            DateTime? StartDate,
            DateTime? EndDate,
            int? CategoryId);
}
