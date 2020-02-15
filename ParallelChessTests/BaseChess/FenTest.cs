using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using ParallelChess;

namespace ParallelChessTests.BaseChess {
    class FenTest {
        [Test]
        public void SimpleFenTest() {
            Board board = BoardFactory.LoadBoardFromFen();

            //Piece piece = Board.GetPiece(board, BoardOffset.A1);
            //Piece piece = board.A1;

            Assert.AreEqual(board.A1, Piece.IS_WHITE | Piece.ROOK);
            Assert.AreEqual(board.E1, Piece.IS_WHITE | Piece.KING);
            Assert.AreEqual(board.E8, Piece.KING);
        }

        [Test]
        public void loadEnpassantSquare() {
            Board board = BoardFactory.LoadBoardFromFen("4k3/8/8/PpP5/8/8/8/4K3 b - b6 0 1");

            Assert.AreEqual(BoardStateOffset.B6, board.EnPassantTarget);
        }

        [Test]
        public void toFenEnpassant() {
            var originalFEN = "4k3/8/8/PpP5/8/8/8/4K3 b - b6 0 1";
            
            var board = BoardFactory.LoadBoardFromFen(originalFEN);

            Assert.AreEqual(originalFEN, board.FEN);
        }

        [Test]
        public void toFenWithCastling() {
            var originalFEN = "r3k2r/pppppppp/8/8/8/8/PPPPPPPP/R3K2R w KQkq - 0 1";

            var board = BoardFactory.LoadBoardFromFen(originalFEN);

            Assert.AreEqual(originalFEN, board.FEN);
        }
    }
}
