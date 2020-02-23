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
            var board = BoardFactory.LoadBoardFromFen("r1b2rk1/ppq1bppp/2nppn2/8/2B1PP2/1NN1B3/PPP3PP/R2Q1RK1 w - - 0 1");
            var minmax = new MinMaxAI();
            minmax.MinMaxList(board, 5);
        }
    }
}
