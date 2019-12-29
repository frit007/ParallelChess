using BenchmarkDotNet.Running;
using ParallelChess;
using ParallelChess.AI;
using System;
using static ParallelChess.AI.MinMaxAI;

namespace ParallelChessPerformance {
    class Program {
        static void Main(string[] args) {
            //var board = Chess.LoadBoardFromFen("r1b1n2r/1q1nNpbk/1p1p2p1/p2NpPPp/2P1P2P/3BB3/PP6/R2QK2R w - - 0 1");

            //var moves = Board.GetMoves(board);

            //BestMove foundMove = MinMaxAI.MinMax(board, 5);

            //Console.WriteLine("hi");
            //Console.WriteLine("Hello World!");
            var summary = BenchmarkRunner.Run<MinMaxPerformance>();

            //UsingBoardStateStruct usingBoardStateStruct = new UsingBoardStateStruct();
            //usingBoardStateStruct.makeAMove();
        }
    }
}
