#pragma once

enum Piece
{
    PIECE_EMPTY            = 0b00000000,

    PAWN             = 0b00000010,
    KNIGHT           = 0b00000100,
    KING             = 0b00000110,
    ROOK             = 0b00001000,
    BISHOP           = 0b00010000,
    QUEEN            = 0b00011000,

    // used to only get what piece it is
    // use the first bit to mark pieces, this is not strictly necessary but it makes the error messages nicer
    // otherwise ROOK and ATTACKS_STRAIGHT have the same color pattern and for some reason is prefers ATTACKS_STRAIGHT
    PIECE_MASK       = 0b00011110,

    // we are using the 4th and 5th bit to store information about if the piece attacks slanted or straight(or both in case of the queen)
    // this is done to speed up attacked checks
    ATTACKS_STRAIGHT = 0b00001000,
    ATTACKS_SLANTED  = 0b00010000,

    // use the furthest right as as isWhite because we can compare isWhite directly to other things that also follow this convention
    IS_WHITE         = 0b00000001,

    // IS_BLACK would be 0000_0000 but it gives anoying error messages(everything is marked as IS_BLACK because it matches every piece), therefor if something is not white it is implied to be black
    //IS_BLACK = 0b0000_0000,


    ENPASSANT_TARGET = 0b001000000
};

inline Piece operator|(Piece a, Piece b)
{
    return static_cast<Piece>(static_cast<int>(a) | static_cast<int>(b));
}

inline Piece operator&(Piece a, Piece b)
{
    return static_cast<Piece>(static_cast<int>(a) & static_cast<int>(b));
}