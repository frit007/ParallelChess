using ParallelChess;
using ParallelChess.MinMax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChessApi.Services {
    public class ChessService {
        public (int row,int column) convertx88ToArrayPosition(int x88Position) {
            int row = x88Position / BoardStateOffset.ROW_OFFSET;
            int column = x88Position - (row * BoardStateOffset.ROW_OFFSET);

            return (row, column);
        }

        public ChessState ChessToState(Chess game) {
            var chessState = new ChessState();
            //var moves = board.GetMoves().Where(move => board.IsLegalMove(move));
            var moves = game.getMoves();

            for (int column = 0; column < 8; column++) {
                for (int row = 0; row < 8 * BoardStateOffset.ROW_OFFSET; row += BoardStateOffset.ROW_OFFSET) {
                    var position = row + column;
                    Piece piece = game.board.GetPiece(position);
                    if(piece != Piece.EMPTY) {
                        (int fromRow, int fromColumn) = convertx88ToArrayPosition(position);
                        var chessPiece = new ChessPiece() {
                            piece = PieceParser.ToChar(piece).ToString(),
                            row = fromRow,
                            column = fromColumn,
                            isWhite = (piece & Piece.IS_WHITE) == Piece.IS_WHITE
                        };
                        
                        chessState.pieces.Add(chessPiece);

                        foreach (var move in moves) {
                            if(move.move.fromPosition == position) {
                                (int toRow, int toColumn) = convertx88ToArrayPosition(move.move.targetPosition);
                                chessPiece.options.Add(new PieceOption() {
                                    row = toRow,
                                    column = toColumn,
                                    isCastle = ((MoveFlags)move.move.moveFlags & MoveFlags.CASTLING) == MoveFlags.CASTLING,
                                    isEnpassant =(((MoveFlags)move.move.moveFlags & MoveFlags.ENPASSANT) == MoveFlags.ENPASSANT),
                                    san = move.san
                                });
                            }
                        }
                    }
                }
            }
            var winner = game.Winner();
            if(winner == Winner.DRAW) {
                chessState.isDraw = true;
            } else if(winner == Winner.WINNER_BLACK) {
                chessState.blackWins = true;
            } else if(winner == Winner.WINNER_WHITE) {
                chessState.whiteWins = true;
            }

            chessState.fen = game.FEN;

            return chessState;
        }

        public Chess ReplayMoves(List<ChessApi.Models.Move> moves) {
            return ReplayMoves(moves.Select(move => move.SAN).ToList());
        }

        public Chess ReplayMoves(List<string> san) {
            Chess game = Chess.StartGame();

            foreach (var move in san) {
               game.Move(move);
            }

            return game;
        }

        public async Task<Move> GetAiMove(Board board) {
            var ai = new AIWorkerManager();

            ai.spawnWorkers(3);

            var cheatMoves = await ai.analyzeBoard(board, 5, new Stack<Move>());

            var move = ai.GetBestMove();

            return move.move;
        }
    }
}
