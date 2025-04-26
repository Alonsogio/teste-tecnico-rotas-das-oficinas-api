using MediatR;
using RO.DevTest.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using RO.DevTest.Application.Features.Products.Commands.CreateProductCommand;
using RO.DevTest.Application.Features.Products.Commands.DeleteProductCommand;
using RO.DevTest.Application.Features.Products.Queries.GetProductByIdQuery;
using RO.DevTest.Application.Features.Products.Commands.UpdateProductCommand;
using RO.DevTest.Application.Features.Products.Queries.GetProductsQuery;
using Microsoft.AspNetCore.Authorization;
using NSwag.Annotations;

namespace RO.DevTest.WebApi.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  [Authorize(Roles = "Admin, Customer")]
  public class ProductsController : ControllerBase
  {
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
      _mediator = mediator;
    }

    [HttpPost]
    [OpenApiOperation("Criar um novo produto", "Cria um novo produto no sistema.")]
    public async Task<IActionResult> Create(CreateProductCommand command)
    {
      var id = await _mediator.Send(command);
      return CreatedAtAction(nameof(GetById), new { id }, null);
    }

    [HttpGet("{id}")]
    [OpenApiOperation("Pegar um produto por id", "Retorna um produto no sistema.")]
    public async Task<ActionResult<Product>> GetById(Guid id)
    {
      var products = await _mediator.Send(new GetProductByIdQuery { Id = id });
      if (products == null) return NotFound();
      return Ok(products);
    }

    [HttpGet]
    [OpenApiOperation("Pegar todos os produtos", "Retorna todos os produtos no sistema.")]
    public async Task<ActionResult<IEnumerable<Product>>> GetAll([FromQuery] GetProductsQuery query)
    {
      var products = await _mediator.Send(query);
      return Ok(products);
    }

    [HttpPut("{id}")]
    [OpenApiOperation("Edita um produto", "Edita um produto no sistema.")]
    public async Task<IActionResult> Update(Guid id, UpdateProductCommand command)
    {
      if (id != command.Id) return BadRequest("ID da URL difere do corpo da requisição");
      await _mediator.Send(command);
      return NoContent();
    }

    [HttpDelete("{id}")]
    [OpenApiOperation("Deleta um produto", "Deleta um produto no sistema.")]
    public async Task<IActionResult> Delete(Guid id)
    {
      await _mediator.Send(new DeleteProductCommand { Id = id });
      return NoContent();
    }
  }
}