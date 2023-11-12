namespace API.DbSync.Models;

public class Produto
{
    public int Id { get; set; }
    public required string Nome { get; set; }
    public double Preco { get; set; }
}
