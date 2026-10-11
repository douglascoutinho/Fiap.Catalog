using Fiap.Catalog.Domain.Entity;
using Fiap.Catalog.Domain.Repositories;
using Fiap.Catalog.Domain.Services.Event;
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

    public async Task RealizarPedido(JogoBiblioteca comprar)
    {
      var result = new EventResult<ProcessarPagamentoEvent>
      {
        Key = Guid.NewGuid(),
        Event = "OrderPlacedEvent",

        Data = new ProcessarPagamentoEvent
        {
          IdUsuario = comprar.Id_Usuario,
          IdJogo = comprar.Id_Jogo,
          Preco = comprar.Preco
        }
      };

      // Envia mensagem para o RabbitMQ (Api - Pagamento) solicitando a aprovação do pagamento
      await _rabbitMQ.SendMessageAsync(result, "queue-order-placed");
    }    

    public async Task Incluir(EventResult<ProcessarPagamentoEvent>? eventResult)
    {
      if (eventResult?.Data?.StatusPagamento == Entity.Enum.StatusPagamentoEnum.Reprovado) return;

      var adicinarBiblioteca = new JogoBiblioteca
      (
        idUsuario: eventResult?.Data?.IdUsuario ?? 0,
        idJogo : eventResult?.Data?.IdJogo ?? 0,
        preco : eventResult?.Data?.Preco ?? 0
      );

      _jogoBibliotecaRepository.Add(adicinarBiblioteca);

      _jogoBibliotecaRepository.Commit();
    }
  }
}
