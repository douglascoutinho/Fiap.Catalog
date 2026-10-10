namespace Fiap.Catalog.Api.Models.Biblioteca
{
  public class PostBibliotecaRequestDto
  {
    /// <summary>
    /// Identificador do jogo
    /// </summary>
    public int Id_Jogo { get; set; }

    /// <summary>
    /// Preco
    /// </summary>
    public decimal Preco { get; set; }
  }
}
