using MediatR;

namespace RO.DevTest.Application.Features.Sales.Queries.GetSalesReportByPeriodQuery
{
  public class GetSalesReportByPeriodQuery : IRequest<SalesReportDto>
  {
    public DateTime Inicio { get; set; }
    public DateTime Fim { get; set; }
  }

  public class SalesReportDto
  {
    public int QuantidadeVendas { get; set; }
    public decimal RendaTotal { get; set; }
    public List<ProdutoVendaDto> ProdutosVendidos { get; set; } = new();
  }

  public class ProdutoVendaDto
  {
    public Guid ProdutoId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public int QuantidadeVendida { get; set; }
    public decimal RendaGerada { get; set; }
  }
}
