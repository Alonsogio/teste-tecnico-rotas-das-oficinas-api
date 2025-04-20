using System.Collections.Concurrent;
using System.Linq.Expressions;
using RO.DevTest.Application.Contracts.Persistence.Repositories;
using RO.DevTest.Domain.Entities;

namespace RO.DevTest.Persistence.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private static readonly ConcurrentDictionary<Guid, Client> _clients = new();

        public Task AddAsync(Client client)
        {
            _clients[client.Id] = client;
            return Task.CompletedTask;
        }

        public Task<IReadOnlyList<Client>> BuscarPorNomeAsync(string nome)
        {
            var result = _clients.Values
                .Where(c => c.Nome.Contains(nome, StringComparison.OrdinalIgnoreCase))
                .ToList();
            return Task.FromResult((IReadOnlyList<Client>)result);
        }

        public Task<Client> CreateAsync(Client entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public void Delete(Client entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id)
        {
            _clients.TryRemove(id, out _);
            return Task.CompletedTask;
        }

        public Client? Get(Expression<Func<Client, bool>> predicate, params Expression<Func<Client, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<Client>> GetAllAsync()
        {
            return Task.FromResult((IReadOnlyList<Client>)_clients.Values.ToList());
        }

        public Task<Client> GetByIdAsync(Guid id)
        {
            _clients.TryGetValue(id, out var client);
            return Task.FromResult(client);
        }

        public void Update(Client entity)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Client client)
        {
            _clients[client.Id] = client;
            return Task.CompletedTask;
        }
    }
}
