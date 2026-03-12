using HabitFlow.Aplicacao.Common.Interfaces;
using HabitFlow.Aplicacao.Common.Models;
using HabitFlow.Domain.Repositories;
using MediatR;

namespace HabitFlow.Aplicacao.Features.Habits.Commands.UpdateHabit
{
    public sealed class UpdateHabitCommandHandler(IHabitRepository habitRepository, IUnitOfWork unitOfWork) : IRequestHandler<UpdateHabitCommand, Result>
    {
        private readonly IHabitRepository _habitRepository = habitRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result> Handle(UpdateHabitCommand request, CancellationToken cancellationToken)
        {
            var habit = await _habitRepository.GetByIdAsync(request.HabitId, cancellationToken);

            if (habit is null)
                return Result.Failure("Habit not found");

            if (habit.UserId != request.UserId)
                return Result.Failure("Unauthorized");

            habit.UpdateDetails(request.Name, request.Description, request.IconName, request.ColorHex);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
