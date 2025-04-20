using RO.DevTest.Application.Contracts.Persistance.Repositories;
using RO.DevTest.Domain.Entities;

namespace RO.DevTest.Application.Contracts.Persistence.Repositories
{
  public interface IClientRepository : IBaseRepository<Client>
  {
    Task AddAsync(Client client);
    Task<IReadOnlyList<Client>> BuscarPorNomeAsync(string nome);
    Task DeleteAsync(Guid id);
    Task<IReadOnlyList<Client>> GetAllAsync();
    Task<Client> GetByIdAsync(Guid id);
    Task UpdateAsync(Client client);
  }
}
