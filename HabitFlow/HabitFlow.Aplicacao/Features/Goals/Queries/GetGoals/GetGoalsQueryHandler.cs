using HabitFlow.Aplicacao.Common.Models;
using HabitFlow.Domain.Repositories;
using MediatR;

namespace HabitFlow.Aplicacao.Features.Goals.Queries.GetGoals
{
    public class GetGoalsQueryHandler(IGoalRepository goalRepository) : IRequestHandler<GetGoalsQuery, Result<GetGoalsResponse>>
    {
        private readonly IGoalRepository _goalRepository = goalRepository;

        public async Task<Result<GetGoalsResponse>> Handle(GetGoalsQuery request, CancellationToken cancellationToken)
        {
            var goals = await _goalRepository.GetActiveByUserIdAsync(request.UserId, cancellationToken);

            var goalDtos = goals.Select(g => new GoalDto(
                g.Id,
                g.Name,
                g.Description,
                g.TargetValue,
                g.TargetUnit,
                g.CurrentValue,
                g.ProgressPercentage,
                g.StartDate,
                g.TargetDate,
                g.Status.ToString()
            ));

            return Result.Success(new GetGoalsResponse(goalDtos));
        }
    }
}
