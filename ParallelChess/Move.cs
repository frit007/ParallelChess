using System;
using System.Collections.Generic;
using System.Text;

namespace ParallelChess {
    [Flags]
    public enum MoveFlags {
        ENPASSANT = 0b0001,
        CASTLING = 0b0010,
        PAWN_MOVE = 0b0100,
        BIG_PAWN_MOVE = 0b1000,
        EMPTY = 0,
    }

    //public readonly byte targetPosition;
    //public readonly byte fromPosition;
    //// use bytes instead of the actual datatype since they are smaller
    //// this has the advantage that fewer bytes are copied when calling functions
    //public readonly byte capturedPiece;
    //public readonly byte promotion;
    //public readonly byte moveFlags;

    //// for undo
    //public readonly byte previousCastlingBits;
    //public readonly byte previousEnpassant;
    //public readonly byte previousHalfMove;

    //public Move(
    //    byte targetPosition,
    //    byte fromPosition,
    //    byte capturedPiece,
    //    byte promotion,
    //    byte moveFlags,
    //    byte previousCastlingBits,
    //    byte previousEnpassant,
    //    byte previousHalfMove) {

    //    this.targetPosition = targetPosition;
    //    this.fromPosition = fromPosition;
    //    this.capturedPiece = capturedPiece;
    //    this.promotion = promotion;
    //    this.moveFlags = moveFlags;
    //    this.previousCastlingBits = previousCastlingBits;
    //    this.previousEnpassant = previousEnpassant;
    //    this.previousHalfMove = previousHalfMove;

    //}

    public struct Move {
        public readonly byte targetPosition;
        public readonly byte fromPosition;
        // use bytes instead of the actual datatype since they are smaller
        // this has the advantage that fewer bytes are copied when calling functions
        public readonly byte capturedPiece;
        public readonly byte promotion;
        public readonly byte moveFlags;

        // for undo
        public readonly byte previousCastlingBits;
        public readonly byte previousEnpassant;
        public readonly byte previousHalfMove;

        public Move(
            byte targetPosition,
            byte fromPosition,
            byte capturedPiece,
            byte promotion,
            byte moveFlags,
            byte previousCastlingBits,
            byte previousEnpassant,
            byte previousHalfMove) {

            this.targetPosition = targetPosition;
            this.fromPosition = fromPosition;
            this.capturedPiece = capturedPiece;
            this.promotion = promotion;
            this.moveFlags = moveFlags;
            this.previousCastlingBits = previousCastlingBits;
            this.previousEnpassant = previousEnpassant;
            this.previousHalfMove = previousHalfMove;

        }

        // for debugging, when you have a list of moves it helps being able to read the 
        public string readable { get { return MoveHelper.ReadableMove(this); } }
    }
}
