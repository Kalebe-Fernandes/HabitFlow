using HabitFlow.Aplicacao.Common.Interfaces;
using HabitFlow.Aplicacao.Common.Models;
using HabitFlow.Domain.Goals;
using HabitFlow.Domain.Repositories;
using MediatR;

namespace HabitFlow.Aplicacao.Features.Goals.Commands.AssociateHabit
{
    public sealed class AssociateHabitCommandHandler(IGoalRepository goalRepository, IHabitRepository habitRepository, IUnitOfWork unitOfWork) : IRequestHandler<AssociateHabitCommand, Result>
    {
        private readonly IGoalRepository _goalRepository = goalRepository;
        private readonly IHabitRepository _habitRepository = habitRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result> Handle(AssociateHabitCommand request, CancellationToken cancellationToken)
        {
            var goal = await _goalRepository.GetByIdAsync(request.GoalId, cancellationToken);

            if (goal is null)
            {
                return Result.Failure("Meta não encontrada");
            }

            if (goal.UserId != request.UserId)
            {
                return Result.Failure("Não autorizado");
            }

            var habit = await _habitRepository.GetByIdAsync(request.HabitId, cancellationToken);

            if (habit is null)
            {
                return Result.Failure("Hábito não encontrado");
            }

            if (habit.UserId != request.UserId)
            {
                return Result.Failure("Não autorizado");
            }

            // Verifica se já está associado
            if (goal.GoalHabits.Any(gh => gh.HabitId == request.HabitId))
            {
                return Result.Failure("Hábito já está associado a esta meta");
            }

            // Adiciona associação diretamente na coleção
            var goalHabit = new GoalHabit
            {
                GoalId = request.GoalId,
                HabitId = request.HabitId,
                ContributionWeight = request.ContributionWeight
            };

            goal.AddHabit(goalHabit);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
