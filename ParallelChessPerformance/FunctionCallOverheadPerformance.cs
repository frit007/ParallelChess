using BenchmarkDotNet.Attributes;
using ParallelChess;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParallelChessPerformance {
    [SimpleJob]
    public class FunctionCallOverheadPerformance {
        [ThreadStatic]
        public static int counter = 0;
        
        public int classCounter = 0;

        //[Benchmark]
        //public void inlineFunction() {
        //    for (int i = 0; i < 1000000000; i++) {
        //        //if ((i & 88) == 0) {
        //        //}
        //            counter++;
        //    }
        //}
        public static void incStatic() {
            counter++;
        }
        public void inc() {
            classCounter++;
        }

        public static bool IsValidStaticPosition(int position) {
            return (position & 0x88) == 0;
        }

        [Benchmark]
        public void callStaticFunction() {
            for (int i = 0; i < 1000000000; i++) {
                incStatic();
                //counter++;
                //if (IsValidStaticPosition(i)) {
                //}
            }
        }

        public bool IsValidPosition(int position) {
            return (position & 0x88) == 0;
        }

        [Benchmark]
        public void callClassFunction() {
            for (int i = 0; i < 1000000000; i++) {
                inc();
                //counter++;
                //if (IsValidPosition(i)) {
                //}
            }
        }

        //[Benchmark]
        //public void evalBoard() {
        //    var board = BoardFactory.LoadBoardFromFen("r1b1n2r/1q1nNpbk/1p1p2p1/p2NpPPp/2P1P2P/3BB3/PP6/R2QK2R w - - 0 1");
        //    ParallelChess.AI.MinMaxAI.MinMax(board, 5);
        //}
    }
}
