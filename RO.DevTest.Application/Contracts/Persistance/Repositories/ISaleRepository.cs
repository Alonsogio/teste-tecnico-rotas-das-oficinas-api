using RO.DevTest.Application.Contracts.Persistance.Repositories;
using RO.DevTest.Domain.Entities;

namespace RO.DevTest.Application.Contracts.Persistence.Repositories
{
  public interface ISaleRepository : IBaseRepository<Sale>
  {
    private static readonly List<Sale> _sales = new();
    public Task<IReadOnlyList<Sale>> GetAllAsync()
    {
      return Task.FromResult((IReadOnlyList<Sale>)_sales.ToList());
    }

    public Task<Sale> GetByIdAsync(Guid id)
    {
      var sale = _sales.FirstOrDefault(s => s.Id == id);
      return Task.FromResult(sale);
    }

    public new Task<IReadOnlyList<Sale>> GetByDateRangeAsync(DateTime inicio, DateTime fim)
    {
      var vendas = _sales.Where(v => v.Data.Date >= inicio.Date && v.Data.Date <= fim.Date).ToList();
      return Task.FromResult((IReadOnlyList<Sale>)vendas);
    }
  }
}