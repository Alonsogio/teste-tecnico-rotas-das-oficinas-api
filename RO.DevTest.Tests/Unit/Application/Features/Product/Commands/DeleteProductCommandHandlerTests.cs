using FluentAssertions;
using Moq;
using RO.DevTest.Application.Contracts.Persistence.Repositories;
using RO.DevTest.Application.Features.Products.Commands.DeleteProductCommand;
using RO.DevTest.Domain.Entities;
using RO.DevTest.Domain.Exception;

namespace RO.DevTest.Tests.Unit.Application.Features.Products.Commands;

public class DeleteProductCommandHandlerTests
{
    private readonly Mock<IProductRepository> _productRepositoryMock = new();
    private readonly DeleteProductCommandHandler _sut;

    public DeleteProductCommandHandlerTests()
    {
        _sut = new DeleteProductCommandHandler(_productRepositoryMock.Object);
    }

    [Fact(DisplayName = "Produto não encontrado deve lançar BadRequestException")]
    public async Task Handle_ProductNotFound_ShouldThrowBadRequestException()
    {
        _productRepositoryMock
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Product)null!);

        var command = new DeleteProductCommand { Id = Guid.NewGuid() };

        var act = async () => await _sut.Handle(command, default);

        await act.Should().ThrowAsync<BadRequestException>();
    }

    [Fact(DisplayName = "Produto existente deve ser deletado")]
    public async Task Handle_ValidProduct_ShouldDelete()
    {
        var produto = new Product { Id = Guid.NewGuid(), Nome = "Produto", Preco = 10, Estoque = 5 };

        _productRepositoryMock
            .Setup(x => x.GetByIdAsync(produto.Id))
            .ReturnsAsync(produto);

        var command = new DeleteProductCommand { Id = produto.Id };

        await _sut.Handle(command, default);

        _productRepositoryMock.Verify(x => x.DeleteAsync(produto.Id), Times.Once);
    }
}
