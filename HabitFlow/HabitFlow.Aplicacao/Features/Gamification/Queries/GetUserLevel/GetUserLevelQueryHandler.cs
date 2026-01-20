using HabitFlow.Aplicacao.Common.Models;
using HabitFlow.Domain.Repositories;
using MediatR;

namespace HabitFlow.Aplicacao.Features.Gamification.Queries.GetUserLevel
{
    public class GetUserLevelQueryHandler(IGamificationRepository gamificationRepository) : IRequestHandler<GetUserLevelQuery, Result<UserLevelResponse>>
    {
        private readonly IGamificationRepository _gamificationRepository = gamificationRepository;

        public async Task<Result<UserLevelResponse>> Handle(GetUserLevelQuery request, CancellationToken cancellationToken)
        {
            var userLevel = await _gamificationRepository.GetByUserIdAsync(request.UserId, cancellationToken);
            if (userLevel is null) return Result.Failure<UserLevelResponse>("User level not found");

            var response = new UserLevelResponse(
                userLevel.CurrentLevel,
                userLevel.TotalXP,
                userLevel.XPRequiredForNextLevel,
                userLevel.ProgressToNextLevel,
                userLevel.Badges.Count,
                userLevel.CurrencyBalance
            );

            return Result.Success(response);
        }
    }
}
