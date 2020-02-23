using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParallelChessPerformance {
    [SimpleJob]
    public class ForVsForeach {

        int[] workArray = new int[] {1,2,3,4,5,6,7,8,9,10,20};
        //List<int> workArray = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 20 };

        [Benchmark]
        public void forLoopReadOnce() {
            int total = 0;
            for (int i = 0; i < workArray.Length; i++) {
                total += workArray[i];
            }
        }

        [Benchmark]
        public void forEachReadOnce() {
            int total = 0;
            foreach (var work in workArray) {
                total += work;
            }
        }

        [Benchmark]
        public void forLoopReadMultiple() {
            int total = 0;
            for (int i = 0; i < workArray.Length; i++) {
                total += workArray[i];
                total += workArray[i];
                total += workArray[i];
                total += workArray[i];
                total += workArray[i];
            }
        }
        [Benchmark]
        public void forLoopReadMultipleLocalCache() {
            int total = 0;
            for (int i = 0; i < workArray.Length; i++) {
                int work = workArray[i];
                total += work;
            }
        }

        [Benchmark]
        public void forEachReadMultiple() {
            int total = 0;
            foreach (var work in workArray) {
                total += work;
                total += work;
                total += work;
                total += work;
                total += work;
            }
        }

    }
}
