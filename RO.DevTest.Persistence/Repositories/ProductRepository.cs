using System.Collections.Concurrent;
using System.Linq.Expressions;
using RO.DevTest.Application.Contracts.Persistence.Repositories;
using RO.DevTest.Domain.Entities;

namespace RO.DevTest.Persistence.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private static readonly ConcurrentDictionary<Guid, Product> _product = new();

        public Task AddAsync(Product product)
        {
            _product[product.Id] = product;
            return Task.CompletedTask;
        }

        public Task<IReadOnlyList<Product>> BuscarPorNomeAsync(string nome)
        {
            var result = _product.Values
                .Where(c => c.Nome.Contains(nome, StringComparison.OrdinalIgnoreCase))
                .ToList();
            return Task.FromResult((IReadOnlyList<Product>)result);
        }

        public Task<Product> CreateAsync(Product entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public void Delete(Product entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id)
        {
            _product.TryRemove(id, out _);
            return Task.CompletedTask;
        }

        public Product? Get(Expression<Func<Product, bool>> predicate, params Expression<Func<Product, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<Product>> GetAllAsync()
        {
            return Task.FromResult((IReadOnlyList<Product>)_product.Values.ToList());
        }

        public Task<Product> GetByIdAsync(Guid id)
        {
            _product.TryGetValue(id, out var Product);
            return Task.FromResult(Product);
        }

        public void Update(Product entity)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Product Product)
        {
            _product[Product.Id] = Product;
            return Task.CompletedTask;
        }
    }
}
