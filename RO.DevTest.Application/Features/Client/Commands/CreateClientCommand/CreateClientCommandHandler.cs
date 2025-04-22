using MediatR;
using RO.DevTest.Domain.Entities;
using RO.DevTest.Application.Contracts.Persistence.Repositories;
using System.Text.Json;

namespace RO.DevTest.Application.Features.Clients.Commands.CreateClientCommand
{
  public class CreateClientCommandHandler : IRequestHandler<CreateClientCommand, Guid>
  {
    private readonly IClientRepository _clientRepository;

    public CreateClientCommandHandler(IClientRepository clientRepository)
    {
      _clientRepository = clientRepository;
    }

    public async Task<Guid> Handle(CreateClientCommand request, CancellationToken cancellationToken)
    {
      var client = new Client
      {
        Nome = request.Nome,
        Email = request.Email,
        Telefone = request.Telefone
      };

      await _clientRepository.AddAsync(client);
      return client.Id;
    }
  }
}