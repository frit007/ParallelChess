using NUnit.Framework;
using ParallelChess;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParallelChessTests {
    class MakeMoveTest {
        [Test]
        public void MovePeasant() {
            BoardState board = Chess.LoadBoardFromFen();

            List<Move> list = Board.GetMovesForPosition(board, BoardStateOffset.E2);
            
            Move move = list.FindTargetPosition(BoardStateOffset.E4);

            Board.MakeMove(board, move);

            Piece piece = board.E4;

            Assert.AreEqual(Piece.PAWN, board.E4 & Piece.PIECE_MASK);
            Assert.AreEqual(BoardStateOffset.E3, board.EnPassantTarget);
        }


        #region enpassant
        public BoardState CreateEnpassant() {
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
            BoardState board = Chess.LoadBoardFromFen("rnbqkbnr/pp1ppppp/8/8/3p4/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
            
            List<Move> list = Board.GetMovesForPosition(board, BoardStateOffset.E2);

            Move move = list.FindTargetPosition(BoardStateOffset.E4);

            Board.MakeMove(board, move);

            list = Board.GetMovesForPosition(board, BoardStateOffset.D4);

            move = list.FindTargetPosition(BoardStateOffset.E3);

            Board.MakeMove(board, move);

            return board;
        }

        [Test]
        public void checkEnpassantSuccededToMove() {
            BoardState board = CreateEnpassant();
            Assert.AreEqual(Piece.PAWN, board.E3 & Piece.PIECE_MASK);
        }

        [Test]
        public void checkEnpassantIsNoLongerPossible() {
            BoardState board = CreateEnpassant();
            Assert.AreEqual(EnPassant.NO_ENPASSANT, board.EnPassantTarget);
        }

        [Test]
        public void checkEnpassantMovedFromPosition() {
            BoardState board = CreateEnpassant();
            Assert.AreEqual(Piece.EMPTY, board.D4&Piece.PIECE_MASK);
        }

        [Test]
        public void enPassantKilledEnemyPawn() {
            BoardState board = CreateEnpassant();
            Assert.AreEqual(Piece.EMPTY, board.E4 & Piece.PIECE_MASK);
        }
        #endregion
    }
}
