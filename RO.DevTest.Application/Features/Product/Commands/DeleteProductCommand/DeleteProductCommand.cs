using MediatR;

namespace RO.DevTest.Application.Features.Products.Commands.DeleteProductCommand
{
  public class DeleteProductCommand : IRequest
  {
    public Guid Id { get; set; }
  }
}
