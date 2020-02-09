using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using ParallelChess;

namespace ParallelChessTests.BaseChess {
    class UtilityTest {

        [Test]
        public void ReadablePositionTestA1() {
            Assert.AreEqual("a1", BoardHelper.ReadablePosition(BoardStateOffset.A1));
        }

        [Test]
        public void ReadablePositionTestA8() {
            Assert.AreEqual("a8", BoardHelper.ReadablePosition(BoardStateOffset.A8));
        }

        [Test]
        public void ReadablePositionTestH8() {
            Assert.AreEqual("h8", BoardHelper.ReadablePosition(BoardStateOffset.H8));
        }

        [Test]
        public void ReadablePositionTestC4() {
            Assert.AreEqual("c4", BoardHelper.ReadablePosition(BoardStateOffset.C4));
        }

        [Test]
        public void positionToColumn() {
            Assert.AreEqual(BoardHelper.A_COLUMN, BoardHelper.PositionColumn(BoardStateOffset.A1));
            Assert.AreEqual(BoardHelper.A_COLUMN, BoardHelper.PositionColumn(BoardStateOffset.A7));
            Assert.AreEqual(BoardHelper.A_COLUMN, BoardHelper.PositionColumn(BoardStateOffset.A8));
            Assert.AreEqual(BoardHelper.E_COLUMN, BoardHelper.PositionColumn(BoardStateOffset.E5));
            Assert.AreEqual(BoardHelper.H_COLUMN, BoardHelper.PositionColumn(BoardStateOffset.H1));
            Assert.AreEqual(BoardHelper.H_COLUMN, BoardHelper.PositionColumn(BoardStateOffset.H8));
        }

        [Test]
        public void positionToRow() {
            Assert.AreEqual(0, BoardHelper.PositionRow(BoardStateOffset.A1));
            Assert.AreEqual(6, BoardHelper.PositionRow(BoardStateOffset.A7));
            Assert.AreEqual(7, BoardHelper.PositionRow(BoardStateOffset.A8));
            Assert.AreEqual(4, BoardHelper.PositionRow(BoardStateOffset.E5));
            Assert.AreEqual(0, BoardHelper.PositionRow(BoardStateOffset.H1));
            Assert.AreEqual(7, BoardHelper.PositionRow(BoardStateOffset.H8));
        }
    }
}
