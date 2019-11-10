using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using ParallelChess;

namespace ParallelChessTests {
    class FenTest {
        [Test]
        public void SimpleFenTest() {
            byte[] board = Chess.LoadBoardFromFen();

            Piece piece = Board.GetPiece(board, BoardOffset.A1);

            Assert.AreEqual(piece, Piece.IS_WHITE | Piece.ROOK);
            Assert.AreEqual(Board.GetPiece(board, BoardOffset.E1), Piece.IS_WHITE | Piece.KING);
            Assert.AreEqual(Board.GetPiece(board, BoardOffset.E8), Piece.IS_BLACK | Piece.KING);
        }
    }
}
