using MediatR;

namespace RO.DevTest.Application.Features.Clients.Commands.CreateClientCommand
{
  public class CreateClientCommand : IRequest<Guid>
  {
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
  }
}