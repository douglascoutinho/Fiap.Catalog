using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;

namespace Fiap.Catalog.Infra.Data.EntityFramework
{
  public class ContextoFactory : IDesignTimeDbContextFactory<Contexto>
  {
    public IConfigurationRoot Configuration { get; set; }

    public Contexto CreateDbContext(string[] args)
    {
      var path = Directory.GetCurrentDirectory();

      var builder = new ConfigurationBuilder()
        .SetBasePath(path)
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);

      IConfigurationRoot config = builder.Build();

      var connectionString = config.GetConnectionString("DefaultConnection");

      var schema = Contexto.DefaultSchema;
      var optionsBuilder = new DbContextOptionsBuilder<Contexto>();
      optionsBuilder.UseSqlite(connectionString, opts =>
      {
        opts.MigrationsHistoryTable(HistoryRepository.DefaultTableName, schema);
        opts.CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds);
      });

      return new Contexto(optionsBuilder.Options, schema);
    }
  }
}