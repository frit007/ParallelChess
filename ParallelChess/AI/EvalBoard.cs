using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParallelChess.AI {

    public struct AttackedField {
        public byte myOptions;
        public byte theirOptions;
    }

    // we mark the functions as unsafe because I want to stackAlloc a array since we only need it for the function duration
    public unsafe static class EvalBoard {
        [ThreadStatic]
        private static List<Move> moves = new List<Move>();

        // For faster lookup this contains all piece values no matter which color it is
        private static float[] PieceValues = new float[(int)Piece.PIECE_MASK+1];

        private static float HANGING_BONUS_MULTIPLIER = 0.33f;
        private static float WEAK_SQUARE_BONUS = 0.3f;
        private static float ATTACK_PIECE_MULTIPLIER = 0.2f;
        private static float MOVE_BONUS = 0.1f;
        // count moves that are not valid meaning they are likely to be valid in the future
        // this means a well positionend queen is usefull even though it is pinned right now. Or atleast that is the intention.
        private static float PSEUDO_VALID_MOVE_PENALTY = 0.1f;

        static EvalBoard() {
            for (int i = 0; i <= (int)Piece.PIECE_MASK; i++) {
                var piece = (Piece)i;
                switch (piece & Piece.PIECE_MASK) {
                    case Piece.PAWN:
                        PieceValues[i] = 1;
                        break;
                    case Piece.KNIGHT:
                        PieceValues[i] = 3;
                        break;
                    case Piece.KING:
                        PieceValues[i] = 20;
                        break;
                    case Piece.ROOK:
                        PieceValues[i] = 5;
                        break;
                    case Piece.BISHOP:
                        PieceValues[i] = 3;
                        break;
                    case Piece.QUEEN:
                        PieceValues[i] = 9;
                        break;
                    default:
                        PieceValues[i] = 0;
                        break;
                }
            }
        }

        public static void initThreadStaticVariables() {
            moves = new List<Move>();
        }

        // positive scores are in our favor negative scores are in their favor
        public static float evalBoard(BoardState board, List<Move> myMoves) {
            float score = 0;
            //var original = Board.CreateCopyBoard(board);
            score += countPieceValues(board);

            List<Move> myValidMoves = new List<Move>();
            foreach (var ourMove in myMoves) {
                if(Board.IsLegalMove(board, ourMove)) {
                    myValidMoves.Add(ourMove);
                }
            }

            // flip the board positions to check for legal moves
            board.IsWhiteTurn ^= 1;
            moves.Clear();
            var theirMoves = Board.GetMoves(board, moves);
            List<Move> theirValidMoves = new List<Move>();
            foreach (var move in theirMoves) {
                if (Board.IsLegalMove(board, move)) {
                    theirValidMoves.Add(move);
                }
            }
            score += countAttackedFields(board, myMoves, theirMoves) * PSEUDO_VALID_MOVE_PENALTY;
            //var theirValidMoves = theirMoves.Where(move => Board.IsValidMove(board, move)).ToList();
            // NOTE: the score has to be counted before switching the turns back to normal, 
            // because otherwise the the linq statement will not evaluate with the correct state.
            score += countAttackedFields(board, myValidMoves, theirValidMoves);
            board.IsWhiteTurn ^= 1;


            return score;
        }


        public static unsafe float countAttackedFields(BoardState board, IEnumerable<Move> myMoves, IEnumerable<Move> theirMoves) {
            float score = 0;

            AttackedField* attackedFields = stackalloc AttackedField[BoardStateOffset.BOARD_STATE_SIZE];

            foreach (var move in myMoves) {
                attackedFields[move.targetPosition].myOptions++;
            }

            foreach (var move in theirMoves) {
                attackedFields[move.targetPosition].theirOptions++;
            }

            for (int row = 0; row < 8 * BoardStateOffset.ROW_OFFSET; row += BoardStateOffset.ROW_OFFSET) {
                for (int column = 0; column < 8; column++) {
                    int position = column + row;
                    var attackedField = attackedFields[column + row];
                    var piece = board.GetPiece(position);
                    byte pieceColor = (byte)(piece & Piece.IS_WHITE);
                    var pieceValue = PieceValues[(int)piece];

                    if (attackedField.theirOptions == 0 && attackedField.myOptions != 0) {
                        // the piece is unprotected (hanging)
                        score += HANGING_BONUS_MULTIPLIER * pieceValue;
                        score += WEAK_SQUARE_BONUS;
                    } else if (attackedField.myOptions == 0 && attackedField.theirOptions != 0) {
                        score -= HANGING_BONUS_MULTIPLIER * pieceValue;
                        score -= WEAK_SQUARE_BONUS;
                    }

                    score += (attackedField.myOptions - attackedField.theirOptions) * ATTACK_PIECE_MULTIPLIER;
                    // bonus just for being a valid move
                    score += (attackedField.myOptions - attackedField.theirOptions) * MOVE_BONUS;
                }
            }

                    return score;
        }

        public static float countPieceValues(BoardState board) {
            float score = 0;
            for (int row = 0; row < 8 * BoardStateOffset.ROW_OFFSET; row += BoardStateOffset.ROW_OFFSET) {
                for (int column = 0; column < 8; column++) {

                    Piece piece = board.GetPiece(column + row);
                    // 1 when piece belongs to us -1 when piece belongs to them;
                    float scoreMultiplier = (board.IsWhiteTurn == (int)(piece & Piece.IS_WHITE)) ? 1 : -1;
                    switch (piece & Piece.PIECE_MASK) {
                        case Piece.PAWN:
                            score += 1 * scoreMultiplier;
                            break;
                        case Piece.KNIGHT:
                            score += 3 * scoreMultiplier;
                            break;
                        case Piece.ROOK:
                            score += 5 * scoreMultiplier;
                            break;
                        case Piece.BISHOP:
                            score += 3 * scoreMultiplier;
                            break;
                        case Piece.QUEEN:
                            score += 9 * scoreMultiplier;
                            break;
                    }
                }
            }
            return score;
        }
    }
}
