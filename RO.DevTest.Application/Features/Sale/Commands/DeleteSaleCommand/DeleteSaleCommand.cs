using MediatR;

namespace RO.DevTest.Application.Features.Sales.Commands.DeleteSaleCommand
{
  public class DeleteSaleCommand : IRequest<Unit>
  {
    public Guid Id { get; set; }
  }
}
