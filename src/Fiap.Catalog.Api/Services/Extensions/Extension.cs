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
  }
}
