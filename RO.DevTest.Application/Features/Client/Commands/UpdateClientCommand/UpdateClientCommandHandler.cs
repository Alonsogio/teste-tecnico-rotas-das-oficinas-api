using MediatR;
using RO.DevTest.Application.Contracts.Persistence.Repositories;

namespace RO.DevTest.Application.Features.Clients.Commands.UpdateClientCommand
{
  public class UpdateClientCommandHandler : IRequestHandler<UpdateClientCommand, Unit>
  {
    private readonly IClientRepository _clientRepository;

    public UpdateClientCommandHandler(IClientRepository clientRepository)
    {
      _clientRepository = clientRepository;
    }

    public async Task<Unit> Handle(UpdateClientCommand request, CancellationToken cancellationToken)
    {
      var client = await _clientRepository.GetByIdAsync(request.Id);
      if (client == null)
        throw new Exception("Cliente não encontrado");

      client.Nome = request.Nome;
      client.Email = request.Email;
      client.Telefone = request.Telefone;

      await _clientRepository.Update(client);
      return Unit.Value;
    }
  }
}
