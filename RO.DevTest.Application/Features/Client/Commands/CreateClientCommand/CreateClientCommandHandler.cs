using MediatR;
using RO.DevTest.Domain.Entities;
using RO.DevTest.Application.Contracts.Persistence.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RO.DevTest.Application.Features.Clients.Commands.CreateClientCommand
{
  public class CreateClientCommandHandler : IRequestHandler<CreateClientCommand, Client>
  {
    private readonly IClientRepository _clientRepository;

    public CreateClientCommandHandler(IClientRepository clientRepository)
    {
      _clientRepository = clientRepository;
    }

    public async Task<Client> Handle(CreateClientCommand request, CancellationToken cancellationToken)
    {
      var client = new Client
      {
        Nome = request.Nome,
        Email = request.Email,
        Telefone = request.Telefone
      };

      await _clientRepository.AddAsync(client);

      return client;
    }
  }
}
