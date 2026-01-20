using HabitFlow.Aplicacao.Common.Interfaces;
using HabitFlow.Aplicacao.Common.Models;
using HabitFlow.Domain.Repositories;
using MediatR;

namespace HabitFlow.Aplicacao.Features.Habits.Commands.CompleteHabit
{
    public class CompleteHabitCommandHandler(IHabitRepository habitRepository, IUnitOfWork unitOfWork) : IRequestHandler<CompleteHabitCommand, Result<CompleteHabitResponse>>
    {
        private readonly IHabitRepository _habitRepository = habitRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<CompleteHabitResponse>> Handle(CompleteHabitCommand request, CancellationToken cancellationToken)
        {
            var habit = await _habitRepository.GetByIdWithCompletionsAsync(request.HabitId, cancellationToken);
            if (habit is null) return Result.Failure<CompleteHabitResponse>("Habit not found");
            if (habit.UserId != request.UserId) return Result.Failure<CompleteHabitResponse>("Unauthorized");

            // Complete habit (domain logic handles streak calculation)
            habit.Complete(
                DateOnly.FromDateTime(request.CompletionDate.Date),
                request.CompletedValue,
                request.Notes,
                request.MoodLevel,
                request.EnergyLevel
            );

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Domain events (HabitCompletedEvent, StreakAchievedEvent) will be dispatched by Infrastructure

            return Result.Success(new CompleteHabitResponse(
                Guid.NewGuid(),
                habit.CurrentStreak,
                10 // Default XP, will be refined by Gamification handler
            ));
        }
    }
}
