using BenchmarkDotNet.Running;
using System;

namespace ParallelChessPerformance {
    class Program {
        static void Main(string[] args) {
            //Console.WriteLine("Hello World!");
            var summary = BenchmarkRunner.Run<MinMaxPerformance>();

            //UsingBoardStateStruct usingBoardStateStruct = new UsingBoardStateStruct();
            //usingBoardStateStruct.makeAMove();
        }
    }
}
