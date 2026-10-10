namespace Fiap.Catalog.Domain.Services.Interface
{
    public interface IRabbitMQService
    {
        Task SendMessageAsync(string message);
    }
}
