using System;
using System.Collections.Generic;
using System.Text;

namespace ParallelChess {

    public class PieceOption {
        public int column { get; set; }
        public int row { get; set; }
        public bool isCastle { get; set; }
        public bool isEnpassant { get; set; }
        // use san(standard algebraic notation) to uniquely identify a move
        public string san { get; set; }
    }

    public class ChessPiece {
        public string piece { get; set; }
        public List<PieceOption> options { get; set; } = new List<PieceOption>();
        public int column { get; set; }
        public int row { get; set; }
        public bool isWhite { get; set; }
    }

    public class ChessState {
        public List<ChessPiece> pieces { get; set; } = new List<ChessPiece>();

        public bool blackWins { get; set; } = false;
        public bool isDraw { get; set; } = false;
        public bool whiteWins { get; set; } = false;

        public string fen { get; set; } = "";
    }
    public static class CoordinateOutput {
        public static ChessState GameToState(ChessGame game) {
            var chessState = new ChessState();
            //var moves = board.GetMoves().Where(move => board.IsLegalMove(move));
            var moves = game.Moves();

            for (int column = 0; column < 8; column++) {
                for (int row = 0; row < 8 * BoardStateOffset.ROW_OFFSET; row += BoardStateOffset.ROW_OFFSET) {
                    var position = row + column;
                    Piece piece = game.board.GetPiece(position);
                    if (piece != Piece.EMPTY) {
                        var from = BoardPosition.x88PositionToCoordinate(position);
                        var chessPiece = new ChessPiece() {
                            piece = PieceParser.ToChar(piece).ToString(),
                            row = from.row,
                            column = from.column,
                            isWhite = (piece & Piece.IS_WHITE) == Piece.IS_WHITE
                        };

                        chessState.pieces.Add(chessPiece);

                        foreach (var move in moves) {
                            if (move.move.fromPosition == position) {
                                var to  = BoardPosition.x88PositionToCoordinate(move.move.targetPosition);
                                chessPiece.options.Add(new PieceOption() {
                                    row = to.row,
                                    column = to.column,
                                    isCastle = ((MoveFlags)move.move.moveFlags & MoveFlags.CASTLING) == MoveFlags.CASTLING,
                                    isEnpassant = (((MoveFlags)move.move.moveFlags & MoveFlags.ENPASSANT) == MoveFlags.ENPASSANT),
                                    san = move.san
                                });
                            }
                        }
                    }
                }
            }
            var winner = game.Winner();
            if (winner == Winner.DRAW) {
                chessState.isDraw = true;
            } else if (winner == Winner.WINNER_BLACK) {
                chessState.blackWins = true;
            } else if (winner == Winner.WINNER_WHITE) {
                chessState.whiteWins = true;
            }

            chessState.fen = game.FEN;

            return chessState;
        }
    }
}
