namespace Fiap.Catalog.Domain.Entity
{
  public class JogoBiblioteca : EntityBase
  {
    protected JogoBiblioteca() { }

    public JogoBiblioteca(int idUsuario, int idJogo, decimal preco)
    {
      Id_Usuario = idUsuario;
      Id_Jogo = idJogo;
      Preco = preco;
    }

    public int Id_Usuario { get; private set; }
    public int Id_Jogo { get; private set; }
    public decimal Preco { get; private set; }
    public virtual Jogo Jogo { get; private set; }
  }
}
