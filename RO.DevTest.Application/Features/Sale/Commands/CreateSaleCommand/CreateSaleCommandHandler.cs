using MediatR;
using RO.DevTest.Application.Contracts.Persistence.Repositories;
using RO.DevTest.Domain.Entities;

namespace RO.DevTest.Application.Features.Sales.Commands.CreateSaleCommand
{
  public class CreateSaleCommandHandler : IRequestHandler<CreateSaleCommand, Guid>
  {
    private readonly ISaleRepository _saleRepository;
    private readonly IClientRepository _clientRepository;
    private readonly IProductRepository _productRepository;

    public CreateSaleCommandHandler(ISaleRepository saleRepository, IClientRepository clientRepository, IProductRepository productRepository)
    {
      _saleRepository = saleRepository;
      _clientRepository = clientRepository;
      _productRepository = productRepository;
    }

    public async Task<Guid> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
    {
      var cliente = await _clientRepository.GetByIdAsync(request.ClienteId);
      if (cliente == null)
        throw new Exception("Cliente não encontrado");

      var itens = new List<SaleItem>();
      decimal total = 0;

      foreach (var item in request.Itens)
      {
        var produto = await _productRepository.GetByIdAsync(item.ProdutoId);
        if (produto == null)
          throw new Exception($"Produto {item.ProdutoId} não encontrado");

        if (produto.Estoque < item.Quantidade)
          throw new Exception($"Estoque insuficiente para o produto {produto.Nome}");

        produto.Estoque -= item.Quantidade;
        await _productRepository.Update(produto);

        var precoUnitario = produto.Preco;
        total += precoUnitario * item.Quantidade;

        itens.Add(new SaleItem
        {
          ProdutoId = produto.Id,
          Nome = produto.Nome,
          PrecoUnitario = precoUnitario,
          Quantidade = item.Quantidade
        });
      }

      var venda = new Sale
      {
        ClienteId = request.ClienteId,
        Total = total,
        Itens = itens
      };

      await _saleRepository.AddAsync(venda);
      return venda.Id;
    }
  }
}