using FluentValidation;

namespace HabitFlow.Aplicacao.Features.Goals.Commands.AssociateHabit
{
    public sealed class AssociateHabitCommandValidator : AbstractValidator<AssociateHabitCommand>
    {
        public AssociateHabitCommandValidator()
        {
            RuleFor(x => x.GoalId)
                .NotEmpty()
                .WithMessage("GoalId é obrigatório");

            RuleFor(x => x.HabitId)
                .NotEmpty()
                .WithMessage("HabitId é obrigatório");

            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("UserId é obrigatório");

            RuleFor(x => x.ContributionWeight)
                .GreaterThan(0)
                .WithMessage("Peso de contribuição deve ser maior que 0")
                .LessThanOrEqualTo(100)
                .WithMessage("Peso de contribuição deve ser menor ou igual a 100");
        }
    }
}
