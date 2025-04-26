using FluentAssertions;
using Moq;
using RO.DevTest.Application.Contracts.Persistence.Repositories;
using RO.DevTest.Application.Features.Clients.Commands.UpdateClientCommand;
using RO.DevTest.Domain.Entities;
using RO.DevTest.Domain.Exception;

namespace RO.DevTest.Tests.Unit.Application.Features.Clients.Commands;

public class UpdateClientCommandHandlerTests
{
    private readonly Mock<IClientRepository> _clientRepositoryMock = new();
    private readonly UpdateClientCommandHandler _sut;

    public UpdateClientCommandHandlerTests()
    {
        _clientRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new Client { Id = Guid.NewGuid(), Nome = "Test Client", Email = "testclient@example.com", Telefone = "123456789" });
        
        _clientRepositoryMock.Setup(x => x.Update(It.IsAny<Client>()))
            .Returns(Task.CompletedTask);

        _sut = new UpdateClientCommandHandler(_clientRepositoryMock.Object);
    }

    [Fact(DisplayName = "Given invalid email should throw a BadRequestException")]
    public void Handle_WhenEmailIsNullOrEmpty_ShouldRaiseABadRequestException()
    {
        string email = string.Empty, nome = "Test Client", telefone = "123456789";
        var clientId = Guid.NewGuid();
        UpdateClientCommand command = new()
        {
            Id = clientId,
            Email = email,
            Nome = nome,
            Telefone = telefone
        };

        Func<Task> action = async () => await _sut.Handle(command, new CancellationToken());

        action.Should().ThrowAsync<BadRequestException>();
    }

    [Fact(DisplayName = "Given valid data should update and return success")]
    public async Task Handle_WhenValidData_ShouldReturnSuccess()
    {
        string email = "updatedclient@example.com", nome = "Updated Client", telefone = "987654321";
        var clientId = Guid.NewGuid();
        UpdateClientCommand command = new()
        {
            Id = clientId,
            Email = email,
            Nome = nome,
            Telefone = telefone
        };

        var result = await _sut.Handle(command, new CancellationToken());

        result.Should().Be(MediatR.Unit.Value);
        _clientRepositoryMock.Verify(x => x.Update(It.IsAny<Client>()), Times.Once);
    }

    [Fact(DisplayName = "Given non-existent client should throw an Exception")]
    public void Handle_WhenClientDoesNotExist_ShouldThrowException()
    {
        var clientId = Guid.NewGuid();
        UpdateClientCommand command = new()
        {
            Id = clientId,
            Email = "updatedclient@example.com",
            Nome = "Updated Client",
            Telefone = "987654321"
        };

        _clientRepositoryMock.Setup(x => x.GetByIdAsync(clientId))
            .ReturnsAsync((Client)null);

        Func<Task> action = async () => await _sut.Handle(command, new CancellationToken());

        action.Should().ThrowAsync<Exception>().WithMessage("Cliente não encontrado");
    }
}
