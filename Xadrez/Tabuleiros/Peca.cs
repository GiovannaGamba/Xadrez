namespace Tabuleiros;
 abstract class Peca
{
    public Posicao Posicao {  get; set; }
    public Cor Cor {  get; protected set; }
    public int QteMovimentos { get; protected set; }
    public Tabuleiro Tab {  get; protected set; }

    public Peca(Tabuleiro tab, Cor cor)
    {
        Posicao=null;
        Cor=cor;
        Tab=tab;
        QteMovimentos = 0;
    }

    public void IncrementarQteMovimentos()
    {
        QteMovimentos++;
    }
    public abstract bool[,] MovimentosPossiveis();
   
}
