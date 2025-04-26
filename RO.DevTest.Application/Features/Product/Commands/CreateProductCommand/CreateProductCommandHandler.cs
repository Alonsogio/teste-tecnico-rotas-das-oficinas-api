using MediatR;
using RO.DevTest.Application.Contracts.Persistence.Repositories;
using RO.DevTest.Domain.Entities;
using RO.DevTest.Domain.Exception;

namespace RO.DevTest.Application.Features.Products.Commands.CreateProductCommand
{
  public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Product>
  {
    private readonly IProductRepository _productRepository;

    public CreateProductCommandHandler(IProductRepository productRepository)
    {
      _productRepository = productRepository;
    }

    public async Task<Product> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
      if (string.IsNullOrWhiteSpace(request.Nome))
        throw new BadRequestException("O nome do produto é obrigatório.");

      if (string.IsNullOrWhiteSpace(request.Descricao))
        throw new BadRequestException("A descrição do produto é obrigatória.");

      if (request.Preco <= 0)
        throw new BadRequestException("O preço deve ser maior que zero.");

      if (request.Estoque < 0)
        throw new BadRequestException("O estoque não pode ser negativo.");

      var product = new Product
      {
        Nome = request.Nome,
        Descricao = request.Descricao,
        Preco = request.Preco,
        Estoque = request.Estoque
      };

      await _productRepository.AddAsync(product);
      return product;
    }
  }
}