using MediatR;
using RO.DevTest.Application.Contracts.Persistence.Repositories;

namespace RO.DevTest.Application.Features.Sales.Commands.DeleteSaleCommand
{
    public class DeleteSaleCommandHandler : IRequestHandler<DeleteSaleCommand, Unit>
    {
        private readonly ISaleRepository _saleRepository;

        public DeleteSaleCommandHandler(ISaleRepository saleRepository)
        {
            _saleRepository = saleRepository;
        }

        public async Task<Unit> Handle(DeleteSaleCommand request, CancellationToken cancellationToken)
        {
          var sale = await _saleRepository.GetByIdAsync(request.Id);
            if (sale == null)
            {
                throw new Exception("Venda não encontrada");
            }
            await _saleRepository.DeleteAsync(request.Id);
            return Unit.Value; 
        }
    }
}
