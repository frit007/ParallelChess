using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParallelChessPerformance {
    [SimpleJob]
    public class BoardCopy {

        [Params(10, 80, 120, 160)]
        static public int arraySize;

        int[] intArray;
        byte[] byteArray;

        [GlobalSetup]
        public void Setup() {
            intArray = new int[arraySize];
            byteArray = new byte[arraySize];

        }

        const int INT_SIZE = 4;
        [Benchmark]
        public void IntBlockCopy() {
            int[] copy = new int[arraySize];
            Buffer.BlockCopy(intArray, 0, copy, 0, arraySize * INT_SIZE);
        }

        [Benchmark]
        public void ByteBlockCopy() {
            byte[] copy = new byte[arraySize];
            Buffer.BlockCopy(byteArray, 0, copy, 0, arraySize);
        }
        [Benchmark]
        public void IntToArray() {
            intArray.ToArray();
        }

        [Benchmark]
        public void ByteToArray() {
            byteArray.ToArray();
        }

        [Benchmark]
        public void IntArrayCopy() {
            int[] copy = new int[arraySize];
            Array.Copy(intArray, copy, arraySize);
        }
        [Benchmark]
        public void ByteArrayCopy() {
            byte[] copy = new byte[arraySize];
            Array.Copy(byteArray, copy, arraySize);
        }

    }
}
