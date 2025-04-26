using MediatR;
using RO.DevTest.Domain.Entities;

namespace RO.DevTest.Application.Features.Products.Queries.GetProductByIdQuery
{
  public class GetProductByIdQuery : IRequest<Product>
  {
    public Guid Id { get; set; }
  }
}