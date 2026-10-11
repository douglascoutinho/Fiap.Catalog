using Fiap.Catalog.Domain.Services.Event;

namespace Fiap.Catalog.Domain.Services.Interface
{
  public interface IRabbitMQService
  {
    Task SendMessageAsync<T>(T messageObject, string targetQueue, string exchangeName = "");
  }
}