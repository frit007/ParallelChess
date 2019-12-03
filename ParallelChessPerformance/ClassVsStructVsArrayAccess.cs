using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace ParallelChessPerformance {
    struct BoardStruct {
        public byte[] board;
        public byte whiteKingPosition;
        public byte blackQueenPosition;
        public byte Castling;
        public byte halfMoveCount;
        public short moveCount;
    }

    
    class BoardClass {
        public byte[] board;
        public byte whiteKingPosition;
        public byte blackQueenPosition;
        public byte Castling;
        public byte halfMoveCount;
        public short moveCount;
    }

    public static class ByteOffset {
        public static byte moveCount_1 = 0;
        public static int moveCount_2 = 1;
        public static byte whiteKingPosition = 2;
        public static byte blackKingPosition = 3;
        public static byte castlingPosition = 4;
        public static byte halfmoveCount = 5;
    }

    [StructLayout(LayoutKind.Explicit)]
    struct UnionBoard {
        [FieldOffset(0)]
        public byte[] byteBoard;

        [FieldOffset(0)]
        public short[] shortBoard;
    }

    [SimpleJob]
    class ClassVsStructVsArrayAccess {
        BoardStruct boardStruct;
        BoardClass boardClass;
        byte[] boardArray;

        //[Params(64, 128)]
        static public int arraySize = 64;

        [GlobalSetup]
        public void Setup() {
            boardStruct = new BoardStruct();
            boardStruct.board = new byte[arraySize];
            boardStruct.Castling = 2;
            boardStruct.whiteKingPosition = 8;
            boardStruct.blackQueenPosition = 9;
            boardStruct.moveCount = 200;

            boardClass = new BoardClass();
            boardStruct.board = new byte[arraySize];
            boardStruct.Castling = 2;
            boardStruct.whiteKingPosition = 8;
            boardStruct.blackQueenPosition = 9;
            boardStruct.moveCount = 200;

            boardArray = new byte[arraySize + 20];
            boardArray[ByteOffset.castlingPosition] = 2;
            boardArray[ByteOffset.whiteKingPosition] = 8;
            boardArray[ByteOffset.blackKingPosition] = 9;
            boardArray[ByteOffset.moveCount_1] = 200;
        }

        //[Benchmark]
        //public void incrementHalfMoveCount_class() {
        //    for (int i = 0; i < 10000000; i++) {
        //        boardClass.halfMoveCount++;
        //    }
        //}

        //[Benchmark]
        //public void incrementHalfMoveCount_struct() {
        //    for (int i = 0; i < 10000000; i++) {
        //        boardStruct.halfMoveCount++;
        //    }
        //}

        //[Benchmark]
        //public void incrementHalfMoveCount_array() {
        //    for (int i = 0; i < 10000000; i++) {
        //        boardArray[ByteOffset.halfmoveCount]++;
        //    }
        //}

        [Benchmark]
        public void IncrementFullMoveCount_class() {
            for (int i = 0; i < 10000000; i++) {
                boardClass.moveCount++;
            }
        }

        [Benchmark]
        public void IncrementFullMoveCount_struct() {
            for (int i = 0; i < 10000000; i++) {
                boardStruct.moveCount++;
            }
        }

        [Benchmark]
        public void IncrementFullMoveCount_array_union() {
            for (int i = 0; i < 10000000; i++) {
                UnionBoard unionBoard = new UnionBoard() { byteBoard = boardArray };
                unionBoard.shortBoard[ByteOffset.moveCount_1]++;
            }
        }

        [Benchmark]
        public void IncrementFullMoveCount_array_unsafe() {

            unsafe {
                for (int i = 0; i < 10000000; i++) {
                    fixed (byte* pByte = boardArray) {
                        short* pshort = (short*)pByte;
                        pshort[ByteOffset.moveCount_1]++;
                    }
                }
            }
        }

        [Benchmark]
        public void IncrementFullMoveCount_array_bitmanipulation() {
            for (int i = 0; i < 10000000; i++) {
                short moveCount = (short)(boardArray[ByteOffset.moveCount_1] | (boardArray[ByteOffset.moveCount_2] << 8));
                moveCount++;
                boardArray[ByteOffset.moveCount_1] = (byte)(moveCount & 0xFF);
                boardArray[ByteOffset.moveCount_2] = (byte)((moveCount & 0xFF00) >> 8);
            }
        }

        //[Benchmark]
        //public void applyCastling_class() {
        //    for (int i = 0; i < 10000000; i++) {
        //        boardClass.Castling &= 6;
        //    }
        //}

        //[Benchmark]
        //public void applyCastling_struct() {
        //    for (int i = 0; i < 10000000; i++) {
        //        boardStruct.Castling &= 6;
        //    }
        //}

        //[Benchmark]
        //public void applyCastling_array() {
        //    for (int i = 0; i < 10000000; i++) {
        //        boardArray[ByteOffset.castlingPosition] &= 6;
        //    }
        //}
    }
}
