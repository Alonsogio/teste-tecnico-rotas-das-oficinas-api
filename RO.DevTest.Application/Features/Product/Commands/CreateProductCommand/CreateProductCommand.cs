using MediatR;
using RO.DevTest.Domain.Entities;

namespace RO.DevTest.Application.Features.Products.Commands.CreateProductCommand
{
  public class CreateProductCommand : IRequest<Product>
  {
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public decimal Preco { get; set; }
    public int Estoque { get; set; }
  }
}