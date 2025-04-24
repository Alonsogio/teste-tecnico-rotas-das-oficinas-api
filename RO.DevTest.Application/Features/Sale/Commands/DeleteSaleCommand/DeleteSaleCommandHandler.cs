using MediatR;
using RO.DevTest.Application.Contracts.Persistence.Repositories;

namespace RO.DevTest.Application.Features.Sales.Commands.DeleteSaleCommand
{
  public class DeleteSaleCommandHandler(ISaleRepository saleRepository) : IRequestHandler<DeleteSaleCommand, Unit>
  {
    private readonly ISaleRepository _saleRepository = saleRepository;

   public async Task<Unit> Handle(DeleteSaleCommand request, CancellationToken cancellationToken)
    {
      await _saleRepository.DeleteAsync(request.Id);
      return Unit.Value;
    }
  }
}