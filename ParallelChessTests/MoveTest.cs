using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using ParallelChess;

namespace ParallelChessTests {
    class MoveTest {
        private int fromPosition = BoardStateOffset.E2;
        private int toPosition = BoardStateOffset.E4;
        Move DoubleMove() => MoveHelper.CreateMove(toPosition, fromPosition, Piece.EMPTY, Piece.EMPTY, Move.ENPASSANT, new BoardState() { bytes = new byte[BoardStateOffset.BOARD_STATE_SIZE]});
        
        [Test]
        public void CheckFromPosition() {
            Move move = DoubleMove();
            int position = MoveHelper.MoveTargetPos(move);
            Assert.AreEqual(fromPosition, position);
        }

        [Test]
        public void CheckToPosition() {
            Move move = DoubleMove();
            int position = MoveHelper.MoveTargetPos(move);
            Assert.AreEqual(toPosition, position);
        }
    }
}
