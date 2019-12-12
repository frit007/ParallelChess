using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using ParallelChess;

namespace ParallelChessTests.Castling {
    class CanCastleTest {



        [Test]
        public void WhiteCastleQueenSide() {
            /*
             * Start position (White to play)
             +---------------+
             |r _ _ _ k _ _ r| 8
             |_ _ _ _ _ _ _ _| 7
             |_ _ _ _ _ _ _ _| 6
             |_ _ _ _ _ _ _ _| 5
             |_ _ _ _ _ _ _ _| 4
             |_ _ _ _ _ _ _ _| 3
             |_ _ _ _ _ _ _ _| 2
             |R _ _ _ K _ _ R| 1
             +---------------+
              A B C D E F G H
             
             Expected move
             +---------------+
             |r _ _ _ k _ _ r| 8
             |_ _ _ _ _ _ _ _| 7
             |_ _ _ _ _ _ _ _| 6
             |_ _ _ _ _ _ _ _| 5
             |_ _ _ _ _ _ _ _| 4
             |_ _ _ _ _ _ _ _| 3
             |_ _ _ _ _ _ _ _| 2
             |_ _ K R _ _ _ R| 1
             +---------------+
              A B C D E F G H
             */
            BoardState board = Chess.LoadBoardFromFen("r3k2r/8/8/8/8/8/8/R3K2R w KQkq - 0 1");
            Chess.MakeMove(board, BoardStateOffset.E1, BoardStateOffset.C1);

            Assert.AreEqual(Piece.KING | Piece.IS_WHITE, board.C1);
            Assert.AreEqual(Piece.ROOK | Piece.IS_WHITE, board.D1);

            Assert.AreEqual(CastlingBits.EMPTY, board.CastlingBits & (CastlingBits.WHITE_KING_SIDE_CASTLE | CastlingBits.WHITE_QUEEN_SIDE_CASTLE), "White can no longer castle");

            Assert.AreEqual(CastlingBits.BLACK_KING_SIDE_CASTLE | CastlingBits.BLACK_QUEEN_SIDE_CASTLE, board.CastlingBits & (CastlingBits.BLACK_KING_SIDE_CASTLE | CastlingBits.BLACK_QUEEN_SIDE_CASTLE), "Black can still castle");
        }

        [Test]
        public void WhiteCastleKingSide() {
            /*
             * Start position (White to play)
             +---------------+
             |r _ _ _ k _ _ r| 8
             |_ _ _ _ _ _ _ _| 7
             |_ _ _ _ _ _ _ _| 6
             |_ _ _ _ _ _ _ _| 5
             |_ _ _ _ _ _ _ _| 4
             |_ _ _ _ _ _ _ _| 3
             |_ _ _ _ _ _ _ _| 2
             |R _ _ _ K _ _ R| 1
             +---------------+
              A B C D E F G H
             
             Expected move
             +---------------+
             |r _ _ _ k _ _ r| 8
             |_ _ _ _ _ _ _ _| 7
             |_ _ _ _ _ _ _ _| 6
             |_ _ _ _ _ _ _ _| 5
             |_ _ _ _ _ _ _ _| 4
             |_ _ _ _ _ _ _ _| 3
             |_ _ _ _ _ _ _ _| 2
             |R _ _ _ _ R K _| 1
             +---------------+
              A B C D E F G H
             */
            BoardState board = Chess.LoadBoardFromFen("r3k2r/8/8/8/8/8/8/R3K2R w KQkq - 0 1");
            Chess.MakeMove(board, BoardStateOffset.E1, BoardStateOffset.G1);

            Assert.AreEqual(Piece.KING | Piece.IS_WHITE, board.G1);
            Assert.AreEqual(Piece.ROOK | Piece.IS_WHITE, board.F1);

            Assert.AreEqual(CastlingBits.EMPTY, board.CastlingBits & (CastlingBits.WHITE_KING_SIDE_CASTLE | CastlingBits.WHITE_QUEEN_SIDE_CASTLE), "White can no longer castle");

            Assert.AreEqual(CastlingBits.BLACK_KING_SIDE_CASTLE | CastlingBits.BLACK_QUEEN_SIDE_CASTLE, board.CastlingBits & (CastlingBits.BLACK_KING_SIDE_CASTLE | CastlingBits.BLACK_QUEEN_SIDE_CASTLE), "Black can still castle");
        }

        [Test]
        public void BlackCastleKingSide() {
            /*
             * Start position (Black to play)
             +---------------+
             |r _ _ _ k _ _ r| 8
             |_ _ _ _ _ _ _ _| 7
             |_ _ _ _ _ _ _ _| 6
             |_ _ _ _ _ _ _ _| 5
             |_ _ _ _ _ _ _ _| 4
             |_ _ _ _ _ _ _ _| 3
             |_ _ _ _ _ _ _ _| 2
             |R _ _ _ K _ _ R| 1
             +---------------+
              A B C D E F G H
             
             Expected move
             +---------------+
             |r _ _ _ _ r k _| 8
             |_ _ _ _ _ _ _ _| 7
             |_ _ _ _ _ _ _ _| 6
             |_ _ _ _ _ _ _ _| 5
             |_ _ _ _ _ _ _ _| 4
             |_ _ _ _ _ _ _ _| 3
             |_ _ _ _ _ _ _ _| 2
             |R _ _ _ R _ _ R| 1
             +---------------+
              A B C D E F G H
             */
            BoardState board = Chess.LoadBoardFromFen("r3k2r/8/8/8/8/8/8/R3K2R b KQkq - 0 1");
            Chess.MakeMove(board, BoardStateOffset.E8, BoardStateOffset.G8);

            Assert.AreEqual(Piece.KING | Piece.IS_BLACK, board.G8);
            Assert.AreEqual(Piece.ROOK | Piece.IS_BLACK, board.F8);

            Assert.AreEqual(CastlingBits.EMPTY, board.CastlingBits & (CastlingBits.BLACK_KING_SIDE_CASTLE | CastlingBits.BLACK_KING_SIDE_CASTLE), "Black can no longer castle");

            Assert.AreEqual(CastlingBits.WHITE_QUEEN_SIDE_CASTLE | CastlingBits.WHITE_KING_SIDE_CASTLE, board.CastlingBits & (CastlingBits.WHITE_KING_SIDE_CASTLE | CastlingBits.WHITE_QUEEN_SIDE_CASTLE), "White can still castle");
        }

        [Test]
        public void BlackCastleQueenSide() {
            /*
             * Start position (Black to play)
             +---------------+
             |r _ _ _ k _ _ r| 8
             |_ _ _ _ _ _ _ _| 7
             |_ _ _ _ _ _ _ _| 6
             |_ _ _ _ _ _ _ _| 5
             |_ _ _ _ _ _ _ _| 4
             |_ _ _ _ _ _ _ _| 3
             |_ _ _ _ _ _ _ _| 2
             |R _ _ _ K _ _ R| 1
             +---------------+
              A B C D E F G H
             
             Expected move
             +---------------+
             |_ _ k r _ _ _ r| 8
             |_ _ _ _ _ _ _ _| 7
             |_ _ _ _ _ _ _ _| 6
             |_ _ _ _ _ _ _ _| 5
             |_ _ _ _ _ _ _ _| 4
             |_ _ _ _ _ _ _ _| 3
             |_ _ _ _ _ _ _ _| 2
             |R _ _ _ L _ _ R| 1
             +---------------+
              A B C D E F G H
             */
            BoardState board = Chess.LoadBoardFromFen("r3k2r/8/8/8/8/8/8/R3K2R b KQkq - 0 1");
            Chess.MakeMove(board, BoardStateOffset.E8, BoardStateOffset.C8);

            Assert.AreEqual(Piece.KING | Piece.IS_BLACK, board.C8);
            Assert.AreEqual(Piece.ROOK | Piece.IS_BLACK, board.D8);

            Assert.AreEqual(CastlingBits.EMPTY, board.CastlingBits & (CastlingBits.BLACK_KING_SIDE_CASTLE | CastlingBits.BLACK_KING_SIDE_CASTLE), "Black can no longer castle");

            Assert.AreEqual(CastlingBits.WHITE_QUEEN_SIDE_CASTLE | CastlingBits.WHITE_KING_SIDE_CASTLE, board.CastlingBits & (CastlingBits.WHITE_KING_SIDE_CASTLE | CastlingBits.WHITE_QUEEN_SIDE_CASTLE), "White can still castle");
        }

        [Test]
        public void BlackCannotCastleQueenSideIfBitIsNotSet() {
            /*
             * Start position (Black to play)
             +---------------+
             |r _ _ _ k _ _ r| 8
             |_ _ _ _ _ _ _ _| 7
             |_ _ _ _ _ _ _ _| 6
             |_ _ _ _ _ _ _ _| 5
             |_ _ _ _ _ _ _ _| 4
             |_ _ _ _ _ _ _ _| 3
             |_ _ _ _ _ _ _ _| 2
             |R _ _ _ K _ _ R| 1
             +---------------+
              A B C D E F G H
             */
            BoardState board = Chess.LoadBoardFromFen("r3k2r/8/8/8/8/8/8/R3K2R b - - 0 1");
            var moves = Board.GetMovesForPosition(board, BoardStateOffset.E8);

            Assert.IsFalse(MoveHelper.isValidMove(moves.FindTargetPosition(BoardStateOffset.C8)));
        }


    }
}
