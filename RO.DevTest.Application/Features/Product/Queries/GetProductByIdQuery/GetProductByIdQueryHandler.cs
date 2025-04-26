using MediatR;
using RO.DevTest.Application.Contracts.Persistence.Repositories;
using RO.DevTest.Domain.Entities;

namespace RO.DevTest.Application.Features.Products.Queries.GetProductByIdQuery
{
  public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Product>
  {
    private readonly IProductRepository _productRepository;

    public GetProductByIdQueryHandler(IProductRepository productRepository)
    {
      _productRepository = productRepository;
    }

    public async Task<Product> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
      var product = await _productRepository.GetByIdAsync(request.Id);

      if (product == null)
        throw new KeyNotFoundException($"Produto com ID '{request.Id}' não foi encontrado.");

      return product;
    }
  }
}