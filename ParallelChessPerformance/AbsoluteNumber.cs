using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParallelChessPerformance {
    [SimpleJob]
    public class AbsoluteNumber {
        public static int x;

        [Benchmark]
        public void MathAbs() {
            for (int i = -100000000; i < 100000000; i++) {
                int dist = i ;
                x = Math.Abs(dist);
                
            }
        }

        [Benchmark]
        public void and() {
            for (int i = -100000000; i < 100000000; i++) {
                int dist = i - 3;
                x = (dist + (dist >> 31)) ^ (dist >> 31);
                
            }
        }

        [Benchmark]
        public void ifTest() {
            for (int i = -100000000; i < 100000000; i++) {
                int dist = i - 3;
                x = dist < 0 ? dist * -1 : dist;
                
            }
        }
    }
}
