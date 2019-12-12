using System;
using System.Collections.Generic;
using System.Text;

namespace ParallelChess {




    [Flags]
    public enum Move {
        // bits dedicated to a specific purpose each letter is a bit
        // EEEh hhhh huuu uecc  cppp ffff fftt tttt
        // legend: 
        // f = from position
        // t = to position
        // c = captured piece (0: Empty, 1: PAWN, 2: ROOK, 3: KNIGHT, 4:BISHOP, 5:KING, 6:QUEEN)
        // p = promotion (0:EMPTY, 1:ROOK, 2:KNIGHT, 3:BISHOP, 4:QUEEN)
        // e = enpassant
        // u = previous castle bits
        // h = half clock
        // E = previous enpassant target

        // removed
        // a = castling // could be removed
        // b = big move (pawn makes a big move open oportunity for enpassant) // can be removed
        // l = pawn move

        // ----------- TARGET POSITION ----------
        TARGET_POS_MASK             = 0b0000_0000_0000_0000__0000_0000_0011_1111,
        TARGET_BIT_OFFSET = 0,

        // ----------- FROM POSITION ----------
        FROM_POS_MASK               = 0b0000_0000_0000_0000__0000_1111_1100_0000,
        FROM_POS_BIT_OFFSET = 6,

        // ----------- PROMOTION PIECE ----------
        PROMOTION_MASK              = 0b0000_0000_0000_0000__0111_0000_0000_0000,
        PROMOTION_BIT_OFFSET = 12,

        // ----------- CAPTURED PIECE ----------
        CAPTURED_MASK               = 0b0000_0000_0000_0011__1000_0000_0000_0000,
        CAPTURED_BIT_OFFSET = 15,

        // ----------- ENPASANT BOOL ----------
        ENPASSANT                   = 0b0000_0000_0000_0100__0000_0000_0000_0000,
        ENPASSANT_BIT_OFFSET = 18,

        // ---------- PREVIOUS CASTLING BITS ----------
        PREVIOUS_CASTLING_BITS_MASK = 0b0000_0000_0111_1000__0000_0000_0000_0000,
        PREVIOUS_CASTLING_BITS_OFFSET = 19,

        HALF_TURN_COUNTER_MASK      = 0b0001_1111_1000_0000__0000_0000_0000_0000,
        HALF_TURN_COUNTER_OFFSET = 23,


        // supposed to be 0b0110_0000_0000_0000__0000_0000_0000_0000
        // but that throws an error since a enum is a integer and the first bit is used for negative numbers
        PREVIOUS_ENPASSANT_TARGET_MASK = -536870912,
        PREVIOUS_ENPASSANT_TARGET_OFFSET = 29,

        //// ----------- BIG PAWN MOVE BOOL ----------
        //BIG_PAWN_MOVE = 0b0000_0000_1000_0000__0000_0000_0000_0000,
        //BIG_PAWN_MOVE_BIT_OFFSET = 23,

        // ---------- CASTLING ----------
        //CASTLING = 0b0000_0001_0000_0000__0000_0000_0000_0000,
        //CASTLING_BIT_OFFSET = 24,

        //// ---------- PAWN MOVE ----------
        //PAWN_MOVE      = 0b0000_0000_0000_1000__0000_0000_0000_0000,
        //PAWN_BIT_OFFSET = 19,

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
            Piece.EMPTY,
            Piece.ROOK,
            Piece.KNIGHT,
            Piece.BISHOP,
            Piece.QUEEN,
        };
        public static byte PieceToCompactedPromotion(Piece piece) {
            switch (piece & Piece.PIECE_MASK) {
                case Piece.ROOK:
                    return 1;
                case Piece.KNIGHT:
                    return 2;
                case Piece.BISHOP:
                    return 3;
                case Piece.QUEEN:
                    return 4;
                default:
                    return 0; // default to no promotion
            }
        }

        // the goal is to compact a 7 bit position down to 3 bit by only pointing at the position in the row
        // 0 means that there is noEnpassant
        // Since we only have 3 bits we can only represent 8 values 0 - 7
        // this would be fine if there always was a enpassant target but since we also need a value that indicates no target
        // we need to do something fancy
        // So what we are going to do is count pawns on the players 4th row from the left.
        // in the rare situation that the player has all their pawns on the 4th row and enpassant is on the 8th column this will technically return the wrong answer
        // but since there is no pawn to take advantage of it on the 7th column it does not matter
        public static int compactEnpassantTarget(BoardState board) {
            int isWhiteTurn = board.IsWhiteTurn;
            int enPassantTarget = board.EnPassantTarget;
            if(board.EnPassantTarget == EnPassant.NO_ENPASSANT) {
                // start with a no enpassant check since that is the most likely situation
                return 0;
            }
            Piece myPawn = Piece.PAWN | (Piece)isWhiteTurn;
            // pawnPosition is A5 when player is black and a4 when player is white
            int pawnPosition = BoardStateOffset.A5 - BoardStateOffset.ROW_OFFSET * isWhiteTurn;
            // relativeRow is +1 row when the player is black and -1 row when the player is white
            int relativeRow = BoardStateOffset.ROW_OFFSET - BoardStateOffset.ROW_OFFSET * 2 * isWhiteTurn;
            int position = 1;
            for (int i = 0; i < 8; i++) {
                if(pawnPosition + i + relativeRow == enPassantTarget) {
                    return position;
                }

                if(board.GetPiece(pawnPosition + i) == myPawn) {
                    position++;
                }
            }
            return 0;
        }

        // the goal is to compact a 7 bit position down to 3 bit by only pointing at the position in the row
        // 0 means that there is noEnpassant
        // Since we only have 3 bits we can only represent 8 values 0 - 7
        // this would be fine if there always was a enpassant target but since we also need a value that indicates no target
        // we need to do something fancy
        // So what we are going to do is count pawns on the players 4th row from the left.
        // in the rare situation that the player has all their pawns on the 4th row and enpassant is on the 8th column this will technically return the wrong answer
        // but since there is no pawn to take advantage of it on the 7th column it does not matter
        public static int decompactEnpassantTarget(BoardState board, int compactedEnpassant) {
            int isWhiteTurn = board.IsWhiteTurn;
            if (compactedEnpassant == 0) {
                // start with a no enpassant check since that is the most likely situation
                return EnPassant.NO_ENPASSANT;
            }
            Piece myPawn = Piece.PAWN | (Piece)isWhiteTurn;
            // pawnPosition is A5 when player is black and a4 when player is white
            int pawnPosition = BoardStateOffset.A5 - BoardStateOffset.ROW_OFFSET * isWhiteTurn;
            // relativeRow is +1 row when the player is black and -1 row when the player is white
            int relativeRow = BoardStateOffset.ROW_OFFSET - BoardStateOffset.ROW_OFFSET * 2 * isWhiteTurn;
            int position = 0;
            for (int i = 0; i < 8; i++) {
                if (board.GetPiece(pawnPosition + i) == myPawn) {
                    position++;
                }

                if (compactedEnpassant == position) {
                    return pawnPosition + relativeRow + i;
                }
            }
            //return EnPassant.NO_ENPASSANT;
            throw new Exception($"Illegal state, unable to find compacted position {compactedEnpassant}");
        }


        public static int compactPosition(int position) {
            int row = position / 16;
            int column = position - row * 16;
            return column + row * 8;
        }
        public static int unCompactPosition(int position) {
            int row = position / 8;
            int column = position - row * 8;
            return column + row * 16;
        }

        public static Move CreateMove(
            int targetPosition, 
            int fromPosition, 
            Piece capturedPiece, 
            Piece promotion,
            Move moveFlags, 
            BoardState board
            ) {

            int move = compactPosition(targetPosition);
            move |= compactPosition(fromPosition) << (int)Move.FROM_POS_BIT_OFFSET;
            move|= PieceToCompactedCapture(capturedPiece) << (int)Move.CAPTURED_BIT_OFFSET;
            move|= PieceToCompactedPromotion(promotion) << (int)Move.PROMOTION_BIT_OFFSET;
            move|= (int)moveFlags;

            // history
            move|= (int)board.CastlingBits << (int)Move.PREVIOUS_CASTLING_BITS_OFFSET;
            move|= board.HalfTurnCounter << (int)Move.HALF_TURN_COUNTER_OFFSET;
            move|= compactEnpassantTarget(board) << (int)Move.PREVIOUS_ENPASSANT_TARGET_OFFSET;
            return (Move)move;
            //return (Move)(
            //    compactPosition(targetPosition)
            //    | compactPosition(fromPosition) << (int)Move.FROM_POS_BIT_OFFSET
            //    | PieceToCompactedCapture(capturedPiece) << (int) Move.CAPTURED_BIT_OFFSET
            //    | PieceToCompactedPromotion(promotion) << (int) Move.PROMOTION_BIT_OFFSET
            //    | (int) moveFlags

            //    // history
            //    | (int)board.CastlingBits << (int) Move.PREVIOUS_CASTLING_BITS_OFFSET
            //    | board.HalfTurnCounter << (int) Move.HALF_TURN_COUNTER_OFFSET
            //    | compactEnpassantTarget(board) << (int) Move.PREVIOUS_ENPASSANT_TARGET_OFFSET
            //    );
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
            return unCompactPosition((int)(move & Move.TARGET_POS_MASK));
        }

        public static int MoveFromPos(Move move) {
            return unCompactPosition((int)(move & Move.FROM_POS_MASK) >> (int)Move.FROM_POS_BIT_OFFSET);
        }

        public static Piece MoveCaptured(Move move, int color) {
            int captured = (int)(move & Move.CAPTURED_MASK) >> (int)Move.CAPTURED_BIT_OFFSET;
            Piece piece = CompactedCapturedMap[captured];
            if(piece == Piece.EMPTY) {
                return Piece.EMPTY;
            }
            return  piece | (Piece)color;
        }
        public static Piece MovePromotion(Move move, int color) {
            int promotion = (int)(move & Move.PROMOTION_MASK) >> (int)Move.PROMOTION_BIT_OFFSET;
            Piece piece = CompactedPromotionMap[promotion];
            if(piece == Piece.EMPTY) {
                return Piece.EMPTY;
            }
            return  piece | (Piece)color;
        }

        public static bool MoveIsNotEmpty(Move move) {
            return MoveTargetPos(move) != MoveFromPos(move);
        }

        public static bool MoveIsEnpassant(Move move) {
            return (move & Move.ENPASSANT) == Move.ENPASSANT;
        }

        public static bool MoveIsBigPawn(int fromPosition, int toPosition) {
            // check if the piece moved 2 rows
            return Math.Abs(fromPosition - toPosition) == 2 * BoardStateOffset.ROW_OFFSET;
        }
        

        public static bool MoveIsCastle(int fromPosition, int toPosition) {
            return Math.Abs(fromPosition - toPosition) == 2;
        }

        public static CastlingBits MovePreviousCastlingBits(Move move) {
            return (CastlingBits)((int)(move & Move.PREVIOUS_CASTLING_BITS_MASK) >> (int)Move.PREVIOUS_CASTLING_BITS_OFFSET);
        }
        public static int PreviousHalfMove(Move move) {
            return (int)( move & Move.HALF_TURN_COUNTER_MASK) >> (int)Move.HALF_TURN_COUNTER_OFFSET;
        }

        public static Move FindTargetPosition(this List<Move> moves, int position, Piece promotion = Piece.EMPTY) {
            return moves.Find(move => {
                int targetPosition = MoveTargetPos(move);
                if(position == targetPosition) {
                    return promotion == Piece.EMPTY || promotion == MovePromotion(move, (int)(promotion & Piece.IS_WHITE));
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
