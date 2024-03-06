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

    public Peca Peca(int linha, int colunas)
    {
        return pecas[linha, colunas];
    }

    public Peca Peca(Posicao pos)
    {
        return pecas[pos.Linha, pos.Coluna];
    }

    public bool ExistePeca(Posicao pos)
    {
        ValidarPosicao(pos);
        return Peca(pos) != null;
    }

    public void ColocarPeca(Peca p, Posicao pos)
    {
        if (ExistePeca(pos))
        {
            throw new TabuleiroException("Já existe uma peça nessa posição");
        }
        pecas[pos.Linha, pos.Coluna] = p;
        p.Posicao = pos;
    }

    public Peca RetirarPeca(Posicao pos)
    {
        if (Peca(pos) == null)
        {
            return null;
        }
        Peca aux = Peca(pos);
        aux.Posicao=null;
        pecas[pos.Linha, pos.Coluna] = null;
        return aux;
    }

    public bool PosicaoValida(Posicao pos)
    {
        if (pos.Linha<0 || pos.Linha>=Linhas || pos.Coluna<0 || pos.Coluna>=Colunas)
        {
            return false;
        }
        return true;
    }

    public void ValidarPosicao(Posicao pos)
    {
        if (!PosicaoValida(pos))
        {
            throw new TabuleiroException("Posição Inválida!");
        }
    }
}
