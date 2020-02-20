using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;
using ParallelChess;

namespace ParallelChessPerformance {

    public struct IntWrapper {
        public int[] i;
        public int getField(int offset) {
            return i[offset];
        }
        public int setField(int offset, int value) {
            return i[offset] = value;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public int mul(int position, int mul) {
            return getField(Math.Abs(position * mul) % i.Length);
        }
        [MethodImpl(MethodImplOptions.NoInlining)]
        public int add(int position, int add) {
            return getField(Math.Abs(position + add) % i.Length);
        }
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void updater(int position, int val) {
            setField(position, Math.Abs(val));
        }

        public int GetMoves(int position) {
            int result = 0;
            position = getField(position % i.Length);

            result = mul(position, 3);
            result += add(position, 2);
            updater(position % i.Length, result);

            return result;
        }
    }

    [SimpleJob]
    public class StructMethodsVsStaticReferenceFunction {
        //public static int checksumModifier(IntWrapper intWrapper) {
        //    int sum = checksum();
        //    for (int j = 0; j < intWrapper.i.Length; j++) {
        //        sum *= intWrapper.getField(j);
        //    }
        //    return sum;
        //}

        public static void IncrementIntWrapperReference(ref IntWrapper intWrapper) {
            intWrapper.i[0]++;
        }
        public static int mul(IntWrapper intWrapper, int position, int mul) {
            return intWrapper.getField(Math.Abs(position * mul) % intWrapper.i.Length);
        }
        public static int add(IntWrapper intWrapper, int position, int add) {
            return intWrapper.getField(Math.Abs(position + add) % intWrapper.i.Length);
        }
        public static void updater(IntWrapper intWrapper, int position, int val) {
            intWrapper.setField(position, Math.Abs(val));    
        }

        //[MethodImpl(MethodImplOptions.Synchronized)]
        //[MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public static int GetMoves(IntWrapper intWrapper, int position) {
            int result = 0;
            position = intWrapper.getField(position % intWrapper.i.Length);

            result = mul(intWrapper, position, 3);
            result += add(intWrapper, position, 2);
            updater(intWrapper, position % intWrapper.i.Length, result);

            return result;
        }

        //[Benchmark]
        //public void IncrementReferenceFunction() {
        //    for (IntWrapper intWrapper = new IntWrapper() { i = 0 }
        //        ; intWrapper.i < 100000000; 
        //        IncrementIntWrapperReference(ref intWrapper)) {}
        //}

        //[Benchmark]
        //public void incrementMethod() {
        //    for (IntWrapper intWrapper = new IntWrapper() { i = 0 }
        //        ; intWrapper.i < 100000000
        //        ; intWrapper.Increment()) {
        //    }
        //}

        //[Benchmark]
        //public void justForLoop() {
        //    for (int i = 0; i < 100000000; i++) {
        //    }
        //}


        //[Benchmark]
        //public void IncrementReferenceFunction() {
        //    IntWrapper intWrapper = new IntWrapper() { i = new int[] { 0, 0, 0, 0, 0, 0 } };
        //    for (int j = 0; j < 1000000; j++) {
        //        complicated(intWrapper);
        //    }
        //}

        //[Benchmark]
        //public void incrementMethod() {
        //    IntWrapper intWrapper = new IntWrapper() { i = new int[] { 1, 2, 3, 4, 5, 6 } };
        //    for (int j = 0; j < 10000000; j++) {
        //        GetMoves(intWrapper, j);
        //        //intWrapper.GetMoves(j);
        //    }
        //}


        [Benchmark]
        public void areGettersInlined() {
            int total = 1;
            var board = BoardFactory.LoadBoardFromFen();
            for (int i = 0; i < 100000000; i++) {
                total += (int) board.GetPiece(i % 64);
                total += board.bytes[i % 64];
            }
        }
    }
}
