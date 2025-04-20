using MediatR;
using RO.DevTest.Domain.Entities;

namespace RO.DevTest.Application.Features.Clients.Queries.GetClientsQuery
{
  public class GetClientsQuery : IRequest<IReadOnlyList<Client>>
  {
    public string? Nome { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
  }
}