using MediatR;
using RO.DevTest.Application.Contracts.Persistence.Repositories;


namespace RO.DevTest.Application.Features.Sales.Queries.GetSalesReportByPeriodQuery
{
  public class GetSalesReportByPeriodQueryHandler : IRequestHandler<GetSalesReportByPeriodQuery, SalesReportDto>
  {
    private readonly ISaleRepository _saleRepository;

    public GetSalesReportByPeriodQueryHandler(ISaleRepository saleRepository)
    {
      _saleRepository = saleRepository;
    }

    public async Task<SalesReportDto> Handle(GetSalesReportByPeriodQuery request, CancellationToken cancellationToken)
    {
      var vendas = await _saleRepository.GetByDateRangeAsync(request.Inicio, request.Fim);

      var produtos = vendas
          .SelectMany(v => v.Itens)
          .GroupBy(i => new { i.ProdutoId, i.Nome })
          .Select(g => new ProdutoVendaDto
          {
            ProdutoId = g.Key.ProdutoId,
            Nome = g.Key.Nome,
            QuantidadeVendida = g.Sum(x => x.Quantidade),
            RendaGerada = g.Sum(x => x.Quantidade * x.PrecoUnitario)
          })
          .ToList();

      return new SalesReportDto
      {
        QuantidadeVendas = vendas.Count,
        RendaTotal = vendas.Sum(v => v.Total),
        ProdutosVendidos = produtos
      };
    }
  }
}