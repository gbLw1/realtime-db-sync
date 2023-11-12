using API.DbSync.Data;
using API.DbSync.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.DbSync.Controllers;
[Route("[controller]")]
[ApiController]
public class ProdutosController : ControllerBase
{
    private readonly SQLServerDbContext _sqlServerDbCtx;

    public ProdutosController(SQLServerDbContext sqlServerDbCtx)
    {
        _sqlServerDbCtx = sqlServerDbCtx;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var produtos = await _sqlServerDbCtx.Produtos.ToListAsync();
        return Ok(produtos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var produto = await _sqlServerDbCtx.Produtos.FindAsync(id);

        if (produto is null)
            return NotFound();

        return Ok(produto);
    }

    [HttpPost]
    public async Task<IActionResult> Post(Produto produto)
    {
        await _sqlServerDbCtx.Produtos.AddAsync(produto);
        await _sqlServerDbCtx.SaveChangesAsync();
        return Ok(produto);
    }

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
