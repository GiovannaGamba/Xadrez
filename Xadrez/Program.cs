using System;
using Tabuleiros;
using pecasXadrez;
using Xadrez;

class Program
{
    static void Main(string[] args)
    {
        Tabuleiro tab = new Tabuleiro(8, 8);

        tab.ColocarPeca(new Torre(tab, Cor.Preto), new Posicao(0, 0));
        tab.ColocarPeca(new Torre(tab, Cor.Preto), new Posicao(1, 3));
        tab.ColocarPeca(new Rei(tab, Cor.Preto), new Posicao(2, 4));

        Tela.imprimirTabuleiro(tab);

        Console.ReadLine();
    }
}