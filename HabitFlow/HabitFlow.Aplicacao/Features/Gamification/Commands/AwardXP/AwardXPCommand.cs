using HabitFlow.Aplicacao.Common.Interfaces;

namespace HabitFlow.Aplicacao.Features.Gamification.Commands.AwardXP
{
    public record AwardXPCommand(
            Guid UserId,
            int Amount,
            string Reason,
            Guid? RelatedEntityId = null
        ) : ICommand<AwardXPResponse>;

    public record AwardXPResponse(int TotalXP, int CurrentLevel, bool LeveledUp);
}
