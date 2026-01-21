using FluentValidation;

namespace HabitFlow.Aplicacao.Features.Habits.Commands.UpdateHabit
{
    public sealed class UpdateHabitCommandValidator : AbstractValidator<UpdateHabitCommand>
    {
        public UpdateHabitCommandValidator()
        {
            RuleFor(x => x.HabitId)
                .NotEmpty();

            RuleFor(x => x.UserId)
                .NotEmpty();

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Nome é obrigatório")
                .MaximumLength(200)
                .WithMessage("Nome deve ter no máximo 200 caracteres");

            RuleFor(x => x.Description)
                .MaximumLength(1000)
                .WithMessage("Descrição deve ter no máximo 1000 caracteres")
                .When(x => !string.IsNullOrEmpty(x.Description));

            RuleFor(x => x.ColorHex)
                .Matches("^#[0-9A-Fa-f]{6}$")
                .WithMessage("ColorHex deve ser uma cor hexadecimal válida (ex: #FF5733)")
                .When(x => !string.IsNullOrEmpty(x.ColorHex));
        }
    }
}
