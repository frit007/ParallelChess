using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using ParallelChess;

namespace ParallelChessTests {
    class MoveTest {
        private int fromPosition = BoardStateOffset.E2;
        private int toPosition = BoardStateOffset.E4;
        Move doubleMove() => MoveHelper.CreateMove(toPosition, fromPosition, Piece.EMPTY, Piece.EMPTY, Move.ENPASSANT, (byte)CastlingBits.CAN_ALL);

        [Test]
        public void checkFromPosition() {
            Move move = doubleMove();
            int position = MoveHelper.MoveFromPos(move);
            Assert.AreEqual(fromPosition, position);
        }

        [Test]
        public void checkToPosition() {
            Move move = doubleMove();
            int position = MoveHelper.MoveTargetPos(move);
            Assert.AreEqual(toPosition, position);
        }
    }
}
