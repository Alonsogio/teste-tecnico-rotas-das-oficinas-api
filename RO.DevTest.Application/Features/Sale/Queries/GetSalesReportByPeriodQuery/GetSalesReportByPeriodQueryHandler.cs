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
      var inicio = request.Inicio == default ? DateTime.UtcNow.AddDays(-30).Date : request.Inicio.Date;
      var fim = request.Fim == default ? DateTime.UtcNow.Date : request.Fim.Date;

      var vendas = await _saleRepository.GetByDateRangeAsync(inicio, fim);

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