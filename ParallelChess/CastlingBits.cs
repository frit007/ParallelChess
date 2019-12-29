using System;
using System.Collections.Generic;
using System.Text;

namespace ParallelChess {
    // the numbers are factor of 2 so they can be combined later.
    // the bits indicate if the castling option is still possible;
    [Flags]
    public enum CastlingBits {
        // 0b0xxx is a way to write binary numbers
        BLACK_QUEEN_SIDE_CASTLE = 0b0000_0001,
        WHITE_QUEEN_SIDE_CASTLE = 0b0000_0010,
        BLACK_KING_SIDE_CASTLE = 0b0000_0100,
        WHITE_KING_SIDE_CASTLE = 0b0000_1000,

        // HELPERS for different combination of caslingbits
        // The idea is to have a negative filter that can be applied with and filter(&)
        // this is used to create a lookup filter where you based on a field can tell if you are allowed to castle
        // for example if the rook moves from A1 white cannot castle queen side(CAN_NOT_WQ)
        CAN_ALL = BLACK_KING_SIDE_CASTLE |BLACK_QUEEN_SIDE_CASTLE | WHITE_KING_SIDE_CASTLE | WHITE_QUEEN_SIDE_CASTLE,
        CAN_NOT_BQ = ~BLACK_QUEEN_SIDE_CASTLE,
        CAN_NOT_BK = ~BLACK_KING_SIDE_CASTLE,
        CAN_NOT_WQ = ~WHITE_QUEEN_SIDE_CASTLE,
        CAN_NOT_WK = ~WHITE_KING_SIDE_CASTLE,
        CAN_NOT_W = BLACK_KING_SIDE_CASTLE | BLACK_QUEEN_SIDE_CASTLE,
        CAN_NOT_B = WHITE_KING_SIDE_CASTLE | WHITE_QUEEN_SIDE_CASTLE,

        EMPTY = 0,
    }



    public static class CastlingHelper {
        // reference 
        public static CastlingBits CAN_ALL = CastlingBits.CAN_ALL;
        public static CastlingBits CAN_NOT_BQ = CastlingBits.CAN_NOT_BQ;
        public static CastlingBits CAN_NOT_BK = CastlingBits.CAN_NOT_BK;
        public static CastlingBits CAN_NOT_WQ = CastlingBits.CAN_NOT_WQ;
        public static CastlingBits CAN_NOT_WK = CastlingBits.CAN_NOT_WK;
        public static CastlingBits CAN_NOT_W = CastlingBits.CAN_NOT_W;
        public static CastlingBits CAN_NOT_B = CastlingBits.CAN_NOT_B;

        // Used to lookup which castling options are still alowed when you move to or from a position on the board
        public static CastlingBits[] castleLookup = {
            CAN_NOT_WQ, CAN_ALL, CAN_ALL, CAN_ALL, CAN_NOT_W, CAN_ALL, CAN_ALL, CAN_NOT_WK,
            CAN_ALL,    CAN_ALL, CAN_ALL, CAN_ALL, CAN_ALL,   CAN_ALL, CAN_ALL, CAN_ALL, // invalidRow (used for 0x88)
            CAN_ALL,    CAN_ALL, CAN_ALL, CAN_ALL, CAN_ALL,   CAN_ALL, CAN_ALL, CAN_ALL,
            CAN_ALL,    CAN_ALL, CAN_ALL, CAN_ALL, CAN_ALL,   CAN_ALL, CAN_ALL, CAN_ALL, // invalidRow (used for 0x88)
            CAN_ALL,    CAN_ALL, CAN_ALL, CAN_ALL, CAN_ALL,   CAN_ALL, CAN_ALL, CAN_ALL,
            CAN_ALL,    CAN_ALL, CAN_ALL, CAN_ALL, CAN_ALL,   CAN_ALL, CAN_ALL, CAN_ALL, // invalidRow (used for 0x88)
            CAN_ALL,    CAN_ALL, CAN_ALL, CAN_ALL, CAN_ALL,   CAN_ALL, CAN_ALL, CAN_ALL,
            CAN_ALL,    CAN_ALL, CAN_ALL, CAN_ALL, CAN_ALL,   CAN_ALL, CAN_ALL, CAN_ALL, // invalidRow (used for 0x88)
            CAN_ALL,    CAN_ALL, CAN_ALL, CAN_ALL, CAN_ALL,   CAN_ALL, CAN_ALL, CAN_ALL,
            CAN_ALL,    CAN_ALL, CAN_ALL, CAN_ALL, CAN_ALL,   CAN_ALL, CAN_ALL, CAN_ALL, // invalidRow (used for 0x88)
            CAN_ALL,    CAN_ALL, CAN_ALL, CAN_ALL, CAN_ALL,   CAN_ALL, CAN_ALL, CAN_ALL,
            CAN_ALL,    CAN_ALL, CAN_ALL, CAN_ALL, CAN_ALL,   CAN_ALL, CAN_ALL, CAN_ALL, // invalidRow (used for 0x88)
            CAN_ALL,    CAN_ALL, CAN_ALL, CAN_ALL, CAN_ALL,   CAN_ALL, CAN_ALL, CAN_ALL, 
            CAN_ALL,    CAN_ALL, CAN_ALL, CAN_ALL, CAN_ALL,   CAN_ALL, CAN_ALL, CAN_ALL, // invalidRow (used for 0x88)
            CAN_NOT_BQ, CAN_ALL, CAN_ALL, CAN_ALL, CAN_NOT_B, CAN_ALL, CAN_ALL, CAN_NOT_BK,
        };
    }
}
