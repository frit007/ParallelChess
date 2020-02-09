using NUnit.Framework;
using ParallelChess;

namespace ParallelChessTests.BaseChess.Castling {
    class CanCastleBlack {
        #region Black cannot move through check
        [Test]
        public void BlackCannotCastleQueenSideThroughCheckOnC3() {
            /*
             * Start position (Black to play)
             +---------------+
             |r _ _ _ k _ _ r| 8
             |_ _ _ _ _ _ _ _| 7
             |_ _ _ _ _ _ _ _| 6
             |_ _ R _ _ _ _ _| 5
             |_ _ _ _ _ _ _ _| 4
             |_ _ _ _ _ _ _ _| 3
             |_ _ _ _ _ _ _ _| 2
             |R _ _ _ K _ _ R| 1
             +---------------+
              A B C D E F G H
             */
            Board board = Chess.LoadBoardFromFen("r3k2r/8/8/2R5/8/8/8/R3K2R b KQkq - 0 1");
            var moves = BoardHelper.GetMovesForPosition(board, BoardStateOffset.E8);

            Assert.IsFalse(MoveHelper.isValidMove(moves.FindTargetPosition(BoardStateOffset.C8)));
        }

        [Test]
        public void BlackCannotCastleQueenSideThroughCheckOnD3() {
            /*
             * Start position (Black to play)
             +---------------+
             |r _ _ _ k _ _ r| 8
             |_ _ _ _ _ _ _ _| 7
             |_ _ _ _ _ _ _ _| 6
             |_ _ _ R _ _ _ _| 5
             |_ _ _ _ _ _ _ _| 4
             |_ _ _ _ _ _ _ _| 3
             |_ _ _ _ _ _ _ _| 2
             |R _ _ _ K _ _ R| 1
             +---------------+
              A B C D E F G H
             */
            Board board = Chess.LoadBoardFromFen("r3k2r/8/8/3R4/8/8/8/R3K2R b KQkq - 0 1");
            var moves = BoardHelper.GetMovesForPosition(board, BoardStateOffset.E8);

            Assert.IsFalse(MoveHelper.isValidMove(moves.FindTargetPosition(BoardStateOffset.C8)));
        }

        [Test]
        public void BlackCannotCastleQueenSideWhileInCheck() {
            /*
             * Start position (Black to play)
             +---------------+
             |r _ _ _ k _ _ r| 8
             |_ _ _ _ _ _ _ _| 7
             |_ _ _ _ _ _ _ _| 6
             |_ _ _ _ R _ _ _| 5
             |_ _ _ _ _ _ _ _| 4
             |_ _ _ _ _ _ _ _| 3
             |_ _ _ _ _ _ _ _| 2
             |R _ _ _ K _ _ R| 1
             +---------------+
              A B C D E F G H
             */
            Board board = Chess.LoadBoardFromFen("r3k2r/8/8/4R3/8/8/8/R3K2R b KQkq - 0 1");
            var moves = BoardHelper.GetMovesForPosition(board, BoardStateOffset.E8);

            Assert.IsFalse(MoveHelper.isValidMove(moves.FindTargetPosition(BoardStateOffset.C8)));
        }

        [Test]
        public void BlackCannotCastleKingSideWhileInCheck() {
            /*
             * Start position (Black to play)
             +---------------+
             |r _ _ _ k _ _ r| 8
             |_ _ _ _ _ _ _ _| 7
             |_ _ _ _ _ _ _ _| 6
             |_ _ _ _ R _ _ _| 5
             |_ _ _ _ _ _ _ _| 4
             |_ _ _ _ _ _ _ _| 3
             |_ _ _ _ _ _ _ _| 2
             |R _ _ _ K _ _ R| 1
             +---------------+
              A B C D E F G H
             */
            Board board = Chess.LoadBoardFromFen("r3k2r/8/8/4R3/8/8/8/R3K2R b KQkq - 0 1");
            var moves = BoardHelper.GetMovesForPosition(board, BoardStateOffset.E8);

            Assert.IsFalse(MoveHelper.isValidMove(moves.FindTargetPosition(BoardStateOffset.G8)));
        }

        [Test]
        public void BlackCannotCastleKingSideThroughCheckOnF8() {
            /*
             * Start position (Black to play)
             +---------------+
             |r _ _ _ k _ _ r| 8
             |_ _ _ _ _ _ _ _| 7
             |_ _ _ _ _ _ _ _| 6
             |_ _ _ _ _ R _ _| 5
             |_ _ _ _ _ _ _ _| 4
             |_ _ _ _ _ _ _ _| 3
             |_ _ _ _ _ _ _ _| 2
             |R _ _ _ K _ _ R| 1
             +---------------+
              A B C D E F G H
             */
            Board board = Chess.LoadBoardFromFen("r3k2r/8/8/5R2/8/8/8/R3K2R b KQkq - 0 1");
            var moves = BoardHelper.GetMovesForPosition(board, BoardStateOffset.E8);

            Assert.IsFalse(MoveHelper.isValidMove(moves.FindTargetPosition(BoardStateOffset.G8)));
        }

        [Test]
        public void BlackCannotCastleKingSideThroughCheckOnG8() {
            /*
             * Start position (Black to play)
             +---------------+
             |r _ _ _ k _ _ r| 8
             |_ _ _ _ _ _ _ _| 7
             |_ _ _ _ _ _ _ _| 6
             |_ _ _ _ _ _ R _| 5
             |_ _ _ _ _ _ _ _| 4
             |_ _ _ _ _ _ _ _| 3
             |_ _ _ _ _ _ _ _| 2
             |R _ _ _ K _ _ R| 1
             +---------------+
              A B C D E F G H
             */
            Board board = Chess.LoadBoardFromFen("r3k2r/8/8/6R1/8/8/8/R3K2R b KQkq - 0 1");
            var moves = BoardHelper.GetMovesForPosition(board, BoardStateOffset.E8);

            Assert.IsFalse(MoveHelper.isValidMove(moves.FindTargetPosition(BoardStateOffset.G8)));
        }
        #endregion

    }
}
