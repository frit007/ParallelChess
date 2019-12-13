﻿using NUnit.Framework;
using ParallelChess;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParallelChessTests.BaseChess {
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

        [Test]
        public void whitePromoteToKnight() {
            /* Starting position (White to play)
            +---------------+
            |r _ b _ _ _ _ _| 8
            |p k p P _ _ _ _| 7
            |N p r _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |R _ _ K _ _ _ _| 1
            +---------------+
             A B C D E F G H

            D7->D8 promote to knight
            +---------------+
            |r _ b N _ _ _ _| 8
            |p k p _ _ _ _ _| 7
            |N p r _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |R _ _ K _ _ _ _| 1
            +---------------+
             A B C D E F G H
             */

            var board = Chess.LoadBoardFromFen("r1b5/pkpP4/Npr5/8/8/8/8/R2K4 w - - 0 1");

            Chess.MakeMove(board, BoardStateOffset.D7, BoardStateOffset.D8, Piece.KNIGHT);

            Assert.AreEqual(Piece.KNIGHT | Piece.IS_WHITE, board.GetPiece(BoardStateOffset.D8));
            Assert.AreEqual(Piece.EMPTY, board.GetPiece(BoardStateOffset.D7));
        }

        [Test]
        public void whitePromoteToQueen() {
            /* Starting position (White to play)
            +---------------+
            |r _ b _ _ _ _ _| 8
            |p k p P _ _ _ _| 7
            |N p r _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |R _ _ K _ _ _ _| 1
            +---------------+
             A B C D E F G H

            D7->C8 promote to Queen
            +---------------+
            |r _ Q _ _ _ _ _| 8
            |p k p _ _ _ _ _| 7
            |N p r _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |R _ _ K _ _ _ _| 1
            +---------------+
             A B C D E F G H
             */

            var board = Chess.LoadBoardFromFen("r1b5/pkpP4/Npr5/8/8/8/8/R2K4 w - - 0 1");

            Chess.MakeMove(board, BoardStateOffset.D7, BoardStateOffset.C8, Piece.QUEEN);

            Assert.AreEqual(Piece.QUEEN | Piece.IS_WHITE, board.GetPiece(BoardStateOffset.C8));
            Assert.AreEqual(Piece.EMPTY, board.GetPiece(BoardStateOffset.D7));
        }

        [Test]
        public void whitePromoteToRook() {
            /* Starting position (White to play)
            +---------------+
            |r _ b _ _ _ _ _| 8
            |p k p P _ _ _ _| 7
            |N p r _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |R _ _ K _ _ _ _| 1
            +---------------+
             A B C D E F G H

            D7->C8 promote to Rook
            +---------------+
            |r _ R _ _ _ _ _| 8
            |p k p _ _ _ _ _| 7
            |N p r _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |R _ _ K _ _ _ _| 1
            +---------------+
             A B C D E F G H
             */

            var board = Chess.LoadBoardFromFen("r1b5/pkpP4/Npr5/8/8/8/8/R2K4 w - - 0 1");

            Chess.MakeMove(board, BoardStateOffset.D7, BoardStateOffset.C8, Piece.ROOK);

            Assert.AreEqual(Piece.ROOK | Piece.IS_WHITE, board.GetPiece(BoardStateOffset.C8));
            Assert.AreEqual(Piece.EMPTY, board.GetPiece(BoardStateOffset.D7));
        }

        [Test]
        public void whitePromoteToBishop() {
            /* Starting position (White to play)
            +---------------+
            |r _ b _ _ _ _ _| 8
            |p k p P _ _ _ _| 7
            |N p r _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |R _ _ K _ _ _ _| 1
            +---------------+
             A B C D E F G H

            D7->C8 promote to Bishop
            +---------------+
            |r _ B _ _ _ _ _| 8
            |p k p _ _ _ _ _| 7
            |N p r _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |R _ _ K _ _ _ _| 1
            +---------------+
             A B C D E F G H
             */

            var board = Chess.LoadBoardFromFen("r1b5/pkpP4/Npr5/8/8/8/8/R2K4 w - - 0 1");

            Chess.MakeMove(board, BoardStateOffset.D7, BoardStateOffset.C8, Piece.BISHOP);

            Assert.AreEqual(Piece.BISHOP | Piece.IS_WHITE, board.GetPiece(BoardStateOffset.C8));
            Assert.AreEqual(Piece.EMPTY, board.GetPiece(BoardStateOffset.D7));
        }

        [Test]
        public void blackPromoteToBishop() {
            /* Starting position (Black to play)
            +---------------+
            |_ _ _ k _ _ _ _| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ p _| 2
            |_ _ _ K _ _ _ _| 1
            +---------------+
             A B C D E F G H

            G2->G1 promote to Bishop
            +---------------+
            |_ _ _ k _ _ _ _| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |_ _ _ K _ _ b _| 1
            +---------------+
             A B C D E F G H
             */

            var board = Chess.LoadBoardFromFen("3k4/8/8/8/8/8/6p1/3K4 b - - 0 1");

            Chess.MakeMove(board, BoardStateOffset.G2, BoardStateOffset.G1, Piece.BISHOP);

            Assert.AreEqual(Piece.BISHOP, board.GetPiece(BoardStateOffset.G1));
            Assert.AreEqual(Piece.EMPTY, board.GetPiece(BoardStateOffset.G2));
        }

        [Test]
        public void blackPromoteToQueen() {
            /* Starting position (Black to play)
            +---------------+
            |_ _ _ k _ _ _ _| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ p _| 2
            |_ _ _ K _ _ _ _| 1
            +---------------+
             A B C D E F G H

            G2->G1 promote to Queen
            +---------------+
            |_ _ _ k _ _ _ _| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |_ _ _ K _ _ q _| 1
            +---------------+
             A B C D E F G H
             */

            var board = Chess.LoadBoardFromFen("3k4/8/8/8/8/8/6p1/3K4 b - - 0 1");

            Chess.MakeMove(board, BoardStateOffset.G2, BoardStateOffset.G1, Piece.QUEEN);

            Assert.AreEqual(Piece.QUEEN, board.GetPiece(BoardStateOffset.G1));
            Assert.AreEqual(Piece.EMPTY, board.GetPiece(BoardStateOffset.G2));
        }

        [Test]
        public void blackPromoteToRook() {
            /* Starting position (Black to play)
            +---------------+
            |_ _ _ k _ _ _ _| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ p _| 2
            |_ _ _ K _ _ _ _| 1
            +---------------+
             A B C D E F G H

            G2->G1 promote to Rook
            +---------------+
            |_ _ _ k _ _ _ _| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |_ _ _ K _ _ b _| 1
            +---------------+
             A B C D E F G H
             */

            var board = Chess.LoadBoardFromFen("3k4/8/8/8/8/8/6p1/3K4 b - - 0 1");

            Chess.MakeMove(board, BoardStateOffset.G2, BoardStateOffset.G1, Piece.ROOK);

            Assert.AreEqual(Piece.ROOK, board.GetPiece(BoardStateOffset.G1));
            Assert.AreEqual(Piece.EMPTY, board.GetPiece(BoardStateOffset.G2));
        }

        [Test]
        public void blackPromoteToKnight() {
            /* Starting position (Black to play)
            +---------------+
            |_ _ _ k _ _ _ _| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ p _| 2
            |_ _ _ K _ _ _ _| 1
            +---------------+
             A B C D E F G H

            G2->G1 promote to Knight
            +---------------+
            |_ _ _ k _ _ _ _| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |_ _ _ K _ _ b _| 1
            +---------------+
             A B C D E F G H
             */

            var board = Chess.LoadBoardFromFen("3k4/8/8/8/8/8/6p1/3K4 b - - 0 1");

            Chess.MakeMove(board, BoardStateOffset.G2, BoardStateOffset.G1, Piece.KNIGHT);

            Assert.AreEqual(Piece.KNIGHT, board.GetPiece(BoardStateOffset.G1));
            Assert.AreEqual(Piece.EMPTY, board.GetPiece(BoardStateOffset.G2));
        }


        #region enpassant
        public BoardState TakeEnpassant() {
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
        public void CheckEnpassantSuccededToMove() {
            BoardState board = TakeEnpassant();
            Assert.AreEqual(Piece.PAWN, board.E3 & Piece.PIECE_MASK);
        }

        [Test]
        public void CheckEnpassantIsNoLongerPossible() {
            BoardState board = TakeEnpassant();
            Assert.AreEqual(EnPassant.NO_ENPASSANT, board.EnPassantTarget);
        }

        [Test]
        public void CheckEnpassantMovedFromPosition() {
            BoardState board = TakeEnpassant();
            Assert.AreEqual(Piece.EMPTY, board.D4&Piece.PIECE_MASK);
        }

        [Test]
        public void EnPassantKilledEnemyPawn() {
            BoardState board = TakeEnpassant();
            Assert.AreEqual(Piece.EMPTY, board.E4 & Piece.PIECE_MASK);
        }
        #endregion
    }
}
