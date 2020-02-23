using BenchmarkDotNet.Running;
using ParallelChess;
using ParallelChess.AI;
using System;

namespace ParallelChessPerformance {
    class Program {
        static void Main(string[] args) {

            //for (int i = 0; i < 10000000; i++) {
            //    var board = BoardFactory.LoadBoardFromFen();

            //    var ai = new AIWorkerManager();

            //    ai.spawnWorkers(4);
            //    ai.analyzeBoard(board, 5).GetAwaiter().GetResult();
            //    ai.killWorkers();
            //    Console.WriteLine($"still working: {i}");
            //}


            //var games = PGNParser.ParseFile("C:/Users/Frithjof/Source/Repos/ParallelChess/ParallelChessTests/Resources/tactics.pgn");

            //var board = BoardFactory.LoadBoardFromFen("r1b1n2r/1q1nNpbk/1p1p2p1/p2NpPPp/2P1P2P/3BB3/PP6/R2QK2R w - - 0 1");

            //var moves = Board.GetMoves(board);

            //BestMove foundMove = MinMaxAI.MinMax(board, 5);

            //Console.WriteLine("hi");
            //Console.WriteLine("Hello World!");
            var summary = BenchmarkRunner.Run<GetMovePerformance>();

            //var summary = BenchmarkRunner.Run<ForVsForeach>();
            //UsingBoardStateStruct usingBoardStateStruct = new UsingBoardStateStruct();
            //usingBoardStateStruct.makeAMove();
        }
    }
}
