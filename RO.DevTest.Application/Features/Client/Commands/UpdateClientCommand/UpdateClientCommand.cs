using MediatR;

namespace RO.DevTest.Application.Features.Clients.Commands.UpdateClientCommand
{
  public class UpdateClientCommand : IRequest<Unit>
  {
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
  }
}