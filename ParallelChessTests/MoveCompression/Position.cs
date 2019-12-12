using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using ParallelChess;

namespace ParallelChessTests.MoveCompression {
    class Position {
        [Test]
        public void CompactPositionD5() {
            //BoardState board = Chess.LoadBoardFromFen();
            var position = BoardStateOffset.D5;

            var compacted = MoveHelper.compactPosition(position);

            var uncompacted = MoveHelper.unCompactPosition(compacted);

            Assert.AreEqual(position, uncompacted);
        }

        [Test]
        public void CompactPositionA1() {
            //BoardState board = Chess.LoadBoardFromFen();
            var position = BoardStateOffset.A1;

            var compacted = MoveHelper.compactPosition(position);

            var uncompacted = MoveHelper.unCompactPosition(compacted);

            Assert.AreEqual(position, uncompacted);
        }

        [Test]
        public void CompactPositionH8() {
            //BoardState board = Chess.LoadBoardFromFen();
            var position = BoardStateOffset.H8;

            var compacted = MoveHelper.compactPosition(position);

            var uncompacted = MoveHelper.unCompactPosition(compacted);

            Assert.AreEqual(position, uncompacted);
        }
    }
}
