using HabitFlow.Aplicacao.Common.Interfaces;
using HabitFlow.Aplicacao.Common.Models;
using HabitFlow.Domain.Gamification;
using HabitFlow.Domain.Repositories;
using MediatR;

namespace HabitFlow.Aplicacao.Features.Gamification.Commands.AwardXP
{
    public class AwardXPCommandHandler(IUserLevelRepository userLevelRepository, IUnitOfWork unitOfWork) : IRequestHandler<AwardXPCommand, Result<AwardXPResponse>>
    {
        private readonly IUserLevelRepository _userLevelRepository = userLevelRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<AwardXPResponse>> Handle(AwardXPCommand request, CancellationToken cancellationToken)
        {
            var userLevel = await _userLevelRepository.GetByUserIdAsync(request.UserId, cancellationToken);

            if (userLevel is null)
            {
                userLevel = UserLevel.Create(request.UserId);
                await _userLevelRepository.AddAsync(userLevel, cancellationToken);
            }

            var previousLevel = userLevel.CurrentLevel;
            userLevel.AwardXP(request.Amount, request.Reason, request.RelatedEntityId);
            var leveledUp = userLevel.CurrentLevel > previousLevel;

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(new AwardXPResponse(
                (int)userLevel.TotalXP,
                userLevel.CurrentLevel,
                leveledUp));
        }
    }
}
