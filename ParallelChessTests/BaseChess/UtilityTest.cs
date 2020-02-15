using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using ParallelChess;

namespace ParallelChessTests.BaseChess {
    class UtilityTest {

        [Test]
        public void ReadablePositionTestA1() {
            Assert.AreEqual("a1", Board.ReadablePosition(BoardStateOffset.A1));
        }

        [Test]
        public void ReadablePositionTestA8() {
            Assert.AreEqual("a8", Board.ReadablePosition(BoardStateOffset.A8));
        }

        [Test]
        public void ReadablePositionTestH8() {
            Assert.AreEqual("h8", Board.ReadablePosition(BoardStateOffset.H8));
        }

        [Test]
        public void ReadablePositionTestC4() {
            Assert.AreEqual("c4", Board.ReadablePosition(BoardStateOffset.C4));
        }

        [Test]
        public void positionToColumn() {
            Assert.AreEqual(Board.A_COLUMN, Board.PositionColumn(BoardStateOffset.A1));
            Assert.AreEqual(Board.A_COLUMN, Board.PositionColumn(BoardStateOffset.A7));
            Assert.AreEqual(Board.A_COLUMN, Board.PositionColumn(BoardStateOffset.A8));
            Assert.AreEqual(Board.E_COLUMN, Board.PositionColumn(BoardStateOffset.E5));
            Assert.AreEqual(Board.H_COLUMN, Board.PositionColumn(BoardStateOffset.H1));
            Assert.AreEqual(Board.H_COLUMN, Board.PositionColumn(BoardStateOffset.H8));
        }

        [Test]
        public void positionToRow() {
            Assert.AreEqual(0, Board.PositionRow(BoardStateOffset.A1));
            Assert.AreEqual(6, Board.PositionRow(BoardStateOffset.A7));
            Assert.AreEqual(7, Board.PositionRow(BoardStateOffset.A8));
            Assert.AreEqual(4, Board.PositionRow(BoardStateOffset.E5));
            Assert.AreEqual(0, Board.PositionRow(BoardStateOffset.H1));
            Assert.AreEqual(7, Board.PositionRow(BoardStateOffset.H8));
        }
    }
}
