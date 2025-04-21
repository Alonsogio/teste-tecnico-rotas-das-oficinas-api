using FluentValidation;
using RO.DevTest.Application.Features.Products.Commands.CreateProductCommand;

namespace RO.DevTest.Application.Validators.Products
{
  public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
  {
    public CreateProductCommandValidator()
    {
      RuleFor(x => x.Nome)
          .NotEmpty().WithMessage("O nome do produto é obrigatório")
          .MaximumLength(100).WithMessage("Nome não pode ter mais de 100 caracteres");

      RuleFor(x => x.Descricao)
          .NotEmpty().WithMessage("A descrição é obrigatória");

      RuleFor(x => x.Preco)
          .GreaterThan(0).WithMessage("O preço deve ser maior que zero");

      RuleFor(x => x.Estoque)
          .GreaterThanOrEqualTo(0).WithMessage("O estoque não pode ser negativo");
    }
  }
}
