using MediatR;
using RO.DevTest.Application.Contracts.Persistence.Repositories;

namespace RO.DevTest.Application.Features.Clients.Commands.DeleteClientCommand
{
  public class DeleteClientCommandHandler(IClientRepository clientRepository) : IRequestHandler<DeleteClientCommand>
  {
    private readonly IClientRepository _clientRepository = clientRepository;

        public async Task<Unit> Handle(DeleteClientCommand request, CancellationToken cancellationToken)
    {
      await _clientRepository.DeleteAsync(request.Id);
      return Unit.Value;
    }

        Task IRequestHandler<DeleteClientCommand>.Handle(DeleteClientCommand request, CancellationToken cancellationToken)
        {
            return Handle(request, cancellationToken);
        }
    }
}