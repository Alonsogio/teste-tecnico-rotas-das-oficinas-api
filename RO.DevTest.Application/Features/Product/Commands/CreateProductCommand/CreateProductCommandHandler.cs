using MediatR;
using RO.DevTest.Application.Contracts.Persistence.Repositories;
using RO.DevTest.Domain.Entities;

namespace RO.DevTest.Application.Features.Products.Commands.CreateProductCommand
{
  public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
  {
    private readonly IProductRepository _productRepository;

    public CreateProductCommandHandler(IProductRepository productRepository)
    {
      _productRepository = productRepository;
    }

    public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
      var product = new Product
      {
        Nome = request.Nome,
        Descricao = request.Descricao,
        Preco = request.Preco,
        Estoque = request.Estoque
      };

      await _productRepository.AddAsync(product);
      return product.Id;
    }
  }
}