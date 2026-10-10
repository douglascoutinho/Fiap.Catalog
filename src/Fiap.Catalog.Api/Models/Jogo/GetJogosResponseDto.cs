namespace Fiap.Catalog.Api.Models.Jogo
{
  public class GetJogosResponseDto
  {
    /// <summary>
    /// Identificador do usuário
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Nome
    /// </summary>
    public string? Nome { get; set; }

    /// <summary>
    /// Preco
    /// </summary>
    public decimal? Preco { get; set; }

    /// <summary>
    /// Ativo
    /// </summary>
    public bool Ativo { get; set; }

    /// <summary>
    /// Data Cadastro
    /// </summary>
    public DateTime DataCadastro { get; set; }
  }
}
