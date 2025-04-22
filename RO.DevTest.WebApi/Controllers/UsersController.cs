using System.Text.Json;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using RO.DevTest.Application.Features.Users.Commands.CreateUserCommand;
using RO.DevTest.Domain.Exception;

namespace RO.DevTest.WebApi.Controllers;

[Route("api/user")]
[OpenApiTags("Users")]
public class UsersController(IMediator mediator) : Controller
{
    private readonly IMediator _mediator = mediator;

    [HttpPost]
    [ProducesResponseType(typeof(CreateUserResult), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(CreateUserResult), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand request)
    {
        try
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }
        catch (BadRequestException ex)
        {
            Console.WriteLine("Erro de validação:");
            Console.WriteLine(JsonSerializer.Serialize(ex.Errors, new JsonSerializerOptions { WriteIndented = true }));

            return BadRequest(new { message = "Erro de validação", erros = ex.Errors });
        }

    }
}
