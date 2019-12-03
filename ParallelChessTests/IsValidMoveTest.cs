using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using ParallelChess;
namespace ParallelChessTests {
    class IsValidMoveTest {
        //[Test]
        //public void RunOutOfBoardLeft() {
        //    Assert.IsFalse(Board.IsValidPosition(BoardStateOffset.A1, -1, 0));
        //}

        //[Test]
        //public void RunOutOfBoardRight() {
        //    Assert.IsFalse(Board.IsValidPosition(BoardStateOffset.H8, 1, 0));
        //}

        //[Test]
        //public void RunOutOfBoardTop() {
        //    Assert.IsFalse(Board.IsValidPosition(BoardStateOffset.A8, 0, 1));
        //}

        //[Test]
        //public void RunOutOfBoardBottom() {
        //    Assert.IsFalse(Board.IsValidPosition(BoardStateOffset.A1, 0, -1));
        //}

        //[Test]
        //public void ValidateHorseMoveFromA2() {
        //    Assert.IsTrue(Board.IsValidPosition(BoardStateOffset.B1, 1, 2));
        //    Assert.False(Board.IsValidPosition(BoardStateOffset.B1, 1, -2));


        //    Assert.True(Board.IsValidPosition(BoardStateOffset.B1, -1, 2));
        //    Assert.IsFalse(Board.IsValidPosition(BoardStateOffset.B1, -1, -2));


        //    Assert.IsFalse(Board.IsValidPosition(BoardStateOffset.B1, -2, 1));
        //    Assert.IsFalse(Board.IsValidPosition(BoardStateOffset.B1, -2, -1));

        //    Assert.IsTrue(Board.IsValidPosition(BoardStateOffset.B1, 2, 1));
        //    Assert.IsFalse(Board.IsValidPosition(BoardStateOffset.B1, 2, -1));
        //}
        [Test]
        public void ValidateHorseProtectKing() {
            /*
             * Start position (White to play)
                +---------------+
                |_ _ _ _ k _ _ _| 8
                |_ _ _ _ q _ _ _| 7
                |_ _ _ _ _ _ _ _| 6
                |_ _ _ _ _ _ _ _| 5
                |_ _ _ _ _ _ _ _| 4
                |_ _ _ _ _ _ _ _| 3
                |_ _ N _ _ _ _ _| 2
                |_ _ _ Q K _ _ _| 1
                +---------------+
                 A B C D E F G H
                The knight should have have 4 invalid move options(marked X) and one valid (marked Y)
                +---------------+
                |_ _ _ _ k _ _ _| 8
                |_ _ _ _ q _ _ _| 7
                |_ _ _ _ _ _ _ _| 6
                |_ _ _ _ _ _ _ _| 5
                |_ X _ X _ _ _ _| 4
                |X _ _ _ Y _ _ _| 3
                |_ _ N _ _ _ _ _| 2
                |X _ _ Q K _ _ _| 1
                +---------------+
                 A B C D E F G H
             */
            BoardState board = Chess.LoadBoardFromFen("4k3/4q3/8/8/8/8/2N5/3QK3 w - - 0 1");

            List<Move> moves = Board.GetMovesForPosition(board, BoardStateOffset.B2);

            //Move move = moves.FindTargetPosition(BoardStateOffset.E3);

            Assert.IsTrue(Board.IsValidMove(board, moves.FindTargetPosition(BoardStateOffset.E3)), "Knight can move to E3 to protect the king");

            Assert.IsFalse(Board.IsValidMove(board, moves.FindTargetPosition(BoardStateOffset.D4)), "Knight cannot move here because the king is under attack");
            Assert.IsFalse(Board.IsValidMove(board, moves.FindTargetPosition(BoardStateOffset.B4)), "Knight cannot move here because the king is under attack");
            Assert.IsFalse(Board.IsValidMove(board, moves.FindTargetPosition(BoardStateOffset.A3)), "Knight cannot move here because the king is under attack");
            Assert.IsFalse(Board.IsValidMove(board, moves.FindTargetPosition(BoardStateOffset.A1)), "Knight cannot move here because the king is under attack");
        }
    }
}
