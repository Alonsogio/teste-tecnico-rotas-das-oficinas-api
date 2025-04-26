using MediatR;
using RO.DevTest.Application.Contracts.Persistence.Repositories;
using RO.DevTest.Domain.Entities;

namespace RO.DevTest.Application.Features.Clients.Queries.GetClientByIdQuery
{
  public class GetClientByIdQueryHandler : IRequestHandler<GetClientByIdQuery, Client>
  {
    private readonly IClientRepository _clientRepository;

    public GetClientByIdQueryHandler(IClientRepository clientRepository)
    {
      _clientRepository = clientRepository;
    }

    public async Task<Client> Handle(GetClientByIdQuery request, CancellationToken cancellationToken)
    {

      var clientId = await _clientRepository.GetByIdAsync(request.Id);
      if (clientId == null)
        throw new KeyNotFoundException($"Client com ID '{request.Id}' não foi encontrado.");

      return clientId;
    }
  }
}
