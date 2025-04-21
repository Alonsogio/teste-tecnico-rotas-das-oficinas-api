using FluentValidation;
using RO.DevTest.Application.Features.Products.Commands.UpdateProductCommand;

namespace RO.DevTest.Application.Validators.Products
{
  public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
  {
    public UpdateProductCommandValidator()
    {
      RuleFor(x => x.Id)
          .NotEmpty().WithMessage("O ID é obrigatório");

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
