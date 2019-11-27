using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParallelChessPerformance {
    [SimpleJob]
    public class BoardInitialize {



        [Benchmark]
        public void initializeFlat() {
            for (var i = 0; i < 10000000; i++) {
                byte[] boardFlat = new byte[64];
            }
        }



        [Benchmark]
        public void initializeMultidimenional() {
            for (var i = 0; i < 10000000; i++) {
                byte[,] boardMultidimensional = new byte[8, 8];
            }
        }

        [Benchmark]
        public void initializeJaggedArray() {
            for (var i = 0; i < 10000000; i++) {
                byte[][] boardJagged = new byte[8][] {
                            new byte[8],
                            new byte[8],
                            new byte[8],
                            new byte[8],
                            new byte[8],
                            new byte[8],
                            new byte[8],
                            new byte[8],
                        };
            }
        }

    }
}
