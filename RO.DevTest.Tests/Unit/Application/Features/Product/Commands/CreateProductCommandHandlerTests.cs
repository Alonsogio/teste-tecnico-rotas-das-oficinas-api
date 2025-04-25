using FluentAssertions;
using Moq;
using RO.DevTest.Application.Contracts.Persistence.Repositories;
using RO.DevTest.Application.Features.Products.Commands.CreateProductCommand;
using RO.DevTest.Domain.Entities;
using RO.DevTest.Domain.Exception;

namespace RO.DevTest.Tests.Unit.Application.Features.Products.Commands
{
    public class CreateProductCommandHandlerTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock = new();
        private readonly CreateProductCommandHandler _sut;

        public CreateProductCommandHandlerTests()
        {
            _productRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Product produto, CancellationToken _) =>
                {
                    produto.Id = Guid.NewGuid();
                    return produto;
                });

            _sut = new CreateProductCommandHandler(_productRepositoryMock.Object);
        }

        [Fact(DisplayName = "Nome vazio deve lançar BadRequestException")]
        public async Task Handle_WhenNomeIsEmpty_ShouldThrowBadRequestException()
        {
            var command = new CreateProductCommand
            {
                Nome = "",
                Descricao = "desc",
                Preco = 100,
                Estoque = 1
            };

            var act = async () => await _sut.Handle(command, default);

            await act.Should().ThrowAsync<BadRequestException>();
        }

        [Fact(DisplayName = "Descrição vazia deve lançar BadRequestException")]
        public async Task Handle_WhenDescricaoIsEmpty_ShouldThrowBadRequestException()
        {
            var command = new CreateProductCommand
            {
                Nome = "Produto",
                Descricao = "",
                Preco = 100,
                Estoque = 1
            };

            var act = async () => await _sut.Handle(command, default);

            await act.Should().ThrowAsync<BadRequestException>();
        }

        [Fact(DisplayName = "Preço menor ou igual a zero deve lançar BadRequestException")]
        public async Task Handle_WhenPrecoIsInvalid_ShouldThrowBadRequestException()
        {
            var command = new CreateProductCommand
            {
                Nome = "Produto",
                Descricao = "desc",
                Preco = 0,
                Estoque = 1
            };

            var act = async () => await _sut.Handle(command, default);

            await act.Should().ThrowAsync<BadRequestException>();
        }

        [Fact(DisplayName = "Estoque negativo deve lançar BadRequestException")]
        public async Task Handle_WhenEstoqueIsNegative_ShouldThrowBadRequestException()
        {
            var command = new CreateProductCommand
            {
                Nome = "Produto",
                Descricao = "desc",
                Preco = 10,
                Estoque = -1
            };

            var act = async () => await _sut.Handle(command, default);

            await act.Should().ThrowAsync<BadRequestException>();
        }

        [Fact(DisplayName = "Dados válidos devem retornar o ID do produto criado")]
        public async Task Handle_ValidData_ShouldReturnProductId()
        {
            var command = new CreateProductCommand
            {
                Nome = "Produto válido",
                Descricao = "Descrição válida",
                Preco = 50,
                Estoque = 5
            };

            var result = await _sut.Handle(command, default);

            result.Should().NotBe(Guid.Empty);
            _productRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
