using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParallelChess {





    public class EnPassant {
        public const byte NO_ENPASSANT = byte.MaxValue;
    }


    public class Chess {

        // Loads a board from the FEN notation(Forsyth–Edwards Notation)
        // Useful for getting to certain chess positions quickly. 
        // Use the website below to generate positions
        // https://lichess.org/editor/8/1QQ2QQ1/QqqQQqqQ/QqqqqqqQ/QqqqqqqQ/1QqqqqQ1/2QqqQ2/3QQ3_b_-_-_0_1
        // FEN consists of 6 parts which are sepperated by space
        // 1. Position
        //  - Describes what pieces are at which positions
        //  - The pieces are described from left to right with a "/" to indicate row changes
        //  - a single letter is used to represent a piece
        //    - P = Pawn
        //    - N = Knight
        //    - B = Bishop
        //    - R = Rook
        //    - Q = Queen
        //    - K = King
        //  - Capital letters indicate White pieces, while lowercase letter indicate Black pieces
        // 2. active color
        //  - Either "w" or "b", which indicates whose turn it is.
        // 3. castling options
        //  - stores information if about who is allowed to castle.
        //  - Capital letters indicate White side lowercase letters are blacks options
        //  - the letters K and Q mean queen or kingside castle.
        //  - If no castling options it is marked with a "-"
        // 4. En passant target
        //  - marks which square is current possible to attack with en passant.
        // 5. Half move clock
        //  - Stores how many half moves have been made since the last capture or pawn move
        //  - This is used for declaring stalemate
        // 6. Fullmove number
        //  - Counts how many full moves have been made
        public static Board LoadBoardFromFen(String fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1") {
            Board board = new Board() {
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
                board.CastlingBits |= CastlingBits.BLACK_KING_SIDE_CASTLE;
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
                board.EnPassantTarget = (byte) Board.ArrayPosition(enPassantAttackedSquare);
            }

            //Board.SetHalfTurnCounter(board, int.Parse(halfMoveClock));
            board.HalfTurnCounter = (byte) int.Parse(halfMoveClock);

            //Board.SetFullMoveClock(board, int.Parse(fullMoveClock));
            board.TurnCounter = (short) int.Parse(fullMoveClock);

            return board;
        }

        public static String BoardToFen(Board board) {
            StringBuilder fen = new StringBuilder();

            for (int row = 7; row >= 0; row--) {
                int count = 0;
                for (int column = 0; column < 8; column++) {
                    var position = row * BoardStateOffset.ROW_OFFSET + column;
                    var piece = board.GetPiece(position);
                    //var piece = Board.GetPiece(board, position);
                    char c = PieceParse.ToChar(piece);
                    if (c == '_') {
                        count++;
                    }else {
                        if(count != 0) {
                            fen.Append(count);
                            count = 0;
                        }
                        fen.Append(c);
                    }
                }
                if (count != 0) {
                    fen.Append(count);
                }

                if(row != 0) {
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
                if((castlingBits & CastlingBits.WHITE_KING_SIDE_CASTLE) != CastlingBits.EMPTY) {
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
            fen.Append(board.EnPassantTarget != EnPassant.NO_ENPASSANT ? Board.ReadablePosition(board.EnPassantTarget) : "-");
            fen.Append(" ");
            fen.Append(board.HalfTurnCounter);
            fen.Append(" ");
            fen.Append(board.TurnCounter);

            return fen.ToString();
        }

        public static String AsciiBoard(Board board, List<Move> moves = null, bool displayCount = false) {
            if(moves == null) {
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
                    if(movesOnPosition > 0) {
                        if(movesOnPosition < 10 && displayCount) {
                            ascii.Append(movesOnPosition);
                        } else {
                            ascii.Append("x");
                        }
                    } else {
                        ascii.Append(PieceParse.ToChar(piece));
                    }
                    if(column != 7) {
                        ascii.Append(" ");
                    }
                }

                ascii.Append($"| {(row + 1)}\n");
            }
            ascii.Append("+---------------+\n");

            ascii.Append(" A B C D E F G H");
            return ascii.ToString();
        }

        public static Move FindMove(Board board, int from , int to) {
            List<Move> moves = board.GetMovesForPosition(from);

            Move targetPosition = moves.FindTargetPosition(to);
            if (!MoveHelper.isValidMove(targetPosition)) {
                throw new Exception("Move not found");
            }
            if(!board.IsLegalMove(targetPosition)) {
                throw new Exception("Illegal move");
            }

            return targetPosition;
        }

        public static Move FindMove(Board board, int from, int to, Piece promotion) {
            List<Move> moves = board.GetMovesForPosition(from);

            Move targetPosition = moves.FindTargetPosition(to, promotion);
            if (!MoveHelper.isValidMove(targetPosition)) {
                throw new Exception("Move not found");
            }

            if (!board.IsLegalMove(targetPosition)) {
                throw new Exception("Illegal move");
            }

            return targetPosition;
        }

        public static Move MakeMove(Board board, int from, int to) {
            Move targetPosition = FindMove(board, from, to);

            board.MakeMove(targetPosition);

            return targetPosition;
        }

        public static Move MakeMove(Board board, string san) {
            List<Move> moves = board.GetMoves()
                .Where(move => board.IsLegalMove(move))
                .ToList();

            foreach (var move in moves) {
                if(board.StandardAlgebraicNotation(move) == san.Trim()) {
                    board.MakeMove(move);
                    return move;
                }
            }

            throw new Exception("Move not found!");
        }

        public static Move MakeMove(Board board, int from, int to, Piece promotion) {
            Move targetPosition = FindMove(board, from, to, promotion);

            board.MakeMove(targetPosition);

            return targetPosition;
        }
    }


}
