using MediatR;

namespace RO.DevTest.Application.Features.Clients.Commands.DeleteClientCommand
{
  public class DeleteClientCommand : IRequest
  {
    public Guid Id { get; set; }
  }
}
