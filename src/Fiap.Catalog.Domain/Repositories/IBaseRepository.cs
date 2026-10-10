namespace Fiap.Catalog.Domain.Repositories
{
  public interface IBaseRepository<TModel> : IDisposable
    where TModel : class
  {
    void Add(TModel model);
    void AddRange(ICollection<TModel> model);
    void Update(TModel model);
    void Delete(TModel model);
    TModel? GetById(int id);
    IQueryable<TModel> Get(System.Linq.Expressions.Expression<Func<TModel, bool>> where);
    IQueryable<TModel> Get();
    void Commit();
  }
}
