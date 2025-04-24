using Microsoft.EntityFrameworkCore;
using RO.DevTest.Application.Contracts.Persistence.Repositories;
using RO.DevTest.Domain.Entities;

namespace RO.DevTest.Persistence.Repositories
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(DefaultContext context) : base(context) { }

        public async Task<IReadOnlyList<Product>> GetAllAsync()
        {
            return await Context.Products.ToListAsync();
        }

        public async Task<Product> GetByIdAsync(Guid id)
        {
            return await Context.Products.FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
