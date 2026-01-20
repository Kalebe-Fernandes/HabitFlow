using HabitFlow.Aplicacao.Common.Models;
using HabitFlow.Domain.Habits.Enums;
using HabitFlow.Domain.Repositories;
using MediatR;

namespace HabitFlow.Aplicacao.Features.Habits.Queries.GetHabits
{
    public class GetHabitsQueryHandler(IHabitRepository habitRepository) : IRequestHandler<GetHabitsQuery, Result<GetHabitsResponse>>
    {
        private readonly IHabitRepository _habitRepository = habitRepository;

        public async Task<Result<GetHabitsResponse>> Handle(GetHabitsQuery request, CancellationToken cancellationToken)
        {
            var status = Enum.Parse<HabitStatus>(request.Status);
            var habits = await _habitRepository.GetPagedByUserIdAsync(
                request.UserId,
                request.Page,
                request.PageSize,
                cancellationToken
            );

            var habitDtos = habits.Habits.Where(h => h.Status == status).Select(h => new HabitDto(
                h.Id,
                h.Name,
                h.Description,
                h.IconName,
                h.ColorHex,
                h.Frequency.Type.ToString(),
                h.Target.Type.ToString(),
                h.Target.Value,
                h.Target.Unit,
                h.CurrentStreak,
                h.LongestStreak,
                h.TotalCompletions,
                h.Status.ToString()
            ));

            var response = new GetHabitsResponse(
                habitDtos,
                habits.TotalCount,
                request.Page,
                request.PageSize
            );

            return Result.Success(response);
        }
    }
}
