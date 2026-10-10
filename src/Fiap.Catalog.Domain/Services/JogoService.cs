using Fiap.Catalog.Domain.Entity;
using Fiap.Catalog.Domain.Repositories;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace Fiap.Catalog.Domain.Services
{
  public class JogoService
  {
    IJogoRepository _jogoRepository;
    IJogoBibliotecaRepository _jogoBibliotecaRepository;

    public JogoService(IJogoRepository jogoRepository, IJogoBibliotecaRepository jogoBibliotecaRepository)
    {
      _jogoRepository = jogoRepository;
      _jogoBibliotecaRepository = jogoBibliotecaRepository;
    }

    public ICollection<Jogo> Buscar(bool? ativo, int offSet, int limit)
    {
      var query = _jogoRepository.Get();

      if (ativo != null)
        query = query.Where(a => a.Ativo == ativo);

      query = query.Skip(offSet).Take(limit);

      return query.ToList();
    }

    public Jogo BuscarPorId(int id)
    {
      var Jogo = _jogoRepository.GetById(id);

      if (Jogo == null)
        throw new ValidationException("Informado identificador que não existe.");

      return Jogo;
    }

    public void Incluir(Jogo jogo)
    {
      var JogoExistente = _jogoRepository.GetById(jogo.Id);

      if (JogoExistente != null)
        throw new ApplicationException("Informado identificador já existe.");

      _jogoRepository.Add(jogo);

      _jogoRepository.Commit();
    }

    public void Alterar(Jogo jogoExistente, string? nome, decimal preco, bool ativo)
    {

      if (jogoExistente == null)
        throw new ValidationException("Informado identificador que não existe.");

      jogoExistente.Atualizar
      (
          nome,
          preco,
          ativo
      );

      _jogoRepository.Update(jogoExistente);

      _jogoRepository.Commit();
    }

    public void Comprar(JogoBiblioteca comprar)
    {
      _jogoBibliotecaRepository.Add(comprar);

      _jogoBibliotecaRepository.Commit();
    }
  }
}
