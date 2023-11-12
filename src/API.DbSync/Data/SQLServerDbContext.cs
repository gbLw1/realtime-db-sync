using API.DbSync.Models;
using Microsoft.EntityFrameworkCore;

namespace API.DbSync.Data;

public class SQLServerDbContext : DbContext
{
    public SQLServerDbContext(DbContextOptions<SQLServerDbContext> options) : base(options) { }

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Fornecedor> Fornecedores { get; set; }
    public DbSet<Produto> Produtos { get; set; }
    public DbSet<Categoria> Categorias { get; set; }
}
