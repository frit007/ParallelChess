#pragma once

#include "Piece.h"
#include "Move.h"
#include "MoveFlags.h"
#include <string>
#include <vector>
// this one might have be replaced with a forward declaration

struct Board;

class MoveHelper
{
    public:
    static Move CreateMove(int targetPosition, int fromPosition, Piece capturedPiece, Piece promotion, MoveFlags moveFlags, Board* board);

    static bool isValidMove(Move move);

    static Move FindTargetPosition(std::vector<Move>* moves, int position);

    static Move FindTargetPosition(std::vector<Move>* moves, int fromPosition, int toPosition);

    static Move FindTargetPosition(std::vector<Move>* moves, int position, Piece promotion);

    static std::string ReadableMove(Move move);

};

