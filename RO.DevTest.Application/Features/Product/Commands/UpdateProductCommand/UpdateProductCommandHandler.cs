using MediatR;
using RO.DevTest.Application.Contracts.Persistence.Repositories;
using RO.DevTest.Domain.Entities;
using RO.DevTest.Domain.Exception;

namespace RO.DevTest.Application.Features.Products.Commands.UpdateProductCommand
{
  public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Product>
  {
    private readonly IProductRepository _productRepository;

    public UpdateProductCommandHandler(IProductRepository productRepository)
    {
      _productRepository = productRepository;
    }

    public async Task<Product> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
      if (string.IsNullOrWhiteSpace(request.Nome))
        throw new BadRequestException("Nome é obrigatório");

      var product = await _productRepository.GetByIdAsync(request.Id);
      if (product == null)
        throw new BadRequestException("Produto não encontrado");

      product.Nome = request.Nome;
      product.Descricao = request.Descricao;
      product.Estoque = request.Estoque;
      product.Preco = request.Preco;

      await _productRepository.Update(product);
      return product;
    }
  }
}
