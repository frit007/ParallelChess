#pragma once

struct Move {
public:
    char targetPosition;
    char fromPosition;
    // use chars instead of the actual datatype since they are smaller
    // this has the advantage that fewer chars are copied when calling functions
    char capturedPiece;
    char promotion;
    char moveFlags;

    // for undo
    char previousCastlingBits;
    char previousEnpassant;
    char previousHalfMove;

    Move(
        char a_targetPosition,
        char a_fromPosition,
        char a_capturedPiece,
        char a_promotion,
        char a_moveFlags,
        char a_previousCastlingBits,
        char a_previousEnpassant,
        char a_previousHalfMove) {

        targetPosition = a_targetPosition;
        fromPosition = a_fromPosition;
        capturedPiece = a_capturedPiece;
        promotion = a_promotion;
        moveFlags = a_moveFlags;
        previousCastlingBits = a_previousCastlingBits;
        previousEnpassant = a_previousEnpassant;
        previousHalfMove = a_previousHalfMove;

    }
};