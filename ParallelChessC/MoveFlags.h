#pragma once

enum MoveFlags {
    ENPASSANT = 0b0001,
    CASTLING = 0b0010,
    PAWN_MOVE = 0b0100,
    BIG_PAWN_MOVE = 0b1000,
    MOVEFLAG_EMPTY = 0,
};

inline MoveFlags operator|(MoveFlags a, MoveFlags b)
{
    return static_cast<MoveFlags>(static_cast<int>(a) | static_cast<int>(b));
}

inline MoveFlags operator&(MoveFlags a, MoveFlags b)
{
    return static_cast<MoveFlags>(static_cast<int>(a)& static_cast<int>(b));
}