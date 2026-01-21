using HabitFlow.Aplicacao.Common.Models;
using HabitFlow.Domain.Repositories;
using MediatR;

namespace HabitFlow.Aplicacao.Features.Gamification.Queries.GetUserLevel
{
    /// <summary>
    /// Handler para consultar informações de nível do usuário.
    /// </summary>
    public sealed class GetUserLevelQueryHandler(IUserLevelRepository userLevelRepository) : IRequestHandler<GetUserLevelQuery, Result<UserLevelResponse>>
    {
        private readonly IUserLevelRepository _userLevelRepository = userLevelRepository;

        public async Task<Result<UserLevelResponse>> Handle(GetUserLevelQuery request, CancellationToken cancellationToken)
        {
            var userLevel = await _userLevelRepository.GetByUserIdAsync(request.UserId, cancellationToken);

            if (userLevel is null)
                return Result.Failure<UserLevelResponse>("Nível do usuário não encontrado");

            // CORREÇÃO: Calcular propriedades dinamicamente
            // UserLevel não tem essas propriedades, então calculamos aqui

            // Fórmula: XP_Required(N) = 100 × N²
            var nextLevel = userLevel.CurrentLevel + 1;
            var xpForNextLevel = CalculateXPForLevel(nextLevel);
            var xpRequiredForNextLevel = xpForNextLevel - userLevel.TotalXP;

            // Calcular progresso para próximo nível
            var xpForCurrentLevel = CalculateXPForLevel(userLevel.CurrentLevel);
            var xpInCurrentLevel = userLevel.TotalXP - xpForCurrentLevel;
            var xpNeededInCurrentLevel = xpForNextLevel - xpForCurrentLevel;
            var progressToNextLevel = xpNeededInCurrentLevel > 0
                ? (decimal)xpInCurrentLevel / xpNeededInCurrentLevel * 100
                : 0;

            var response = new UserLevelResponse(
                userLevel.CurrentLevel,
                userLevel.TotalXP,
                xpRequiredForNextLevel,
                Math.Round(progressToNextLevel, 2),
                userLevel.Badges.Count,
                userLevel.CurrentBalance); // CORREÇÃO: usar CurrentBalance (não CurrencyBalance)

            return Result.Success(response);
        }

        /// <summary>
        /// Calcula o XP total necessário para atingir determinado nível.
        /// Fórmula: 100 × N²
        /// </summary>
        private static long CalculateXPForLevel(int level)
        {
            return 100L * level * level;
        }
    }
}
