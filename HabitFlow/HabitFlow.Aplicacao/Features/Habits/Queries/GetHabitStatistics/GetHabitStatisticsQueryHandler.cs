using HabitFlow.Aplicacao.Common.Models;
using HabitFlow.Domain.Habits;
using HabitFlow.Domain.Repositories;
using MediatR;

namespace HabitFlow.Aplicacao.Features.Habits.Queries.GetHabitStatistics
{
    public sealed class GetHabitStatisticsQueryHandler(IHabitRepository habitRepository) : IRequestHandler<GetHabitStatisticsQuery, Result<HabitStatisticsDto>>
    {
        private readonly IHabitRepository _habitRepository = habitRepository;

        public async Task<Result<HabitStatisticsDto>> Handle(GetHabitStatisticsQuery request, CancellationToken cancellationToken)
        {
            var habit = await _habitRepository.GetByIdWithCompletionsAsync(request.HabitId, cancellationToken);
            if (habit is null)
            {
                return Result.Failure<HabitStatisticsDto>("Habit not found");
            }

            if (habit.UserId != request.UserId)
            {
                return Result.Failure<HabitStatisticsDto>("Unauthorized");
            }

            var statistics = CalculateStatistics(habit);
            return Result.Success(statistics);
        }

        private static HabitStatisticsDto CalculateStatistics(Habit habit)
        {
            var daysSinceCreation = (DateTime.UtcNow - habit.CreatedAt).Days;
            var expectedCompletions = CalculateExpectedCompletions(habit, daysSinceCreation);

            var successRate = expectedCompletions > 0
                ? (decimal)habit.TotalCompletions / expectedCompletions * 100
                : 0;

            var lastCompletion = habit.Completions
                .OrderByDescending(c => c.CompletionDate)
                .FirstOrDefault();

            return new HabitStatisticsDto(
                habit.Id,
                habit.Name,
                habit.CurrentStreak,
                habit.LongestStreak,
                habit.TotalCompletions,
                Math.Round(successRate, 2),
                Convert.ToDateTime(lastCompletion?.CompletionDate),
                daysSinceCreation);
        }

        private static int CalculateExpectedCompletions(Habit habit, int daysSinceCreation)
        {
            return habit.Frequency.Type.ToString().ToLowerInvariant() switch
            {
                "daily" => daysSinceCreation,
                "weekly" => habit.Frequency.DaysOfWeek * (daysSinceCreation / 7),
                _ => daysSinceCreation
            };
        }
    }
}
