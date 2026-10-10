using Fiap.Catalog.Domain.Entity;
using Fiap.Catalog.Domain.Repositories;
using Fiap.Catalog.Infra.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Fiap.Catalog.Infra.Repositories
{
  public class JogoBibliotecaRepository : BaseRepository<JogoBiblioteca>, IJogoBibliotecaRepository
  {
    readonly Contexto? _contexto;

    public JogoBibliotecaRepository(Contexto? contexto) : base(contexto)
    {
      _contexto = contexto;
    }

    public override IQueryable<JogoBiblioteca> Get()
    {
      return _contexto.Set<JogoBiblioteca>().Include(x => x.Jogo);
    }
  }
}
