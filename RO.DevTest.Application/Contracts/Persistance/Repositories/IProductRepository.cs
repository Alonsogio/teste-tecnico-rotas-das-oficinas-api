using RO.DevTest.Application.Contracts.Persistance.Repositories;
using RO.DevTest.Domain.Entities;

namespace RO.DevTest.Application.Contracts.Persistence.Repositories
{
  public interface IProductRepository : IBaseRepository<Product>
  {
    Task<IReadOnlyList<Product>> GetAllAsync();
    Task<Product> GetByIdAsync(Guid id);

  }
}
