using RO.DevTest.Application.Contracts.Persistance.Repositories;
using RO.DevTest.Domain.Entities;

namespace RO.DevTest.Application.Contracts.Persistence.Repositories
{
    public interface IClientRepository : IBaseRepository<Client>
    {
        Task<IReadOnlyList<Client>> BuscarPorNomeAsync(string nome);
        Task<IReadOnlyList<Client>> GetAllAsync();
        Task<Client> GetByIdAsync(Guid id);
    }
}
