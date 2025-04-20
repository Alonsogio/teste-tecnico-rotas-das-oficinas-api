using MediatR;
using RO.DevTest.Domain.Entities;

namespace RO.DevTest.Application.Features.Clients.Queries.GetClientByIdQuery
{
  public class GetClientByIdQuery : IRequest<Client>
  {
    public Guid Id { get; set; }
  }
}