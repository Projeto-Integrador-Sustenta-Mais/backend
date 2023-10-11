using FluentValidation;
using SustentaMais.Model;

namespace SustentaMais.Validator
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(u => u.Nome)
                    .NotEmpty()
                    .MinimumLength(2)
                    .MaximumLength(50);

            RuleFor(u => u.Usuario)
                    .NotEmpty()
                    .MinimumLength(10)
                    .MaximumLength(50);

            RuleFor(u => u.Senha)
                    .NotEmpty()
                    .MinimumLength(8)
                    .MaximumLength(255);

            RuleFor(u => u.Foto)
                    .MaximumLength(5000);
        }
    }
}


