using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using ParallelChess;

namespace ParallelChessTests.BaseChess {
    class SanTest {

        [Test]
        public void sanPawn() {
            /*
             * Starting position (White to play)
            +---------------+
            |_ _ _ k _ _ _ _| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ P _ _ _| 2
            |_ _ _ K _ _ _ _| 1
            +---------------+
             A B C D E F G H
            E2 -> E4
            +---------------+
            |_ _ _ k _ _ _ _| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ P _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |_ _ _ K _ _ _ _| 1
            +---------------+
             A B C D E F G H
             */
            var board = Chess.LoadBoardFromFen("3k4/8/8/8/8/8/4P3/3K4 w - - 0 1");

            var moves = board.GetMoves();
            var move = moves.FindTargetPosition(BoardStateOffset.E4);

            Assert.AreEqual("e4", board.StandardAlgebraicNotation(move));
        }

        [Test]
        public void sanBishopCheckKing() {
            /*
             * Starting position (White to play)
            +---------------+
            |_ _ _ k _ _ _ _| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ B _ P| 2
            |_ _ _ K _ _ _ _| 1
            +---------------+
             A B C D E F G H
            F2 -> B6
            +---------------+
            |_ _ _ k _ _ _ _| 8
            |_ _ _ _ _ _ _ _| 7
            |_ B _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ P| 2
            |_ _ _ K _ _ _ _| 1
            +---------------+
             A B C D E F G H
             */
            var board = Chess.LoadBoardFromFen("3k4/8/8/8/8/8/5B1P/3K4 w - - 0 1");

            var moves = board.GetMoves();
            //Chess.MakeMove(board, BoardStateOffset.F2, BoardStateOffset.B6, Piece.EMPTY);
            var move = moves.FindTargetPosition(BoardStateOffset.B6);

            Assert.AreEqual("Bb6+", board.StandardAlgebraicNotation(move));
        }

        [Test]
        public void sanAmbigousBishopMove() {
            /*
             * Starting position (White to play)
            +---------------+
            |_ _ _ k _ _ _ _| 8
            |B _ _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ B _ P| 2
            |_ _ _ K _ _ _ _| 1
            +---------------+
             A B C D E F G H
            F2 -> B6
            +---------------+
            |_ _ _ k _ _ _ _| 8
            |B _ _ _ _ _ _ _| 7
            |_ B _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ P| 2
            |_ _ _ K _ _ _ _| 1
            +---------------+
             A B C D E F G H
             */
            var board = Chess.LoadBoardFromFen("3k4/B7/8/8/8/8/5B1P/3K4 w - - 0 1");

            var moves = board.GetMoves();
            var move = moves.FindTargetPosition(BoardStateOffset.F2,BoardStateOffset.B6);
            // we need to specify that the move started from the F file
            Assert.AreEqual("Bfb6+", board.StandardAlgebraicNotation(move));
        }

        [Test]
        public void sanPromoteToKnight() {
            /*
             * Starting position (White to play)
            +---------------+
            |_ _ _ r k _ _ _| 8
            |_ P _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |_ _ _ _ K _ _ _| 1
            +---------------+
             A B C D E F G H
            B7 -> B8
            +---------------+
            |_ N _ r k _ _ _| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |_ _ _ _ K _ _ _| 1
            +---------------+
             A B C D E F G H
             */
            var board = Chess.LoadBoardFromFen("3rk3/1P6/8/8/8/8/8/4K3 w - - 0 1");

            var moves = board.GetMoves();

            var move = moves.FindTargetPosition(BoardStateOffset.B8, Piece.KNIGHT);

            Assert.AreEqual("b8=N", board.StandardAlgebraicNotation(move));
        }

        [Test]
        public void sanPromoteToQueen() {
            /*
             * Starting position (White to play)
            +---------------+
            |_ _ _ r k _ _ _| 8
            |_ P _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |_ _ _ _ K _ _ _| 1
            +---------------+
             A B C D E F G H
            B7 -> B8
            +---------------+
            |_ Q _ r k _ _ _| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |_ _ _ _ K _ _ _| 1
            +---------------+
             A B C D E F G H
             */
            var board = Chess.LoadBoardFromFen("3rk3/1P6/8/8/8/8/8/4K3 w - - 0 1");

            var moves = board.GetMoves();

            var move = moves.FindTargetPosition(BoardStateOffset.B8, Piece.QUEEN);

            Assert.AreEqual("b8=Q", board.StandardAlgebraicNotation(move));
        }

        [Test]
        public void san8QueenCanMoveToSameSquare() {
            /*
             * Starting position (White to play)
            +---------------+
            |_ _ _ _ k _ _ _| 8
            |p p p p p p p p| 7
            |_ _ _ _ _ _ _ _| 6
            |Q Q Q _ _ _ _ _| 5
            |Q _ Q _ _ _ _ _| 4
            |Q Q Q _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |_ _ _ _ K _ _ _| 1
            +---------------+
             A B C D E F G H
            B5 -> B4
            +---------------+
            |_ _ _ _ k _ _ _| 8
            |p p p p p p p p| 7
            |_ _ _ _ _ _ _ _| 6
            |Q _ Q _ _ _ _ _| 5
            |Q Q Q _ _ _ _ _| 4
            |Q Q Q _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |_ _ _ _ K _ _ _| 1
            +---------------+
             A B C D E F G H
             */
            var board = Chess.LoadBoardFromFen("4k3/pppppppp/8/QQQ5/Q1Q5/QQQ5/8/4K3 w - - 0 1");

            var moves = board.GetMoves();

            var move = moves.FindTargetPosition(BoardStateOffset.B5, BoardStateOffset.B4);

            Assert.AreEqual("Qb5b4", board.StandardAlgebraicNotation(move));
        }

        [Test]
        public void CastlingWhiteQueenSide() {
            /*
             * Starting position (White to play)
            +---------------+
            |_ _ _ _ k _ _ _| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |R _ _ _ K _ _ _| 1
            +---------------+
             A B C D E F G H
            E1 -> C1
            +---------------+
            |_ _ _ _ k _ _ _| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |_ _ K R _ _ _ _| 1
            +---------------+
             A B C D E F G H
             */
            var board = Chess.LoadBoardFromFen("4k3/8/8/8/8/8/8/R3K3 w Q - 0 1");

            var moves = board.GetMoves();

            var move = moves.FindTargetPosition(BoardStateOffset.E1, BoardStateOffset.C1);

            Assert.AreEqual("O-O-O", board.StandardAlgebraicNotation(move));
        }

        [Test]
        public void CastlingWhiteKingSide() {
            /*
             * Starting position (White to play)
            +---------------+
            |_ _ _ _ k _ _ _| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |_ _ _ _ K _ _ R| 1
            +---------------+
             A B C D E F G H
            E1 -> G1
            +---------------+
            |_ _ _ _ k _ _ _| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |_ _ _ _ _ R K _| 1
            +---------------+
             A B C D E F G H
             */
            var board = Chess.LoadBoardFromFen("4k3/8/8/8/8/8/8/4K2R w K - 0 1");

            var moves = board.GetMoves();

            var move = moves.FindTargetPosition(BoardStateOffset.E1, BoardStateOffset.G1);

            Assert.AreEqual("O-O", board.StandardAlgebraicNotation(move));
        }

        [Test]
        public void CastlingBlackQueenSide() {
            /*
             * Starting position (Black to play)
            +---------------+
            |r _ _ _ k _ _ r| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |_ _ _ _ K _ _ _| 1
            +---------------+
             A B C D E F G H
            E8 -> C8
            +---------------+
            |_ _ k r _ _ _ r| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |_ _ _ _ K _ _ _| 1
            +---------------+
             A B C D E F G H
             */
            var board = Chess.LoadBoardFromFen("r3k2r/8/8/8/8/8/8/4K3 b kq - 0 1");

            var moves = board.GetMoves();

            var move = moves.FindTargetPosition(BoardStateOffset.E8, BoardStateOffset.C8);

            Assert.AreEqual("O-O-O", board.StandardAlgebraicNotation(move));
        }

        [Test]
        public void CastlingBlackKingSide() {
            /*
             * Starting position (Black to play)
            +---------------+
            |r _ _ _ k _ _ r| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |_ _ _ _ K _ _ _| 1
            +---------------+
             A B C D E F G H
            E8 -> G8
            +---------------+
            |r _ _ _ _ r k _| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |_ _ _ _ K _ _ _| 1
            +---------------+
             A B C D E F G H
             */
            var board = Chess.LoadBoardFromFen("r3k2r/8/8/8/8/8/8/4K3 b kq - 0 1");

            var moves = board.GetMoves();

            var move = moves.FindTargetPosition(BoardStateOffset.E8, BoardStateOffset.G8);

            Assert.AreEqual("O-O", board.StandardAlgebraicNotation(move));
        }

        [Test]
        public void CastlingWhiteQueenSideCheckmate() {
            /*
             * Starting position (White to play)
            +---------------+
            |_ _ _ k _ _ _ _| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ R _ R _ _ _| 2
            |R _ _ _ K _ _ _| 1
            +---------------+
             A B C D E F G H
            E1 -> C1
            +---------------+
            |_ _ _ k _ _ _ _| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ R _ R _ _ _| 2
            |_ _ K R _ _ _ _| 1
            +---------------+
             A B C D E F G H
             */
            var board = Chess.LoadBoardFromFen("3k4/8/8/8/8/8/2R1R3/R3K3 w Q - 0 1");

            var moves = board.GetMoves();

            var move = moves.FindTargetPosition(BoardStateOffset.E1, BoardStateOffset.C1);
            
            Assert.AreEqual("O-O-O#", board.StandardAlgebraicNotation(move));
        }

        [Test]
        public void SanPlayE4() {
            /*
             * Starting position (White to play)
            +---------------+
            |r n b q k b n r| 8
            |p p p p p p p p| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |P P P P P P P P| 2
            |R N B Q K B N R| 1
            +---------------+
             A B C D E F G H
            E2 -> E4
            +---------------+
            |r n b q k b n r| 8
            |p p p p p p p p| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ P _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |P P P P _ P P P| 2
            |R N B Q K B N R| 1
            +---------------+
             A B C D E F G H
             */
            var board = Chess.LoadBoardFromFen("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");

            Chess.MakeMove(board, "e4");

            Assert.AreEqual(Piece.PAWN|Piece.IS_WHITE,board.E4);
        }

        [Test]
        public void blackPlaysD5() {
            /*
             * Starting position (Black to play)
            +---------------+
            |r n b q k b n r| 8
            |p p p p p p p p| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ P _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |P P P P P _ P P| 2
            |R N B Q K B N R| 1
            +---------------+
             A B C D E F G H
            D7 -> D5
            +---------------+
            |r n b q k b n r| 8
            |p p p _ p p p p| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ p _ _ _ _| 5
            |_ _ _ _ _ P _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |P P P P P _ P P| 2
            |R N B Q K B N R| 1
            +---------------+
             A B C D E F G H
             */
            var board = Chess.LoadBoardFromFen("rnbqkbnr/pppppppp/8/8/5P2/8/PPPPP1PP/RNBQKBNR b KQkq - 0 1");

            var moves = board.GetMoves();
            var move = moves.FindTargetPosition(BoardStateOffset.D7, BoardStateOffset.D5);

            Assert.AreEqual("d5", board.StandardAlgebraicNotation(move));
        }

        [Test]
        public void differentiateBetweenRookMoves() {
            /*
             * Starting position (White to play)
            +---------------+
            |_ _ _ k _ _ _ _| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |R _ _ _ _ _ _ R| 2
            |_ _ _ K _ _ _ _| 1
            +---------------+
             A B C D E F G H
            A2 -> D2
            +---------------+
            |_ _ _ k _ _ _ _| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ R _ _ _ R| 2
            |_ _ _ K _ _ _ _| 1
            +---------------+
             A B C D E F G H
             */
            var board = Chess.LoadBoardFromFen("3k4/8/8/8/8/8/R6R/3K4 w - - 0 1");

            var moves = board.GetMoves();
            var move = moves.FindTargetPosition(BoardStateOffset.A2, BoardStateOffset.D2);

            Assert.AreEqual("Rad2+", board.StandardAlgebraicNotation(move));
        }

    }
}
