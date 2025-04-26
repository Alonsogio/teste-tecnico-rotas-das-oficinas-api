using MediatR;

namespace RO.DevTest.Application.Features.Products.Commands.UpdateProductCommand
{
  public class UpdateProductCommand : IRequest<Unit>
  {
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public decimal Preco { get; set; }
    public int Estoque { get; set; }
  }
}