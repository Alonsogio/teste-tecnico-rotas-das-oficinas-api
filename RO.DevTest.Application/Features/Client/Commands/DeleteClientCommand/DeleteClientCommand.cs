using MediatR;

namespace RO.DevTest.Application.Features.Client.Commands.DeleteClientCommand
{
  public class DeleteClientCommand : IRequest
  {
    public Guid Id { get; set; }
  }
}
