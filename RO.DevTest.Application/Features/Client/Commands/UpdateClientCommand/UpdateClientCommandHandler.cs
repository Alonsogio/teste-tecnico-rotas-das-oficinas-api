using MediatR;
using RO.DevTest.Application.Contracts.Persistence.Repositories;
using RO.DevTest.Domain.Entities;

namespace RO.DevTest.Application.Features.Clients.Commands.UpdateClientCommand
{
  public class UpdateClientCommandHandler : IRequestHandler<UpdateClientCommand, Client>
  {
    private readonly IClientRepository _clientRepository;

    public UpdateClientCommandHandler(IClientRepository clientRepository)
    {
      _clientRepository = clientRepository;
    }

    public async Task<Client> Handle(UpdateClientCommand request, CancellationToken cancellationToken)
    {
      var client = await _clientRepository.GetByIdAsync(request.Id);
      if (client == null)
        throw new Exception("Cliente não encontrado");

      client.Nome = request.Nome;
      client.Email = request.Email;
      client.Telefone = request.Telefone;

      await _clientRepository.Update(client);
      return client;
    }
  }
}
