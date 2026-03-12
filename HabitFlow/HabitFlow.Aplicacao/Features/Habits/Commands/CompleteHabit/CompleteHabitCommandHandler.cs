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

            var completionDate = DateOnly.FromDateTime(request.CompletionDate.Date);

            habit.Complete(
                completionDate,
                request.CompletedValue,
                request.Notes,
                request.MoodLevel,
                request.EnergyLevel);

            // SaveChangesAsync dispatches domain events and populates the IDENTITY Id
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // After save, EF Core has populated the database-generated Id
            var completionId = habit.Completions
                .OrderByDescending(c => c.CreatedAt)
                .First(c => c.CompletionDate == completionDate)
                .Id;

            return Result.Success(new CompleteHabitResponse(
                completionId,
                habit.CurrentStreak,
                habit.XPPerCompletion));
        }
    }
}
