using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using ParallelChess;
namespace ParallelChessTests {
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
    }
}
