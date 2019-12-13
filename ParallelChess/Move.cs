using System;
using System.Collections.Generic;
using System.Text;

namespace ParallelChess {


    //[Flags]
    //public enum Move {
    //    // bits dedicated to a specific purpose
    //    // 0000 uuuu labe pppp  cccc ffff fftt tttt
    //    // 00uu uula bepp ppcc  ccff ffff fttt tttt
    //    // legend: 
    //    // f = from 
    //    // t = to
    //    // c = captured piece
    //    // p = promotion
    //    // e = enpassant
    //    // b = big move (pawn makes a big move open oportunity for enpassant)
    //    // a = castling
    //    // l = pawn move (used to half move clock)
    //    // u = previous castle bits

    //    // ----------- TARGET POSITION ----------
    //    TARGET_POS_START = 0b0000_0000_0000_0000__0000_0000_0000_0000,
    //    TARGET_POS_MASK =  0b0000_0000_0000_0000__0000_0000_0111_1111,
    //    TARGET_BIT_OFFSET = 0,

    //    // ----------- FROM POSITION ----------
    //    FROM_POS_START = 0b0000_0000_0000_0000__0000_0000_1000_0000,
    //    FROM_POS_MASK =  0b0000_0000_0000_0000__0011_1111_1000_0000,
    //    FROM_POS_BIT_OFFSET = 7,

    //    // ----------- CAPTURED PIECE ----------
    //    CAPTURED_START = 0b0000_0000_0000_0000__0100_0000_0000_0000,
    //    CAPTURED_MASK =  0b0000_0000_0000_0011__1100_0000_0000_0000,
    //    CAPTURED_BIT_OFFSET = 14,

    //    // ----------- PROMOTION PIECE ----------
    //    PROMOTION_START = 0b0000_0000_0000_0100__0000_0000_0000_0000,
    //    PROMOTION_MASK =  0b0000_0000_0011_1100__0000_0000_0000_0000,
    //    PROMOTION_BIT_OFFSET = 18,

    //    // ----------- ENPASANT BOOL ----------
    //    ENPASSANT =       0b0000_0000_0100_0000__0000_0000_0000_0000,
    //    ENPASSANT_BIT_OFFSET = 23,

    //    // ----------- BIG PAWN MOVE BOOL ----------
    //    BIG_PAWN_MOVE =   0b0000_0000_1000_0000__0000_0000_0000_0000,
    //    BIG_PAWN_MOVE_BIT_OFFSET = 24,

    //    // ---------- CASTLING ----------
    //    CASTLING =        0b0000_0001_0000_0000__0000_0000_0000_0000,
    //    CASTLING_BIT_OFFSET = 25,

    //    // ---------- PAWN MOVE ----------
    //    PAWN_MOVE =       0b0000_0010_0000_0000__0000_0000_0000_0000,
    //    PAWN_BIT_OFFSET = 26,

    //    // ---------- PREVIOUS CASTLING BITS ----------
    //    PREVIOUS_CASTLING_BITS_START = 0b0000_0100_0000_0000__0000_0000_0000_0000,
    //    PREVIOUS_CASTLING_BITS_MASK =  0b0011_1100_0000_0000__0000_0000_0000_0000,
    //    PREVIOUS_CASTLING_BITS_OFFSET = 27,

    //    EMPTY = 0,
    //}

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
        public int filler;
    }

    public static class MoveHelper {
        public static Move CreateMove(int targetPosition, int fromPosition, Piece capturedPiece, Piece promotion, MoveFlags moveFlags, BoardState board) {
            return new Move() {
                targetPosition = (byte) targetPosition,
                fromPosition = (byte) fromPosition,
                capturedPiece = (byte) capturedPiece,
                moveFlags = (byte)moveFlags,
                promotion = (byte) promotion,

                previousCastlingBits =(byte) board.CastlingBits,
                previousEnpassant = (byte) board.EnPassantTarget,
                previousHalfMove = (byte) board.HalfTurnCounter
            };
            //Move move = bits;
            //move |= (Move) targetPosition;
            //move |= (Move)(fromPosition << (int)Move.FROM_POS_BIT_OFFSET);
            //move |= (Move)((int)capturedPiece << (int)Move.CAPTURED_BIT_OFFSET);
            //move |= (Move)((int)promotion << (int)Move.PROMOTION_BIT_OFFSET);
            ////move |= (Move)(enpassant ? 1 : 0 << (int)Move.ENPASSANT_BIT_OFFSET);
            ////move |= (Move)(bigPawnMove ? 1 : 0 << (int)Move.BIG_PAWN_MOVE_BIT_OFFSET);
            //move |= (Move)(castlingBits << (int)Move.PREVIOUS_CASTLING_BITS_OFFSET);
            //return move;
        }

        //public static int MoveTargetPos(Move move) {
        //    return (int) (move & Move.TARGET_POS_MASK);
        //}

        //public static int MoveFromPos(Move move) {
        //    return ((int)(move & Move.FROM_POS_MASK)) >> (int) Move.FROM_POS_BIT_OFFSET;
        //}

        //public static Piece MoveCaptured(Move move) {
        //    return (Piece) (((int)(move & Move.CAPTURED_MASK)) >> (int)Move.CAPTURED_BIT_OFFSET);
        //}
        //public static Piece MovePromotion(Move move) {
        //    return (Piece)(((int)(move & Move.PROMOTION_MASK)) >> (int)Move.PROMOTION_BIT_OFFSET);
        //}

        //public static bool MoveIsEnpassant(Move move) {
        //    return (move & Move.ENPASSANT) == Move.ENPASSANT;
        //}

        //public static bool MoveBigPawn(Move move) {
        //    return (move & Move.BIG_PAWN_MOVE) == Move.BIG_PAWN_MOVE;
        //}

        //public static int MovePreviousCastlingBits(Move move) {
        //    return (int)(move & Move.PREVIOUS_CASTLING_BITS_MASK) >> (int)Move.PREVIOUS_CASTLING_BITS_OFFSET;
        //}
        public static bool isValidMove(Move move) {
            return move.targetPosition != move.fromPosition;
        }

        public static Move FindTargetPosition(this List<Move> moves, int position) {
            return moves.Find(move => {
                int targetPosition = move.targetPosition;
                return position == targetPosition;
            });
            //return moves.Find(move => MoveTargetPos(move) == position);
        }

        public static Move FindTargetPosition(this List<Move> moves, int position, Piece promotion) {
            return moves.Find(move => {
                int targetPosition = move.targetPosition;
                if(position == targetPosition) {
                    return promotion == Piece.EMPTY || promotion == (Piece)move.promotion;
                }
                return false;
            });
            //return moves.Find(move => MoveTargetPos(move) == position);
        }

        public static string ReadableMove(Move move) {
            var fromPosition = move.fromPosition;
            var toPosition = move.targetPosition;
            return $"from: {Board.ReadablePosition(fromPosition)} to: {Board.ReadablePosition(toPosition)}";
        }
    }



}
