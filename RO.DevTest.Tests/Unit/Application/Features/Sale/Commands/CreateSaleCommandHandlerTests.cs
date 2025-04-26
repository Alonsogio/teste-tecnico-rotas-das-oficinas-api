using FluentAssertions;
using Moq;
using RO.DevTest.Application.Contracts.Persistence.Repositories;
using RO.DevTest.Application.Features.Sales.Commands.CreateSaleCommand;
using RO.DevTest.Domain.Entities;

namespace RO.DevTest.Tests.Unit.Application.Features.Sales.Commands
{
    public class CreateSaleCommandHandlerTests
    {
        private readonly Mock<ISaleRepository> _saleRepositoryMock;
        private readonly Mock<IClientRepository> _clientRepositoryMock;
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly CreateSaleCommandHandler _sut;

        public CreateSaleCommandHandlerTests()
        {
            _saleRepositoryMock = new Mock<ISaleRepository>();
            _clientRepositoryMock = new Mock<IClientRepository>();
            _productRepositoryMock = new Mock<IProductRepository>();

            _sut = new CreateSaleCommandHandler(
                _saleRepositoryMock.Object,
                _clientRepositoryMock.Object,
                _productRepositoryMock.Object
            );
        }

        [Fact(DisplayName = "Deve lançar exceção quando o cliente não for encontrado")]
        public async Task Handle_WhenClientNotFound_ShouldThrowException()
        {
            var command = new CreateSaleCommand
            {
                ClienteId = Guid.NewGuid(),
                Itens = new List<CreateSaleCommand.CreateSaleItemDto>
                {
                    new CreateSaleCommand.CreateSaleItemDto { ProdutoId = Guid.NewGuid(), Quantidade = 2 }
                }
            };

            _clientRepositoryMock.Setup(x => x.GetByIdAsync(command.ClienteId))
                .ReturnsAsync((Client)null);

            var act = async () => await _sut.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<Exception>().WithMessage("Cliente não encontrado");
        }

        [Fact(DisplayName = "Deve lançar exceção quando o produto não for encontrado")]
        public async Task Handle_WhenProductNotFound_ShouldThrowException()
        {
            var command = new CreateSaleCommand
            {
                ClienteId = Guid.NewGuid(),
                Itens = new List<CreateSaleCommand.CreateSaleItemDto>
                {
                    new CreateSaleCommand.CreateSaleItemDto { ProdutoId = Guid.NewGuid(), Quantidade = 2 }
                }
            };

            var client = new Client { Id = command.ClienteId };
            _clientRepositoryMock.Setup(x => x.GetByIdAsync(command.ClienteId)).ReturnsAsync(client);

            _productRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Product)null);

            var act = async () => await _sut.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<Exception>().WithMessage($"Produto {command.Itens[0].ProdutoId} não encontrado");
        }

        [Fact(DisplayName = "Deve criar uma venda com sucesso e retornar o ID")]
        public async Task Handle_WhenSaleIsValid_ShouldReturnSaleId()
        {
            var command = new CreateSaleCommand
            {
                ClienteId = Guid.NewGuid(),
                Itens = new List<CreateSaleCommand.CreateSaleItemDto>
                {
                    new CreateSaleCommand.CreateSaleItemDto { ProdutoId = Guid.NewGuid(), Quantidade = 2 }
                }
            };

            var client = new Client { Id = command.ClienteId };
            _clientRepositoryMock.Setup(x => x.GetByIdAsync(command.ClienteId)).ReturnsAsync(client);

            var product = new Product { Id = command.Itens[0].ProdutoId, Estoque = 10, Preco = 100 };
            _productRepositoryMock.Setup(x => x.GetByIdAsync(command.Itens[0].ProdutoId)).ReturnsAsync(product);
            _productRepositoryMock.Setup(x => x.Update(It.IsAny<Product>())).Returns(Task.CompletedTask);

            var saleId = Guid.NewGuid();
            _saleRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Sale>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Sale { Id = saleId });

            var result = await _sut.Handle(command, CancellationToken.None);

            result.Should().NotBe(Guid.Empty);
            _saleRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Sale>(), It.IsAny<CancellationToken>()), Times.Once);
            _productRepositoryMock.Verify(x => x.Update(It.IsAny<Product>()), Times.Once);
        }
    }
}
