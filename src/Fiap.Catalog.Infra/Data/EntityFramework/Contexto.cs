using Fiap.Catalog.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Fiap.Catalog.Infra.Data.EntityFramework
{
  public class Contexto : DbContext
  {
    readonly string _defaultSchema;

    public static string DefaultSchema { get { return "dbo"; } }

    protected Contexto() : base() { }

    public Contexto(DbContextOptions<Contexto> options, string defaultSchema = "dbo") : base(options)
    {
      _defaultSchema = defaultSchema;

      this.ChangeTracker.LazyLoadingEnabled = false;
    }

    public DbSet<Jogo> Usuario { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.HasDefaultSchema(_defaultSchema);

      modelBuilder.Entity<Jogo>(entity =>
      {
        entity.HasKey(x => x.Id)
                  .HasName("PK_Jogo");

        entity.ToTable("Jogo", "dbo");

      });

      modelBuilder.Entity<JogoBiblioteca>(entity =>
      {
        entity.HasKey(x => x.Id)
                  .HasName("PK_JogoBiblioteca");

        entity.ToTable("JogoBiblioteca", "dbo");

        entity.HasOne(d => d.Jogo)
              .WithMany(p => p.JogoBibliotecas)
              .HasForeignKey(d => d.Id_Jogo)
              .OnDelete(DeleteBehavior.ClientSetNull)
              .HasConstraintName("FK_JogoBiblioteca_Jogo")
              ;
      });

      base.OnModelCreating(modelBuilder);
    }
  }
}