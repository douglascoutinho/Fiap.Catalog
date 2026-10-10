using Fiap.Catalog.Domain.Entity;
using Fiap.Catalog.Domain.Repositories;
using Fiap.Catalog.Infra.Data.EntityFramework;

namespace Fiap.Catalog.Infra.Repositories
{
  public class JogoRepository : BaseRepository<Jogo>, IJogoRepository
  {
    readonly Contexto? _contexto;

    public JogoRepository(Contexto? contexto) : base(contexto)
    {
      _contexto = contexto;
    }
  }
}
