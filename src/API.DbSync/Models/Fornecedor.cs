namespace API.DbSync.Models;

public class Fornecedor
{
    public int Id { get; set; }
    public required string Nome { get; set; }
    public required string TipoProdutos { get; set; }
}
