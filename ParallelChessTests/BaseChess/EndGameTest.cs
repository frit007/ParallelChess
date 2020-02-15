using NUnit.Framework;
using ParallelChess;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParallelChessTests.BaseChess {
    class EndGameTest {
        [Test]
        public void CheckMateWhite() {
            /*
             * Start position (Black to play)
            +---------------+
            |r n b q k b n r| 8
            |_ p p p p Q p _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ B P P _ _ _| 4
            |p _ _ _ _ _ _ p| 3
            |P P P _ _ P P P| 2
            |R N B _ K _ N R| 1
            +---------------+
             A B C D E F G H
             */
            var board = Board.LoadBoardFromFen("rnbqkbnr/1ppppQp1/8/8/2BPP3/p6p/PPP2PPP/RNB1K1NR b KQkq - 0 1");

            var moves = board.GetMoves();

            var winner = board.detectWinner(moves);

            Assert.AreEqual(Winner.WINNER_WHITE, winner);
        }

        [Test]
        public void QueenCheckMateWhite() {
            /*
             * Starting position (Black to play)
            +---------------+
            |_ k _ _ Q _ _ _| 8
            |p p p p _ p p p| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |_ _ K _ _ _ _ _| 1
            +---------------+
             A B C D E F G H
             */
            var board = Board.LoadBoardFromFen("1k2Q3/pppp1ppp/8/8/8/8/8/2K5 b - - 0 1");

            var moves = board.GetMoves();

            var winner = board.detectWinner(moves);

            Assert.AreEqual(Winner.WINNER_WHITE, winner);
        }

        [Test]
        public void CheckMateBlack() {
            /*
             * Starting position (White to play)
            +---------------+
            |_ _ _ k _ _ _ _| 8
            |_ _ p p p _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ P P P _ _ _| 2
            |_ _ _ K _ q _ _| 1
            +---------------+
             A B C D E F G H
             */
            var board = Board.LoadBoardFromFen("3k4/2ppp3/8/8/8/8/2PPP3/3K1q2 w - - 0 1");

            var moves = board.GetMoves();

            var winner = board.detectWinner(moves);

            Assert.AreEqual(Winner.WINNER_BLACK, winner);
        }

        [Test]
        public void Stalemate() {
            /*
             * Starting position (White to play)
            +---------------+
            |_ _ _ k _ _ _ _| 8
            |_ _ p p p _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ r _ r _ _ _| 3
            |_ _ _ _ _ r _ _| 2
            |_ _ _ K _ _ _ _| 1
            +---------------+
             A B C D E F G H
             */
            var board = Board.LoadBoardFromFen("3k4/2ppp3/8/8/8/2r1r3/5r2/3K4 w - - 0 1");

            var moves = board.GetMoves();

            var winner = board.detectWinner(moves);

            Assert.AreEqual(Winner.DRAW, winner);
        }

        [Test]
        public void InsufficientMaterial2Kings() {
            /*
             * Starting position (White to play)
            +---------------+
            |_ _ _ k _ _ _ _| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |_ _ _ K _ _ _ _| 1
            +---------------+
             A B C D E F G H
             */
            var board = Board.LoadBoardFromFen("3k4/8/8/8/8/8/8/3K4 w - - 0 1");

            var moves = board.GetMoves();

            var winner = board.detectWinner(moves);

            Assert.AreEqual(Winner.DRAW, winner);
        }

        [Test]
        public void InsufficientMaterial1Bishop() {
            /*
             * Starting position (White to play)
            +---------------+
            |_ _ _ k _ _ _ _| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ B _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |_ _ _ K _ _ _ _| 1
            +---------------+
             A B C D E F G H
             */
            var board = Board.LoadBoardFromFen("3k4/8/8/8/8/5B2/8/3K4 w - - 0 1");

            var moves = board.GetMoves();

            var winner = board.detectWinner(moves);

            Assert.AreEqual(Winner.DRAW, winner);
        }

        [Test]
        public void InsufficientMaterial1Knight() {
            /*
             * Starting position (White to play)
            +---------------+
            |_ _ _ k _ _ _ _| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ N _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |_ _ _ K _ _ _ _| 1
            +---------------+
             A B C D E F G H
             */
            var board = Board.LoadBoardFromFen("3k4/8/8/8/8/5N2/8/3K4 w - - 0 1");

            var moves = board.GetMoves();

            var winner = board.detectWinner(moves);

            Assert.AreEqual(Winner.DRAW, winner);
        }
        [Test]
        public void SufficientMaterial2Knights() {
            /*
             * Starting position (White to play)
            +---------------+
            |_ _ _ k _ _ _ _| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ N N _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |_ _ _ K _ _ _ _| 1
            +---------------+
             A B C D E F G H
             */
            var board = Board.LoadBoardFromFen("3k4/8/8/8/8/4NN2/8/3K4 w - - 0 1");

            var moves = board.GetMoves();


            var winner = board.detectWinner(moves);

            Assert.AreEqual(Winner.NONE, winner);
        }
        [Test]
        public void SufficientMaterial1KnightAnd1Bishop() {
            /*
             * Starting position (White to play)
            +---------------+
            |_ _ _ k _ _ _ _| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ N B _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |_ _ _ K _ _ _ _| 1
            +---------------+
             A B C D E F G H
             */
            var board = Board.LoadBoardFromFen("3k4/8/8/8/8/4NB2/8/3K4 w - - 0 1");

            var moves = board.GetMoves();


            var winner = board.detectWinner(moves);

            Assert.AreEqual(Winner.NONE, winner);
        }
        [Test]
        public void SufficientMaterial1Pawn() {
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
             */
            var board = Board.LoadBoardFromFen("3k4/8/8/8/8/8/4P3/3K4 w - - 0 1");

            var moves = board.GetMoves();


            var winner = board.detectWinner(moves);

            Assert.AreEqual(Winner.NONE, winner);
        }

        [Test]
        public void InsufficientMaterial2BishopSameColor() {
            /*
             * Starting position (White to play)
            +---------------+
            |_ _ _ k _ _ _ _| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ _ _ b _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ B _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |_ _ _ K _ _ _ _| 1
            +---------------+
             A B C D E F G H
             */
            var board = Board.LoadBoardFromFen("3k4/8/4b3/8/4B3/8/8/3K4 w - - 0 1");

            var moves = board.GetMoves();

            var winner = board.detectWinner(moves);

            Assert.AreEqual(Winner.DRAW, winner);
        }

        [Test]
        public void SufficientMaterial2BishopDifferentColor() {
            /*
             * Starting position (White to play)
            +---------------+
            |_ _ _ k _ _ _ _| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ _ _ _ b _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ B _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |_ _ _ K _ _ _ _| 1
            +---------------+
             A B C D E F G H
             */
            var board = Board.LoadBoardFromFen("3k4/8/5b2/8/4B3/8/8/3K4 w - - 0 1");

            var moves = board.GetMoves();

            var winner = board.detectWinner(moves);

            Assert.AreEqual(Winner.NONE, winner);
        }

        [Test]
        public void FiftyStaleMoves() {
            /*
             * Starting position (White to play)
            +---------------+
            |_ _ _ k _ _ _ _| 8
            |_ _ p p p _ R _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ r| 2
            |_ _ _ K _ _ _ _| 1
            +---------------+
             A B C D E F G H
            G7 -> G2
            +---------------+
            |_ _ _ k _ _ _ _| 8
            |_ _ p p p _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ R r| 2
            |_ _ _ K _ _ _ _| 1
            +---------------+
             A B C D E F G H
            H2 -> H7
            +---------------+
            |_ _ _ k _ _ _ _| 8
            |_ _ p p p _ _ r| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ R _| 2
            |_ _ _ K _ _ _ _| 1
            +---------------+
             A B C D E F G H
             */
            var board = Board.LoadBoardFromFen("3k4/2ppp1R1/8/8/8/8/7r/3K4 w - - 48 2");

            var moves = board.GetMoves();
            var winner = board.detectWinner(moves);
            Assert.AreEqual(Winner.NONE, winner);

            board.MakeMove(BoardStateOffset.G7, BoardStateOffset.G2, Piece.EMPTY);
            moves = board.GetMoves();
            winner = board.detectWinner(moves);
            Assert.AreEqual(Winner.NONE, winner);
            
            board.MakeMove(BoardStateOffset.H2, BoardStateOffset.H7, Piece.EMPTY);
            moves = board.GetMoves();
            winner = board.detectWinner(moves);

            Assert.AreEqual(Winner.DRAW, winner);
        }

        [Test]
        public void PawnMovePrevents50StaleMoves() {
            /*
             * Starting position (White to play)
            +---------------+
            |_ _ _ k _ _ _ _| 8
            |_ _ p p p _ R _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ r| 2
            |_ _ _ K _ _ _ _| 1
            +---------------+
             A B C D E F G H
            G7 -> G2
            +---------------+
            |_ _ _ k _ _ _ _| 8
            |_ _ p p p _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ R r| 2
            |_ _ _ K _ _ _ _| 1
            +---------------+
             A B C D E F G H
            D7 -> D6
            +---------------+
            |_ _ _ k _ _ _ _| 8
            |_ _ p _ p _ _ _| 7
            |_ _ _ p _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ R r| 2
            |_ _ _ K _ _ _ _| 1
            +---------------+
             A B C D E F G H
             */
            var board = Board.LoadBoardFromFen("3k4/2ppp1R1/8/8/8/8/7r/3K4 w - - 48 2");

            var moves = board.GetMoves();
            var winner = board.detectWinner(moves);
            Assert.AreEqual(Winner.NONE, winner);

            board.MakeMove(BoardStateOffset.G7, BoardStateOffset.G2, Piece.EMPTY);
            moves = board.GetMoves();
            winner = board.detectWinner(moves);
            Assert.AreEqual(Winner.NONE, winner);

            board.MakeMove(BoardStateOffset.D7, BoardStateOffset.D6, Piece.EMPTY);
            moves = board.GetMoves();
            winner = board.detectWinner(moves);

            Assert.AreEqual(Winner.NONE, winner);
        }

        [Test]
        public void ThreefoldRepetition() {
            // remember
            // the state has to be the exact same which means it has to be same players turn, same pieces in the same position, same enpassant options and same castling options.
            // currently not implemented because you need to keep track of every move ever made, which is unfortunate.
            Assert.Fail();
        }
    }
}
