using MediatR;
using RO.DevTest.Application.Contracts.Persistence.Repositories;
using RO.DevTest.Domain.Entities;

namespace RO.DevTest.Application.Features.Clients.Queries.GetClientsQuery
{
  public class GetClientsQueryHandler : IRequestHandler<GetClientsQuery, IReadOnlyList<Clients>>
  {
    private readonly IClientRepository _clientRepository;

    public GetClientsQueryHandler(IClientRepository clientRepository)
    {
      _clientRepository = clientRepository;
    }

    public async Task<IReadOnlyList<Clients>> Handle(GetClientsQuery request, CancellationToken cancellationToken)
    {
      var todos = await _clientRepository.GetAllAsync();

      if (!string.IsNullOrWhiteSpace(request.Nome))
        todos = todos.Where(c => c.Nome.Contains(request.Nome, StringComparison.OrdinalIgnoreCase)).ToList();

      return todos
          .Skip((request.PageNumber - 1) * request.PageSize)
          .Take(request.PageSize)
          .ToList();
    }
  }
}
