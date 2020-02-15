using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using ParallelChess;

namespace ParallelChessTests.BaseChess {
    class UtilityTest {

        [Test]
        public void ReadablePositionTestA1() {
            Assert.AreEqual("a1", BoardPosition.ReadablePosition(BoardStateOffset.A1));
        }

        [Test]
        public void ReadablePositionTestA8() {
            Assert.AreEqual("a8", BoardPosition.ReadablePosition(BoardStateOffset.A8));
        }

        [Test]
        public void ReadablePositionTestH8() {
            Assert.AreEqual("h8", BoardPosition.ReadablePosition(BoardStateOffset.H8));
        }

        [Test]
        public void ReadablePositionTestC4() {
            Assert.AreEqual("c4", BoardPosition.ReadablePosition(BoardStateOffset.C4));
        }

        [Test]
        public void positionToColumn() {
            Assert.AreEqual(BoardPosition.A_COLUMN, BoardPosition.PositionColumn(BoardStateOffset.A1));
            Assert.AreEqual(BoardPosition.A_COLUMN, BoardPosition.PositionColumn(BoardStateOffset.A7));
            Assert.AreEqual(BoardPosition.A_COLUMN, BoardPosition.PositionColumn(BoardStateOffset.A8));
            Assert.AreEqual(BoardPosition.E_COLUMN, BoardPosition.PositionColumn(BoardStateOffset.E5));
            Assert.AreEqual(BoardPosition.H_COLUMN, BoardPosition.PositionColumn(BoardStateOffset.H1));
            Assert.AreEqual(BoardPosition.H_COLUMN, BoardPosition.PositionColumn(BoardStateOffset.H8));
        }

        [Test]
        public void positionToRow() {
            Assert.AreEqual(0, BoardPosition.PositionRow(BoardStateOffset.A1));
            Assert.AreEqual(6, BoardPosition.PositionRow(BoardStateOffset.A7));
            Assert.AreEqual(7, BoardPosition.PositionRow(BoardStateOffset.A8));
            Assert.AreEqual(4, BoardPosition.PositionRow(BoardStateOffset.E5));
            Assert.AreEqual(0, BoardPosition.PositionRow(BoardStateOffset.H1));
            Assert.AreEqual(7, BoardPosition.PositionRow(BoardStateOffset.H8));
        }
    }
}
