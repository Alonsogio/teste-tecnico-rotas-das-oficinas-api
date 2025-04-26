using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RO.DevTest.Application.Contracts.Persistence.Repositories;
using RO.DevTest.Application.Features.Sales.Commands.CreateSaleCommand;
using RO.DevTest.Application.Features.Sales.Commands.DeleteSaleCommand;
using RO.DevTest.Application.Features.Sales.Queries.GetSalesReportByPeriodQuery;
using NSwag.Annotations;

namespace RO.DevTest.WebApi.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  [Authorize(Roles = "Admin, Customer")]
  public class SalesController : ControllerBase
  {
    private readonly IMediator _mediator;
    private readonly ISaleRepository _saleRepository;

    public SalesController(IMediator mediator, ISaleRepository saleRepository)
    {
      _mediator = mediator;
      _saleRepository = saleRepository;
    }

    [HttpPost]
    [OpenApiOperation("Criar uma nova venda", "Cria uma nova venda no sistema.")]
    public async Task<IActionResult> Create([FromBody] CreateSaleCommand command)
    {
      var id = await _mediator.Send(command);
      var vendaCriada = await _saleRepository.GetByIdAsync(id);
      return CreatedAtAction(nameof(GetReport), new { id }, vendaCriada);
    }

    [HttpGet("relatorio")]
    [OpenApiOperation("Retorna relatorio de vendas", "Retorna um relatorio de vendas no sistema.")]

    public async Task<IActionResult> GetReport([FromQuery] DateTime inicio, [FromQuery] DateTime fim)
    {
      var query = new GetSalesReportByPeriodQuery { Inicio = inicio, Fim = fim };
      var result = await _mediator.Send(query);
      return Ok(result);
    }

    [HttpDelete("{id}")]
    [OpenApiOperation("Deleta uma venda", "Deleta uma venda no sistema.")]
    public async Task<IActionResult> Delete(Guid id)
    {
      await _mediator.Send(new DeleteSaleCommand { Id = id });
      return NoContent();
    }
  }
}