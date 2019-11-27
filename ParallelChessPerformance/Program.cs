using BenchmarkDotNet.Running;
using System;

namespace ParallelChessPerformance {
    class Program {
        static void Main(string[] args) {
            //Console.WriteLine("Hello World!");
            var summary = BenchmarkRunner.Run<AddVsIf>();
        }
    }
}
