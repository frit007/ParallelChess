using System;
using System.Collections.Generic;
using System.Text;

namespace ParallelChess {
    public static class MoveHelper {
        public static Move CreateMove(int targetPosition, int fromPosition, Piece capturedPiece, Piece promotion, MoveFlags moveFlags, Board board) {
            //return new Move() {
            //    targetPosition = (byte)targetPosition,
            //    fromPosition = (byte)fromPosition,
            //    capturedPiece = (byte)capturedPiece,
            //    moveFlags = (byte)moveFlags,
            //    promotion = (byte)promotion,

            //    previousCastlingBits = (byte)board.CastlingBits,
            //    previousEnpassant = (byte)board.EnPassantTarget,
            //    previousHalfMove = (byte)board.HalfTurnCounter
            //};

            return new Move(
    (byte)targetPosition,
    (byte)fromPosition,
    (byte)capturedPiece,
    (byte)promotion,
    (byte)moveFlags,

    (byte)board.CastlingBits,
    (byte)board.EnPassantTarget,
    (byte)board.HalfTurnCounter
    );
        }
        public static bool isValidMove(Move move) {
            return move.targetPosition != move.fromPosition;
        }

        public static Move FindTargetPosition(this List<Move> moves, int position) {
            return moves.Find(move => {
                int targetPosition = move.targetPosition;
                return position == targetPosition;
            });
        }

        public static Move FindTargetPosition(this List<Move> moves, int fromPosition, int toPosition) {
            return moves.Find(move => {
                int targetPosition = move.targetPosition;
                return toPosition == targetPosition && move.fromPosition == fromPosition;
            });
        }



        public static Move FindTargetPosition(this List<Move> moves, int position, Piece promotion) {
            return moves.Find(move => {
                int targetPosition = move.targetPosition;
                if (position == targetPosition) {
                    return promotion == Piece.EMPTY || promotion == (Piece)move.promotion;
                }
                return false;
            });
        }

        public static string ReadableMove(Move move) {
            var fromPosition = move.fromPosition;
            var toPosition = move.targetPosition;
            return $"from: {BoardPosition.ReadablePosition(fromPosition)} to: {BoardPosition.ReadablePosition(toPosition)}";
        }


    }
}
