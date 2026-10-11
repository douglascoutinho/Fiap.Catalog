using Fiap.Catalog.Domain.Services.Event;
using RabbitMQ.Client;
using System.Security.Claims;
using System.Text.Json;

namespace Fiap.Catalog.Api.Services.Extensions
{
  public static class Extension
  {
    public static int ConsultarUsuario(this ClaimsPrincipal claim)
    {
      var identificador = claim.FindFirst("identificador-usuario")?.Value;
      if (int.TryParse(identificador, out int id)) return id;

      return 0;
    }

    public static ConnectionFactory CarregarConnectionFactory(this IConfiguration config)
    {
      var factory = new ConnectionFactory
      {
        HostName = config["RabbitMQ:HostName"] ?? string.Empty,
        Port = int.Parse(config["RabbitMQ:Port"] ?? string.Empty),
        UserName = config["RabbitMQ:UserName"] ?? string.Empty,
        Password = config["RabbitMQ:Password"] ?? string.Empty,
        VirtualHost = config["RabbitMQ:VirtualHost"] ?? "/"
      };

      return factory;
    }

    public static string ProcessarEvento(this EventResult result, string json)
    {
      switch (result.Event)
      {
        case "PaymentProcessedEvent":
          var pagamento = JsonSerializer.Deserialize<EventResult<ProcessarPagamentoEvent>>(json);
          return $"Pagamento Processado: Status: {pagamento?.Data?.StatusPagamento} - {result.Key}";

        default: return string.Empty;
      }
    }


    public static string ProcessarMensagemConsole(this EventResult<ProcessarPagamentoEvent> result) => $"Pagamento Processado: Status: {result?.Data?.StatusPagamento} - {result?.Key}";

  }
}
