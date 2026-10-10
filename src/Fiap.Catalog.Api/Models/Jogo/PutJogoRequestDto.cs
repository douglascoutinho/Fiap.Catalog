namespace Fiap.Catalog.Api.Models.Jogo
{
  public class PutJogoRequestDto
  {
    /// <summary>
    /// Nome
    /// </summary>
    public string? Nome { get; set; }

    /// <summary>
    /// Preco
    /// </summary>
    public decimal Preco { get; set; }

    /// <summary>
    /// Ativo
    /// </summary>
    public bool Ativo { get; set; }
  }
}
