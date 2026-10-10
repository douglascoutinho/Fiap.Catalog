using Fiap.Catalog.Domain.Repositories;
using Fiap.Catalog.Infra.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Fiap.Catalog.Infra.Repositories
{
  public abstract class BaseRepository<TModel> : IBaseRepository<TModel>
    where TModel : class
  {
    private readonly Contexto? _contexto;
    protected BaseRepository(Contexto? context) =>  _contexto = context;
    public virtual void Add(TModel model) => _contexto?.Add(model);
    public virtual void AddRange(ICollection<TModel> model) => _contexto?.AddRange(model);
    public virtual void Update(TModel model) =>  _contexto?.Update(model);
    public virtual void Delete(TModel model) =>  _contexto?.Remove(model);
    public virtual TModel? GetById(int id) => _contexto?.Set<TModel>().Find(id);
    public virtual IQueryable<TModel?> Get(System.Linq.Expressions.Expression<Func<TModel, bool>> where) => _contexto?.Set<TModel>().AsNoTracking().Where(where) ?? Enumerable.Empty<TModel>().AsQueryable();
    public virtual IQueryable<TModel> Get() => _contexto?.Set<TModel>().AsNoTracking() ?? Enumerable.Empty<TModel>().AsQueryable();
    public void Commit() => _contexto?.SaveChanges();
    public virtual void Dispose() => _contexto?.Dispose();
  }
}
