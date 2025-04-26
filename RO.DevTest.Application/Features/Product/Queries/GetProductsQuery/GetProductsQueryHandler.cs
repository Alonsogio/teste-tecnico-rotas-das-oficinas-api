using MediatR;
using RO.DevTest.Domain.Entities;
using RO.DevTest.Application.Contracts.Persistence.Repositories;

namespace RO.DevTest.Application.Features.Products.Queries.GetProductsQuery
{
  public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, IReadOnlyList<Product>>
  {
    private readonly IProductRepository _productRepository;

    public GetProductsQueryHandler(IProductRepository productRepository)
    {
      _productRepository = productRepository;
    }

    public async Task<IReadOnlyList<Product>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
      var todos = (await _productRepository.GetAllAsync()).ToList();

      if (!string.IsNullOrWhiteSpace(request.Nome))
      {
        todos = todos.Where(c => c.Nome.Contains(request.Nome, StringComparison.OrdinalIgnoreCase)).ToList();
      }

      return todos
          .Skip((request.PageNumber - 1) * request.PageSize)
          .Take(request.PageSize)
          .ToList();
    }
  }
}
