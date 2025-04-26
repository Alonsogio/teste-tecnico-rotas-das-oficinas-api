using MediatR;
namespace RO.DevTest.Application.Features.Sales.Commands.CreateSaleCommand
{
  public class CreateSaleCommand : IRequest<Guid>
  {
    public Guid ClienteId { get; set; }
    public List<CreateSaleItemDto> Itens { get; set; } = new();

        public class CreateSaleItemDto
        {
          public Guid ProdutoId { get; set; }
          public int Quantidade { get; set; }
        }
      }

}