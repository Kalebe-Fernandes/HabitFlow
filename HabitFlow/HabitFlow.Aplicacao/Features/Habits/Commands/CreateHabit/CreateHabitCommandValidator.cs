using FluentValidation;

namespace HabitFlow.Aplicacao.Features.Habits.Commands.CreateHabit
{
    public class CreateHabitCommandValidator : AbstractValidator<CreateHabitCommand>
    {
        public CreateHabitCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Description).MaximumLength(1000);
            RuleFor(x => x.IconName).NotEmpty();
            RuleFor(x => x.ColorHex).NotEmpty().Matches("^#[0-9A-Fa-f]{6}$");
            RuleFor(x => x.FrequencyType).NotEmpty().Must(x => new[] { "Daily", "Weekly", "Custom" }.Contains(x));
            RuleFor(x => x.TargetType).NotEmpty().Must(x => new[] { "Binary", "Numeric" }.Contains(x));

            When(x => x.TargetType == "Numeric", () =>
            {
                RuleFor(x => x.TargetValue).NotNull().GreaterThan(0);
                RuleFor(x => x.TargetUnit).NotEmpty();
            });

            When(x => x.EndDate.HasValue, () =>
            {
                RuleFor(x => x.StartDate).NotNull();
                RuleFor(x => x.EndDate).GreaterThan(x => x.StartDate);
            });
        }
    }
}
