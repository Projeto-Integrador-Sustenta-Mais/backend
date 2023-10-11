using FluentValidation;
using SustentaMais.Model;

namespace SustentaMais.Validator
{
    public class CategoriaValidator : AbstractValidator<Categoria>
    {
        public CategoriaValidator() {
            
            RuleFor(c => c.Tipo)
                .NotEmpty() 
                .MaximumLength(255);
        }
    }
}