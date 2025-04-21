using FluentValidation;
using RO.DevTest.Application.Features.Clients.Commands.CreateClientCommand;

namespace RO.DevTest.Application.Validators.Clients
{
  public class CreateClientCommandValidator : AbstractValidator<CreateClientCommand>
  {
    public CreateClientCommandValidator()
    {
      RuleFor(x => x.Nome)
          .NotEmpty().WithMessage("O nome é obrigatório")
          .MaximumLength(100).WithMessage("O nome não pode ter mais de 100 caracteres");

      RuleFor(x => x.Email)
          .NotEmpty().WithMessage("O e-mail é obrigatório")
          .EmailAddress().WithMessage("Formato de e-mail inválido");

      RuleFor(x => x.Telefone)
          .NotEmpty().WithMessage("O telefone é obrigatório")
          .Matches(@"^\(?\d{2}\)?\s?\d{4,5}-\d{4}$")
          .WithMessage("Telefone deve estar no formato (99) 99999-9999 ou 9999-9999");
    }
  }
}
