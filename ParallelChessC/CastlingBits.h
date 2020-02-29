#pragma once
    // the numbers are factor of 2 so they can be combined later.
    // the bits indicate if the castling option is still possible;

#include "BoardStateOffset.h"

enum CastlingBits {
    // 0b0xxx is a way to write binary numbers
    BLACK_QUEEN_SIDE_CASTLE = 0b0001,
    WHITE_QUEEN_SIDE_CASTLE = 0b0010,
    BLACK_KING_SIDE_CASTLE = 0b0100,
    WHITE_KING_SIDE_CASTLE = 0b1000,

    // HELPERS for different combination of caslingbits
    // The idea is to have a negative filter that can be applied with and filter(&)
    // this is used to create a lookup filter where you based on a field can tell if you are allowed to castle
    // for example if the rook moves from A1 white cannot castle queen side(CAN_NOT_WQ)
    CAN_ALL = BLACK_KING_SIDE_CASTLE | BLACK_QUEEN_SIDE_CASTLE | WHITE_KING_SIDE_CASTLE | WHITE_QUEEN_SIDE_CASTLE,
    CAN_NOT_BQ = ~BLACK_QUEEN_SIDE_CASTLE,
    CAN_NOT_BK = ~BLACK_KING_SIDE_CASTLE,
    CAN_NOT_WQ = ~WHITE_QUEEN_SIDE_CASTLE,
    CAN_NOT_WK = ~WHITE_KING_SIDE_CASTLE,
    CAN_NOT_W = BLACK_KING_SIDE_CASTLE | BLACK_QUEEN_SIDE_CASTLE,
    CAN_NOT_B = WHITE_KING_SIDE_CASTLE | WHITE_QUEEN_SIDE_CASTLE,

    CATLING_BITS_EMPTY = 0,
};

inline CastlingBits operator|(CastlingBits a, CastlingBits b)
{
    return static_cast<CastlingBits>(static_cast<int>(a) | static_cast<int>(b));
}

inline CastlingBits operator&(CastlingBits a, CastlingBits b)
{
    return static_cast<CastlingBits>(static_cast<int>(a)& static_cast<int>(b));
}


// Used to lookup which castling options are still alowed when you move to or from a position on the board
const CastlingBits castleLookup[BOARD_STATE_SIZE] = {
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