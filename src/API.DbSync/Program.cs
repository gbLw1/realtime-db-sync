using API.DbSync.BackgroundServices;
using API.DbSync.Data;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.IncludeXmlComments(
        Path.Combine(
            AppContext.BaseDirectory,
            $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"
        )
    );
});

var mySqlConnectionString = builder.Configuration.GetConnectionString("MySql")
    ?? throw new ArgumentNullException("MySQL connection string not found");
builder.Services.AddDbContext<MySQLDbContext>(opt =>
    opt.UseMySql(mySqlConnectionString, ServerVersion.AutoDetect(mySqlConnectionString)));

var sqlServerConnectionString = builder.Configuration.GetConnectionString("SqlServer")
    ?? throw new ArgumentNullException("SQLServer connection string not found");
builder.Services.AddDbContext<SQLServerDbContext>(opt =>
    opt.UseSqlServer(sqlServerConnectionString));

builder.Services.AddHostedService<SyncDbTimer>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
