using System;
using System.Collections.Generic;
using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using ParallelChess;

namespace ParallelChessPerformance {
    [SimpleJob]
    class UsingBoardStateStruct {
        [Benchmark]
        public void makeAMove() {
            BoardState board = Chess.LoadBoardFromFen();
            BoardState boardState = new BoardState();
            unsafe {

                for (int i = 0; i < BoardStateOffset.BOARD_STATE_SIZE; i++) {
                    boardState.bytes[i] = board.bytes[i];
                }

                //boardState.everything[0] = 1;


                //byte* bytes = boardState.everything;



                //Console.WriteLine(boardState.A1);
                Piece piece = (Piece)boardState.A1;

                Console.WriteLine(piece);
            }
        }
    }
}
