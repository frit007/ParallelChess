using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using ParallelChess;

namespace ParallelChessTests {
    class CanCastleTest {

        [Test]
        public void WhiteCannotCastleQueenSideAfterA1Moves() {
            CastlingBits castling = CastlingBits.CAN_ALL;

            castling = castling & CastlingHelper.castleLookup[BoardOffset.A1];

            Assert.IsFalse((castling & CastlingBits.WHITE_QUEEN_SIDE_CASTLE) == CastlingBits.WHITE_QUEEN_SIDE_CASTLE,
                "White cannot castle queen side after a1 moves");

            Assert.IsTrue((castling & CastlingBits.WHITE_KING_SIDE_CASTLE) == CastlingBits.WHITE_KING_SIDE_CASTLE,
                "White can still castle king side after a1 moves");

            Assert.IsTrue((castling & CastlingBits.BLACK_QUEEN_SIDE_CASTLE) == CastlingBits.BLACK_QUEEN_SIDE_CASTLE,
                "Black can still castle");

            Assert.IsTrue((castling & CastlingBits.BLACK_KING_SIDE_CASTLE) == CastlingBits.BLACK_KING_SIDE_CASTLE,
               "Black can still castle");
        }

        [Test]
        public void WhiteCannotCastleKingSideAfterH1Moves() {
            CastlingBits castling = CastlingBits.CAN_ALL;

            castling = castling & CastlingHelper.castleLookup[BoardOffset.H1];

            Assert.IsFalse((castling & CastlingBits.WHITE_KING_SIDE_CASTLE) == CastlingBits.WHITE_KING_SIDE_CASTLE,
                "White cannot castle king side after h1 moves");

            Assert.IsTrue((castling & CastlingBits.WHITE_QUEEN_SIDE_CASTLE) == CastlingBits.WHITE_QUEEN_SIDE_CASTLE,
                "White can still castle queen side after h1 moves");

            Assert.IsTrue((castling & CastlingBits.BLACK_QUEEN_SIDE_CASTLE) == CastlingBits.BLACK_QUEEN_SIDE_CASTLE,
                "Black can still castle");

            Assert.IsTrue((castling & CastlingBits.BLACK_KING_SIDE_CASTLE) == CastlingBits.BLACK_KING_SIDE_CASTLE,
                "Black can still castle");
        }

        [Test]
        public void WhiteCannotCastleAfterE1Moves() {
            CastlingBits castling = CastlingBits.CAN_ALL;

            castling = castling & CastlingHelper.castleLookup[BoardOffset.E1];

            Assert.IsFalse((castling & CastlingBits.WHITE_KING_SIDE_CASTLE) == CastlingBits.WHITE_KING_SIDE_CASTLE,
                "White cannot cannot king side after e1 moves");

            Assert.IsFalse((castling & CastlingBits.WHITE_QUEEN_SIDE_CASTLE) == CastlingBits.WHITE_QUEEN_SIDE_CASTLE,
                "White castle cannot queen side after e1 moves");



            Assert.IsTrue((castling & CastlingBits.BLACK_QUEEN_SIDE_CASTLE) == CastlingBits.BLACK_QUEEN_SIDE_CASTLE,
                "Black can still castle");

            Assert.IsTrue((castling & CastlingBits.BLACK_KING_SIDE_CASTLE) == CastlingBits.BLACK_KING_SIDE_CASTLE,
                "Black can still castle");
        }


        [Test]
        public void BlackCannotCastleQueenSideAfterA8Moves() {
            CastlingBits castling = CastlingBits.CAN_ALL;

            castling = castling & CastlingHelper.castleLookup[BoardOffset.A8];

            Assert.IsFalse((castling & CastlingBits.BLACK_QUEEN_SIDE_CASTLE) == CastlingBits.BLACK_QUEEN_SIDE_CASTLE,
                "Black cannot castle queen side after a8 moves");

            Assert.IsTrue((castling & CastlingBits.BLACK_KING_SIDE_CASTLE) == CastlingBits.BLACK_KING_SIDE_CASTLE,
                "Black can still castle king side after a8 moves");
            
            Assert.IsTrue((castling & CastlingBits.WHITE_KING_SIDE_CASTLE) == CastlingBits.WHITE_KING_SIDE_CASTLE,
                "White can still castle");

            Assert.IsTrue((castling & CastlingBits.WHITE_QUEEN_SIDE_CASTLE) == CastlingBits.WHITE_QUEEN_SIDE_CASTLE,
                "White can still castle");
        }

        [Test]
        public void BlackCannotCastleQueenSideAfterH8Moves() {
            CastlingBits castling = CastlingBits.CAN_ALL;

            castling = castling & CastlingHelper.castleLookup[BoardOffset.H8];

            Assert.IsTrue((castling & CastlingBits.BLACK_QUEEN_SIDE_CASTLE) == CastlingBits.BLACK_QUEEN_SIDE_CASTLE,
                "Black can still castle queen side after h8 moves");

            Assert.IsFalse((castling & CastlingBits.BLACK_KING_SIDE_CASTLE) == CastlingBits.BLACK_KING_SIDE_CASTLE,
                "Black cannot castle king side after h8 moves");

            Assert.IsTrue((castling & CastlingBits.WHITE_KING_SIDE_CASTLE) == CastlingBits.WHITE_KING_SIDE_CASTLE,
                "White can still castle");

            Assert.IsTrue((castling & CastlingBits.WHITE_QUEEN_SIDE_CASTLE) == CastlingBits.WHITE_QUEEN_SIDE_CASTLE,
                "White can still castle");
        }

        [Test]
        public void BlackCannotCastleQueenSideAfterKingMoves() {
            CastlingBits castling = CastlingBits.CAN_ALL;

            castling = castling & CastlingHelper.castleLookup[BoardOffset.E8];

            Assert.IsFalse((castling & CastlingBits.BLACK_QUEEN_SIDE_CASTLE) == CastlingBits.BLACK_QUEEN_SIDE_CASTLE,
                "Black canont castle queen side after a8 moves");

            Assert.IsFalse((castling & CastlingBits.BLACK_KING_SIDE_CASTLE) == CastlingBits.BLACK_KING_SIDE_CASTLE,
                "Black cannot castle king side after a8 moves");

            Assert.IsTrue((castling & CastlingBits.WHITE_KING_SIDE_CASTLE) == CastlingBits.WHITE_KING_SIDE_CASTLE,
                "White can still castle");

            Assert.IsTrue((castling & CastlingBits.WHITE_QUEEN_SIDE_CASTLE) == CastlingBits.WHITE_QUEEN_SIDE_CASTLE,
                "White can still castle");
        }

        
    }
}
