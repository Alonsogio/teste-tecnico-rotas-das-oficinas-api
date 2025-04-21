using RO.DevTest.Application.Contracts.Persistance.Repositories;
using RO.DevTest.Domain.Entities;

namespace RO.DevTest.Application.Contracts.Persistence.Repositories
{
  public interface IProductRepository : IBaseRepository<Product>
  {
    Task AddAsync(Product product);
    Task<IReadOnlyList<Product>> BuscarPorNomeAsync(string nome);
    Task DeleteAsync(Guid id);
    Task<IReadOnlyList<Product>> GetAllAsync();
    Task<Product> GetByIdAsync(Guid id);
    Task UpdateAsync(Product product);
  }
}
