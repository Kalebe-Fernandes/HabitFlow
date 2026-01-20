using HabitFlow.Aplicacao.Common.Interfaces;
using HabitFlow.Aplicacao.Common.Models;
using HabitFlow.Domain.Habits;
using HabitFlow.Domain.Habits.Enums;
using HabitFlow.Domain.Habits.ValueObjects;
using HabitFlow.Domain.Repositories;
using MediatR;

namespace HabitFlow.Aplicacao.Features.Habits.Commands.CreateHabit
{
    public class CreateHabitCommandHandler(IHabitRepository habitRepository, IUnitOfWork unitOfWork) : IRequestHandler<CreateHabitCommand, Result<CreateHabitResponse>>
    {
        private readonly IHabitRepository _habitRepository = habitRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<CreateHabitResponse>> Handle(CreateHabitCommand request, CancellationToken cancellationToken)
        {
            // Parse enums
            var frequencyType = Enum.Parse<FrequencyType>(request.FrequencyType);
            var targetType = Enum.Parse<TargetType>(request.TargetType);

            // Create frequency value object
            var dayOfWeek = request.DaysOfWeekFrequency.HasValue ? request.DaysOfWeekFrequency.Value : 0;
            var frequency = frequencyType == FrequencyType.Weekly 
                ? HabitFrequency.Weekly(dayOfWeek) 
                : frequencyType == FrequencyType.Custom ? HabitFrequency.Custom(dayOfWeek)
                : HabitFrequency.Daily();
                
            var target = targetType == TargetType.Binary ? HabitTarget.Binary() : HabitTarget.Numeric(request.TargetValue.Value, request.TargetUnit);

            // Create habit aggregate
            var habit = Habit.Create(
                request.UserId,
                request.Name,
                request.Description,
                request.IconName,
                request.ColorHex,
                frequency,
                target,
                request.StartDate,
                request.EndDate,
                request.CategoryId
            );

            await _habitRepository.AddAsync(habit, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(new CreateHabitResponse(habit.Id, habit.Name));
        }
    }
}
