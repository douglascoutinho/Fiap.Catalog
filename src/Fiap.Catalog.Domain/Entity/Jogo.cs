namespace Fiap.Catalog.Domain.Entity
{
  public class Jogo : EntityBase
  {
    protected Jogo() { }

    public Jogo(string? nome, decimal preco)
    {
      this.Nome = nome;
      this.Preco = preco;
      this.Ativo = true;
      this.DataCadastro = DateTime.Now;
    }

    public string? Nome { get; private set; }
    public decimal Preco { get; private set; }
    public virtual ICollection<JogoBiblioteca> JogoBibliotecas { get; private set; }

    internal void Atualizar(string? nome, decimal preco, bool ativo)
    {
      this.Nome = nome;
      this.Preco = preco;
      this.Ativo = ativo;
    }
  }
}
