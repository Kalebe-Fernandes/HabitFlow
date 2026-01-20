using HabitFlow.Aplicacao.Common.Interfaces;

namespace HabitFlow.Aplicacao.Features.Gamification.Queries.GetUserLevel
{
    public record GetUserLevelQuery(Guid UserId) : IQuery<UserLevelResponse>;

    public record UserLevelResponse(
        int CurrentLevel,
        long TotalXP,
        long XPForNextLevel,
        decimal ProgressToNextLevel,
        int BadgesCount,
        int CurrencyBalance
    );
}
