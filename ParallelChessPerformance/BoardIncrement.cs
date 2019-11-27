using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParallelChessPerformance {
    [SimpleJob]
    public class BoardIncrement {
        byte[] boardFlat = new byte[64];

        byte[,] boardMultidimensional = new byte[8, 8];

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

        [Benchmark]
        public void incrementEntireBoardFlat() {
            for (var i = 0; i < 100000; i++) {
                for (int x = 0; x < 8; x++) {
                    for (int y = 0; y < 8; y++) {
                        boardFlat[y * 8 + x]++;
                    }
                }
            }
        }

        [Benchmark]
        public void incrementEntireBoardMultidimensional() {
            for (var i = 0; i < 100000; i++) {
                for (int x = 0; x < 8; x++) {
                    for (int y = 0; y < 8; y++) {
                        boardMultidimensional[x, y]++;
                    }
                }
            }
        }

        [Benchmark]
        public void incrementEntireBoardJaggedArray() {
            for (var i = 0; i < 100000; i++) {
                for (int x = 0; x < 8; x++) {
                    for (int y = 0; y < 8; y++) {
                        boardJagged[x][y]++;
                    }
                }
            }
        }



    }
}
