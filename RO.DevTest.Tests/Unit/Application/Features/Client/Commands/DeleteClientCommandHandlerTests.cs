using MediatR;
using RO.DevTest.Application.Contracts.Persistence.Repositories;

namespace RO.DevTest.Application.Features.Clients.Commands.DeleteClientCommand;

public class DeleteClientCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
}

public class DeleteClientCommandHandler : IRequestHandler<DeleteClientCommand, Unit>
{
    private readonly IClientRepository _clientRepository;

    public DeleteClientCommandHandler(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }

    public async Task<Unit> Handle(DeleteClientCommand request, CancellationToken cancellationToken)
    {
        var client = await _clientRepository.GetByIdAsync(request.Id);
        if (client is null)
            throw new Exception("Cliente não encontrado");

        await _clientRepository.Delete(client);
        return Unit.Value;
    }
}
