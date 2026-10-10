using Fiap.Catalog.Domain.Entity;
using Fiap.Catalog.Domain.Repositories;
using Fiap.Catalog.Domain.Services.Interface;
using System.Data;

namespace Fiap.Catalog.Domain.Services
{
  public class BibliotecaService
  {
    IJogoBibliotecaRepository _jogoBibliotecaRepository;
    IRabbitMQService _rabbitMQ;

    public BibliotecaService(IJogoBibliotecaRepository jogoBibliotecaRepository, IRabbitMQService rabbitMQ)
    {
      _jogoBibliotecaRepository = jogoBibliotecaRepository;
      _rabbitMQ = rabbitMQ;
    }

    public ICollection<JogoBiblioteca> Buscar(bool? ativo, int offSet, int limit)
    {
      var query = _jogoBibliotecaRepository.Get();

      if (ativo != null)
        query = query.Where(a => a.Ativo == ativo);

      query = query.Skip(offSet).Take(limit);

      return query.ToList();
    }

    public void Incluir(JogoBiblioteca comprar)
    {
      _jogoBibliotecaRepository.Add(comprar);

      _jogoBibliotecaRepository.Commit();
    }
  }
}
