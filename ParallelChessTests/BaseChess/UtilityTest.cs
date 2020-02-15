using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using ParallelChess;

namespace ParallelChessTests.BaseChess {
    class UtilityTest {

        [Test]
        public void ReadablePositionTestA1() {
            Assert.AreEqual("a1", BoardPositionHelpers.ReadablePosition(BoardStateOffset.A1));
        }

        [Test]
        public void ReadablePositionTestA8() {
            Assert.AreEqual("a8", BoardPositionHelpers.ReadablePosition(BoardStateOffset.A8));
        }

        [Test]
        public void ReadablePositionTestH8() {
            Assert.AreEqual("h8", BoardPositionHelpers.ReadablePosition(BoardStateOffset.H8));
        }

        [Test]
        public void ReadablePositionTestC4() {
            Assert.AreEqual("c4", BoardPositionHelpers.ReadablePosition(BoardStateOffset.C4));
        }

        [Test]
        public void positionToColumn() {
            Assert.AreEqual(BoardPositionHelpers.A_COLUMN, BoardPositionHelpers.PositionColumn(BoardStateOffset.A1));
            Assert.AreEqual(BoardPositionHelpers.A_COLUMN, BoardPositionHelpers.PositionColumn(BoardStateOffset.A7));
            Assert.AreEqual(BoardPositionHelpers.A_COLUMN, BoardPositionHelpers.PositionColumn(BoardStateOffset.A8));
            Assert.AreEqual(BoardPositionHelpers.E_COLUMN, BoardPositionHelpers.PositionColumn(BoardStateOffset.E5));
            Assert.AreEqual(BoardPositionHelpers.H_COLUMN, BoardPositionHelpers.PositionColumn(BoardStateOffset.H1));
            Assert.AreEqual(BoardPositionHelpers.H_COLUMN, BoardPositionHelpers.PositionColumn(BoardStateOffset.H8));
        }

        [Test]
        public void positionToRow() {
            Assert.AreEqual(0, BoardPositionHelpers.PositionRow(BoardStateOffset.A1));
            Assert.AreEqual(6, BoardPositionHelpers.PositionRow(BoardStateOffset.A7));
            Assert.AreEqual(7, BoardPositionHelpers.PositionRow(BoardStateOffset.A8));
            Assert.AreEqual(4, BoardPositionHelpers.PositionRow(BoardStateOffset.E5));
            Assert.AreEqual(0, BoardPositionHelpers.PositionRow(BoardStateOffset.H1));
            Assert.AreEqual(7, BoardPositionHelpers.PositionRow(BoardStateOffset.H8));
        }
    }
}
