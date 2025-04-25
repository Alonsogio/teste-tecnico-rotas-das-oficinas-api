using MediatR;
using RO.DevTest.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using RO.DevTest.Application.Features.Clients.Commands.CreateClientCommand;
using RO.DevTest.Application.Features.Clients.Commands.DeleteClientCommand;
using RO.DevTest.Application.Features.Clients.Queries.GetClientByIdQuery;
using RO.DevTest.Application.Features.Clients.Commands.UpdateClientCommand;
using RO.DevTest.Application.Features.Clients.Queries.GetClientsQuery;
using Microsoft.AspNetCore.Authorization;

namespace RO.DevTest.WebApi.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class ClientsController : ControllerBase
  {
    private readonly IMediator _mediator;

    public ClientsController(IMediator mediator)
    {
      _mediator = mediator;
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Create(CreateClientCommand command)
    {
      var id = await _mediator.Send(command);
      return CreatedAtAction(nameof(GetById), new { id }, null);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Client>> GetById(Guid id)
    {
      var clients = await _mediator.Send(new GetClientByIdQuery { Id = id });
      if (clients == null) return NotFound();

      return Ok(clients);
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<Client>>> GetAll([FromQuery] GetClientsQuery query)
    {
      var clients = await _mediator.Send(query);
      return Ok(clients);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin, Customer")]

    public async Task<IActionResult> Update(Guid id, UpdateClientCommand command)
    {
      if (id != command.Id) return BadRequest("ID da URL difere do corpo da requisição");
      await _mediator.Send(command);
      return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin, Customer")]
    public async Task<IActionResult> Delete(Guid id)
    {
      await _mediator.Send(new DeleteClientCommand { Id = id });
      return NoContent();
    }
  }
}