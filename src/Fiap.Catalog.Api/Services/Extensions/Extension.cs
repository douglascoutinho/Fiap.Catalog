using Fiap.Catalog.Domain.Services.Event;
using RabbitMQ.Client;
using System.Security.Claims;

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
      var section = config.GetSection("RabbitMQ");

      return new ConnectionFactory
      {
        HostName = ValorOuPadrao(section["Host"], "localhost"),
        Port = int.TryParse(section["Port"], out var port) ? port : 5672,
        UserName = ValorOuPadrao(section["User"], "admin"),
        Password = ValorOuPadrao(section["Password"], "admin1234"),
        VirtualHost = ValorOuPadrao(section["VirtualHost"], "/")
      };
    }

    public static string ProcessarMensagemConsole(this EventResult<ProcessarPagamentoEvent> result) => $"Pagamento Processado: Status: {result?.Data?.StatusPagamento} - {result?.Key}";

    static string ValorOuPadrao(string? valor, string padrao) => string.IsNullOrWhiteSpace(valor) ? padrao : valor;

  }
}
