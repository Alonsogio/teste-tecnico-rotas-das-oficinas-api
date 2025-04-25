using System.Reflection.Metadata;
using MediatR;
using RO.DevTest.Application.Contracts.Persistence.Repositories;
using RO.DevTest.Domain.Exception;

namespace RO.DevTest.Application.Features.Products.Commands.DeleteProductCommand
{
  public class DeleteProductCommandHandler(IProductRepository productRepository) : IRequestHandler<DeleteProductCommand, Unit>
  {
    private readonly IProductRepository _productRepository = productRepository;

    public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
      var product = await _productRepository.GetByIdAsync(request.Id);
      if (product == null)
        throw new BadRequestException("Produto não encontrado");

      await _productRepository.DeleteAsync(request.Id);
      return Unit.Value;
    }
  }
}