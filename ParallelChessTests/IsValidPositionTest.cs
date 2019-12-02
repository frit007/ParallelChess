using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using ParallelChess;
namespace ParallelChessTests {
    class IsValidPositionTest {
        [Test]
        public void RunOutOfBoardLeft() {
            Assert.IsFalse(Board.IsValidPosition(BoardStateOffset.A1, -1, 0));
        }

        [Test]
        public void RunOutOfBoardRight() {
            Assert.IsFalse(Board.IsValidPosition(BoardStateOffset.H8, 1, 0));
        }

        [Test]
        public void RunOutOfBoardTop() {
            Assert.IsFalse(Board.IsValidPosition(BoardStateOffset.A8, 0, 1));
        }

        [Test]
        public void RunOutOfBoardBottom() {
            Assert.IsFalse(Board.IsValidPosition(BoardStateOffset.A1, 0, -1));
        }

        [Test]
        public void ValidateHorseMoveFromA2() {
            Assert.IsTrue(Board.IsValidPosition(BoardStateOffset.B1, 1, 2));
            Assert.False(Board.IsValidPosition(BoardStateOffset.B1, 1, -2));


            Assert.True(Board.IsValidPosition(BoardStateOffset.B1, -1, 2));
            Assert.IsFalse(Board.IsValidPosition(BoardStateOffset.B1, -1, -2));


            Assert.IsFalse(Board.IsValidPosition(BoardStateOffset.B1, -2, 1));
            Assert.IsFalse(Board.IsValidPosition(BoardStateOffset.B1, -2, -1));

            Assert.IsTrue(Board.IsValidPosition(BoardStateOffset.B1, 2, 1));
            Assert.IsFalse(Board.IsValidPosition(BoardStateOffset.B1, 2, -1));
        }
    }
}
