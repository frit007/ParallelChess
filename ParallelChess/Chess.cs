using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParallelChess {





    public class EnPassant {
        public const byte NO_ENPASSANT = byte.MaxValue;
    }


    public class Chess {

        public static BoardState LoadBoardFromFen(String fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1") {
            BoardState board = new BoardState() {
                bytes = new byte[BoardStateOffset.BOARD_STATE_SIZE]
            };

            var sections = fen.Split(" ");

            if (sections.Length != 6) {
                throw new ArgumentException("FEN has to contanin 6 sections");
            }

            string positions = sections[0];
            string activeColor = sections[1];
            string castlingOptions = sections[2];
            string enPassantAttackedSquare = sections[3];
            string halfMoveClock = sections[4];
            string fullMoveClock = sections[5];

            int square = BoardStateOffset.A8;

            for (var i = 0; i < positions.Length; i++) {
                char piece = positions[i];
                if (piece == '/') {
                    square -= 24;
                } else if (Char.IsDigit(piece)) {
                    square += int.Parse(piece.ToString());
                } else {
                    Piece parsedPiece = PieceParse.FromChar(piece);
                    //Board.PutPiece(board, square, parsedPiece);
                    board.SetPiece(square, parsedPiece);
                    if((parsedPiece & Piece.PIECE_MASK) == Piece.KING) {
                        //Board.SetKingPosition(board, (parsedPiece & Piece.IS_WHITE) == Piece.IS_WHITE, square);
                        board.SetKingPosition((int)(parsedPiece & Piece.IS_WHITE), (byte)square);
                    }
                    square++;
                }
            }

            //Board.SetIsWhitesTurn(board, activeColor == "w");
            board.IsWhiteTurnBool = activeColor == "w";

            if (castlingOptions.Contains("K")) {
                //Board.SetCastleBit(board, CastlingBits.WHITE_KING_SIDE_CASTLE, true);
                board.CastlingBits |= CastlingBits.WHITE_KING_SIDE_CASTLE;
            }
            if (castlingOptions.Contains("Q")) {
                //Board.SetCastleBit(board, CastlingBits.WHITE_QUEEN_SIDE_CASTLE, true);
                board.CastlingBits |= CastlingBits.WHITE_QUEEN_SIDE_CASTLE;
            }
            if (castlingOptions.Contains("k")) {
                //Board.SetCastleBit(board, CastlingBits.BLACK_KING_SIDE_CASTLE, true);
                board.CastlingBits |= CastlingBits.BLACK_QUEEN_SIDE_CASTLE;
            }
            if (castlingOptions.Contains("q")) {
                //Board.SetCastleBit(board, CastlingBits.BLACK_QUEEN_SIDE_CASTLE, true);
                board.CastlingBits |= CastlingBits.BLACK_QUEEN_SIDE_CASTLE;
            }

            if (enPassantAttackedSquare == "-") {
                //Board.SetEnPassantAttackedSquare(board, EnPassant.NO_ENPASSANT);
                board.EnPassantTarget = EnPassant.NO_ENPASSANT;
            } else {
                //Board.SetEnPassantAttackedSquare(board, Board.AlgebraicPosition(enPassantAttackedSquare));
                board.EnPassantTarget = (byte) Board.AlgebraicPosition(enPassantAttackedSquare);
            }

            //Board.SetHalfTurnCounter(board, int.Parse(halfMoveClock));
            board.HalfTurnCounter = (byte) int.Parse(halfMoveClock);

            //Board.SetFullMoveClock(board, int.Parse(fullMoveClock));
            board.TurnCounter = (short) int.Parse(fullMoveClock);

            return board;
        }

        public static String AsciiBoard(BoardState board) {
            StringBuilder ascii = new StringBuilder();

            ascii.Append("+---------------+\n");
            for (int row = 7; row >= 0; row--) {
                ascii.Append("|");
                for (int column = 0; column < 8; column++) {
                    var position = row * BoardStateOffset.ROW_OFFSET + column;
                    var piece = board.GetPiece(position);
                    //var piece = Board.GetPiece(board, position);
                    ascii.Append(PieceParse.ToChar(piece));
                    if(column != 7) {
                        ascii.Append(" ");
                    }
                }

                ascii.Append($"| {(row + 1)}\n");
            }
            ascii.Append("+---------------+\n");

            ascii.Append(" A B C D E F G H");

            //for (int position = 0; position <= BoardOffset.H8; position++) {
            //    var piece = Board.GetPiece(board, position);

            //    if(position ) {

            //    }

            //    ascii += PieceParse.ToChar(piece);

            //}

            return ascii.ToString();
        }

        public static void MakeMove(BoardState board, int from, int to) {
            List<Move> moves = Board.GetMovesForPosition(board, from);

            Move targetPosition = moves.FindTargetPosition(to);

            Board.MakeMove(board, targetPosition);
        }
    }


}
