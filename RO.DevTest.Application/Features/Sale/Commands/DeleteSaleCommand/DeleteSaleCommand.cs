using MediatR;

namespace RO.DevTest.Application.Features.Sales.Commands.DeleteSaleCommand
{
  public class DeleteSaleCommand : IRequest
  {
    public Guid Id { get; set; }
  }
}
