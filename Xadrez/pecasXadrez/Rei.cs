using Tabuleiros;
namespace pecasXadrez;
class Rei : Peca
{
    private PartidaDeXadrez partida;
    public Rei(Tabuleiro tab, Cor cor, PartidaDeXadrez partida) : base(tab, cor)
    {
        this.partida = partida;
    }

    public override string ToString()
    {
        return "R";
    }

    private bool PodeMover(Posicao pos)
    {
        Peca p = Tab.Peca(pos);
        return p == null || p.Cor != Cor;
    }

    private bool TesteTorreParaRoque(Posicao pos)
    {
        Peca p = Tab.Peca(pos);
        return p != null && p is Torre && p.Cor == Cor && p.QteMovimentos == 0;
    }

    public override bool[,] MovimentosPossiveis()
    {
        bool[,] mat = new bool[Tab.Linhas, Tab.Colunas];

        Posicao pos = new Posicao(0, 0);

        pos.DefinirValores(Posicao.Linha - 1, Posicao.Coluna);

        // acima
        if (Tab.PosicaoValida(pos) && PodeMover(pos))
        {
            mat[pos.Linha, pos.Coluna] = true;
        }

        // ne
        pos.DefinirValores(Posicao.Linha - 1, Posicao.Coluna + 1);

        if (Tab.PosicaoValida(pos) && PodeMover(pos))
        {
            mat[pos.Linha, pos.Coluna] = true;
        }

        // direita
        pos.DefinirValores(Posicao.Linha, Posicao.Coluna + 1);

        if (Tab.PosicaoValida(pos) && PodeMover(pos))
        {
            mat[pos.Linha, pos.Coluna] = true;
        }

        // se
        pos.DefinirValores(Posicao.Linha + 1, Posicao.Coluna + 1);

        if (Tab.PosicaoValida(pos) && PodeMover(pos))
        {
            mat[pos.Linha, pos.Coluna] = true;
        }

        // abaixo
        pos.DefinirValores(Posicao.Linha + 1, Posicao.Coluna);

        if (Tab.PosicaoValida(pos) && PodeMover(pos))
        {
            mat[pos.Linha, pos.Coluna] = true;
        }

        // so
        pos.DefinirValores(Posicao.Linha + 1, Posicao.Coluna - 1);

        if (Tab.PosicaoValida(pos) && PodeMover(pos))
        {
            mat[pos.Linha, pos.Coluna] = true;
        }

        // esquerda
        pos.DefinirValores(Posicao.Linha, Posicao.Coluna - 1);

        if (Tab.PosicaoValida(pos) && PodeMover(pos))
        {
            mat[pos.Linha, pos.Coluna] = true;
        }

        // no
        pos.DefinirValores(Posicao.Linha - 1, Posicao.Coluna - 1);

        if (Tab.PosicaoValida(pos) && PodeMover(pos))
        {
            mat[pos.Linha, pos.Coluna] = true;
        }

        // #jogadaespecial roque
        if (QteMovimentos==0 && !partida.Xeque)
        {
            // #jogadaespecial roque pequeno
            Posicao posT1 = new Posicao(Posicao.Linha, Posicao.Coluna + 3);
            if (TesteTorreParaRoque(posT1))
            {
                Posicao p1 = new Posicao(Posicao.Linha, Posicao.Coluna + 1);
                Posicao p2 = new Posicao(Posicao.Linha, Posicao.Coluna + 2);
                if (Tab.Peca(p1)==null && Tab.Peca(p2)==null)
                {
                    mat[Posicao.Linha, Posicao.Coluna + 2] = true;
                }
            }

            // #jogadaespecial roque grande
            Posicao posT2 = new Posicao(Posicao.Linha, Posicao.Coluna - 4);
            if (TesteTorreParaRoque(posT2))
            {
                Posicao p1 = new Posicao(Posicao.Linha, Posicao.Coluna - 1);
                Posicao p2 = new Posicao(Posicao.Linha, Posicao.Coluna - 2);
                Posicao p3 = new Posicao(Posicao.Linha, Posicao.Coluna - 3);
                if (Tab.Peca(p1)==null && Tab.Peca(p2)==null && Tab.Peca(p3)==null)
                {
                    mat[Posicao.Linha, Posicao.Coluna - 2] = true;
                }
            }
        }
        return mat;
    }
}
