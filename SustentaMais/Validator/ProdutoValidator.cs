using FluentValidation;
using SustentaMais.Model;

namespace SustentaMais.Validator
{
    public class ProdutoValidator : AbstractValidator<Produto>
    {
        public ProdutoValidator()
        {
            RuleFor(p => p.Nome)
                    .NotEmpty()
                    .MinimumLength(2)
                    .MaximumLength(50);

            RuleFor(p => p.Descricao)
                    .NotEmpty()
                    .MinimumLength(10)
                    .MaximumLength(1000);

            RuleFor(p => p.Preco)
                    .NotNull()
                    .GreaterThan(0)
                    .PrecisionScale(7, 2, false); 

            RuleFor(p => p.Foto)
                    .MaximumLength(5000);
        }

    }
}
           







