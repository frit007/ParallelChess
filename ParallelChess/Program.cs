using System;

namespace ParallelChess {
    class Program {
        static void Main() {

            long i = 0b1000_0000_0000_0000__0000_0000_0000_0000_0000;
            long ii = 0b1100_0000_0000_0000__0000_0000_0000_0000_0000;
            Console.WriteLine(i & i);
            Console.WriteLine("Please enter a FEN");
            
            //BoardState board = Chess.LoadBoardFromFen("4k3/8/8/8/7q/3N4/8/3QK3 w - - 0 1");
            BoardState board = Chess.LoadBoardFromFen(Console.ReadLine());

            //Chess.MakeMove(board, BoardStateOffset.B8, BoardStateOffset.B7);

            Console.WriteLine(Chess.AsciiBoard(board));
        }
    }
}