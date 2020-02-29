#include "pch.h"
#include "MoveHelper.h"
#include "Board.h"
#include "Piece.h"

Move MoveHelper::CreateMove(int targetPosition, int fromPosition, Piece capturedPiece, Piece promotion, MoveFlags moveFlags, Board* board) {
    return Move::Move(
        (char)targetPosition,
        (char)fromPosition,
        (char)capturedPiece,
        (char)promotion,
        (char)moveFlags,

        (char)board->getCastlingBits(),
        (char)board->getEnPassantTarget(),
        (char)board->getHalfTurnCounter()
    );
}

bool MoveHelper::isValidMove(Move move) {
    return move.targetPosition != move.fromPosition;
}

Move MoveHelper::FindTargetPosition(std::vector<Move>* moves, int position) {
    for (auto move : *moves) {
        if (position == move.targetPosition) {
            return move;
        }
    }
}

Move MoveHelper::FindTargetPosition(std::vector<Move>* moves, int fromPosition, int toPosition) {
    for (auto move : *moves) {
        if (toPosition== move.targetPosition && move.fromPosition == fromPosition) {
            return move;
        }
    }
}



Move MoveHelper::FindTargetPosition(std::vector<Move>* moves, int position, Piece promotion) {
    for (auto move : *moves) {
        if (position == move.targetPosition && (promotion == PIECE_EMPTY || promotion == (Piece)move.promotion)) {
            return move;
        }
    }
}

std::string MoveHelper::ReadableMove(Move move) {
    auto fromPosition = move.fromPosition;
    auto toPosition = move.targetPosition;
    return "from: " + BoardPosition::ReadablePosition(fromPosition) + " to: "+ BoardPosition::ReadablePosition(toPosition);
}
