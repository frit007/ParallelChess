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

    public struct Move {
        public byte targetPosition;
        public byte fromPosition;
        public byte capturedPiece;
        public byte promotion;
        public byte moveFlags;

        // for undo
        public byte previousCastlingBits;
        public byte previousEnpassant;
        public byte previousHalfMove;

        public string readable { get { return MoveHelper.ReadableMove(this); } }
    }
}
