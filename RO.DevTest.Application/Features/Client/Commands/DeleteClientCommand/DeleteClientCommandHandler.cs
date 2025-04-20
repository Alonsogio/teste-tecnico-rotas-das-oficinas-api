using MediatR;
using RO.DevTest.Application.Contracts.Persistence.Repositories;

namespace RO.DevTest.Application.Features.Client.Commands.DeleteClientCommand
{
  public class DeleteClientCommandHandler : IRequestHandler<DeleteClientCommand>
  {
    private readonly IClientRepository _clientRepository;

    public DeleteClienteCommandHandler(IClientRepository clientRepository)
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