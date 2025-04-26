using Microsoft.EntityFrameworkCore;
using RO.DevTest.Application.Contracts.Persistence.Repositories;
using RO.DevTest.Domain.Entities;

namespace RO.DevTest.Persistence.Repositories
{
    public class ClientRepository(DefaultContext context)
     : BaseRepository<Client>(context), IClientRepository
    {
        public async Task<IReadOnlyList<Client>> BuscarPorNomeAsync(string nome)
        {
            return await Context.Clients
                .Where(c => EF.Functions.ILike(c.Nome, $"%{nome}%"))
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Client>> GetAllAsync()
        {
            return await Context.Clients.ToListAsync();
        }

        public async Task<Client> GetByIdAsync(Guid id)
        {
            return await Context.Clients.FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
