using NUnit.Framework;
using ParallelChess;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParallelChessTests {
    class MakeMoveTest {
        [Test]
        public void MovePeasant() {
            byte[] board = Chess.LoadBoardFromFen();

            List<Move> list = Board.GetMovesForPosition(board, BoardOffset.E2);
            
            Move move = list.FindTargetPosition(BoardOffset.E4);

            Board.MakeMove(board, move);

            Piece piece = Board.GetPiece(board, BoardOffset.E4);

            Assert.AreEqual(Piece.PAWN, Board.GetPiece(board, BoardOffset.E4) & Piece.PIECE_MASK);
            Assert.AreEqual(BoardOffset.E3, Board.GetEnPassantAttackedSquare(board));
        }

    }
}
