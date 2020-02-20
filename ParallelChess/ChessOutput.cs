using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParallelChess {
    public static class ChessOutput {

        public static String AsciiBoard(Chess game, List<Move> moves = null, bool displayCount = false) {

            return AsciiBoard(game.board, moves, displayCount);
        }
        public static String AsciiBoard(Board board, List<Move> moves = null, bool displayCount = false) {
            if (moves == null) {
                moves = new List<Move>();
            }
            moves = moves.Where(move => board.IsLegalMove(move)).ToList();

            StringBuilder ascii = new StringBuilder();

            ascii.Append("+---------------+\n");
            for (int row = 7; row >= 0; row--) {
                ascii.Append("|");
                for (int column = 0; column < 8; column++) {
                    var position = row * BoardStateOffset.ROW_OFFSET + column;
                    var piece = board.GetPiece(position);
                    var movesOnPosition = moves.Count(move => move.targetPosition == position);
                    if (movesOnPosition > 0) {
                        if (movesOnPosition < 10 && displayCount) {
                            ascii.Append(movesOnPosition);
                        } else {
                            ascii.Append("x");
                        }
                    } else {
                        ascii.Append(PieceParser.ToChar(piece));
                    }
                    if (column != 7) {
                        ascii.Append(" ");
                    }
                }

                ascii.Append($"| {(row + 1)}\n");
            }
            ascii.Append("+---------------+\n");

            ascii.Append(" A B C D E F G H");
            return ascii.ToString();
        }


        public static String BoardToFen(Board board) {
            StringBuilder fen = new StringBuilder();

            for (int row = 7; row >= 0; row--) {
                int count = 0;
                for (int column = 0; column < 8; column++) {
                    var position = row * BoardStateOffset.ROW_OFFSET + column;
                    var piece = board.GetPiece(position);
                    //var piece = Board.GetPiece(board, position);
                    char c = PieceParser.ToChar(piece);
                    if (c == '_') {
                        count++;
                    } else {
                        if (count != 0) {
                            fen.Append(count);
                            count = 0;
                        }
                        fen.Append(c);
                    }
                }
                if (count != 0) {
                    fen.Append(count);
                }

                if (row != 0) {
                    fen.Append("/");
                }
            }
            fen.Append(" ");
            fen.Append(board.IsWhiteTurnBool ? "w" : "b");
            fen.Append(" ");
            CastlingBits castlingBits = board.CastlingBits;
            if (castlingBits == CastlingBits.EMPTY) {
                fen.Append("-");
            } else {
                if ((castlingBits & CastlingBits.WHITE_KING_SIDE_CASTLE) != CastlingBits.EMPTY) {
                    fen.Append("K");
                }
                if ((castlingBits & CastlingBits.WHITE_QUEEN_SIDE_CASTLE) != CastlingBits.EMPTY) {
                    fen.Append("Q");
                }
                if ((castlingBits & CastlingBits.BLACK_KING_SIDE_CASTLE) != CastlingBits.EMPTY) {
                    fen.Append("k");
                }
                if ((castlingBits & CastlingBits.BLACK_QUEEN_SIDE_CASTLE) != CastlingBits.EMPTY) {
                    fen.Append("q");
                }
            }
            fen.Append(" ");
            fen.Append(board.EnPassantTarget != EnPassant.NO_ENPASSANT ? BoardPosition.ReadablePosition(board.EnPassantTarget) : "-");
            fen.Append(" ");
            fen.Append(board.HalfTurnCounter);
            fen.Append(" ");
            fen.Append(board.TurnCounter);

            return fen.ToString();
        }
    }
}
