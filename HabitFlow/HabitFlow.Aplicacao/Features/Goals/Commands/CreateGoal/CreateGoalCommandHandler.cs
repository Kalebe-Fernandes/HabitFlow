using HabitFlow.Aplicacao.Common.Interfaces;
using HabitFlow.Aplicacao.Common.Models;
using HabitFlow.Domain.Goals;
using HabitFlow.Domain.Repositories;
using MediatR;

namespace HabitFlow.Aplicacao.Features.Goals.Commands.CreateGoal
{
    public class CreateGoalCommandHandler(IGoalRepository goalRepository, IUnitOfWork unitOfWork) : IRequestHandler<CreateGoalCommand, Result<CreateGoalResponse>>
    {
        private readonly IGoalRepository _goalRepository = goalRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<CreateGoalResponse>> Handle(CreateGoalCommand request, CancellationToken cancellationToken)
        {
            var goal = Goal.Create(
                request.UserId,
                request.Name,
                request.Description!,
                request.TargetValue,
                request.TargetUnit,
                request.StartDate,
                request.TargetDate
            );

            await _goalRepository.AddAsync(goal, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(new CreateGoalResponse(goal.Id, goal.Name));
        }
    }
}
