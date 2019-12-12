using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ParallelChess {
    public static class Board {
        public static void initThreadStaticVariables() {
            IsValidMoveVirtualBoard = new byte[BoardStateOffset.BOARD_STATE_SIZE];
        }

        public const int A_COLUMN = 0;
        public const int B_COLUMN = 1;
        public const int C_COLUMN = 2;
        public const int D_COLUMN = 3;
        public const int E_COLUMN = 4;
        public const int F_COLUMN = 5;
        public const int G_COLUMN = 6;
        public const int H_COLUMN = 7;



        //public static void PutPiece(byte[] board, int position, Piece piece) {
        //    board[position] = (byte)piece;
        //}

        //public static Piece GetPiece(byte[] board, int position) {
        //    return (Piece)board[position];
        //}

        //public static void SetIsWhitesTurn(byte[] board, bool isWhiteTurn) {

        //    board[BoardStateOffset.IS_WHITE_TURN] = (byte)(isWhiteTurn ? 1 : 0);
        //}
        //public static bool IsWhitesTurn(byte[] board) {
        //    return board[BoardStateOffset.IS_WHITE_TURN] == 1;
        //}

        //public static void SetCastleBit(BoardState board, CastlingBits castlingBit, bool isSet) {
        //    if (isSet) {
        //        board.CastlingBits |= castlingBit;
        //    } else {
        //        board.CastlingBits &= ~castlingBit;
        //    }
        //}

        //public static int GetKingPosition(byte[] board, bool white) {
        //    return white ? board[BoardStateOffset.WHITE_KING_POSITION] : board[BoardStateOffset.BLACK_KING_POSITION];
        //}

        //public static void SetKingPosition(byte[] board, bool white, int position) {
        //    if(white) {
        //        board[BoardStateOffset.WHITE_KING_POSITION] = (byte) position;
        //    } else {
        //        board[BoardStateOffset.BLACK_KING_POSITION] = (byte) position;
        //    }
        //}

        //public static void SetAllCastlingBit(byte[] board, CastlingBits castlingBits) {
        //    board[BoardStateOffset.CASTLING] = (byte)castlingBits;
        //}

        //public static CastlingBits GetCastleBit(byte[] board) {
        //    return (CastlingBits)board[BoardStateOffset.CASTLING];
        //}


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
            string move =   Convert.ToChar('a' + (column)) + (row + 1).ToString();
            return move;
        }

        //public static void SetEnPassantAttackedSquare(byte[] board, int position) {
        //    board[BoardStateOffset.EN_PASSANT_FIELD] = (byte)position;
        //}

        //public static int GetEnPassantAttackedSquare(byte[] board) {
        //    return board[BoardStateOffset.EN_PASSANT_FIELD];
        //}

        //public static void SetHalfTurnCounter(byte[] board, int halfTurnCounter) {
        //    //throw new NotImplementedException();
        //    board[BoardStateOffset.HALF_TURN_COUNTER] = (byte)halfTurnCounter;
        //}
        //public static void IncrementHalfTurnCounter(byte[] board) {
        //    //throw new NotImplementedException();
        //    board[BoardStateOffset.HALF_TURN_COUNTER] = (byte) (GetHalfTurnCounter(board) + 1);
        //}

        //public static int GetHalfTurnCounter(byte[] board) {
        //    return board[BoardStateOffset.HALF_TURN_COUNTER];
        //}

        //public static void SetFullMoveClock(byte[] board, int moveClock) {
        //    byte firstByte = (byte)(moveClock & 0xFF);
        //    byte secondByte = (byte)((moveClock & 0xFF00) >> 8);
        //    board[BoardStateOffset.TURN_COUNTER_1_FROM_RIGHT] = firstByte;
        //    board[BoardStateOffset.TURN_COUNTER_2_FROM_RIGHT] = secondByte;
        //}

        //public static int GetFullMoveClock(byte[] board) {
        //    return (board[BoardStateOffset.TURN_COUNTER_1_FROM_RIGHT] +
        //        (board[BoardStateOffset.TURN_COUNTER_2_FROM_RIGHT] << 8));
        //}

        //public static void IncrementFullMoveClock(byte[] board) {
        //    SetFullMoveClock(board,GetFullMoveClock(board) + 1);
        //}


        // me refers to the current player
        public static bool PieceBelongsToMe(BoardState board, Piece piece) {
            // we abuse that Piece stores the information about color in its last bit
            // therefor we can mask that bit away for example the piece 0x07 (white king) & 0x01(isWhitecheck) = 0x01
            // this always produces 1 or 0 which can be directly compared to isWhiteTurn.
            // IsWhite is either 0x01 (true) or 0x00 (false)
            return  board.IsWhiteTurn == (byte)(piece & Piece.IS_WHITE);
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
            return position - (PositionRow(position));
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
            foreach(var move in directSlantedMoves) {
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
                            if(isFirstPosition) {
                                Piece kingFilter = (theirColorPiece | Piece.KING);
                                if((piece & (Piece.PIECE_MASK | Piece.IS_WHITE)) == kingFilter) {
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
            if(IsValidPosition(leftPawnPosition)) {
                Piece leftPawn = board.GetPiece(leftPawnPosition);
                if(leftPawn == (theirColorPiece|Piece.PAWN)) {
                    return true;
                }
            }

            if(IsValidPosition(rightPawnPosition)) {
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

        private static void AddPawnMove(BoardState board, int fromPosition, int targetPosition, Move moveBits, List<Move> moves) {
            Piece takenPiece;
            int row = Board.PositionRow(targetPosition);
            if ((moveBits & Move.ENPASSANT) == Move.ENPASSANT) {
                takenPiece = Piece.PAWN;
            } else {
                takenPiece = board.GetPiece(targetPosition);
            }

            // check if the pawn is going to move into a promotion row.
            // we don't check the color because a pawn can never enter its own promotion area
            byte castlingBit = (byte) board.CastlingBits;
            if (row == 0 || row == 7) {
                moves.Add(MoveHelper.CreateMove(targetPosition, fromPosition, takenPiece, Piece.BISHOP, moveBits, board));
                moves.Add(MoveHelper.CreateMove(targetPosition, fromPosition, takenPiece, Piece.QUEEN, moveBits, board));
                moves.Add(MoveHelper.CreateMove(targetPosition, fromPosition, takenPiece, Piece.ROOK, moveBits, board));
                moves.Add(MoveHelper.CreateMove(targetPosition, fromPosition, takenPiece, Piece.KNIGHT, moveBits, board));
            } else {
                moves.Add(MoveHelper.CreateMove(targetPosition, fromPosition, takenPiece, Piece.EMPTY, moveBits, board));
            }
        }

        private static void AddMove(BoardState board, int fromPosition, int targetPosition, Move moveBits, List<Move> moves) {
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
                            AddMove(board, fromPosition, move, Move.EMPTY, moves);
                        } else if (moveOption == MoveOption.CAPTURE) {
                            AddMove(board, fromPosition, move, Move.EMPTY, moves);
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



        //public static bool IsValidMove(BoardState board, Move move) {
        //    var virtualBoard = new byte[BoardStateOffset.BOARD_STATE_SIZE];

        //    return IsValidMove(board, move, virtualBoard);
        //}
        
        public static ArrayPool<byte> arrayPool = ArrayPool<byte>.Create();

        //public static bool IsValidMove(BoardState board, Move move) {
        //    var virtualBoard = arrayPool.Rent(BoardStateOffset.BOARD_STATE_SIZE);

        //    var isValid = IsValidMove(board, move, virtualBoard);

        //    arrayPool.Return(virtualBoard);

        //    return isValid;
        //}


        public static bool IsValidMove(BoardState board, Move move) {
            byte myTurn = board.IsWhiteTurn;
            MakeMove(board, move);
            
            var attacked = !Attacked(board, board.GetKingPosition(myTurn), myTurn);

            UndoMove(board, move);

            return attacked;
        }


        public static List<Move> GetMoves(BoardState board, List<Move> moves = null) {
            if(moves == null) {
                moves = new List<Move>();
            }

            for (int column = 0; column < 8; column++) {
                for (int row = 0; row < 8*BoardStateOffset.ROW_OFFSET; row += BoardStateOffset.ROW_OFFSET) {
                    moves = GetMovesForPosition(board, column+row, moves);
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
                    int moveOne = RelativePosition(fromPosition, 0, 1);
                    if (IsPositionEmpty(board, moveOne)) {
                        //moves.Add(moveOne);
                        AddPawnMove(board, fromPosition, moveOne, Move.EMPTY, moves);
                    }

                    // check if the pawn is on the starting position. If it is then assume that it is possible to move forward
                    if (isWhitesTurn ? PositionRow(fromPosition) == 1 : PositionRow(fromPosition) == 6) {
                        int move = RelativePosition(fromPosition, 0, 2 * direction);
                        if (IsPositionEmpty(board, move)) {
                            AddPawnMove(board, fromPosition, move, Move.EMPTY, moves);
                        }
                    }

                    // check if the pawn is on the right column. 
                    // We don't need to check if there is a next row is outside the board.
                    // because the pawn is never able to stand on the last because of promotion
                    if (PositionColumn(fromPosition) != Board.H_COLUMN) {
                        int move = RelativePosition(fromPosition, 1, direction);
                        bool isEnpassant = board.EnPassantTarget == move;
                        if (
                            // targetposition has to either be the enpassant square.
                            isEnpassant
                            || CanITakeSquare(board, move) == MoveOption.CAPTURE)
                            // or be empty or contain an enemy) {
                            AddPawnMove(board, fromPosition, move, (isEnpassant ? Move.ENPASSANT : Move.ENPASSANT), moves);
                    }
                    // check if the pawn is on the left column. 
                    // We don't need to check if there is a next row is outside the board.
                    // because the pawn is never able to stand on the last because of promotion
                    if (PositionColumn(fromPosition) != Board.A_COLUMN) {
                        int move = RelativePosition(fromPosition, -1, direction);
                        bool isEnpassant = board.EnPassantTarget == move;
                        if (
                            // targetposition has to either be the enpassant square.
                            isEnpassant
                            || CanITakeSquare(board, move) == MoveOption.CAPTURE)
                            // or be empty or contain an enemy) {
                            AddPawnMove(board, fromPosition, move, (isEnpassant ? Move.ENPASSANT : Move.ENPASSANT), moves);
                    }

                    break;
                case Piece.KING:
                    foreach (var relativeMove in directKingMoves) {
                        int move = relativeMove + fromPosition;
                        if(IsValidPosition(move)) {
                            var moveOption = CanITakeSquare(board, move);
                            if (moveOption != MoveOption.INVALID) {
                                AddMove(board, fromPosition, move, Move.EMPTY, moves);
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
                        AddMove(board, fromPosition, fromPosition + 2, Move.EMPTY, moves);
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
                        AddMove(board,fromPosition, fromPosition - 2, Move.EMPTY, moves);
                    }

                    break;
                case Piece.KNIGHT:
                    foreach (var relativeMove in directKnightMoves) {
                        int move = fromPosition + relativeMove;
                        if (IsValidPosition(move)) {
                            var moveOption = CanITakeSquare(board, move);
                            if (moveOption != MoveOption.INVALID) {
                                AddMove(board, fromPosition, move, Move.EMPTY, moves);
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

        public static void UndoMove(BoardState copy, Move move) {
            int targetPosition = MoveHelper.MoveTargetPos(move);
            int fromPosition = MoveHelper.MoveFromPos(move);
            int theirColour = copy.IsWhiteTurn;
            int ourTurn = copy.IsWhiteTurn ^ 1;

            copy.bytes[fromPosition] = copy.bytes[targetPosition];
            //copy.bytes[targetPosition] = (byte) Piece.EMPTY;
            copy.bytes[targetPosition] = (byte) MoveHelper.MoveCaptured(move, theirColour);

            Piece movedPiece = copy.GetPiece(fromPosition);

            CastlingBits previous = MoveHelper.MovePreviousCastlingBits(move);
            copy.bytes[BoardStateOffset.HALF_TURN_COUNTER] = (byte) MoveHelper.PreviousHalfMove(move);
            copy.bytes[BoardStateOffset.CASTLING] = (byte)previous;

            if ((move & Move.ENPASSANT) == Move.ENPASSANT) {
                // when undoing a enpassant move spawn their pawn back
                // we abuse that isWhite is a integer which is 1 on whites turn and 0 and blacks turn
                // if it was black move then we have to spawn it one row above
                // if it was whites move we spawn it one move below
                // keep in mind the IsWhiteTurn is currently opposite of who made the move
                int enpassantSpawnPosition = targetPosition - BoardStateOffset.ROW_OFFSET + 2 * BoardStateOffset.ROW_OFFSET * copy.IsWhiteTurn;
                copy.SetPiece(enpassantSpawnPosition, Piece.PAWN | (Piece)copy.IsWhiteTurn);
            }

            // if black made a move decrement the turn counter
            // we abuse that isWhite is a integer which is 1 on whites turn and 0 and blacks turn
            copy.TurnCounter -= copy.IsWhiteTurn;

            if ((movedPiece & Piece.KING) == Piece.KING) {
                if (MoveHelper.MoveIsCastle(fromPosition, targetPosition)) {
                    // if the target move is less than the kingsposition it is queenside castling, 
                    // otherwise it is kingside castle 
                    if (targetPosition < fromPosition) {
                        // copy the rook back to its starting position 
                        copy.bytes[fromPosition - 4] = copy.bytes[fromPosition - 1];
                        copy.bytes[fromPosition - 1] = 0;
                    } else {
                        copy.bytes[fromPosition + 3] = copy.bytes[fromPosition + 1];
                        copy.bytes[fromPosition + 1] = 0;
                    }
                }
                copy.SetKingPosition(ourTurn, (byte)fromPosition);
            }

            // switch turn back to whites turn
            copy.IsWhiteTurn = (byte) ourTurn;







            long i = 0b1000_0000_0000_0000__0000_0000_0000_0000_0000;
        }

        public static void MakeMove(BoardState board, Move move) {
            int toPosition = MoveHelper.MoveTargetPos(move);
            int fromPosition = MoveHelper.MoveFromPos(move);
            bool isWhitesTurn = board.IsWhiteTurnBool;

            Piece piece = board.GetPiece(fromPosition);
            Piece pieceType = piece & Piece.PIECE_MASK;
            Piece takenPiece = board.GetPiece(toPosition);
            Piece promotion = MoveHelper.MovePromotion(move, board.IsWhiteTurn);

            switch (pieceType) {
                case Piece.PAWN:
                    if (MoveHelper.MoveIsEnpassant(move)) {
                        // When taking with enpassant remove the piece
                        if (isWhitesTurn) {
                            board.SetPiece(toPosition - BoardStateOffset.ROW_OFFSET, Piece.EMPTY);
                            //Board.PutPiece(board, toPosition - BoardOffset.ROW, Piece.EMPTY);
                        } else {
                            board.SetPiece(toPosition + BoardStateOffset.ROW_OFFSET, Piece.EMPTY);
                            //Board.PutPiece(board, toPosition + BoardOffset.ROW, Piece.EMPTY);
                        }
                    }
                    if (MoveHelper.MoveIsBigPawn(fromPosition,toPosition)) {
                        // when making a big pawn move mark the square behind the moving pawn vulnerable to 
                        if (isWhitesTurn) {
                            board.EnPassantTarget =(byte) (toPosition - BoardStateOffset.ROW_OFFSET);
                            //Board.SetEnPassantAttackedSquare(board, toPosition - BoardOffset.ROW);
                        } else {
                            board.EnPassantTarget = (byte)(toPosition + BoardStateOffset.ROW_OFFSET);
                            //Board.SetEnPassantAttackedSquare(board, toPosition + BoardOffset.ROW);
                        }
                    } else {
                        board.EnPassantTarget = EnPassant.NO_ENPASSANT;
                        //Board.SetEnPassantAttackedSquare(board, EnPassant.NO_ENPASSANT);
                    }
                    break;
                case Piece.KING:
                    //SetKingPosition(board, isWhitesTurn, toPosition);
                    if(MoveHelper.MoveIsCastle(fromPosition, toPosition)) {
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
                board.SetPiece(toPosition, promotion);
            }

            // remove piece from previous position
            board.SetPiece(fromPosition, Piece.EMPTY);

            if(pieceType == Piece.PAWN || ((takenPiece & Piece.PIECE_MASK) != Piece.EMPTY)) {
                //Board.SetHalfTurnCounter(board, 0);
                board.HalfTurnCounter = 0;
            } else {
                //Board.IncrementHalfTurnCounter(board);
                board.HalfTurnCounter++;
            }


            if(!isWhitesTurn) {
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
