using FluentAssertions;
using Moq;
using RO.DevTest.Application.Contracts.Persistence.Repositories;
using RO.DevTest.Application.Features.Products.Commands.UpdateProductCommand;
using RO.DevTest.Domain.Entities;
using RO.DevTest.Domain.Exception;

namespace RO.DevTest.Tests.Unit.Application.Features.Products.Commands;

public class UpdateProductCommandHandlerTests
{
    private readonly Mock<IProductRepository> _productRepositoryMock = new();
    private readonly UpdateProductCommandHandler _sut;

    public UpdateProductCommandHandlerTests()
    {
        _sut = new UpdateProductCommandHandler(_productRepositoryMock.Object);
    }

    [Fact(DisplayName = "Nome vazio deve lançar BadRequestException")]
    public async Task Handle_WhenNomeIsEmpty_ShouldThrowBadRequestException()
    {
        var command = new UpdateProductCommand
        {
            Id = Guid.NewGuid(),
            Nome = "",
            Descricao = "desc",
            Preco = 100,
            Estoque = 1
        };

        var act = async () => await _sut.Handle(command, default);

        await act.Should().ThrowAsync<BadRequestException>();
    }

    [Fact(DisplayName = "Produto não encontrado deve lançar BadRequestException")]
    public async Task Handle_WhenProductNotFound_ShouldThrowBadRequestException()
    {
        _productRepositoryMock
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Product)null!);

        var command = new UpdateProductCommand
        {
            Id = Guid.NewGuid(),
            Nome = "Produto",
            Descricao = "desc",
            Preco = 100,
            Estoque = 1
        };

        var act = async () => await _sut.Handle(command, default);

        await act.Should().ThrowAsync<BadRequestException>();
    }

    [Fact(DisplayName = "Dados válidos devem atualizar o produto")]
    public async Task Handle_ValidData_ShouldUpdateProduct()
    {
        var produtoExistente = new Product
        {
            Id = Guid.NewGuid(),
            Nome = "Antigo",
            Descricao = "Antiga desc",
            Preco = 50,
            Estoque = 10
        };

        _productRepositoryMock
            .Setup(x => x.GetByIdAsync(produtoExistente.Id))
            .ReturnsAsync(produtoExistente);

        var command = new UpdateProductCommand
        {
            Id = produtoExistente.Id,
            Nome = "Novo nome",
            Descricao = "Nova descrição",
            Preco = 99.99m,
            Estoque = 15
        };

        await _sut.Handle(command, default);

        _productRepositoryMock.Verify(x => x.Update(
     It.Is<Product>(p =>
         p.Id == command.Id &&
         p.Nome == command.Nome &&
         p.Descricao == command.Descricao &&
         p.Preco == command.Preco &&
         p.Estoque == command.Estoque
     )),
     Times.Once
     );
    }
}
