namespace RO.DevTest.Domain.Entities
{
  public class Product
  {
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public decimal Preco { get; set; }
    public int Estoque { get; set; }
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
  }
}