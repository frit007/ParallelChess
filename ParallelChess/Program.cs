using System;

namespace ParallelChess {
    class Program {
        static void Main() {
            //var boardw = BoardFactory.LoadBoardFromFen();
            //AI.MinMax.minMax(boardw, 7);
            //Console.WriteLine($"evaluated moved: {AI.MinMaxAI.movesEvaluated}");
            Console.WriteLine("Please enter a FEN");
            
            //BoardState board = BoardFactory.LoadBoardFromFen("4k3/8/8/8/7q/3N4/8/3QK3 w - - 0 1");
            Board board = BoardFactory.LoadBoardFromFen(Console.ReadLine());

            //Chess.MakeMove(board, BoardStateOffset.B8, BoardStateOffset.B7);

            Console.WriteLine(ChessOutput.AsciiBoard(board));
        }
    }
}