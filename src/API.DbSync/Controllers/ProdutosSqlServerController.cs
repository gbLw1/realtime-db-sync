using API.DbSync.Data;
using API.DbSync.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.DbSync.Controllers;

/// <summary>
/// Controller para acesso aos dados da tabela Produtos no SQL Server
/// </summary>
[Route("produtos-sqlserver")]
[ApiController]
public class ProdutosSqlServerController : ControllerBase
{
    private readonly SQLServerDbContext _sqlServerDbCtx;

    /// <summary>
    /// Inicializa uma nova instância de <see cref="ProdutosSqlServerController"/>
    /// </summary>
    /// <param name="sqlServerDbCtx"></param>
    public ProdutosSqlServerController(SQLServerDbContext sqlServerDbCtx)
    {
        _sqlServerDbCtx = sqlServerDbCtx;
    }

    /// <summary>
    /// Obtém todos os produtos
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var produtos = await _sqlServerDbCtx.Produtos.ToListAsync();
        return Ok(produtos);
    }

    /// <summary>
    /// Obtém um produto pelo seu Id
    /// </summary>
    /// <param name="id"></param>
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var produto = await _sqlServerDbCtx.Produtos.FindAsync(id);

        if (produto is null)
            return NotFound();

        return Ok(produto);
    }

    /// <summary>
    /// Cria um novo produto
    /// </summary>
    /// <param name="produto"></param>
    [HttpPost]
    public async Task<IActionResult> Post(Produto produto)
    {
        await _sqlServerDbCtx.Produtos.AddAsync(produto);
        await _sqlServerDbCtx.SaveChangesAsync();
        return Ok(produto);
    }

    /// <summary>
    /// Atualiza um produto
    /// </summary>
    /// <param name="id"></param>
    /// <param name="produto"></param>
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, Produto produto)
    {
        var dbProduto = await _sqlServerDbCtx.Produtos.FirstOrDefaultAsync(p => p.Id == id);

        if (dbProduto is null)
            return NotFound();

        dbProduto.Nome = produto.Nome;
        dbProduto.Preco = produto.Preco;

        _sqlServerDbCtx.Produtos.Update(dbProduto);
        await _sqlServerDbCtx.SaveChangesAsync();

        return Ok(dbProduto);
    }

    /// <summary>
    /// Exclui um produto
    /// </summary>
    /// <param name="id"></param>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var produto = await _sqlServerDbCtx.Produtos.FirstOrDefaultAsync(p => p.Id == id);

        if (produto is null)
            return NotFound();

        _sqlServerDbCtx.Produtos.Remove(produto);
        await _sqlServerDbCtx.SaveChangesAsync();

        return Ok(produto);
    }
}
