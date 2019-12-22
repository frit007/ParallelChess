using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using ParallelChess;
using ParallelChess.AI;
namespace ParallelChessPerformance {
    [SimpleJob]
    public class MinMaxPerformance {
        [Benchmark]
        public void MinMaxTest() {
            var board = Chess.LoadBoardFromFen();
            MinMaxAI.MinMax(board, 6);
        //ParallelChess.AI.MinMax()
        }
    }
}
