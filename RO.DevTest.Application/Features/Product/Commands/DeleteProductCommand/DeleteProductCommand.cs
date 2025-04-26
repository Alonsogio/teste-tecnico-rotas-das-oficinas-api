using MediatR;

namespace RO.DevTest.Application.Features.Products.Commands.DeleteProductCommand
{
  public class DeleteProductCommand : IRequest<Unit>
  {
    public Guid Id { get; set; }
  }
}
