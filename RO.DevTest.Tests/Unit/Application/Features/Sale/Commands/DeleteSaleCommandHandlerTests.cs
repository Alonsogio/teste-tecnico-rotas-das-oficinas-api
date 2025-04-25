using FluentAssertions;
using Moq;
using RO.DevTest.Application.Contracts.Persistence.Repositories;
using RO.DevTest.Application.Features.Sales.Commands.DeleteSaleCommand;
using RO.DevTest.Domain.Entities;

namespace RO.DevTest.Tests.Unit.Application.Features.Sales.Commands
{
    public class DeleteSaleCommandHandlerTests
    {
        private readonly Mock<ISaleRepository> _saleRepositoryMock = new();
        private readonly DeleteSaleCommandHandler _sut;

        public DeleteSaleCommandHandlerTests()
        {
            _sut = new DeleteSaleCommandHandler(_saleRepositoryMock.Object);
        }

        [Fact(DisplayName = "Venda não encontrada deve lançar Exception")]
        public async Task Handle_SaleNotFound_ShouldThrowException()
        {
            _saleRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Sale)null!);

            var command = new DeleteSaleCommand { Id = Guid.NewGuid() };

            var act = async () => await _sut.Handle(command, default);

            await act.Should().ThrowAsync<Exception>().WithMessage("Venda não encontrada");
        }

        [Fact(DisplayName = "Venda existente deve ser deletada")]
        public async Task Handle_ValidSale_ShouldDelete()
        {
            var sale = new Sale { Id = Guid.NewGuid(), ClienteId = Guid.NewGuid(), Total = 100 };

            _saleRepositoryMock
                .Setup(x => x.GetByIdAsync(sale.Id))
                .ReturnsAsync(sale);

            var command = new DeleteSaleCommand { Id = sale.Id };

            await _sut.Handle(command, default);

            _saleRepositoryMock.Verify(x => x.DeleteAsync(sale.Id), Times.Once);
        }
    }
}
