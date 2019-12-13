using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using ParallelChess;

namespace ParallelChessTests.BaseChess {
    class KingTrackingTest {
        [Test]
        public void LoadedFromFEN() {
            var board = Chess.LoadBoardFromFen();

            Assert.AreEqual(BoardStateOffset.E1, board.WhiteKingPosition);
            Assert.AreEqual(BoardStateOffset.E8, board.BlackKingPosition);
        }

        [Test]
        public void FollowBlackKingOnMove() {
            /* Starting position Blacks turn
            +---------------+
            |_ k _ _ _ _ _ _| 8
            |_ _ _ _ _ _ p p| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ P P| 2
            |_ K _ _ _ _ _ _| 1
            +---------------+
             A B C D E F G H
             will move to here 
            +---------------+
            |_ _ _ _ _ _ _ _| 8
            |_ k _ _ _ _ p p| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ P P| 2
            |_ K _ _ _ _ _ _| 1
            +---------------+
             A B C D E F G H
             The board should still know where the black king is
            */
            var board = Chess.LoadBoardFromFen("1k6/6pp/8/8/8/8/6PP/1K6 b - - 0 1");

            Chess.MakeMove(board, BoardStateOffset.B8, BoardStateOffset.B7);

            Assert.AreEqual(BoardStateOffset.B7, board.BlackKingPosition);
        }

        [Test]
        public void FollowWhiteKingOnMove() {
            /* Starting position Whites turn
            +---------------+
            |_ k _ _ _ _ _ _| 8
            |_ _ _ _ _ _ p p| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ P P| 2
            |_ K _ _ _ _ _ _| 1
            +---------------+
             A B C D E F G H
             will move to here
            +---------------+
            |_ k _ _ _ _ _ _| 8
            |_ _ _ _ _ _ p p| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ K _ _ _ _ P P| 2
            |_ _ _ _ _ _ _ _| 1
            +---------------+
             A B C D E F G H
             the board should still know where the white king is
            */

            var board = Chess.LoadBoardFromFen("1k6/6pp/8/8/8/8/6PP/1K6 w - - 0 1");

            Chess.MakeMove(board, BoardStateOffset.B1, BoardStateOffset.B2);

            Assert.AreEqual(BoardStateOffset.B2, board.WhiteKingPosition);
        }
    }
}
