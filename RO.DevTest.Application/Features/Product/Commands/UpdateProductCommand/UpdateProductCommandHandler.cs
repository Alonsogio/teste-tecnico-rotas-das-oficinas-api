using MediatR;
using RO.DevTest.Application.Contracts.Persistence.Repositories;

namespace RO.DevTest.Application.Features.Products.Commands.UpdateProductCommand
{
  public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Unit>
  {
    private readonly IProductRepository _productRepository;

    public UpdateProductCommandHandler(IProductRepository productRepository)
    {
      _productRepository = productRepository;
    }

    public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
      var product = await _productRepository.GetByIdAsync(request.Id);
      if (product == null)
        throw new Exception("Produto não encontrado");

      product.Nome = request.Nome;
      product.Descricao = request.Descricao;
      product.Estoque = request.Estoque;
      product.Preco = request.Preco;

      await _productRepository.Update(product);
      return Unit.Value;
    }
  }
}
