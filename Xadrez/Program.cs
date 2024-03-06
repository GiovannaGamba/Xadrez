using System;
using Tabuleiros;
using pecasXadrez;
using Xadrez;

class Program
{
    static void Main(string[] args)
    {

        PosicaoXadrez pos = new PosicaoXadrez('a', 1);

        Console.WriteLine(pos);

        Console.WriteLine(pos.ToPosicao());

        Console.ReadLine();
    }
}