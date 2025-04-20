using MediatR;

namespace RO.DevTest.Application.Features.Clientes.Commands.UpdateClienteCommand
{
  public class UpdateClienteCommand : IRequest
  {
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
  }
}