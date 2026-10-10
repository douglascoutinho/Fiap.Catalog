namespace Fiap.Catalog.Api.Models.Biblioteca
{
  public class GetBibliotecasResponseDto
  {
    /// <summary>
    /// Identificador da biblioteca
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Nome do Jogo Comprado
    /// </summary>
    public string? Nome { get; set; }

    /// <summary>
    /// Preco
    /// </summary>
    public decimal Preco { get; set; }

    /// <summary>
    /// Identificador do usuário
    /// </summary>
    public int Id_Usuario { get; set; }
  }
}
