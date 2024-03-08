using System.Collections.Generic;
using Tabuleiros;

namespace pecasXadrez;
class PartidaDeXadrez
{
    public Tabuleiro Tab {  get; private set; }
    public int Turno { get; private set; }
    public Cor JogadorAtual { get; private set; }
    public bool Terminada {  get; private set; }
    private HashSet<Peca> Pecas;
    private HashSet<Peca> Capturadas;
    public bool Xeque { get; private set; }

    public PartidaDeXadrez()
    {
        Tab = new Tabuleiro(8, 8);
        Turno = 1;
        JogadorAtual = Cor.Branco;
        Terminada = false;
        Xeque = false;
        Pecas = new HashSet<Peca>();
        Capturadas = new HashSet<Peca>();
        ColocarPecas();
    }

    public Peca ExecutaMovimento(Posicao origem, Posicao destino)
    {
        Peca p = Tab.RetirarPeca(origem);
        p.IncrementarQteMovimentos();
        Peca pecaCapturada = Tab.RetirarPeca(destino);
        Tab.ColocarPeca(p, destino);
        if (pecaCapturada != null) 
        {
            Capturadas.Add(pecaCapturada);
        }


        return pecaCapturada;
    }

    public void DesfazMovimento(Posicao origem, Posicao destino, Peca pecaCapturada)
    {
        Peca p = Tab.RetirarPeca(destino);
        p.DecrementarQteMovimentos();
        if (pecaCapturada != null)
        {
            Tab.ColocarPeca(pecaCapturada, destino);
            Capturadas.Remove(pecaCapturada);
        }
        Tab.ColocarPeca(p, origem);
    }

    public void RealizaJogada(Posicao origem, Posicao destino)
    {
        Peca pecaCapturada = ExecutaMovimento(origem, destino);

        if (EstaEmXeque(JogadorAtual))
        {
            DesfazMovimento(origem, destino, pecaCapturada);
            throw new TabuleiroException("Você não pode se colocar em xeque! ");
        }
        
        if (EstaEmXeque(Adversaria(JogadorAtual)))
        {
            Xeque = true;
        }
        else
        {
            Xeque = false;
        }

        if (TesteXequeMate(Adversaria(JogadorAtual)))
        {
            Terminada = true;
        }
        else
        {
            Turno++;
            MudaJogador();
        }
    }

    public void ValidarPosicaoDeOrigem(Posicao pos)
    {
        if (Tab.Peca(pos) == null)
        {
            throw new TabuleiroException("Não existe peça na posição de origem escolhida!");
        }
        if (JogadorAtual != Tab.Peca(pos).Cor)
        {
            throw new TabuleiroException("A peça de origem escolhida não é sua! ");
        }
        if (!Tab.Peca(pos).ExisteMovimentosPossiveis())
        {
            throw new TabuleiroException("Não há movimentos possíveis para a peça de origem escolhida! ");
        }
    }

    public void ValidarPosicaoDeDestino(Posicao origem, Posicao destino)
    {
        if (! Tab.Peca(origem).MovimentoPossivel(destino))
        {
            throw new TabuleiroException("Posição de destino inválida!");
        }
    }

    private void MudaJogador()
    {
        if (JogadorAtual == Cor.Branco)
        {
            JogadorAtual = Cor.Preto;
        }
        else
        {
            JogadorAtual = Cor.Branco;
        }
    }

    public HashSet<Peca> PecasCapturadas (Cor cor)
    {
        HashSet<Peca> aux = new HashSet<Peca>();
        foreach (Peca x in Capturadas)
        {
            if (x.Cor == cor)
            {
                aux.Add(x);
            }
        }
        return aux;
    }

    public HashSet<Peca> PecasEmJogo(Cor cor)
    {
        HashSet<Peca> aux = new HashSet<Peca>();
        foreach (Peca x in Pecas)
        {
            if (x.Cor == cor)
            {
                aux.Add(x);
            }
        }
        aux.ExceptWith(PecasCapturadas(cor));
        return aux;
    }

    private Cor Adversaria(Cor cor)
    {
        if (cor == Cor.Branco)
        {
            return Cor.Preto;
        }
        else
        {
            return Cor.Branco;
        }
    }

    private Peca Rei(Cor cor)
    {
        foreach (Peca x in PecasEmJogo(cor))
        {
            if (x is Rei)
            {
                return x;
            }
        }
        return null;
    }

    public bool EstaEmXeque(Cor cor)
    {
        Peca r = Rei(cor);
        if (r == null)
        {
            throw new TabuleiroException("Não tem rei da cor" + cor + " no tabuleiro!");
        }

        foreach (Peca x in PecasEmJogo(Adversaria(cor)))
        {
            bool[,] mat = x.MovimentosPossiveis();
            if (mat[r.Posicao.Linha, r.Posicao.Coluna])
            {
                return true;
            }
        }
        return false;
    }

    public bool TesteXequeMate(Cor cor)
    {
        if (!EstaEmXeque(cor))
        {
            return false;   
        }
        foreach (Peca x in PecasEmJogo(cor))
        {
            bool[,] mat = x.MovimentosPossiveis();
            for (int i = 0; i<Tab.Linhas; i++)
            {
                for (int j = 0; j < Tab.Colunas; j++)
                {
                    if (mat[i,j])
                    {
                        Posicao origem = x.Posicao;
                        Posicao destino = new Posicao(i,j);
                        Peca pecaCapturada = ExecutaMovimento(origem, destino);
                        bool testeXeque = EstaEmXeque(cor);
                        DesfazMovimento(origem, destino, pecaCapturada);
                        if(!testeXeque)
                        {
                            return false;
                        }
                    }
                }
            }
        }
        return true;
    }

    public void ColocarNovaPeca( char coluna, int linha, Peca peca)
    {
        Tab.ColocarPeca(peca, new PosicaoXadrez(coluna, linha).ToPosicao());
        Pecas.Add(peca);
    }

    private void ColocarPecas()
    {
        ColocarNovaPeca('a', 1, new Torre(Tab, Cor.Branco));
        ColocarNovaPeca('b', 1, new Cavalo(Tab, Cor.Branco));
        ColocarNovaPeca('c', 1, new Bispo(Tab, Cor.Branco));
        ColocarNovaPeca('d', 1, new Dama(Tab, Cor.Branco));
        ColocarNovaPeca('e', 1, new Rei(Tab, Cor.Branco));
        ColocarNovaPeca('f', 1, new Bispo(Tab, Cor.Branco));
        ColocarNovaPeca('g', 1, new Cavalo(Tab, Cor.Branco));
        ColocarNovaPeca('h', 1, new Torre(Tab, Cor.Branco));
        ColocarNovaPeca('a', 2, new Peao(Tab, Cor.Branco, this));
        ColocarNovaPeca('c', 2, new Peao(Tab, Cor.Branco, this));
        ColocarNovaPeca('b', 2, new Peao(Tab, Cor.Branco, this));
        ColocarNovaPeca('d', 2, new Peao(Tab, Cor.Branco, this));
        ColocarNovaPeca('e', 2, new Peao(Tab, Cor.Branco, this));
        ColocarNovaPeca('f', 2, new Peao(Tab, Cor.Branco, this));
        ColocarNovaPeca('g', 2, new Peao(Tab, Cor.Branco, this));
        ColocarNovaPeca('h', 2, new Peao(Tab, Cor.Branco, this));

        ColocarNovaPeca('a', 8, new Torre(Tab, Cor.Preto));
        ColocarNovaPeca('b', 8, new Cavalo(Tab, Cor.Preto));
        ColocarNovaPeca('c', 8, new Bispo(Tab, Cor.Preto));
        ColocarNovaPeca('d', 8, new Dama(Tab, Cor.Preto));
        ColocarNovaPeca('e', 8, new Rei(Tab, Cor.Preto));
        ColocarNovaPeca('f', 8, new Bispo(Tab, Cor.Preto));
        ColocarNovaPeca('g', 8, new Cavalo(Tab, Cor.Preto));
        ColocarNovaPeca('h', 8, new Torre(Tab, Cor.Preto));
        ColocarNovaPeca('a', 7, new Peao(Tab, Cor.Preto, this));
        ColocarNovaPeca('b', 7, new Peao(Tab, Cor.Preto, this));
        ColocarNovaPeca('c', 7, new Peao(Tab, Cor.Preto, this));
        ColocarNovaPeca('d', 7, new Peao(Tab, Cor.Preto, this));
        ColocarNovaPeca('e', 7, new Peao(Tab, Cor.Preto, this));
        ColocarNovaPeca('f', 7, new Peao(Tab, Cor.Preto, this));
        ColocarNovaPeca('g', 7, new Peao(Tab, Cor.Preto, this));
        ColocarNovaPeca('h', 7, new Peao(Tab, Cor.Preto, this));
    }

}
