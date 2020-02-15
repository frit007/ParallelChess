using NUnit.Framework;
using ParallelChess;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParallelChessTests.BaseChess {
    class MakeMoveTest {
        [Test]
        public void MovePawn() {
            Board board = Board.LoadBoardFromFen();

            List<Move> list = board.GetMovesForPosition(BoardStateOffset.E2);
            
            Move move = list.FindTargetPosition(BoardStateOffset.E4);

            board.MakeMove(move);

            Piece piece = board.E4;

            Assert.AreEqual(Piece.PAWN, board.E4 & Piece.PIECE_MASK);
            Assert.AreEqual(BoardStateOffset.E3, board.EnPassantTarget);
        }

        [Test]
        public void PawnsCanNotJumpOverPiece() {
            /*
             * Starting position (White to play)
            +---------------+
            |_ _ _ _ _ _ k _| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ r _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ p _ p _| 3
            |_ _ _ _ P _ P _| 2
            |_ _ N _ _ _ K _| 1
            +---------------+
             A B C D E F G H
             */
            var board = Board.LoadBoardFromFen("6k1/8/8/3r4/8/4p1p1/4P1P1/2N3K1 w - - 0 1");

            var moves = board.GetMoves();

            var move = moves.FindTargetPosition(BoardStateOffset.E4);
            Assert.IsFalse(MoveHelper.isValidMove(move));
        }

        [Test]
        public void WhitePawnAttackFromH2() {
            /*
             * Start position (white to play)
            +---------------+
            |_ _ _ k _ _ _ _| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ p _| 3
            |_ _ _ _ _ _ P P| 2
            |_ _ _ K _ _ _ _| 1
            +---------------+
             A B C D E F G H
            */
            var board = Board.LoadBoardFromFen("3k4/8/8/8/8/6p1/6PP/3K4 w - - 0 1");
            board.MakeMove(BoardStateOffset.H2, BoardStateOffset.G3);
            Assert.AreEqual(Piece.EMPTY, board.H2);

            Assert.AreEqual(Piece.PAWN | Piece.IS_WHITE, board.G3);
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

            var board = Board.LoadBoardFromFen("r1b5/pkpP4/Npr5/8/8/8/8/R2K4 w - - 0 1");

            board.MakeMove(BoardStateOffset.D7, BoardStateOffset.D8, Piece.KNIGHT);

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

            var board = Board.LoadBoardFromFen("r1b5/pkpP4/Npr5/8/8/8/8/R2K4 w - - 0 1");

            board.MakeMove(BoardStateOffset.D7, BoardStateOffset.C8, Piece.QUEEN);

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

            var board = Board.LoadBoardFromFen("r1b5/pkpP4/Npr5/8/8/8/8/R2K4 w - - 0 1");

            board.MakeMove(BoardStateOffset.D7, BoardStateOffset.C8, Piece.ROOK);

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

            var board = Board.LoadBoardFromFen("r1b5/pkpP4/Npr5/8/8/8/8/R2K4 w - - 0 1");

            board.MakeMove(BoardStateOffset.D7, BoardStateOffset.C8, Piece.BISHOP);

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

            var board = Board.LoadBoardFromFen("3k4/8/8/8/8/8/6p1/3K4 b - - 0 1");

            board.MakeMove(BoardStateOffset.G2, BoardStateOffset.G1, Piece.BISHOP);

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

            var board = Board.LoadBoardFromFen("3k4/8/8/8/8/8/6p1/3K4 b - - 0 1");

            board.MakeMove(BoardStateOffset.G2, BoardStateOffset.G1, Piece.QUEEN);

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

            var board = Board.LoadBoardFromFen("3k4/8/8/8/8/8/6p1/3K4 b - - 0 1");

            board.MakeMove(BoardStateOffset.G2, BoardStateOffset.G1, Piece.ROOK);

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

            var board = Board.LoadBoardFromFen("3k4/8/8/8/8/8/6p1/3K4 b - - 0 1");

            board.MakeMove(BoardStateOffset.G2, BoardStateOffset.G1, Piece.KNIGHT);

            Assert.AreEqual(Piece.KNIGHT, board.GetPiece(BoardStateOffset.G1));
            Assert.AreEqual(Piece.EMPTY, board.GetPiece(BoardStateOffset.G2));
        }


        #region enpassant
        public Board TakeEnpassant() {
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
            Board board = Board.LoadBoardFromFen("rnbqkbnr/pp1ppppp/8/8/3p4/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
            
            List<Move> list = board.GetMovesForPosition(BoardStateOffset.E2);

            Move move = list.FindTargetPosition(BoardStateOffset.E4);

            board.MakeMove(move);

            list = board.GetMovesForPosition(BoardStateOffset.D4);

            move = list.FindTargetPosition(BoardStateOffset.E3);

            board.MakeMove(move);

            return board;
        }

        [Test]
        public void CheckEnpassantSuccededToMove() {
            Board board = TakeEnpassant();
            Assert.AreEqual(Piece.PAWN, board.E3 & Piece.PIECE_MASK);
        }

        [Test]
        public void CheckEnpassantIsNoLongerPossible() {
            Board board = TakeEnpassant();
            Assert.AreEqual(EnPassant.NO_ENPASSANT, board.EnPassantTarget);
        }

        [Test]
        public void CheckEnpassantMovedFromPosition() {
            Board board = TakeEnpassant();
            Assert.AreEqual(Piece.EMPTY, board.D4&Piece.PIECE_MASK);
        }

        [Test]
        public void EnPassantKilledEnemyPawn() {
            Board board = TakeEnpassant();
            Assert.AreEqual(Piece.EMPTY, board.E4 & Piece.PIECE_MASK);
        }

        [Test]
        public void EnPassantIsClearedAfterEnemyMove() {
            /*
             * Start position (White to play)
            +---------------+
            |_ _ _ _ _ _ _ _| 8
            |_ _ _ q k _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ P _ _ _ _| 2
            |_ _ _ _ K _ _ _| 1
            +---------------+
             A B C D E F G H
             D2->D4
            +---------------+
            |_ _ _ _ _ _ _ _| 8
            |_ _ _ q k _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ P _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |_ _ _ _ K _ _ _| 1
            +---------------+
             A B C D E F G H
            D7->D4 (move a piece that is not a pawn)
            +---------------+
            |_ _ _ _ _ _ _ _| 8
            |_ _ _ q k _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ P _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |_ _ _ _ K _ _ _| 1
            +---------------+
             A B C D E F G H
             */
            Board board = Board.LoadBoardFromFen("8/3qk3/8/8/8/8/3P4/4K3 w - - 0 1");
            board.MakeMove(BoardStateOffset.D2, BoardStateOffset.D4);
            board.MakeMove(BoardStateOffset.D7, BoardStateOffset.D4);
            Assert.AreEqual(EnPassant.NO_ENPASSANT, board.EnPassantTarget);
        }
        #endregion


        // this position is tested because it threw an outside the bounds error at one point
        [Test]
        public void WeirdPosition() {
            /*
             * Start position (Black to play)
            +---------------+
            |r n b q k b n r| 8
            |_ p p p p p p p| 7
            |_ _ _ _ _ _ _ _| 6
            |p _ _ _ _ _ _ _| 5
            |P _ _ _ _ _ _ _| 4
            |_ _ P _ _ _ _ _| 3
            |p P _ P P P P P| 2
            |_ N B Q K B N R| 1
            +---------------+
             A B C D E F G H
             */
            var board = Board.LoadBoardFromFen("rnbqkbnr/1ppppppp/8/p7/P7/2P5/pP1PPPPP/1NBQKBNR b Kkq - 0 4");

            board.GetMovesForPosition(BoardStateOffset.A2);
        }

        [Test]
        public void BlackPromotionStressUndoTest() {
            var board = Board.LoadBoardFromFen("1nbqkbnr/rppppppp/p7/8/1P6/P1P5/1p1PPPPP/R1BQKBNR b KQk - 0 3");
            var original = board.CreateCopyBoard();
            var moves = board.GetMoves();
            foreach (var move in moves) {
                board.MakeMove(move);
                board.UndoMove(move);
            }
        }

        // this test was added because there was a scenario where a king was counted as a knight, 
        // which meant black king defended the knight on f6
        [Test]
        public void KingTakesKnight() {
            /*
             * Starting position (White to play)
            +---------------+
            |_ _ _ _ _ _ k _| 8
            |p p _ _ _ _ _ _| 7
            |_ _ _ _ K n p _| 6
            |_ _ _ _ _ _ p p| 5
            |_ P _ _ _ _ _ _| 4
            |_ _ _ _ r _ _ _| 3
            |_ _ _ _ _ _ P P| 2
            |R _ _ _ _ _ _ _| 1
            +---------------+
             A B C D E F G H
             E6 -> F6
            +---------------+
            |_ _ _ _ _ _ k _| 8
            |p p _ _ _ _ _ _| 7
            |_ _ _ _ _ K p _| 6
            |_ _ _ _ _ _ p p| 5
            |_ P _ _ _ _ _ _| 4
            |_ _ _ _ r _ _ _| 3
            |_ _ _ _ _ _ P P| 2
            |R _ _ _ _ _ _ _| 1
            +---------------+
             A B C D E F G H
             */
            var board = Board.LoadBoardFromFen("6k1/pp6/4Knp1/6pp/1P6/4r3/6PP/R7 w - - 2 43");

            var moves = board.GetMoves();

            var move = moves.FindTargetPosition(BoardStateOffset.F6);

            Assert.True(MoveHelper.isValidMove(move));

            Assert.IsTrue(board.IsLegalMove(move));
        }
    }
}
