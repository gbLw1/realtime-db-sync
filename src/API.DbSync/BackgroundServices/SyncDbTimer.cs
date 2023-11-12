
using API.DbSync.Data;
using Microsoft.EntityFrameworkCore;

namespace API.DbSync.BackgroundServices;

public class SyncDbTimer : BackgroundService
{
    private readonly ILogger<SyncDbTimer> _logger;
    private readonly IServiceProvider _serviceProvider;

    public SyncDbTimer(
        ILogger<SyncDbTimer> logger,
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Executa a cada 1 minuto
        using PeriodicTimer timer = new(TimeSpan.FromMinutes(1));

        while (!stoppingToken.IsCancellationRequested)
        {
            await timer.WaitForNextTickAsync(stoppingToken);
            _logger.LogInformation("SyncDbTimer executado em: {time}", DateTime.Now);

            using IServiceScope scope = _serviceProvider.CreateScope();
            var mySqlDbCtx = scope.ServiceProvider.GetRequiredService<MySQLDbContext>();
            var sqlServerDbCtx = scope.ServiceProvider.GetRequiredService<SQLServerDbContext>();

            try
            {
                await SyncDatabases(mySqlDbCtx, sqlServerDbCtx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao sincronizar bancos de dados");
            }
        }
    }

    public async Task SyncDatabases(MySQLDbContext dbMySql, SQLServerDbContext dbSqlServer)
    {
        // get all data from sqlserver
        var produtosSqlServer = await dbSqlServer.Produtos.ToListAsync();
        var fornecedoresSqlServer = await dbSqlServer.Fornecedores.ToListAsync();
        var categoriasSqlServer = await dbSqlServer.Categorias.ToListAsync();
        var usuariosSqlServer = await dbSqlServer.Usuarios.ToListAsync();

        // delete all data from mysql
        dbMySql.Produtos.RemoveRange(dbMySql.Produtos);
        dbMySql.Fornecedores.RemoveRange(dbMySql.Fornecedores);
        dbMySql.Categorias.RemoveRange(dbMySql.Categorias);
        dbMySql.Usuarios.RemoveRange(dbMySql.Usuarios);

        // insert all data from sqlserver to mysql
        await dbMySql.Database.BeginTransactionAsync();
        try
        {
            await dbMySql.Produtos.AddRangeAsync(produtosSqlServer);
            await dbMySql.Fornecedores.AddRangeAsync(fornecedoresSqlServer);
            await dbMySql.Categorias.AddRangeAsync(categoriasSqlServer);
            await dbMySql.Usuarios.AddRangeAsync(usuariosSqlServer);

            await dbMySql.SaveChangesAsync();

            await dbMySql.Database.CommitTransactionAsync();
        }
        catch (Exception)
        {
            await dbMySql.Database.RollbackTransactionAsync();
            throw;
        }
    }
}
