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
            var board = Board.LoadBoardFromFen();
            var minmax = new MinMaxAI();
            minmax.MinMaxList(board, 6);
        }
    }
}
