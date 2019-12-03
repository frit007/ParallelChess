using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ParallelChess {
    public static class Board {

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
        // converts it to a number from 0 to 63, which is the number system used internally
        // examples
        // a1 -> 0
        // a3 -> 2
        // h8 -> 63
        public static int AlgebraicPosition(string readablePosition) {
            readablePosition = readablePosition.ToLower();

            // abuse the ascii system by converting the row to a number from 0 to 7
            // subtract 'a' because then the number will start from 0
            // a -> 0 * 8 = 0
            // c -> 2 * 8 = 16
            // h -> 7 * 8 = 56
            int algebraicPosition = (readablePosition[0] - 'a') * BoardStateOffset.ROW_OFFSET;

            // add the column to the positon, subtract one because 
            algebraicPosition += int.Parse(readablePosition.Substring(1, 0)) - 1;

            return algebraicPosition;
        }

        // convert from internal number format to a readble form
        // examples
        // 0 -> a1
        // 2 -> a3
        // 63 -> h8
        public static string ReadablePosition(int algebraicPosition) {
            int row = algebraicPosition / BoardStateOffset.ROW_OFFSET;
            int column = algebraicPosition - (row * BoardStateOffset.ROW_OFFSET);
            return ('a' + (row - 1)).ToString() + column.ToString();
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


        public static bool Attacked(BoardState board, int position, bool pretendToBeWhite) {
            // TODO implement
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
                moves.Add(MoveHelper.CreateMove(targetPosition, fromPosition, takenPiece, Piece.BISHOP, moveBits, castlingBit));
                moves.Add(MoveHelper.CreateMove(targetPosition, fromPosition, takenPiece, Piece.BISHOP, moveBits, castlingBit));
                moves.Add(MoveHelper.CreateMove(targetPosition, fromPosition, takenPiece, Piece.BISHOP, moveBits, castlingBit));
                moves.Add(MoveHelper.CreateMove(targetPosition, fromPosition, takenPiece, Piece.BISHOP, moveBits, castlingBit));
            } else {
                moves.Add(MoveHelper.CreateMove(targetPosition, fromPosition, takenPiece, Piece.EMPTY, moveBits, castlingBit));
            }
        }

        private static void AddMove(BoardState board, int fromPosition, int targetPosition, Move moveBits, List<Move> moves) {
            Piece takenPiece = board.GetPiece(targetPosition);
            moves.Add(MoveHelper.CreateMove(targetPosition, fromPosition, takenPiece, Piece.EMPTY, moveBits, (byte) board.CastlingBits));
        }

        private static void WalkRelativePaths(BoardState board, int fromPosition, int[] movePositions, List<Move> moves) {
            foreach (var relativePosition in movePositions) {
                int move = 0;
                do {
                    move = relativePosition + fromPosition;
                    if ((move & 0x88) == 0) {
                        var moveOption = CanITakeSquare(board, move);
                        if (moveOption == MoveOption.CAPTURE) {
                            AddMove(board, fromPosition, move, Move.EMPTY, moves);
                            break;
                        } else if (moveOption == MoveOption.NO_FIGHT) {
                            AddMove(board, fromPosition, move, Move.EMPTY, moves);
                        } else {
                            break;
                        }
                    } else {
                        break;
                    }
                } while (true);
            }
        }

        public static bool IsValidMove(BoardState board, Move move) {
            // TODO: OPTIMIZE: make sure we don't create a virtual board every time
            var virtualBoard = CreateCopyBoard(board);

            MakeMove(virtualBoard, move);

            bool isWhite = board.IsWhiteTurnBool;
            
            return Attacked(virtualBoard, virtualBoard.GetKingPosition(board.IsWhiteTurn), isWhite);
        }


        public static List<Move> GetMoves(BoardState board, List<Move> moves = null) {
            if(moves == null) {
                moves = new List<Move>();
            }

            for(int i = 0; i<64;i++) {
                moves = GetMovesForPosition(board, i, moves);
            }

            return moves;
        }

        public static List<Move> GetMovesForPosition(BoardState board, int fromPosition, List<Move> moves = null) {
            if (moves == null) {
                moves = new List<Move>();
            }
            //Piece piece = Board.GetPiece(board, fromPosition);
            Piece piece = board.GetPiece(fromPosition);
            if (!PieceBelongsToMe(board, piece)) {
                return moves;
            }
            Piece justPiece = piece & Piece.PIECE_MASK;

            //Action<byte[],int,int,Move,List<Move>> addPawnMove = (byte[] _d1, int _d2, int targetPosition, Move moveBits, List<Move> _d3) => {
            //    Piece takenPiece;
            //    int row = Board.PositionRow(targetPosition);
            //    if ((moveBits & Move.ENPASSANT) == Move.ENPASSANT) {
            //        takenPiece = Piece.PAWN;
            //    } else {
            //        takenPiece = GetPiece(board, targetPosition);
            //    }

            //    // check if the pawn is going to move into a promotion row.
            //    // we don't check the color because a pawn can never enter its own promotion area
            //    byte castlingBit = (byte)Board.GetCastleBit(board);
            //    if (row == 0 || row == 7) {
            //        moves.Add(MoveHelper.CreateMove(targetPosition, fromPosition, takenPiece, Piece.BISHOP, moveBits, castlingBit));
            //        moves.Add(MoveHelper.CreateMove(targetPosition, fromPosition, takenPiece, Piece.BISHOP, moveBits, castlingBit));
            //        moves.Add(MoveHelper.CreateMove(targetPosition, fromPosition, takenPiece, Piece.BISHOP, moveBits, castlingBit));
            //        moves.Add(MoveHelper.CreateMove(targetPosition, fromPosition, takenPiece, Piece.BISHOP, moveBits, castlingBit));
            //    } else {
            //        moves.Add(MoveHelper.CreateMove(targetPosition, fromPosition, takenPiece, Piece.EMPTY, moveBits, castlingBit));
            //    }
            //};
            
            //Action<byte[], int, int, Move, List<Move>> AddMove = (byte[] _d1, int _d2, int targetPosition, Move moveBits, List<Move> _d3) => {
            //    Piece takenPiece = GetPiece(board, targetPosition);
            //    moves.Add(MoveHelper.CreateMove(targetPosition, fromPosition, takenPiece, Piece.EMPTY, moveBits, (byte)Board.GetCastleBit(board)));
            //};

            //Action<byte[], int, (int, int)[], List<Move>> walkRelativePaths = (byte[] _d1, int _d2, (int column, int row)[] movePositions, List<Move> _d3) => {
            //    foreach (var relativeMove in movePositions) {
            //        int move = 0;
            //        int count = 0;
            //        do {
            //            if (IsValidPosition(fromPosition, relativeMove.column * count, relativeMove.row * count)) {
            //                move = RelativePosition(fromPosition, relativeMove.column * count, relativeMove.row * count);
            //                var moveOption = CanTakeSquare(board, move);
            //                if (moveOption == MoveOption.CAPTURE) {
            //                    AddMove(board, fromPosition, move, Move.EMPTY, moves);
            //                    break;
            //                } else if (moveOption == MoveOption.NO_FIGHT) {
            //                    AddMove(board, fromPosition, move, Move.EMPTY, moves);
            //                } else {
            //                    break;
            //                }
            //            } else {
            //                break;
            //            }
            //        } while (true);
            //    }
            //};
            

            // is the piece of the same color as the current turn
            // TODO: maybe move this check out to a higher level

            switch (justPiece) {
                case Piece.PAWN:
                    bool isWhitesTurn = board.IsWhiteTurnBool;
                    int direction = isWhitesTurn ? 1 : -1;

                    // We don't need to check if there is a next row is outside the board.
                    // because the pawn is never able to stand on the last because of promotion
                    int moveOne = RelativePosition(fromPosition, 0, 1);
                    if (IsPositionEmpty(board, moveOne)) {
                        //moves.Add(moveOne);
                        AddPawnMove(board, fromPosition, moveOne, Move.PAWN_MOVE, moves);
                    }

                    // check if the pawn is on the starting position. If it is then assume that it is possible to move forward
                    if (isWhitesTurn ? PositionRow(fromPosition) == 1 : PositionRow(fromPosition) == 6) {
                        int move = RelativePosition(fromPosition, 0, 2 * direction);
                        if (IsPositionEmpty(board, move)) {
                            AddPawnMove(board, fromPosition, move, Move.BIG_PAWN_MOVE | Move.PAWN_MOVE, moves);
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
                            AddPawnMove(board, fromPosition, move, Move.PAWN_MOVE | (isEnpassant ? Move.ENPASSANT : Move.ENPASSANT), moves);
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
                            AddPawnMove(board, fromPosition, move, Move.PAWN_MOVE | (isEnpassant ? Move.ENPASSANT : Move.ENPASSANT), moves);
                    }

                    break;
                case Piece.KING:
                    foreach (var relativeMove in directKingMoves) {
                        int move = relativeMove + fromPosition;
                        if((0x88 & move) == 0) {
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
                    bool isWhite = board.IsWhiteTurnBool;
                    // check if they are allowed to castle
                    // 1. Check history if rook or king has moved. This information is stored in the castlingBit
                    // 2. Check if the king moves through any square that is under attack
                    // 3. Check if the king moves through a square that is not empty
                    if (CanCastleKingSide(board)
                        && !Attacked(board, fromPosition + 0, isWhite)
                        && !Attacked(board, fromPosition + 1, isWhite)
                        && !Attacked(board, fromPosition + 2, isWhite)
                        && IsPositionEmpty(board, fromPosition + 1)
                        && IsPositionEmpty(board, fromPosition + 2)
                        ) {
                        AddMove(board, fromPosition, fromPosition + 2, Move.CASTLING, moves);
                    }

                    // Do the same
                    if (CanCastleKingSide(board)
                        && !Attacked(board, fromPosition - 0, isWhite)
                        && !Attacked(board, fromPosition - 1, isWhite)
                        && !Attacked(board, fromPosition - 2, isWhite)
                        && !Attacked(board, fromPosition - 3, isWhite)
                        && IsPositionEmpty(board, fromPosition - 1)
                        && IsPositionEmpty(board, fromPosition - 2)
                        && IsPositionEmpty(board, fromPosition - 2)
                        ) {
                        AddMove(board,fromPosition, fromPosition - 3, Move.CASTLING, moves);
                    }

                    break;
                case Piece.KNIGHT:
                    foreach (var relativeMove in directKnightMoves) {
                        int move = fromPosition + relativeMove;
                        if ((move & 0x88) == 0) {
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


        public static void MakeMove(BoardState board, Move move) {
            int toPosition = MoveHelper.MoveTargetPos(move);
            int fromPosition = MoveHelper.MoveFromPos(move);
            bool isWhitesTurn = board.IsWhiteTurnBool;

            Piece piece = board.GetPiece(fromPosition);
            Piece pieceType = piece & Piece.PIECE_MASK;
            Piece takenPiece = board.GetPiece(toPosition);
            Piece promotion = MoveHelper.MovePromotion(move);

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
                    if ((move & Move.BIG_PAWN_MOVE) == Move.BIG_PAWN_MOVE) {
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
