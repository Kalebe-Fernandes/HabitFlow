using FluentValidation;

namespace HabitFlow.Aplicacao.Features.Users.Commands.UpdateProfile
{
    public sealed class UpdateProfileCommandValidator : AbstractValidator<UpdateProfileCommand>
    {
        public UpdateProfileCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty();

            RuleFor(x => x.FirstName)
                .MaximumLength(100)
                .WithMessage("Nome deve ter no máximo 100 caracteres")
                .When(x => !string.IsNullOrEmpty(x.FirstName));

            RuleFor(x => x.LastName)
                .MaximumLength(100)
                .WithMessage("Sobrenome deve ter no máximo 100 caracteres")
                .When(x => !string.IsNullOrEmpty(x.LastName));

            RuleFor(x => x.DisplayName)
                .MaximumLength(100)
                .WithMessage("Nome de exibição deve ter no máximo 100 caracteres")
                .When(x => !string.IsNullOrEmpty(x.DisplayName));
        }
    }

}
