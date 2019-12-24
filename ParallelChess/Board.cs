using ParallelChess.AI;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ParallelChess {
    public enum Winner {
        WINNER_BLACK = 0,
        WINNER_WHITE = 1,
        DRAW = 2,
        NONE = 3,
    }

    public static class Board {
        public static void initThreadStaticVariables() {
            IsValidMoveVirtualBoard = new byte[BoardStateOffset.BOARD_STATE_SIZE];
            EvalBoard.initThreadStaticVariables();
            MinMaxAI.initThreadStaticVariables();
        }

        public const int A_COLUMN = 0;
        public const int B_COLUMN = 1;
        public const int C_COLUMN = 2;
        public const int D_COLUMN = 3;
        public const int E_COLUMN = 4;
        public const int F_COLUMN = 5;
        public const int G_COLUMN = 6;
        public const int H_COLUMN = 7;

        public static Winner detectWinnerAreThereValidMoves(BoardState board, bool areThereValidMoves) {
            if (!areThereValidMoves) {
                // check if king is under attack
                if (Attacked(board, board.GetKingPosition(board.IsWhiteTurn), board.IsWhiteTurn)) {
                    if (board.IsWhiteTurnBool) {
                        return Winner.WINNER_BLACK;
                    } else {
                        return Winner.WINNER_WHITE;
                    }
                } else {
                    return Winner.DRAW;
                }
            }

            if (board.HalfTurnCounter == 50) {
                return Winner.DRAW;
            }

            if (detectInsufficientMaterial(board)) {
                return Winner.DRAW;
            }

            return Winner.NONE;
        }

        public static Winner detectWinner(BoardState board, IEnumerable<Move> moves) {
            //int count = history.Count();
            var validMoves = moves.Where((move) => {
                return IsValidMove(board, move);
            });

            return detectWinnerAreThereValidMoves(board, validMoves.Count() != 0);
        }

        // Detect Insufficient material. according to https://www.chessstrategyonline.com/content/tutorials/how-to-play-chess-draws there are 4 options for insufficient material
        // According to the website these combinations are invalid
        // King vs king
        // King and bishop vs king
        // King and knight vs king
        // King and bishop vs king and bishop of the same colour.
        // technically even though unlikely a player can have 2 bishops on the same color, in which case it is still a stalemate
        // This means we need to keep track of if there are more than 2 knights and bishops of more than 1 color
        public static bool detectInsufficientMaterial(BoardState board) {
            // the goal is to return as soon as piece is found that indicates it is not a stalemate to increase performance
            bool bishopSquareColor = false;
            bool foundBishop = false;
            bool foundHorse = false;


            bool squareColor = false;
            for (int row = 0; row < 8 * BoardStateOffset.ROW_OFFSET; row += BoardStateOffset.ROW_OFFSET) {
                for (int column = 0; column < 8; column++) {

                    Piece piece = board.GetPiece(column + row);

                    switch (piece & Piece.PIECE_MASK) {
                        case Piece.EMPTY:
                        case Piece.KING:
                            // ignore king and empty since they don't matter
                            // the king doesn't matter because it is always there
                            break;
                        case Piece.KNIGHT:
                            // if there are 2 horses of any color then it is not insufficient material
                            if (!foundHorse && !foundBishop) {
                                foundHorse = true;
                            } else {
                                return false;
                            }
                            break;
                        case Piece.BISHOP:
                            if(foundHorse) {
                                return false;
                            }

                            if (!foundBishop) {
                                bishopSquareColor = squareColor;
                                foundBishop = true;
                            } else {
                                if(bishopSquareColor != squareColor) {
                                    return false;
                                }
                            }
                            break;
                        default:
                            return false;
                    }

                    // flip the square color
                    squareColor = !squareColor;
                }
            }

            // if we get to this point there are only 2 kings left, or found which is a stalemate
            return true;
        }

        public static BoardState CreateCopyBoard(BoardState board) {
            BoardState newBoard = new BoardState {
                bytes = new byte[BoardStateOffset.BOARD_STATE_SIZE],
            };
            Buffer.BlockCopy(board.bytes, 0, newBoard.bytes, 0, BoardStateOffset.BOARD_STATE_SIZE);
            return newBoard;
        }

        public static void CopyBoard(BoardState fromBoard, BoardState toBoard) {
            Buffer.BlockCopy(fromBoard.bytes, 0, toBoard.bytes, 0, BoardStateOffset.BOARD_STATE_SIZE);
            //fromBoard.CopyTo(toBoard, 0);
        }

        // position is expected be in the format "a1" or "b5"
        // converts it to a number from 0 to 119, which is the number system used internally
        // examples
        // a1 -> 0
        // a3 -> 2
        // h8 -> 119
        public static int AlgebraicPosition(string readablePosition) {
            readablePosition = readablePosition.ToLower();

            // abuse the ascii system by converting the row to a number from 0 to 7
            // subtract 'a' because then the number will start from 0
            int algebraicPosition = (readablePosition[0] - 'a');

            // add the column to the positon, subtract one because
            // multiply by the rowoffset to get the correct row.
            algebraicPosition += (int.Parse(readablePosition.Substring(1, 1)) - 1) * BoardStateOffset.ROW_OFFSET;

            return algebraicPosition;
        }

        // convert from internal number format to a readble form
        // examples
        // 0 -> a1
        // 2 -> a3
        // 119 -> h8
        public static string ReadablePosition(int algebraicPosition) {
            int row = algebraicPosition / BoardStateOffset.ROW_OFFSET;
            int column = algebraicPosition - (row * BoardStateOffset.ROW_OFFSET);
            string move = Convert.ToChar('a' + (column)) + (row + 1).ToString();
            return move;
        }


        // me refers to the current player
        public static bool PieceBelongsToMe(BoardState board, Piece piece) {
            // we abuse that Piece stores the information about color in its last bit
            // therefor we can mask that bit away for example the piece 0x07 (white king) & 0x01(isWhitecheck) = 0x01
            // this always produces 1 or 0 which can be directly compared to isWhiteTurn.
            // IsWhite is either 0x01 (true) or 0x00 (false)
            return board.IsWhiteTurn == (byte)(piece & Piece.IS_WHITE);
        }

        //// TODO: undersøg om det er værd at bruge 0x88 til at tjekke valide positioner.
        //public static bool IsValidPosition(int position, int relativeColumn, int relativeRow) {
        //    int targetPosition = Board.RelativePosition(position, relativeColumn, relativeRow);
        //    int targetRow = targetPosition / 8;
        //    int expectedRow = position / 8 + relativeRow;
        //    return
        //        // vi tjekker at position er på skakbrettet for at vi ikke løber ud på række 9 eller -1
        //        // Dette tjek kunne erstattes med targetPosition >= 0 && targetPosition <= 63
        //        // det virker ved at bruge en bitmask til at finde alle tal som ikke er inden for 63
        //        // mask:                        1111 1111 1111 1111 1111 1111 1110 0000
        //        // et tal uden for brættet(64): 0000 0000 0000 0000 0000 0000 0010 0000
        //        // resultat:                    0000 0000 0000 0000 0000 0000 0010 0000
        //        ((targetPosition & ~63) == 0)
        //        // Dette tjek er for at forhindre at en brik der prøver at komme til venstre ender i en anden række
        //        // dette er et problem på grund af brættet wrapper rundt.
        //        // foreksmpel feltet A3 har position 16, det burde ikke være muligt at gå til venstre herfra, 
        //        // men hvis man trækker 1 fra 15 ender man på 15 hvilket er feltet H2
        //        && targetRow == expectedRow;
        //}

        public static bool IsValidPosition(int position) {
            // ------------0x88-------------
            // 0x88 is method used checking if location is out of bound very fast
            // it is done by creating an array and leaving every second row blank.
            // We can then check if a position is in a invalid row by anding a position with 0x88
            // for example if you tried to move one right from H3(39) + 1 = 40. 40 is a invalid position 
            // which can be caught by checking 0x88 & 40 = 8. Any invalid position will result in a non zero value
            // the potential disadvantage to this approach is that the boardState is larger, which will take longer to copy and 
            // might mean that the array is no longer cached in CPU cache.
            return (0x88 & position) == 0;
        }

        public static int RelativePosition(int position, int relativeColumn, int relativeRow) {
            return position + relativeRow * BoardStateOffset.ROW_OFFSET + relativeColumn;
        }

        public static int PositionRow(int position) {
            return position / BoardStateOffset.ROW_OFFSET;
        }

        public static int PositionColumn(int position) {
            return position - (PositionRow(position) * BoardStateOffset.ROW_OFFSET);
        }

        public static bool IsPositionEmpty(BoardState board, int position) {
            return board.GetPiece(position) == Piece.EMPTY;
        }

        public enum MoveOption {
            INVALID,
            CAPTURE,
            NO_FIGHT
        }

        public static MoveOption CanITakeSquare(BoardState board, int position) {
            var piece = board.GetPiece(position);
            if (piece == Piece.EMPTY) {
                //return true;
                return MoveOption.NO_FIGHT;
            }

            // check that the current color is not the same as the moving piece
            if (PieceBelongsToMe(board, piece)) {
                return MoveOption.INVALID;
            } else {
                return MoveOption.CAPTURE;
            }
        }

        //public static (int column, int row)[] kingMoves = {
        //    (-1, 1), (0, 1), (1, 1),
        //    (-1, 0),         (0, 1),
        //    (-1,-1), (0,-1), (1,-1)
        //};

        //public static (int column, int row)[] slantedMoves = {
        //    (1,1),(1,-1),(-1,1),(-1,-1)
        //};
        //public static (int column, int row)[] straightMoves = {
        //    (1,0),(-1,0),(0,1),(0,-1)
        //};

        //public static (int column, int row)[] knightMoves = {
        //           (-1,2), (1,2),
        //    (-2,1),              (2,1),
        //    (-2,-1),             (2,-1),
        //           (-1,-2),(1,-2),
        //};

        public static int[] directKingMoves = {
            BoardStateOffset.ROW_OFFSET * 1 + 1,
            BoardStateOffset.ROW_OFFSET * -1 + 1,
            BoardStateOffset.ROW_OFFSET * 1 + -1,
            BoardStateOffset.ROW_OFFSET * -1 + -1,
            BoardStateOffset.ROW_OFFSET,
            -BoardStateOffset.ROW_OFFSET,
            1,
            -1
        };

        public static int[] directStraightMoves = {
            BoardStateOffset.ROW_OFFSET,
            -BoardStateOffset.ROW_OFFSET,
            1,
            -1
        };

        public static int[] directSlantedMoves = {
            BoardStateOffset.ROW_OFFSET * 1 + 1,
            BoardStateOffset.ROW_OFFSET * -1 + 1,
            BoardStateOffset.ROW_OFFSET * 1 + -1,
            BoardStateOffset.ROW_OFFSET * -1 + -1,
        };

        public static int[] directKnightMoves = {
                   -1 + BoardStateOffset.ROW_OFFSET * 2 , 1 + BoardStateOffset.ROW_OFFSET * 2,
            -2 + BoardStateOffset.ROW_OFFSET * 1,              2 + BoardStateOffset.ROW_OFFSET * 1,
            -2 + BoardStateOffset.ROW_OFFSET * -1,              2 + BoardStateOffset.ROW_OFFSET * -1,
                   -1 + BoardStateOffset.ROW_OFFSET * -2 , 1 + BoardStateOffset.ROW_OFFSET * -2,
        };

        public static bool Attacked(BoardState board, int position, byte pretendToBeWhite) {
            int theirColor = pretendToBeWhite ^ 1;
            Piece theirColorPiece = (Piece)theirColor;
            foreach (var move in directSlantedMoves) {
                int relativePosition = position;
                // king filter is used to allow kings to attack one square
                // they are disabled are the first rotation
                bool isFirstPosition = true;
                do {
                    relativePosition += move;
                    if (IsValidPosition(relativePosition)) {
                        var piece = board.GetPiece(relativePosition);
                        if (piece != Piece.EMPTY) {
                            Piece enemySlantedAttacked = (theirColorPiece | Piece.ATTACKS_SLANTED);
                            if ((piece & (Piece.ATTACKS_SLANTED | Piece.IS_WHITE)) == enemySlantedAttacked) {
                                return true;
                            }
                            if (isFirstPosition) {
                                Piece kingFilter = (theirColorPiece | Piece.KING);
                                if ((piece & (Piece.PIECE_MASK | Piece.IS_WHITE)) == kingFilter) {
                                    return true;
                                }
                            }
                            break;
                        }
                    } else {
                        break;
                    }
                    isFirstPosition = false;
                } while (true);
            }

            foreach (var move in directStraightMoves) {
                int relativePosition = position;
                // king filter is used to allow kings to attack one square
                // they are disabled are the first square
                bool isFirstPosition = true;
                do {
                    relativePosition += move;
                    if (IsValidPosition(relativePosition)) {
                        var piece = board.GetPiece(relativePosition);
                        if (piece != Piece.EMPTY) {
                            Piece enemySlantedAttacked = (theirColorPiece | Piece.ATTACKS_STRAIGHT);
                            if ((piece & (Piece.ATTACKS_STRAIGHT | Piece.IS_WHITE)) == enemySlantedAttacked) {
                                return true;
                            }
                            if (isFirstPosition) {
                                Piece kingFilter = (theirColorPiece | Piece.KING);
                                if ((piece & (Piece.PIECE_MASK | Piece.IS_WHITE)) == kingFilter) {
                                    return true;
                                }
                            }
                            break;
                        }
                    } else {
                        break;
                    }
                    isFirstPosition = false;
                } while (true);
            }

            foreach (var move in directKnightMoves) {
                int relativePosition = position + move;

                if (IsValidPosition(relativePosition)) {
                    var piece = board.GetPiece(relativePosition);
                    Piece enemySlantedAttacked = (theirColorPiece | Piece.KNIGHT);
                    if ((piece & (Piece.KNIGHT | Piece.IS_WHITE)) == enemySlantedAttacked) {
                        return true;
                    }
                }
            }

            int leftPawnPosition = position - BoardStateOffset.ROW_OFFSET - 1 + BoardStateOffset.ROW_OFFSET * pretendToBeWhite * 2;
            int rightPawnPosition = position - BoardStateOffset.ROW_OFFSET + 1 + BoardStateOffset.ROW_OFFSET * pretendToBeWhite * 2;
            if (IsValidPosition(leftPawnPosition)) {
                Piece leftPawn = board.GetPiece(leftPawnPosition);
                if (leftPawn == (theirColorPiece | Piece.PAWN)) {
                    return true;
                }
            }

            if (IsValidPosition(rightPawnPosition)) {
                Piece rightPawn = board.GetPiece(rightPawnPosition);
                if (rightPawn == (theirColorPiece | Piece.PAWN)) {
                    return true;
                }
            }

            return false;
        }

        public static bool CanICastleQueenSide(BoardState board) {
            CastlingBits castlingBits = board.CastlingBits;
            if (board.IsWhiteTurnBool) {
                return (castlingBits & CastlingBits.WHITE_QUEEN_SIDE_CASTLE) == CastlingBits.WHITE_QUEEN_SIDE_CASTLE;
            } else {
                return (castlingBits & CastlingBits.BLACK_QUEEN_SIDE_CASTLE) == CastlingBits.BLACK_QUEEN_SIDE_CASTLE;
            }
        }

        public static bool CanCastleKingSide(BoardState board) {
            CastlingBits castlingBits = board.CastlingBits;

            // TODO bitshift optimize?
            if (board.IsWhiteTurnBool) {
                return (castlingBits & CastlingBits.WHITE_KING_SIDE_CASTLE) == CastlingBits.WHITE_KING_SIDE_CASTLE;
            } else {
                return (castlingBits & CastlingBits.BLACK_KING_SIDE_CASTLE) == CastlingBits.BLACK_KING_SIDE_CASTLE;
            }
        }

        private static void AddPawnMove(BoardState board, int fromPosition, int targetPosition, MoveFlags move, List<Move> moves) {
            Piece takenPiece;
            int row = Board.PositionRow(targetPosition);
            if ((move & MoveFlags.ENPASSANT) == MoveFlags.ENPASSANT) {
                takenPiece = Piece.PAWN;
            } else {
                takenPiece = board.GetPiece(targetPosition);
            }

            // check if the pawn is going to move into a promotion row.
            // we don't check the color because a pawn can never enter its own promotion area
            if (row == 0 || row == 7) {
                moves.Add(MoveHelper.CreateMove(targetPosition, fromPosition, takenPiece, Piece.QUEEN, move, board));
                moves.Add(MoveHelper.CreateMove(targetPosition, fromPosition, takenPiece, Piece.BISHOP, move, board));
                moves.Add(MoveHelper.CreateMove(targetPosition, fromPosition, takenPiece, Piece.KNIGHT, move, board));
                moves.Add(MoveHelper.CreateMove(targetPosition, fromPosition, takenPiece, Piece.ROOK, move, board));
            } else {
                moves.Add(MoveHelper.CreateMove(targetPosition, fromPosition, takenPiece, Piece.EMPTY, move, board));
            }
        }

        private static void AddMove(BoardState board, int fromPosition, int targetPosition, MoveFlags moveBits, List<Move> moves) {
            Piece takenPiece = board.GetPiece(targetPosition);
            moves.Add(MoveHelper.CreateMove(targetPosition, fromPosition, takenPiece, Piece.EMPTY, moveBits, board));
        }

        private static void WalkRelativePaths(BoardState board, int fromPosition, int[] movePositions, List<Move> moves) {
            foreach (var relativePosition in movePositions) {
                int move = fromPosition;
                do {
                    move += relativePosition;
                    if (IsValidPosition(move)) {
                        var moveOption = CanITakeSquare(board, move);
                        if (moveOption == MoveOption.NO_FIGHT) {
                            AddMove(board, fromPosition, move, MoveFlags.EMPTY, moves);
                        } else if (moveOption == MoveOption.CAPTURE) {
                            AddMove(board, fromPosition, move, MoveFlags.EMPTY, moves);
                            break;
                        } else {
                            break;
                        }
                    } else {
                        break;
                    }
                } while (true);
            }
        }

        [ThreadStatic]
        private static byte[] IsValidMoveVirtualBoard = new byte[BoardStateOffset.BOARD_STATE_SIZE];

        //private static ThreadLocal<byte[]> IsValidMoveVirtualBoard = new ThreadLocal<byte[]>(() => {
        //    return new byte[BoardStateOffset.BOARD_STATE_SIZE];
        //});


        public static bool IsValidMove(BoardState board, Move move) {
            byte myTurn = board.IsWhiteTurn;
            MakeMove(board, move);

            var notAttacked = !Attacked(board, board.GetKingPosition(myTurn), myTurn);

            UndoMove(board, move);

            return notAttacked;
        }

        public static List<Move> GetMoves(BoardState board, List<Move> moves = null) {
            if (moves == null) {
                moves = new List<Move>();
            }

            for (int column = 0; column < 8; column++) {
                for (int row = 0; row < 8 * BoardStateOffset.ROW_OFFSET; row += BoardStateOffset.ROW_OFFSET) {
                    moves = GetMovesForPosition(board, column + row, moves);
                }
            }

            return moves;
        }

        public static List<Move> GetMovesForPosition(BoardState board, int fromPosition, List<Move> moves = null) {
            if (moves == null) {
                moves = new List<Move>();
            }

            Piece piece = board.GetPiece(fromPosition);
            if (!PieceBelongsToMe(board, piece)) {
                // is the piece of the same color as the current turn
                // TODO: maybe move this check out to a higher level
                return moves;
            }
            Piece justPiece = piece & Piece.PIECE_MASK;



            switch (justPiece) {
                case Piece.PAWN:
                    bool isWhitesTurn = board.IsWhiteTurnBool;
                    int direction = isWhitesTurn ? 1 : -1;

                    // We don't need to check if there is a next row is outside the board.
                    // because the pawn is never able to stand on the last because of promotion
                    int moveOne = RelativePosition(fromPosition, 0, direction);
                    if (IsPositionEmpty(board, moveOne)) {
                        //moves.Add(moveOne);
                        AddPawnMove(board, fromPosition, moveOne, MoveFlags.PAWN_MOVE, moves);
                    }

                    // check if the pawn is on the starting position. If it is then assume that it is possible to move forward
                    if (isWhitesTurn ? PositionRow(fromPosition) == 1 : PositionRow(fromPosition) == 6) {
                        int hasToBeEmptyPosition = RelativePosition(fromPosition, 0, direction);
                        int move = RelativePosition(fromPosition, 0, 2 * direction);
                        if (IsPositionEmpty(board, move) && IsPositionEmpty(board, hasToBeEmptyPosition)) {
                            AddPawnMove(board, fromPosition, move, MoveFlags.BIG_PAWN_MOVE | MoveFlags.PAWN_MOVE, moves);
                        }
                    }

                    int column = PositionColumn(fromPosition);
                    // check if the pawn is on the right column. 
                    // We don't need to check if there is a next row is outside the board.
                    // because the pawn is never able to stand on the last because of promotion
                    if (column != Board.H_COLUMN) {
                        int move = RelativePosition(fromPosition, 1, direction);
                        bool isEnpassant = board.EnPassantTarget == move;
                        if (
                            // targetposition has to either be the enpassant square.
                            isEnpassant
                            || CanITakeSquare(board, move) == MoveOption.CAPTURE)
                            // or be empty or contain an enemy) {
                            AddPawnMove(board, fromPosition, move, MoveFlags.PAWN_MOVE | (isEnpassant ? MoveFlags.ENPASSANT : MoveFlags.EMPTY), moves);
                    }
                    // check if the pawn is on the left column. 
                    // We don't need to check if there is a next row is outside the board.
                    // because the pawn is never able to stand on the last because of promotion
                    if (column != Board.A_COLUMN) {
                        int move = RelativePosition(fromPosition, -1, direction);
                        bool isEnpassant = board.EnPassantTarget == move;
                        if (
                            // targetposition has to either be the enpassant square.
                            isEnpassant
                            || CanITakeSquare(board, move) == MoveOption.CAPTURE)
                            // or be empty or contain an enemy) {
                            AddPawnMove(board, fromPosition, move, MoveFlags.PAWN_MOVE | (isEnpassant ? MoveFlags.ENPASSANT : MoveFlags.EMPTY), moves);
                    }

                    break;
                case Piece.KING:
                    foreach (var relativeMove in directKingMoves) {
                        int move = relativeMove + fromPosition;
                        if (IsValidPosition(move)) {
                            var moveOption = CanITakeSquare(board, move);
                            if (moveOption != MoveOption.INVALID) {
                                AddMove(board, fromPosition, move, MoveFlags.EMPTY, moves);
                            }
                        }
                    }
                    //foreach (var relativeMove in kingMoves) {
                    //    int move = RelativePosition(fromPosition, relativeMove.column, relativeMove.row);
                    //    if ((0x88 & move) == 0) {
                    //        var moveOption = CanITakeSquare(board, move);
                    //        if (moveOption != MoveOption.INVALID) {
                    //            AddMove(board, fromPosition, move, Move.EMPTY, moves);
                    //        }
                    //    }
                    //}
                    byte isWhite = board.IsWhiteTurn;
                    // check if they are allowed to castle
                    // 1. Check history if rook or king has moved. This information is stored in the castlingBit
                    // 2. Check if the king moves through any square that is under attack
                    // 3. Check if the king moves through a square that is not empty
                    if (CanCastleKingSide(board)
                        && IsPositionEmpty(board, fromPosition + 1)
                        && IsPositionEmpty(board, fromPosition + 2)
                        && !Attacked(board, fromPosition + 0, isWhite)
                        && !Attacked(board, fromPosition + 1, isWhite)
                        && !Attacked(board, fromPosition + 2, isWhite)
                        ) {
                        AddMove(board, fromPosition, fromPosition + 2, MoveFlags.CASTLING, moves);
                    }

                    // Do the same queen side
                    if (CanICastleQueenSide(board)
                        && IsPositionEmpty(board, fromPosition - 1)
                        && IsPositionEmpty(board, fromPosition - 2)
                        && IsPositionEmpty(board, fromPosition - 3)
                        && !Attacked(board, fromPosition - 0, isWhite)
                        && !Attacked(board, fromPosition - 1, isWhite)
                        && !Attacked(board, fromPosition - 2, isWhite)
                        ) {
                        AddMove(board, fromPosition, fromPosition - 2, MoveFlags.CASTLING, moves);
                    }

                    break;
                case Piece.KNIGHT:
                    foreach (var relativeMove in directKnightMoves) {
                        int move = fromPosition + relativeMove;
                        if (IsValidPosition(move)) {
                            var moveOption = CanITakeSquare(board, move);
                            if (moveOption != MoveOption.INVALID) {
                                AddMove(board, fromPosition, move, MoveFlags.EMPTY, moves);
                            }
                        }
                    }
                    break;
                case Piece.QUEEN:
                    WalkRelativePaths(board, fromPosition, directSlantedMoves, moves);
                    WalkRelativePaths(board, fromPosition, directStraightMoves, moves);
                    break;
                case Piece.ROOK:
                    WalkRelativePaths(board, fromPosition, directStraightMoves, moves);
                    break;
                case Piece.BISHOP:
                    WalkRelativePaths(board, fromPosition, directSlantedMoves, moves);
                    break;
                case Piece.EMPTY:
                    break;
                default:
                    // assume it is either ROOK,BISHOP or QUEEN
                    //if ((Piece.ATTACKS_SLANTED & piece) == Piece.ATTACKS_SLANTED) {
                    //    WalkRelativePaths(board, fromPosition, slantedMoves, moves);
                    //}
                    //if ((Piece.ATTACKS_STRAIGHT & piece) == Piece.ATTACKS_STRAIGHT) {
                    //    WalkRelativePaths(board, fromPosition, straightMoves, moves);
                    //}
                    break;
            }

            return moves;
        }

        public static void UndoMove(BoardState board, Move move) {
            int targetPosition = move.targetPosition;
            int fromPosition = move.fromPosition;
            int theirColor = board.IsWhiteTurn;
            int ourColor = board.IsWhiteTurn ^ 1;

            board.bytes[fromPosition] = board.bytes[targetPosition];
            //copy.bytes[targetPosition] = (byte) Piece.EMPTY;
            board.bytes[targetPosition] = move.capturedPiece;

            Piece movedPiece = board.GetPiece(fromPosition);

            CastlingBits previous = (CastlingBits)move.previousCastlingBits;
            board.bytes[BoardStateOffset.HALF_TURN_COUNTER] = move.previousHalfMove;
            board.bytes[BoardStateOffset.CASTLING] = move.previousCastlingBits;
            board.bytes[BoardStateOffset.EN_PASSANT_FIELD] = move.previousEnpassant;
            MoveFlags moveFlags = (MoveFlags)move.moveFlags;

            if ((moveFlags & MoveFlags.ENPASSANT) == MoveFlags.ENPASSANT) {
                // when undoing a enpassant move spawn their pawn back
                // we abuse that isWhite is a integer which is 1 on whites turn and 0 and blacks turn
                // if it was black move then we have to spawn it one row above
                // if it was whites move we spawn it one move below
                // keep in mind the IsWhiteTurn is currently opposite of who made the move
                int enpassantSpawnPosition = targetPosition - BoardStateOffset.ROW_OFFSET + 2 * BoardStateOffset.ROW_OFFSET * board.IsWhiteTurn;
                board.SetPiece(enpassantSpawnPosition, Piece.PAWN | (Piece)theirColor);

                // when capturing with enpassant don't place the captured piece back since it was taken from another square
                board.bytes[targetPosition] = (byte)Piece.EMPTY;
            }
            if (move.promotion != 0) {
                board.bytes[fromPosition] = (byte)(Piece.PAWN | (Piece)ourColor);
            }

            // if black made a move decrement the turn counter
            // we abuse that isWhite is a integer which is 1 on whites turn and 0 and blacks turn
            board.TurnCounter -= board.IsWhiteTurn;

            if ((movedPiece & Piece.KING) == Piece.KING) {
                if ((moveFlags & MoveFlags.CASTLING) == MoveFlags.CASTLING) {
                    // if the target move is less than the kingsposition it is queenside castling, 
                    // otherwise it is kingside castle 
                    if (targetPosition < fromPosition) {
                        // copy the rook back to its starting position 
                        board.bytes[fromPosition - 4] = board.bytes[fromPosition - 1];
                        board.bytes[fromPosition - 1] = 0;
                    } else {
                        board.bytes[fromPosition + 3] = board.bytes[fromPosition + 1];
                        board.bytes[fromPosition + 1] = 0;
                    }
                }
                board.SetKingPosition(ourColor, (byte)fromPosition);
            }

            // switch turn back to whites turn
            board.IsWhiteTurn = (byte)ourColor;
        }

        public static void MakeMove(BoardState board, Move move) {
            int toPosition = move.targetPosition;
            int fromPosition = move.fromPosition;
            bool isWhitesTurn = board.IsWhiteTurnBool;

            Piece piece = board.GetPiece(fromPosition);
            Piece pieceType = piece & Piece.PIECE_MASK;
            Piece takenPiece = board.GetPiece(toPosition);
            Piece promotion = (Piece)move.promotion;
            MoveFlags moveFlags = (MoveFlags)move.moveFlags;

            if ((moveFlags & MoveFlags.BIG_PAWN_MOVE) == MoveFlags.BIG_PAWN_MOVE) {
                // when making a big pawn move mark the square behind the moving pawn vulnerable to 
                if (isWhitesTurn) {
                    board.EnPassantTarget = (byte)(toPosition - BoardStateOffset.ROW_OFFSET);
                    //Board.SetEnPassantAttackedSquare(board, toPosition - BoardOffset.ROW);
                } else {
                    board.EnPassantTarget = (byte)(toPosition + BoardStateOffset.ROW_OFFSET);
                    //Board.SetEnPassantAttackedSquare(board, toPosition + BoardOffset.ROW);
                }
            } else {
                board.EnPassantTarget = EnPassant.NO_ENPASSANT;
                //Board.SetEnPassantAttackedSquare(board, EnPassant.NO_ENPASSANT);
            }
            switch (pieceType) {
                case Piece.PAWN:
                    if ((moveFlags & MoveFlags.ENPASSANT) == MoveFlags.ENPASSANT) {
                        // When taking with enpassant remove the piece
                        if (isWhitesTurn) {
                            board.SetPiece(toPosition - BoardStateOffset.ROW_OFFSET, Piece.EMPTY);
                            //Board.PutPiece(board, toPosition - BoardOffset.ROW, Piece.EMPTY);
                        } else {
                            board.SetPiece(toPosition + BoardStateOffset.ROW_OFFSET, Piece.EMPTY);
                            //Board.PutPiece(board, toPosition + BoardOffset.ROW, Piece.EMPTY);
                        }
                    }

                    break;
                case Piece.KING:
                    //SetKingPosition(board, isWhitesTurn, toPosition);
                    if ((moveFlags & MoveFlags.CASTLING) == MoveFlags.CASTLING) {
                        switch (toPosition) {
                            case BoardStateOffset.C1:
                                board.D1 = board.A1;
                                board.A1 = Piece.EMPTY;
                                break;
                            case BoardStateOffset.G1:
                                board.F1 = board.H1;
                                board.H1 = Piece.EMPTY;
                                break;
                            case BoardStateOffset.C8:
                                board.D8 = board.A8;
                                board.A8 = Piece.EMPTY;
                                break;
                            case BoardStateOffset.G8:
                                board.F8 = board.H8;
                                board.H8 = Piece.EMPTY;
                                break;
                        }
                    }
                    board.SetKingPosition(board.IsWhiteTurn, (byte)toPosition);
                    break;
            }


            var castleBit = board.CastlingBits;
            // remove opportunity to castle based on the position on the board
            //Board.SetAllCastlingBit(board, castleBit
            //    & CastlingHelper.castleLookup[toPosition]
            //    & CastlingHelper.castleLookup[fromPosition]);
            board.CastlingBits = board.CastlingBits
                & CastlingHelper.castleLookup[toPosition]
                & CastlingHelper.castleLookup[fromPosition];


            // move piece to new position
            if (promotion == Piece.EMPTY) {
                //PutPiece(board, toPosition, piece);
                board.SetPiece(toPosition, piece);
            } else {
                //PutPiece(board, toPosition, promotion);
                board.SetPiece(toPosition, promotion | (Piece)board.IsWhiteTurn);
            }

            // remove piece from previous position
            board.SetPiece(fromPosition, Piece.EMPTY);

            if (pieceType == Piece.PAWN || ((takenPiece & Piece.PIECE_MASK) != Piece.EMPTY)) {
                //Board.SetHalfTurnCounter(board, 0);
                board.HalfTurnCounter = 0;
            } else {
                //Board.IncrementHalfTurnCounter(board);
                board.HalfTurnCounter++;
            }


            if (!isWhitesTurn) {
                // increment fullMoveClock after blacks turn
                //Board.IncrementFullMoveClock(board);
                board.TurnCounter++;
            }

            // flip turn
            //SetIsWhitesTurn(board, !isWhitesTurn);
            board.IsWhiteTurn = (byte)(board.IsWhiteTurn ^ 1);
        }

    }
}
