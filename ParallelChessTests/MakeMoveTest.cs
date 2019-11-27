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


        #region enpassant
        public byte[] CreateEnpassant() {
            /* Starting position
            +---------------+
            |r n b q k b n r| 8
            |p p _ p p p p p| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ p _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |P P P P P P P P| 2
            |R N B Q K B N R| 1
            +---------------+
             A B C D E F G H
             White creates opening for enPassant
            +---------------+
            |r n b q k b n r| 8
            |p p _ p p p p p| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ p P _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |P P P P _ P P P| 2
            |R N B Q K B N R| 1
            +---------------+
             A B C D E F G H
             Black enpassant
            +---------------+
            |r n b q k b n r| 8
            |p p _ p p p p p| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ p _ _ _| 3
            |P P P P _ P P P| 2
            |R N B Q K B N R| 1
            +---------------+
             A B C D E F G H
            */
            byte[] board = Chess.LoadBoardFromFen("rnbqkbnr/pp1ppppp/8/8/3p4/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
            
            List<Move> list = Board.GetMovesForPosition(board, BoardOffset.E2);

            Move move = list.FindTargetPosition(BoardOffset.E4);

            Board.MakeMove(board, move);

            list = Board.GetMovesForPosition(board, BoardOffset.D4);

            move = list.FindTargetPosition(BoardOffset.E3);

            Board.MakeMove(board, move);

            return board;
        }

        [Test]
        public void checkEnpassantSuccededToMove() {
            byte[] board = CreateEnpassant();
            Assert.AreEqual(Piece.PAWN, Board.GetPiece(board, BoardOffset.E3) & Piece.PIECE_MASK);
        }

        [Test]
        public void checkEnpassantIsNoLongerPossible() {
            byte[] board = CreateEnpassant();
            Assert.AreEqual(EnPasasnt.NO_ENPASSANT, Board.GetEnPassantAttackedSquare(board));
        }

        [Test]
        public void checkEnpassantMovedFromPosition() {
            byte[] board = CreateEnpassant();
            Assert.AreEqual(Piece.EMPTY, Board.GetPiece(board,BoardOffset.D4)&Piece.PIECE_MASK);
        }

        [Test]
        public void enPassantKilledEnemyPawn() {
            byte[] board = CreateEnpassant();
            Assert.AreEqual(Piece.EMPTY, Board.GetPiece(board, BoardOffset.E4) & Piece.PIECE_MASK);
        }
        #endregion
    }
}
