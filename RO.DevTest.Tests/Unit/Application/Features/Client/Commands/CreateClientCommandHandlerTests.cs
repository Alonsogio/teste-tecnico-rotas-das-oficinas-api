using FluentAssertions;
using Moq;
using RO.DevTest.Application.Contracts.Persistence.Repositories;
using RO.DevTest.Application.Features.Clients.Commands.CreateClientCommand;
using RO.DevTest.Domain.Entities;
using RO.DevTest.Domain.Exception;

namespace RO.DevTest.Tests.Unit.Application.Features.Clients.Commands
{
    public class CreateClientCommandHandlerTests
    {
        private readonly Mock<IClientRepository> _clientRepositoryMock = new();
        private readonly CreateClientCommandHandler _sut;

        public CreateClientCommandHandlerTests()
        {
            _clientRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Client>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Client { Id = Guid.NewGuid(), Nome = "Test Client", Email = "testclient@example.com", Telefone = "123456789" });

            _sut = new CreateClientCommandHandler(_clientRepositoryMock.Object);
        }

        [Fact(DisplayName = "Given invalid email should throw a BadRequestException")]
        public void Handle_WhenEmailIsNullOrEmpty_ShouldRaiseABadRequestException()
        {
            string email = string.Empty, nome = "Test Client", telefone = "123456789";
            CreateClientCommand command = new()
            {
                Email = email,
                Nome = nome,
                Telefone = telefone
            };

            Func<Task> action = async () => await _sut.Handle(command, new CancellationToken());

            action.Should().ThrowAsync<BadRequestException>();
        }

        [Fact(DisplayName = "Given empty phone should throw a BadRequestException")]
        public void Handle_WhenPhoneIsNullOrEmpty_ShouldRaiseABadRequestException()
        {
            string email = "testclient@example.com", nome = "Test Client", telefone = string.Empty;
            CreateClientCommand command = new()
            {
                Email = email,
                Nome = nome,
                Telefone = telefone
            };

            Func<Task> action = async () => await _sut.Handle(command, new CancellationToken());

            action.Should().ThrowAsync<BadRequestException>();
        }

        [Fact(DisplayName = "Given valid data should return the created client")]
        public async Task Handle_WhenValidClientData_ShouldReturnCreatedClient()
        {
            string email = "testclient@example.com", nome = "Test Client", telefone = "123456789";
            CreateClientCommand command = new()
            {
                Email = email,
                Nome = nome,
                Telefone = telefone
            };

            var result = await _sut.Handle(command, new CancellationToken());

            result.Should().NotBe(Guid.Empty);
        }
    }
}
