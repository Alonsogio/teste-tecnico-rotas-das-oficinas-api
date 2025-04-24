using MediatR;
using RO.DevTest.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using RO.DevTest.Application.Features.Products.Commands.CreateProductCommand;
using RO.DevTest.Application.Features.Products.Commands.DeleteProductCommand;
using RO.DevTest.Application.Features.Products.Queries.GetProductByIdQuery;
using RO.DevTest.Application.Features.Products.Commands.UpdateProductCommand;
using RO.DevTest.Application.Features.Products.Queries.GetProductsQuery;
using Microsoft.AspNetCore.Authorization;

namespace RO.DevTest.WebApi.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  // [Authorize]
  public class ProductsController : ControllerBase
  {
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
      _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateProductCommand command)
    {
      var id = await _mediator.Send(command);
      return CreatedAtAction(nameof(GetById), new { id }, null);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetById(Guid id)
    {
      var products = await _mediator.Send(new GetProductByIdQuery { Id = id });
      if (products == null) return NotFound();
      return Ok(products);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetAll([FromQuery] GetProductsQuery query)
    {
      var products = await _mediator.Send(query);
      return Ok(products);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateProductCommand command)
    {
      if (id != command.Id) return BadRequest("ID da URL difere do corpo da requisição");
      await _mediator.Send(command);
      return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
      await _mediator.Send(new DeleteProductCommand { Id = id });
      return NoContent();
    }
  }
}