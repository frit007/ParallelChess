using System;
using System.Collections.Generic;
using System.Text;

namespace ParallelChess {




    [Flags]
    public enum Move {
        // bits dedicated to a specific purpose each letter is a bit
        // hhhh hhuu uuab eccc  ppff ffff fttt tttt
        // legend: 
        // f = from position
        // t = to position
        // c = captured piece (0: Empty, 1: PAWN, 2: ROOK, 3: KNIGHT, 4:BISHOP, 5:KING, 6:QUEEN)
        // p = promotion (0:ROOK, 1:KNIGHT, 2:BISHOP, 3:QUEEN)
        // e = enpassant
        // b = big move (pawn makes a big move open oportunity for enpassant)
        // a = castling
        // u = previous castle bits
        // h = half clock




        // 0000 00uu uula bepp  ccff ffff fttt tttt

        // hhhh hhuu uuab eccc  ppff ffff fttt tttt

        // ----------- TARGET POSITION ----------
        //TARGET_POS_START = 0b0000_0000_0000_0000__0000_0000_0000_0000,
        TARGET_POS_MASK = 0b0000_0000_0000_0000__0000_0000_0111_1111,
        TARGET_BIT_OFFSET = 0,

        // ----------- FROM POSITION ----------
        //FROM_POS_START = 0b0000_0000_0000_0000__0000_0000_1000_0000,
        FROM_POS_MASK = 0b0000_0000_0000_0000__0011_1111_1000_0000,
        FROM_POS_BIT_OFFSET = 7,

        // ----------- CAPTURED PIECE ----------
        //CAPTURED_START = 0b0000_0000_0000_0000__0100_0000_0000_0000,
        CAPTURED_MASK = 0b0000_0000_0000_0011__1100_0000_0000_0000,
        CAPTURED_BIT_OFFSET = 14,

        // ----------- PROMOTION PIECE ----------
        //PROMOTION_START = 0b0000_0000_0000_0100__0000_0000_0000_0000,
        PROMOTION_MASK = 0b0000_0000_0011_1100__0000_0000_0000_0000,
        PROMOTION_BIT_OFFSET = 18,

        // ----------- ENPASANT BOOL ----------
        ENPASSANT = 0b0000_0000_0100_0000__0000_0000_0000_0000,
        ENPASSANT_BIT_OFFSET = 22,

        // ----------- BIG PAWN MOVE BOOL ----------
        BIG_PAWN_MOVE = 0b0000_0000_1000_0000__0000_0000_0000_0000,
        BIG_PAWN_MOVE_BIT_OFFSET = 23,

        // ---------- CASTLING ----------
        CASTLING = 0b0000_0001_0000_0000__0000_0000_0000_0000,
        CASTLING_BIT_OFFSET = 24,

        // ---------- PAWN MOVE ----------
        PAWN_MOVE = 0b0000_0010_0000_0000__0000_0000_0000_0000,
        PAWN_BIT_OFFSET = 25,

        // ---------- PREVIOUS CASTLING BITS ----------
        //PREVIOUS_CASTLING_BITS_START = 0b0000_0100_0000_0000__0000_0000_0000_0000,
        PREVIOUS_CASTLING_BITS_MASK = 0b0011_1100_0000_0000__0000_0000_0000_0000,
        PREVIOUS_CASTLING_BITS_OFFSET = 26,

        HALF_TURN_COUNTER_MASK = 1,
        HALF_TURN_COUNTER_OFFSET = 26,


        PREVIOUS_ENPASSANT_TARGET_MASK = 777,
        PREVIOUS_ENPASSANT_TARGET_OFFSET = 777,

        EMPTY = 0,
    }


    //public static class MoveHelper {
    //    public static Move CreateMove(int targetPosition, int fromPosition, Piece capturedPiece, Piece promotion, Move bits, byte castlingBits) {
    //        Move move = bits;
    //        move |= (Move)targetPosition;
    //        move |= (Move)(fromPosition << (int)Move.FROM_POS_BIT_OFFSET);
    //        move |= (Move)((int)capturedPiece << (int)Move.CAPTURED_BIT_OFFSET);
    //        move |= (Move)((int)promotion << (int)Move.PROMOTION_BIT_OFFSET);
    //        //move |= (Move)(enpassant ? 1 : 0 << (int)Move.ENPASSANT_BIT_OFFSET);
    //        //move |= (Move)(bigPawnMove ? 1 : 0 << (int)Move.BIG_PAWN_MOVE_BIT_OFFSET);
    //        move |= (Move)(castlingBits << (int)Move.PREVIOUS_CASTLING_BITS_OFFSET);
    //        return move;
    //    }

    //    public static int MoveTargetPos(Move move) {
    //        return (int)(move & Move.TARGET_POS_MASK);
    //    }

    //    public static int MoveFromPos(Move move) {
    //        return ((int)(move & Move.FROM_POS_MASK)) >> (int)Move.FROM_POS_BIT_OFFSET;
    //    }

    //    public static Piece MoveCaptured(Move move) {
    //        return (Piece)(((int)(move & Move.CAPTURED_MASK)) >> (int)Move.CAPTURED_BIT_OFFSET);
    //    }
    //    public static Piece MovePromotion(Move move) {
    //        return (Piece)(((int)(move & Move.PROMOTION_MASK)) >> (int)Move.PROMOTION_BIT_OFFSET);
    //    }

    //    public static bool MoveIsEnpassant(Move move) {
    //        return (move & Move.ENPASSANT) == Move.ENPASSANT;
    //    }

    //    public static bool MoveBigPawn(Move move) {
    //        return (move & Move.BIG_PAWN_MOVE) == Move.BIG_PAWN_MOVE;
    //    }

    //    public static int MovePreviousCastlingBits(Move move) {
    //        return (int)(move & Move.PREVIOUS_CASTLING_BITS_MASK) >> (int)Move.PREVIOUS_CASTLING_BITS_OFFSET;
    //    }

    //    public static Move FindTargetPosition(this List<Move> moves, int position) {
    //        return moves.Find(move => {
    //            int targetPosition = MoveTargetPos(move);
    //            return position == targetPosition;
    //        });
    //        //return moves.Find(move => MoveTargetPos(move) == position);
    //    }

    //    public static string ReadableMove(Move move) {
    //        var fromPosition = MoveFromPos(move);
    //        var toPosition = MoveTargetPos(move);
    //        return $"from: {Board.ReadablePosition(fromPosition)} to: {Board.ReadablePosition(toPosition)}";
    //    }
    //}


    //[Flags]
    //public enum MoveFlags {
    //    ENPASSANT = 0b0001,
    //    CASTLING = 0b0010,
    //    //PAWN_MOVE = 0b0100,
    //    BIG_PAWN_MOVE = 0b1000,
    //    EMPTY = 0,
    //}

    //public class Move {
    //    public byte targetPosition;
    //    public byte fromPosition;
    //    public byte capturedPiece;
    //    public byte promotion;
    //    public byte moveFlags;

    //    // for undo
    //    public byte previousCastlingBits;
    //    public byte previousEnpassant;
    //    public byte previousHalfMove;
    //}


    public static class MoveHelper {

        public static Piece[] CompactedCapturedMap = new Piece[]{
            Piece.EMPTY,
            Piece.PAWN,
            Piece.ROOK,
            Piece.KNIGHT,
            Piece.BISHOP,
            Piece.KING,
            Piece.QUEEN,
        };

        public static byte PieceToCompactedCapture(Piece piece) {
            switch (piece & Piece.PIECE_MASK) {
                case Piece.PAWN:
                    return 1;
                case Piece.ROOK:
                    return 2;
                case Piece.KNIGHT:
                    return 3;
                case Piece.BISHOP:
                    return 4;
                case Piece.KING:
                    return 5;
                case Piece.QUEEN:
                    return 6;
                default:
                    return 0; // empty
            }
        }

        public static Piece[] CompactedPromotionMap = new Piece[]{
            Piece.ROOK,
            Piece.KNIGHT,
            Piece.BISHOP,
            Piece.QUEEN,
        };

        public static byte PieceToCompactedPromotion(Piece piece) {
            switch (piece & Piece.PIECE_MASK) {
                case Piece.ROOK:
                    return 0;
                case Piece.KNIGHT:
                    return 1;
                case Piece.BISHOP:
                    return 2;
                case Piece.QUEEN:
                    return 3;
                default:
                    throw new ArgumentException("You can only promote to Rook, Knight, Bishop or queen");
                    //return 3; // defalult to queen, because we have to do something
            }
        }
        public static Move CreateMove(
            int targetPosition, 
            int fromPosition, 
            Piece capturedPiece, 
            Piece promotion,
            Move moveFlags, 
            BoardState board
            ) {
            return (Move)(
                targetPosition
                | fromPosition << (int)Move.FROM_POS_BIT_OFFSET
                | PieceToCompactedCapture(capturedPiece) << (int) Move.CAPTURED_BIT_OFFSET
                | PieceToCompactedPromotion(promotion) << (int) Move.PROMOTION_BIT_OFFSET
                | (int) moveFlags

                // history
                | (int)board.CastlingBits << (int) Move.PREVIOUS_CASTLING_BITS_OFFSET
                | board.HalfTurnCounter << (int) Move.HALF_TURN_COUNTER_OFFSET
                | board.EnPassantTarget << (int) Move.PREVIOUS_ENPASSANT_TARGET
                );
            //return new Move() {
            //    targetPosition = (byte)targetPosition,
            //    fromPosition = (byte)fromPosition,
            //    capturedPiece = (byte)capturedPiece,
            //    moveFlags = (byte)moveFlags,
            //    promotion = (byte)promotion,

            //    previousCastlingBits = (byte) board.CastlingBits,
            //    previousEnpassant = (byte)board.EnPassantTarget,
            //    previousHalfMove = (byte) board.HalfTurnCounter,
            //};
        }

        public static int MoveTargetPos(Move move) {
            return (int)(move & Move.TARGET_POS_MASK);
        }

        public static int MoveFromPos(Move move) {
            return (int)(move & Move.FROM_POS_MASK) >> (int)Move.FROM_POS_BIT_OFFSET;
        }

        public static Piece MoveCaptured(Move move) {
            return CompactedCapturedMap[(int) (move & Move.CAPTURED_MASK) >> (int) Move.CAPTURED_BIT_OFFSET];
        }
        public static Piece MovePromotion(Move move) {
            return CompactedPromotionMap[(int)(move & Move.PROMOTION_MASK) >> (int)Move.PROMOTION_BIT_OFFSET];
        }

        public static bool MoveIsNotEmpty(Move move) {
            return MoveTargetPos(move) != MoveFromPos(move);
        }

        public static bool MoveIsEnpassant(Move move) {
            return (move & Move.ENPASSANT) == Move.ENPASSANT;
        }

        public static bool MoveBigPawn(Move move) {
            return (move & Move.BIG_PAWN_MOVE) == Move.BIG_PAWN_MOVE;
        }

        public static CastlingBits MovePreviousCastlingBits(Move move) {
            return (CastlingBits)((int)(move & Move.PREVIOUS_CASTLING_BITS_MASK) >> (int)Move.PREVIOUS_CASTLING_BITS_OFFSET);
        }
        public static int PreviousHalfMove(Move move) {
            return (int)( move & Move.PREVIOUS_CASTLING_BITS_MASK) >> (int)Move.PREVIOUS_CASTLING_BITS_OFFSET;
        }

        public static Move FindTargetPosition(this List<Move> moves, int position, Piece promotion = Piece.EMPTY) {
            return moves.Find(move => {
                int targetPosition = MoveTargetPos(move);
                if(position == targetPosition) {
                    return promotion == Piece.EMPTY || promotion == MovePromotion(move);
                }
                return false;
            });
            //return moves.Find(move => MoveTargetPos(move) == position);
        }

        public static string ReadableMove(Move move) {
            var fromPosition = MoveFromPos(move);
            var toPosition = MoveTargetPos(move);
            return $"from: {Board.ReadablePosition(fromPosition)} to: {Board.ReadablePosition(toPosition)}";
        }
    }

}
