using NUnit.Framework;
using ParallelChess;
using ParallelChess.AI;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParallelChessTests.AI.Easy {
    class EvalBoardTest {
        [Test]
        public void blackQueenIsWinning() {
            /*
             * Starting position (White to play)
            +---------------+
            |_ _ _ _ k _ _ _| 8
            |_ _ q _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |_ _ _ _ K _ _ _| 1
            +---------------+
             A B C D E F G H
             */
            var board = Chess.LoadBoardFromFen("4k3/2q5/8/8/8/8/8/4K3 w - - 0 1");

            var moves = Board.GetMoves(board);

            var score = EvalBoard.evalBoard(board, moves);

            Assert.Less(score, 0); // black is winning
        }

        [Test]
        public void whiteQueenIsWinning() {
            /*
             * Starting position (White to play)
            +---------------+
            |_ _ _ _ k _ _ _| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ Q _ _ _ _ _| 2
            |_ _ _ _ K _ _ _| 1
            +---------------+
             A B C D E F G H
             */
            var board = Chess.LoadBoardFromFen("4k3/8/8/8/8/8/2Q5/4K3 w - - 0 1");

            var moves = Board.GetMoves(board);

            var score = EvalBoard.evalBoard(board, moves);

            Assert.Greater(score, 0); // white is winning
        }

        [Test]
        public void whiteQueenThreatendIsWorseThanNoThreat() {
            /*
             * Starting position (White to play)
            +---------------+
            |_ _ _ _ k _ _ _| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ p _ _ _| 4
            |_ _ _ p _ _ _ _| 3
            |_ _ Q _ _ _ _ _| 2
            |_ _ _ _ K _ _ _| 1
            +---------------+
             A B C D E F G H
             */
            var threatendBoard = Chess.LoadBoardFromFen("4k3/8/8/8/4p3/3p4/2Q5/4K3 w - - 0 1");

            var threatendMoves = Board.GetMoves(threatendBoard);
            var threatendScore = EvalBoard.evalBoard(threatendBoard, threatendMoves);

            /*
             * Starting position (White to play)
            +---------------+
            |_ _ _ _ k _ _ _| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ Q _ _ _ _ _| 2
            |_ _ _ _ K _ _ _| 1
            +---------------+
             A B C D E F G H
             */
            var board = Chess.LoadBoardFromFen("4k3/8/8/8/8/8/2Q5/4K3 w - - 0 1");

            var moves = Board.GetMoves(board);

            var score = EvalBoard.evalBoard(board, moves);

            Assert.Less(threatendScore, score);
        }

        [Test]
        public void blackQueenThreatendIsWorseThanNoThreat() {
            /*
             * Starting position (White to play)
            +---------------+
            |_ _ _ _ k _ _ _| 8
            |_ _ q _ _ _ _ _| 7
            |_ _ _ P _ _ _ _| 6
            |_ _ _ _ P _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |_ _ _ _ K _ _ _| 1
            +---------------+
             A B C D E F G H
             */
            var threatendBoard = Chess.LoadBoardFromFen("4k3/2q5/3P4/4P3/8/8/8/4K3 w - - 0 1");

            var threatendMoves = Board.GetMoves(threatendBoard);

            var threatendScore = EvalBoard.evalBoard(threatendBoard, threatendMoves);

            /*
             * Starting position (White to play)
            +---------------+
            |_ _ _ _ k _ _ _| 8
            |_ _ q _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |_ _ _ _ K _ _ _| 1
            +---------------+
             A B C D E F G H
             */
            var board = Chess.LoadBoardFromFen("4k3/2q5/8/8/8/8/8/4K3 w - - 0 1");

            var moves = Board.GetMoves(board);

            var score = EvalBoard.evalBoard(board, moves);

            Assert.Greater(threatendScore, score);
        }

        [Test]
        public void pawnsShouldBeEqual() {
            /*
             * Starting position (White to play)
            +---------------+
            |_ _ _ _ k _ _ _| 8
            |_ _ p _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ P _ _ _ _ _| 2
            |_ _ _ _ K _ _ _| 1
            +---------------+
             A B C D E F G H
             */
            var board = Chess.LoadBoardFromFen("4k3/2p5/8/8/8/8/2P5/4K3 w - - 0 1");

            var moves = Board.GetMoves(board);

            var score = EvalBoard.evalBoard(board, moves);

            Assert.Greater(score, -0.01);
            Assert.Less(score, 0.01);
        }
    }
}
