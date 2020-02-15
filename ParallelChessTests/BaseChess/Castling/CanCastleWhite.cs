using NUnit.Framework;
using ParallelChess;

namespace ParallelChessTests.BaseChess.Castling {
    class CanCastleWhite {

        #region White cannot move through check
        [Test]
        public void WhiteCannotCastleQueenSideThroughCheckOnC3() {
            /*
             * Start position (White to play)
             +---------------+
             |r _ _ _ k _ _ r| 8
             |_ _ _ _ _ _ _ _| 7
             |_ _ _ _ _ _ _ _| 6
             |_ _ r _ _ _ _ _| 5
             |_ _ _ _ _ _ _ _| 4
             |_ _ _ _ _ _ _ _| 3
             |_ _ _ _ _ _ _ _| 2
             |R _ _ _ K _ _ R| 1
             +---------------+
              A B C D E F G H
             */
            Board board = Board.LoadBoardFromFen("r3k2r/8/8/2r5/8/8/8/R3K2R w KQkq - 0 1");
            var moves = board.GetMovesForPosition(BoardStateOffset.E1);

            Assert.IsFalse(MoveHelper.isValidMove( moves.FindTargetPosition(BoardStateOffset.C1)));
        }

        [Test]
        public void WhiteCannotCastleQueenSideThroughCheckOnD3() {
            /*
             * Start position (White to play)
             +---------------+
             |r _ _ _ k _ _ r| 8
             |_ _ _ _ _ _ _ _| 7
             |_ _ _ _ _ _ _ _| 6
             |_ _ _ r _ _ _ _| 5
             |_ _ _ _ _ _ _ _| 4
             |_ _ _ _ _ _ _ _| 3
             |_ _ _ _ _ _ _ _| 2
             |R _ _ _ K _ _ R| 1
             +---------------+
              A B C D E F G H
             */
            Board board = Board.LoadBoardFromFen("r3k2r/8/8/3r4/8/8/8/R3K2R w KQkq - 0 1");
            var moves = board.GetMovesForPosition(BoardStateOffset.E1);

            Assert.IsFalse(MoveHelper.isValidMove( moves.FindTargetPosition(BoardStateOffset.C1)));
        }

        [Test]
        public void WhiteCannotCastleQueenSideWhileInCheck() {
            /*
             * Start position (White to play)
             +---------------+
             |r _ _ _ k _ _ r| 8
             |_ _ _ _ _ _ _ _| 7
             |_ _ _ _ _ _ _ _| 6
             |_ _ _ _ r _ _ _| 5
             |_ _ _ _ _ _ _ _| 4
             |_ _ _ _ _ _ _ _| 3
             |_ _ _ _ _ _ _ _| 2
             |R _ _ _ K _ _ R| 1
             +---------------+
              A B C D E F G H
             */
            Board board = Board.LoadBoardFromFen("r3k2r/8/8/4r3/8/8/8/R3K2R w KQkq - 0 1");
            var moves = board.GetMovesForPosition(BoardStateOffset.E1);

            Assert.IsFalse(MoveHelper.isValidMove( moves.FindTargetPosition(BoardStateOffset.C1)));
        }

        [Test]
        public void WhiteCannotCastleKingSideWhileInCheck() {
            /*
             * Start position (White to play)
             +---------------+
             |r _ _ _ k _ _ r| 8
             |_ _ _ _ _ _ _ _| 7
             |_ _ _ _ _ _ _ _| 6
             |_ _ _ _ r _ _ _| 5
             |_ _ _ _ _ _ _ _| 4
             |_ _ _ _ _ _ _ _| 3
             |_ _ _ _ _ _ _ _| 2
             |R _ _ _ K _ _ R| 1
             +---------------+
              A B C D E F G H
             */
            Board board = Board.LoadBoardFromFen("r3k2r/8/8/4r3/8/8/8/R3K2R w KQkq - 0 1");
            var moves = board.GetMovesForPosition(BoardStateOffset.E1);

            Assert.IsFalse(MoveHelper.isValidMove( moves.FindTargetPosition(BoardStateOffset.G1)));
        }

        [Test]
        public void WhiteCannotCastleKingSideThroughCheckOnF8() {
            /*
             * Start position (White to play)
             +---------------+
             |r _ _ _ k _ _ r| 8
             |_ _ _ _ _ _ _ _| 7
             |_ _ _ _ _ _ _ _| 6
             |_ _ _ _ _ r _ _| 5
             |_ _ _ _ _ _ _ _| 4
             |_ _ _ _ _ _ _ _| 3
             |_ _ _ _ _ _ _ _| 2
             |R _ _ _ K _ _ R| 1
             +---------------+
              A B C D E F G H
             */
            Board board = Board.LoadBoardFromFen("r3k2r/8/8/5r2/8/8/8/R3K2R w KQkq - 0 1");
            var moves = board.GetMovesForPosition(BoardStateOffset.E1);

            Assert.IsFalse(MoveHelper.isValidMove( moves.FindTargetPosition(BoardStateOffset.G1)));
        }

        [Test]
        public void WhiteCannotCastleKingSideThroughCheckOnG8() {
            /*
             * Start position (White to play)
             +---------------+
             |r _ _ _ k _ _ r| 8
             |_ _ _ _ _ _ _ _| 7
             |_ _ _ _ _ _ _ _| 6
             |_ _ _ _ _ _ r _| 5
             |_ _ _ _ _ _ _ _| 4
             |_ _ _ _ _ _ _ _| 3
             |_ _ _ _ _ _ _ _| 2
             |R _ _ _ K _ _ R| 1
             +---------------+
              A B C D E F G H
             */
            Board board = Board.LoadBoardFromFen("r3k2r/8/8/6r1/8/8/8/R3K2R w KQkq - 0 1");
            var moves = board.GetMovesForPosition(BoardStateOffset.E1);

            Assert.IsFalse(MoveHelper.isValidMove( moves.FindTargetPosition(BoardStateOffset.G1)));
        }
        #endregion
    }
}
