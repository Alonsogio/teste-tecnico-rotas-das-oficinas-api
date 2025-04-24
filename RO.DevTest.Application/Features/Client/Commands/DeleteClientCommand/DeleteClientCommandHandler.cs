using MediatR;
using RO.DevTest.Application.Contracts.Persistence.Repositories;

namespace RO.DevTest.Application.Features.Clients.Commands.DeleteClientCommand
{
  public class DeleteClientCommandHandler : IRequestHandler<DeleteClientCommand, Unit>
  {
    private readonly IClientRepository _clientRepository;

    public DeleteClientCommandHandler(IClientRepository clientRepository)
    {
      _clientRepository = clientRepository;
    }
    public async Task<Unit> Handle(DeleteClientCommand request, CancellationToken cancellationToken)
    {
      await _clientRepository.DeleteAsync(request.Id);
      return Unit.Value;
    }
  }
}
