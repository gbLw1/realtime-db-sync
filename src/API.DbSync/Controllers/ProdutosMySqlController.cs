using API.DbSync.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.DbSync.Controllers;

/// <summary>
/// Controller para acesso aos dados da tabela Produtos no MySQL
/// </summary>
[Route("produtos-mysql")]
[ApiController]
public class ProdutosMySqlController : ControllerBase
{
    private readonly MySQLDbContext _mySqlDbCtx;

    /// <summary>
    /// Inicializa uma nova instância de <see cref="ProdutosMySqlController"/>
    /// </summary>
    /// <param name="mySqlDbCtx"></param>
    public ProdutosMySqlController(MySQLDbContext mySqlDbCtx)
    {
        _mySqlDbCtx = mySqlDbCtx;
    }

    /// <summary>
    /// Obtém todos os produtos
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var produtos = await _mySqlDbCtx.Produtos.ToListAsync();
        return Ok(produtos);
    }

    /// <summary>
    /// Obtém um produto pelo seu Id
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var produto = await _mySqlDbCtx.Produtos.FindAsync(id);

        if (produto is null)
            return NotFound();

        return Ok(produto);
    }
}
