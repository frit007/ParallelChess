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

            Assert.AreEqual(board.GetPiece(BoardStateOffset.A1), Piece.IS_WHITE | Piece.ROOK);
            Assert.AreEqual(board.GetPiece(BoardStateOffset.E1), Piece.IS_WHITE | Piece.KING);
            Assert.AreEqual(board.GetPiece(BoardStateOffset.E8), Piece.KING);
        }

        [Test]
        public void loadEnpassantSquare() {
            Board board = BoardFactory.LoadBoardFromFen("4k3/8/8/PpP5/8/8/8/4K3 b - b6 0 1");

            Assert.AreEqual(BoardStateOffset.B6, board.EnPassantTarget);
        }

        [Test]
        public void toFenEnpassant() {
            var originalFEN = "4k3/8/8/PpP5/8/8/8/4K3 b - b6 0 1";

            var game = Chess.ContinueFromFEN(originalFEN);

            Assert.AreEqual(originalFEN, game.FEN);
        }

        [Test]
        public void toFenWithCastling() {
            var originalFEN = "r3k2r/pppppppp/8/8/8/8/PPPPPPPP/R3K2R w KQkq - 0 1";

            var game = Chess.ContinueFromFEN(originalFEN);

            Assert.AreEqual(originalFEN, game.FEN);
        }

        [Test]
        public void SanitizeFenCastling() {
            // this fen is technically illegal because you cannot castle while the king isn't on the correct position
            // if it is not sanitized it will cause the program to crash
            var poisionFEN = "3r3k/6pp/3Q4/q7/8/4P2P/6P1/5RK1 b Qq - 0 1";

            var board = BoardFactory.LoadBoardFromFen(poisionFEN);

            Assert.AreEqual(CastlingBits.EMPTY, board.CastlingBits);
        }

        [Test]
        public void preserveTurnCounter() {
            var originalFen = "rnbqkb1r/ppp2ppp/4pn2/3p4/3PP3/2N5/PPP2PPP/R1BQKBNR w KQkq - 3 4";

            var game = Chess.ContinueFromFEN(originalFen);

            Assert.AreEqual(originalFen, game.FEN);
        }
    }
}
