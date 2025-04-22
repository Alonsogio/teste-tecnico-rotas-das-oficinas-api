namespace RO.DevTest.Domain.Entities
{
  public class Sale
  {
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ClienteId { get; set; }
    public DateTime Data { get; set; } = DateTime.UtcNow;
    public decimal Total { get; set; }
    public List<SaleItem> Itens { get; set; } = new();
  }

  public class SaleItem
  {
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ProdutoId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public decimal PrecoUnitario { get; set; }
    public int Quantidade { get; set; }
  }
}