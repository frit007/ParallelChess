using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using ParallelChess;

namespace ParallelChessTests {
    class FenTest {
        [Test]
        public void SimpleFenTest() {
            BoardState board = Chess.LoadBoardFromFen();

            //Piece piece = Board.GetPiece(board, BoardOffset.A1);
            //Piece piece = board.A1;

            Assert.AreEqual(board.A1, Piece.IS_WHITE | Piece.ROOK);
            Assert.AreEqual(board.E1, Piece.IS_WHITE | Piece.KING);
            Assert.AreEqual(board.E8, Piece.IS_BLACK | Piece.KING);
        }
    }
}
