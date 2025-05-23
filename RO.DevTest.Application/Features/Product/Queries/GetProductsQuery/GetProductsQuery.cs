using MediatR;
using RO.DevTest.Domain.Entities;

namespace RO.DevTest.Application.Features.Products.Queries.GetProductsQuery
{
  public class GetProductsQuery : IRequest<IReadOnlyList<Product>>
  {
    public string? Nome { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
  }
}