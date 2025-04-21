using RO.DevTest.Application.Contracts.Persistence.Repositories;
using RO.DevTest.Domain.Entities;

namespace RO.DevTest.Persistence.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        private static readonly List<Sale> _sales = new();

        public Task AddAsync(Sale sale)
        {
            _sales.Add(sale);
            return Task.CompletedTask;
        }

        public Task<IReadOnlyList<Sale>> GetAllAsync()
        {
            return Task.FromResult((IReadOnlyList<Sale>)_sales.ToList());
        }

        public Task<Sale> GetByIdAsync(Guid id)
        {
            var sale = _sales.FirstOrDefault(s => s.Id == id);
            return Task.FromResult(sale);
        }

        public Task<IReadOnlyList<Sale>> GetByDateRangeAsync(DateTime inicio, DateTime fim)
        {
            var vendas = _sales.Where(v => v.Data.Date >= inicio.Date && v.Data.Date <= fim.Date).ToList();
            return Task.FromResult((IReadOnlyList<Sale>)vendas);
        }

        public Task<IReadOnlyList<Sale>> GetByClientIdAsync(Guid clienteId)
        {
            var vendas = _sales.Where(s => s.ClienteId == clienteId).ToList();
            return Task.FromResult((IReadOnlyList<Sale>)vendas);
        }

        public Task DeleteAsync(Guid id)
        {
            var sale = _sales.FirstOrDefault(s => s.Id == id);
            if (sale != null)
            {
                _sales.Remove(sale);
            }
            return Task.CompletedTask;
        }
    }
}