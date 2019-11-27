using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParallelChessPerformance {
    [SimpleJob]
    public class AddVsIf {
        int[] CastlingOptions = new int[] {
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, // Filler for more fair comparison
            1, // BLACK value
            10 // WHITE value
        };
        int BLACKS_POSITION = 10;
        int WHITE_POSITION = 11;

        [Benchmark]
        public void addAccess() {
            int total = 0;
            for (int i = 0; i < 1000000000; i++) {
                // skift mellem 0 og 1 
                int isWhite = i % 2;
                total += CastlingOptions[isWhite + BLACKS_POSITION];
            }
        }

        [Benchmark]
        public void ifAccess() {
            int total = 0;
            for (int i = 0; i < 1000000000; i++) {
                // skift mellem 0 og 1 
                int isWhite = i % 2;
                if (isWhite == 1) {
                    total += CastlingOptions[WHITE_POSITION];
                } else {
                    total += CastlingOptions[BLACKS_POSITION];
                }
            }
        }

        [Benchmark]
        public void ternaryAccess() {
            int total = 0;
            for (int i = 0; i < 1000000000; i++) {
                // skift mellem 0 og 1 
                int isWhite = i % 2;
                total += CastlingOptions[isWhite == 1 ? WHITE_POSITION : BLACKS_POSITION];
            }
        }
    }
}
