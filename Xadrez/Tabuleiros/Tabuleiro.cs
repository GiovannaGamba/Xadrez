namespace Tabuleiros;
class Tabuleiro
{
    public int Linhas { get; set; }
    public int Colunas { get; set; }
    private Peca[,] pecas;

    public Tabuleiro(int linha, int colunas)
    {
        Linhas=linha;
        Colunas=colunas;
        pecas = new Peca[Linhas, Colunas];
    }
}
