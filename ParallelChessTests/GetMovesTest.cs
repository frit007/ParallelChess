using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using ParallelChess;

namespace ParallelChessTests {
    class GetMovesTest {
        [Test]
        public void PawnMoveFromStart() {
            byte[] board = Chess.LoadBoardFromFen();

            var moves = Board.GetMovesForPosition(board, BoardOffset.C2);

            Console.WriteLine(moves[0]);
        }

        [Test]
        public void PawnPerformanceTest() {
            byte[] board = Chess.LoadBoardFromFen();

            for (int i = 0; i < 1000000; i++) {
                Board.GetMovesForPosition(board, BoardOffset.E2);
            }
        }

        [Test]
        public void KnightPerformanceTest() {
            byte[] board = Chess.LoadBoardFromFen();

            for (int i = 0; i < 1000000; i++) {
                Board.GetMovesForPosition(board, BoardOffset.B1);
            }
        }

        [Test]
        public void KingPerformanceTest() {
            byte[] board = Chess.LoadBoardFromFen();

            for (int i = 0; i < 1000000; i++) {
                Board.GetMovesForPosition(board, BoardOffset.E1);
            }
        }

        [Test]
        public void QueenPerformanceTest() {
            byte[] board = Chess.LoadBoardFromFen();

            for (int i = 0; i < 1000000; i++) {
                Board.GetMovesForPosition(board, BoardOffset.D1);
            }
        }

        [Test]
        public void QueenWorstCasePerformanceTest() {
            // Basically a board where the queen stands in the midle of the board which means she has to check the entire board.
            byte[] board = Chess.LoadBoardFromFen("r2qk2r/8/8/3Q4/8/8/8/R3K2R w KQkq - 0 1");

            for (int i = 0; i < 1000000; i++) {
                Board.GetMovesForPosition(board, BoardOffset.D5);
            }
        }
    }
}
