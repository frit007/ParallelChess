using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace ParallelChess {


    // Board is essentially a wrapper around a array, making is easier to perform actions on the array(and enabling typechecking)
    // Ideally the Board shuold not include any other variables outside the array since the board is a struct, 
    // which will be copied by value every time it is used in an argument. 
    // This also means that it technically shuold be as efficient as using a byte[] directly, 
    // since all it is doing is passing the pointer to the array.
    public struct Board {
        public byte[] bytes;
        public Board(byte[] bytes) {
            // temporaily assign ints and shorts to make the compiler happy
            //shorts = new short[0];
            this.bytes = bytes;
        }

        #region get/set pieces
        public Piece GetPiece(int position) {
            return (Piece) bytes[position];
        }
        public void SetPiece(int position, Piece piece) {
            bytes[position] = (byte) piece;
        }
        #endregion

        #region properties
        //[FieldOffset(64)]
        //private CastlingBits castlingBits; //  size 4
        public CastlingBits CastlingBits {
            get {
                return (CastlingBits)this.bytes[BoardStateOffset.CASTLING];
            }
            set { 
                this.bytes[BoardStateOffset.CASTLING] = (byte)value;
            }
        }

        public byte EnPassantTarget { 
            get { return bytes[BoardStateOffset.EN_PASSANT_FIELD]; } 
            set { bytes[BoardStateOffset.EN_PASSANT_FIELD] = value; } 
        }
        public byte HalfTurnCounter { 
            get { return bytes[BoardStateOffset.HALF_TURN_COUNTER]; } 
            set { bytes[BoardStateOffset.HALF_TURN_COUNTER] = value; }
        }

        public byte GetKingPosition(int isWhite) {
            return bytes[BoardStateOffset.BLACK_KING_POSITION + isWhite];
        }

        public void SetKingPosition(int isWhite, byte position) {
            bytes[BoardStateOffset.BLACK_KING_POSITION + isWhite] = position;
        }

        public byte WhiteKingPosition { 
            get { return bytes[BoardStateOffset.WHITE_KING_POSITION]; }
            set { bytes[BoardStateOffset.WHITE_KING_POSITION] = value; } 
        }

        public byte BlackKingPosition {
            get { return bytes[BoardStateOffset.BLACK_KING_POSITION]; }
            set { bytes[BoardStateOffset.BLACK_KING_POSITION] = value; }
        }

        //public short TurnCounter {
        //    get {
        //        return 0;
        //        //return (short)(bytes[BoardStateOffset.TURN_COUNTER_1_FROM_RIGHT]
        //        //    | (bytes[BoardStateOffset.TURN_COUNTER_2_FROM_RIGHT] << 8));
        //        //return shorts[BoardStateOffset.TURN_COUNTER_FROM_SHORT]; 
        //    }
        //    set {
        //        //shorts[BoardStateOffset.TURN_COUNTER_FROM_SHORT] = value; 
        //        //bytes[BoardStateOffset.TURN_COUNTER_1_FROM_RIGHT] = (byte)(value & 0xff);
        //        //bytes[BoardStateOffset.TURN_COUNTER_2_FROM_RIGHT] = (byte)((value & 0xff00) >> 8);
        //    } 
        //}

        public byte VirtualLevel { 
            get { return bytes[BoardStateOffset.VIRTUAL_LEVEL]; } 
            set { bytes[BoardStateOffset.VIRTUAL_LEVEL] = value; } 
        }

        public byte IsWhiteTurn { 
            get { return bytes[BoardStateOffset.IS_WHITE_TURN]; }
            set { bytes[BoardStateOffset.IS_WHITE_TURN] = value; }
        }
        public bool IsWhiteTurnBool {
            get { return 0 != bytes[BoardStateOffset.IS_WHITE_TURN]; }
            set { bytes[BoardStateOffset.IS_WHITE_TURN] = (byte)(value ? 1 : 0); }
        }
        // Forsyth–Edwards Notation of the current chess board state
        // the board doesn't keep track of the moves since it doesn't affect any chess rule
        // that part is stored in the Chess wrapper
        public string simplifiedFEN {
            get { return ChessOutput.BoardToFen(this, 0); }
        }
        #endregion

        #region GetMoves

        // me refers to the current player
        
        public bool PieceBelongsToMe(Piece piece) {
            // we abuse that Piece stores the information about color in its last bit
            // therefor we can mask that bit away for example the piece 0x07 (white king) & 0x01(isWhitecheck) = 0x01
            // this always produces 1 or 0 which can be directly compared to isWhiteTurn.
            // IsWhite is either 0x01 (true) or 0x00 (false)
            return IsWhiteTurn == (byte)(piece & Piece.IS_WHITE);
        }

        
        public bool IsPositionEmpty(int position) {
            return GetPiece(position) == Piece.EMPTY;
        }

        public bool IsPositionEmptyByte(int position) {
            return bytes[position] == 0;
        }

        public static bool IsPositionEmptyByte(Board board, int position) {
            return board.bytes[position] == 0;
        }

        public static bool IsPositionEmpty(Board board, int position) {
            return board.GetPiece(position) == Piece.EMPTY;
        }

        public MoveOption CanITakeSquare(int position) {
            var piece = GetPiece(position);
            if (piece == Piece.EMPTY) {
                //return true;
                return MoveOption.NO_FIGHT;
            }

            // check that the current color is not the same as the moving piece
            if (PieceBelongsToMe(piece)) {
                return MoveOption.INVALID;
            } else {
                return MoveOption.CAPTURE;
            }
        }
        public static int[] kingMoves = {
            BoardStateOffset.ROW_OFFSET * 1 + 1,
            BoardStateOffset.ROW_OFFSET * -1 + 1,
            BoardStateOffset.ROW_OFFSET * 1 + -1,
            BoardStateOffset.ROW_OFFSET * -1 + -1,
            BoardStateOffset.ROW_OFFSET,
            -BoardStateOffset.ROW_OFFSET,
            1,
            -1
        };

        public static int[] straightMoves = {
            BoardStateOffset.ROW_OFFSET,
            -BoardStateOffset.ROW_OFFSET,
            1,
            -1
        };

        public static int[] slantedMoves = {
            BoardStateOffset.ROW_OFFSET * 1 + 1,
            BoardStateOffset.ROW_OFFSET * -1 + 1,
            BoardStateOffset.ROW_OFFSET * 1 + -1,
            BoardStateOffset.ROW_OFFSET * -1 + -1,
        };

        public static int[] knightMoves = {
                   -1 + BoardStateOffset.ROW_OFFSET * 2 , 1 + BoardStateOffset.ROW_OFFSET * 2,
            -2 + BoardStateOffset.ROW_OFFSET * 1,              2 + BoardStateOffset.ROW_OFFSET * 1,
            -2 + BoardStateOffset.ROW_OFFSET * -1,              2 + BoardStateOffset.ROW_OFFSET * -1,
                   -1 + BoardStateOffset.ROW_OFFSET * -2 , 1 + BoardStateOffset.ROW_OFFSET * -2,
        };
        
        public bool CanICastleQueenSide() {
            CastlingBits castlingBits = CastlingBits;

            // TODO bitshift optimize?
            if (IsWhiteTurnBool) {
                return (castlingBits & CastlingBits.WHITE_QUEEN_SIDE_CASTLE) == CastlingBits.WHITE_QUEEN_SIDE_CASTLE;
            } else {
                return (castlingBits & CastlingBits.BLACK_QUEEN_SIDE_CASTLE) == CastlingBits.BLACK_QUEEN_SIDE_CASTLE;
            }
        }

        
        public bool CanCastleKingSide() {
            CastlingBits castlingBits = CastlingBits;

            // TODO bitshift optimize?
            if (IsWhiteTurnBool) {
                return (castlingBits & CastlingBits.WHITE_KING_SIDE_CASTLE) == CastlingBits.WHITE_KING_SIDE_CASTLE;
            } else {
                return (castlingBits & CastlingBits.BLACK_KING_SIDE_CASTLE) == CastlingBits.BLACK_KING_SIDE_CASTLE;
            }
        }

        
        public void AddPawnMove(int fromPosition, int targetPosition, MoveFlags move, List<Move> moves) {
            Piece takenPiece;
            int row = BoardPosition.PositionRow(targetPosition);
            if ((move & MoveFlags.ENPASSANT) == MoveFlags.ENPASSANT) {
                takenPiece = Piece.PAWN;
            } else {
                takenPiece = GetPiece(targetPosition);
            }

            // check if the pawn is going to move into a promotion row.
            // we don't check the color because a pawn can never enter its own promotion area
            if (row == 0 || row == 7) {
                moves.Add(MoveHelper.CreateMove(targetPosition, fromPosition, takenPiece, Piece.QUEEN, move, this));
                moves.Add(MoveHelper.CreateMove(targetPosition, fromPosition, takenPiece, Piece.BISHOP, move, this));
                moves.Add(MoveHelper.CreateMove(targetPosition, fromPosition, takenPiece, Piece.KNIGHT, move, this));
                moves.Add(MoveHelper.CreateMove(targetPosition, fromPosition, takenPiece, Piece.ROOK, move, this));
            } else {
                moves.Add(MoveHelper.CreateMove(targetPosition, fromPosition, takenPiece, Piece.EMPTY, move, this));
            }
        }

        public void AddMove(int fromPosition, int targetPosition, MoveFlags moveBits, List<Move> moves) {
            Piece takenPiece = GetPiece(targetPosition);
            moves.Add(MoveHelper.CreateMove(targetPosition, fromPosition, takenPiece, Piece.EMPTY, moveBits, this));
        }

        
        public void WalkRelativePaths(int fromPosition, int[] movePositions, List<Move> moves) {

            foreach (var relativePosition in movePositions) {
                int move = fromPosition;
                do {
                    move += relativePosition;
                    if (BoardPosition.IsValidPosition(move)) {
                        var moveOption = CanITakeSquare(move);
                        if (moveOption == MoveOption.NO_FIGHT) {
                            AddMove(fromPosition, move, MoveFlags.EMPTY, moves);
                        } else if (moveOption == MoveOption.CAPTURE) {
                            AddMove(fromPosition, move, MoveFlags.EMPTY, moves);
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
        private void RelativePath(int fromPosition, int[] relativePaths, List<Move> moves) {


            foreach (var relativePath in relativePaths) {
                int move = relativePath + fromPosition;
                if (BoardPosition.IsValidPosition(move)) {
                    var moveOption = CanITakeSquare(move);
                    if (moveOption != MoveOption.INVALID) {
                        AddMove(fromPosition, move, MoveFlags.EMPTY, moves);
                    }
                }
            }
        }

        
        public List<Move> GetMoves(List<Move> moves = null) {
            if (moves == null) {
                moves = new List<Move>();
            }

            for (int column = 0; column < 8; column++) {
                for (int row = 0; row < 8 * BoardStateOffset.ROW_OFFSET; row += BoardStateOffset.ROW_OFFSET) {
                    moves = GetMovesForPosition(column + row, moves);
                }
            }

            return moves;
        }

        
        public List<Move> GetMovesForPosition(int fromPosition, List<Move> moves = null) {
            if (moves == null) {
                moves = new List<Move>();
            }

            Piece piece = GetPiece(fromPosition);
            if (!PieceBelongsToMe(piece)) {
                // is the piece of the same color as the current turn
                // TODO: maybe move this check out to a higher level
                return moves;
            }
            Piece justPiece = piece & Piece.PIECE_MASK;



            switch (justPiece) {
                case Piece.PAWN:
                    bool isWhitesTurn = IsWhiteTurnBool;
                    int direction = isWhitesTurn ? 1 : -1;

                    // We don't need to check if there is a next row is outside the board.
                    // because the pawn is never able to stand on the last because of promotion
                    int moveOne = BoardPosition.RelativePosition(fromPosition, 0, direction);
                    if (IsPositionEmpty(moveOne)) {
                        AddPawnMove(fromPosition, moveOne, MoveFlags.PAWN_MOVE, moves);
                    }

                    // check if the pawn is on the starting position. If it is then assume that it is possible to move forward
                    if (isWhitesTurn ? BoardPosition.PositionRow(fromPosition) == 1 : BoardPosition.PositionRow(fromPosition) == 6) {
                        int hasToBeEmptyPosition = BoardPosition.RelativePosition(fromPosition, 0, direction);
                        int move = BoardPosition.RelativePosition(fromPosition, 0, 2 * direction);
                        if (IsPositionEmpty(move) && IsPositionEmpty(hasToBeEmptyPosition)) {
                            AddPawnMove(fromPosition, move, MoveFlags.BIG_PAWN_MOVE | MoveFlags.PAWN_MOVE, moves);
                        }
                    }

                    int column = BoardPosition.PositionColumn(fromPosition);
                    // check if the pawn is on the right column. 
                    // We don't need to check if there is a next row is outside the board.
                    // because the pawn is never able to stand on the last because of promotion
                    if (column != BoardPosition.H_COLUMN) {
                        int move = BoardPosition.RelativePosition(fromPosition, 1, direction);
                        bool isEnpassant = EnPassantTarget == move;
                        if (
                            // targetposition has to either be the enpassant square.
                            isEnpassant
                            || CanITakeSquare(move) == MoveOption.CAPTURE)
                            // or be empty or contain an enemy) {
                            AddPawnMove(fromPosition, move, MoveFlags.PAWN_MOVE | (isEnpassant ? MoveFlags.ENPASSANT : MoveFlags.EMPTY), moves);
                    }
                    // check if the pawn is on the left column. 
                    // We don't need to check if there is a next row is outside the board.
                    // because the pawn is never able to stand on the last because of promotion
                    if (column != BoardPosition.A_COLUMN) {
                        int move = BoardPosition.RelativePosition(fromPosition, -1, direction);
                        bool isEnpassant = EnPassantTarget == move;
                        if (
                            // targetposition has to either be the enpassant square.
                            isEnpassant
                            || CanITakeSquare(move) == MoveOption.CAPTURE)
                            // or be empty or contain an enemy) {
                            AddPawnMove(fromPosition, move, MoveFlags.PAWN_MOVE | (isEnpassant ? MoveFlags.ENPASSANT : MoveFlags.EMPTY), moves);
                    }

                    break;
                case Piece.KING:
                    RelativePath(fromPosition, kingMoves, moves);

                    byte isWhite = IsWhiteTurn;
                    // check if they are allowed to castle
                    // 1. Check history if rook or king has moved. This information is stored in the castlingBit
                    // 2. Check if the king moves through any square that is under attack
                    // 3. Check if the king moves through a square that is not empty
                    if (CanCastleKingSide()
                        && IsPositionEmpty(fromPosition + 1)
                        && IsPositionEmpty(fromPosition + 2)
                        && !Attacked(fromPosition + 0, isWhite)
                        && !Attacked(fromPosition + 1, isWhite)
                        && !Attacked(fromPosition + 2, isWhite)
                        ) {
                        AddMove(fromPosition, fromPosition + 2, MoveFlags.CASTLING, moves);
                    }

                    // Do the same queen side
                    if (CanICastleQueenSide()
                        && IsPositionEmpty(fromPosition - 1)
                        && IsPositionEmpty(fromPosition - 2)
                        && IsPositionEmpty(fromPosition - 3)
                        && !Attacked(fromPosition - 0, isWhite)
                        && !Attacked(fromPosition - 1, isWhite)
                        && !Attacked(fromPosition - 2, isWhite)
                        ) {
                        AddMove(fromPosition, fromPosition - 2, MoveFlags.CASTLING, moves);
                    }

                    break;
                case Piece.KNIGHT:
                    RelativePath(fromPosition, knightMoves, moves);

                    break;
                case Piece.QUEEN:
                    WalkRelativePaths(fromPosition, slantedMoves, moves);
                    WalkRelativePaths(fromPosition, straightMoves, moves);
                    break;
                case Piece.ROOK:
                    WalkRelativePaths(fromPosition, straightMoves, moves);
                    break;
                case Piece.BISHOP:
                    WalkRelativePaths(fromPosition, slantedMoves, moves);
                    break;
                case Piece.EMPTY:
                    break;
                default:
                    // assume it is either ROOK,BISHOP or QUEEN
                    throw new Exception("invalid piece: " + (int)piece);
            }

            return moves;
        }
        #endregion

        #region LegalMoves
        public bool Attacked(int position, byte pretendToBeWhite) {
            int theirColor = pretendToBeWhite ^ 1;
            Piece theirColorPiece = (Piece)theirColor;
            
            foreach (var move in slantedMoves) {
                int relativePosition = position;
                // king filter is used to allow kings to attack one square
                // they are disabled are the first rotation
                bool isFirstPosition = true;
                do {
                    relativePosition += move;
                    if (BoardPosition.IsValidPosition(relativePosition)) {
                        var piece = GetPiece(relativePosition);
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

            foreach (var move in straightMoves) {
                int relativePosition = position;
                // king filter is used to allow kings to attack one square
                // they are disabled are the first square
                bool isFirstPosition = true;
                do {
                    relativePosition += move;
                    if (BoardPosition.IsValidPosition(relativePosition)) {
                        var piece = GetPiece(relativePosition);
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

            foreach (var move in knightMoves) {
                int relativePosition = position + move;

                if (BoardPosition.IsValidPosition(relativePosition)) {
                    var piece = GetPiece(relativePosition);
                    Piece enemySlantedAttacked = (theirColorPiece | Piece.KNIGHT);
                    if ((piece & (Piece.PIECE_MASK | Piece.IS_WHITE)) == enemySlantedAttacked) {
                        return true;
                    }
                }
            }

            int leftPawnPosition = position - BoardStateOffset.ROW_OFFSET - 1 + BoardStateOffset.ROW_OFFSET * pretendToBeWhite * 2;
            int rightPawnPosition = position - BoardStateOffset.ROW_OFFSET + 1 + BoardStateOffset.ROW_OFFSET * pretendToBeWhite * 2;
            if (BoardPosition.IsValidPosition(leftPawnPosition)) {
                Piece leftPawn = GetPiece(leftPawnPosition);
                if (leftPawn == (theirColorPiece | Piece.PAWN)) {
                    return true;
                }
            }

            if (BoardPosition.IsValidPosition(rightPawnPosition)) {
                Piece rightPawn = GetPiece(rightPawnPosition);
                if (rightPawn == (theirColorPiece | Piece.PAWN)) {
                    return true;
                }
            }

            return false;
        }


        public bool IsLegalMove(Move move) {
            byte myTurn = IsWhiteTurn;
            Move(move);

            var notAttacked = !Attacked(GetKingPosition(myTurn), myTurn);

            UndoMove(move);

            return notAttacked;
        }

        #endregion

        #region MakeMove

        public Move FindMove(int from, int to) {
            List<Move> moves = GetMovesForPosition(from);

            Move targetPosition = moves.FindTargetPosition(to);
            if (!MoveHelper.isValidMove(targetPosition)) {
                throw new Exception("Move not found");
            }
            if (!IsLegalMove(targetPosition)) {
                throw new Exception("Illegal move");
            }

            return targetPosition;
        }

        public Move FindMove(int from, int to, Piece promotion) {
            List<Move> moves = GetMovesForPosition(from);

            Move targetPosition = moves.FindTargetPosition(to, promotion);
            if (!MoveHelper.isValidMove(targetPosition)) {
                throw new Exception("Move not found");
            }

            if (!IsLegalMove(targetPosition)) {
                throw new Exception("Illegal move");
            }

            return targetPosition;
        }

        public Move Move(int from, int to) {
            Move targetPosition = FindMove(from, to);

            Move(targetPosition);

            return targetPosition;
        }

        public Move Move(string san) {
            foreach (var move in GetMoves()) {
                if (IsLegalMove(move) && StandardAlgebraicNotation(move) == san.Trim()) {
                    Move(move);
                    return move;
                }
            }

            throw new Exception("Move not found!");
        }

        public Move Move(int from, int to, Piece promotion) {
            Move targetPosition = FindMove(from, to, promotion);

            Move(targetPosition);

            return targetPosition;
        }

        public void Move(Move move) {
            int toPosition = move.targetPosition;
            int fromPosition = move.fromPosition;
            bool isWhitesTurn = IsWhiteTurnBool;

            Piece piece = GetPiece(fromPosition);
            Piece pieceType = piece & Piece.PIECE_MASK;
            Piece takenPiece = GetPiece(toPosition);
            Piece promotion = (Piece)move.promotion;
            MoveFlags moveFlags = (MoveFlags)move.moveFlags;

            if ((moveFlags & MoveFlags.BIG_PAWN_MOVE) == MoveFlags.BIG_PAWN_MOVE) {
                // when making a big pawn move mark the square behind the moving pawn vulnerable to 
                if (isWhitesTurn) {
                    EnPassantTarget = (byte)(toPosition - BoardStateOffset.ROW_OFFSET);
                } else {
                    EnPassantTarget = (byte)(toPosition + BoardStateOffset.ROW_OFFSET);
                }
            } else {
                EnPassantTarget = EnPassant.NO_ENPASSANT;
            }
            switch (pieceType) {
                case Piece.PAWN:
                    if ((moveFlags & MoveFlags.ENPASSANT) == MoveFlags.ENPASSANT) {
                        // When taking with enpassant remove the piece
                        if (isWhitesTurn) {
                            SetPiece(toPosition - BoardStateOffset.ROW_OFFSET, Piece.EMPTY);
                        } else {
                            SetPiece(toPosition + BoardStateOffset.ROW_OFFSET, Piece.EMPTY);
                        }
                    }

                    break;
                case Piece.KING:
                    if ((moveFlags & MoveFlags.CASTLING) == MoveFlags.CASTLING) {
                        if (BoardPosition.PositionColumn(toPosition) < BoardPosition.E_COLUMN) {
                            // if position is less than E_COLUMN then we are castling queen side
                            // copy the rook from the square so we correctly handle the color
                            SetPiece(toPosition + 1, GetPiece(toPosition - 2));
                            SetPiece(toPosition - 2, Piece.EMPTY);
                        } else {
                            // otherwise we are castling king side
                            // copy the rook from the square so we correctly handle the color
                            SetPiece(toPosition - 1, GetPiece(toPosition + 1));
                            SetPiece(toPosition + 1, Piece.EMPTY);
                        }
                    }
                    SetKingPosition(IsWhiteTurn, (byte)toPosition);
                    break;
            }


            // remove opportunity to castle based on the position on the board
            CastlingBits = CastlingBits
                & CastlingHelper.castleLookup[toPosition]
                & CastlingHelper.castleLookup[fromPosition];


            // move piece to new position
            if (promotion == Piece.EMPTY) {
                SetPiece(toPosition, piece);
            } else {
                SetPiece(toPosition, promotion | (Piece)IsWhiteTurn);
            }

            // remove piece from previous position
            SetPiece(fromPosition, Piece.EMPTY);

            if (pieceType == Piece.PAWN || ((takenPiece & Piece.PIECE_MASK) != Piece.EMPTY)) {
                HalfTurnCounter = 0;
            } else {
                HalfTurnCounter++;
            }


            //if (!isWhitesTurn) {
            //    // increment fullMoveClock after blacks turn
            //    TurnCounter++;
            //}

            // flip turn
            IsWhiteTurn = (byte)(IsWhiteTurn ^ 1);
        }


        public void UndoMove(Move move) {
            int targetPosition = move.targetPosition;
            int fromPosition = move.fromPosition;
            int theirColor = IsWhiteTurn;
            int ourColor = IsWhiteTurn ^ 1;

            bytes[fromPosition] = bytes[targetPosition];

            bytes[targetPosition] = move.capturedPiece;

            Piece movedPiece = GetPiece(fromPosition);

            CastlingBits previous = (CastlingBits)move.previousCastlingBits;
            bytes[BoardStateOffset.HALF_TURN_COUNTER] = move.previousHalfMove;
            bytes[BoardStateOffset.CASTLING] = move.previousCastlingBits;
            bytes[BoardStateOffset.EN_PASSANT_FIELD] = move.previousEnpassant;
            MoveFlags moveFlags = (MoveFlags)move.moveFlags;

            if ((moveFlags & MoveFlags.ENPASSANT) == MoveFlags.ENPASSANT) {
                // when undoing a enpassant move spawn their pawn back
                // we abuse that isWhite is a integer which is 1 on whites turn and 0 and blacks turn
                // if it was black move then we have to spawn it one row above
                // if it was whites move we spawn it one move below
                // keep in mind the IsWhiteTurn is currently opposite of who made the move
                int enpassantSpawnPosition = targetPosition - BoardStateOffset.ROW_OFFSET + 2 * BoardStateOffset.ROW_OFFSET * IsWhiteTurn;
                SetPiece(enpassantSpawnPosition, Piece.PAWN | (Piece)theirColor);

                // when capturing with enpassant don't place the captured piece back since it was taken from another square
                bytes[targetPosition] = (byte)Piece.EMPTY;
            }
            if (move.promotion != 0) {
                bytes[fromPosition] = (byte)(Piece.PAWN | (Piece)ourColor);
            }

            // if black made a move decrement the turn counter
            // we abuse that isWhite is a integer which is 1 on whites turn and 0 and blacks turn
            //TurnCounter -= IsWhiteTurn;

            if ((movedPiece & Piece.PIECE_MASK) == Piece.KING) {
                if ((moveFlags & MoveFlags.CASTLING) == MoveFlags.CASTLING) {
                    // if the target move is less than the kingsposition it is queenside castling, 
                    // otherwise it is kingside castle 
                    if (targetPosition < fromPosition) {
                        // copy the rook back to its starting position 
                        bytes[fromPosition - 4] = bytes[fromPosition - 1];
                        bytes[fromPosition - 1] = 0;
                    } else {
                        bytes[fromPosition + 3] = bytes[fromPosition + 1];
                        bytes[fromPosition + 1] = 0;
                    }
                }
                SetKingPosition(ourColor, (byte)fromPosition);
            }

            // switch turn back to whites turn
            IsWhiteTurn = (byte)ourColor;
        }
        #endregion

        #region detectWinner
        public Winner detectWinnerAreThereValidMoves(bool areThereValidMoves) {
            if (!areThereValidMoves) {
                // check if king is under attack
                if (Attacked(GetKingPosition(IsWhiteTurn), IsWhiteTurn)) {
                    if (IsWhiteTurnBool) {
                        return Winner.WINNER_BLACK;
                    } else {
                        return Winner.WINNER_WHITE;
                    }
                } else {
                    return Winner.DRAW;
                }
            }

            if (hasInsufficientMaterialOrTimeLimit()) {
                return Winner.DRAW;
            }

            return Winner.NONE;
        }

        public bool hasInsufficientMaterialOrTimeLimit() {
            return HalfTurnCounter == 50 || detectInsufficientMaterial();
        }

        public Dictionary<ulong, int> RepeatedPositions(IEnumerable<Move> history) {
            Stack<Move> madeMoves = new Stack<Move>();

            var repeatedPositions = new Dictionary<ulong, int>();
            var boardHash = HashBoard.hash(this);
            repeatedPositions[boardHash] = 1;
            foreach (var move in history.Reverse()) {
                if ((Piece)move.capturedPiece != Piece.EMPTY || (MoveFlags)move.moveFlags == MoveFlags.PAWN_MOVE) {
                    while (madeMoves.Count != 0) {
                        // replay the moves we undid
                        this.Move(madeMoves.Pop());
                    }
                    // if a piece was captured or a pawn was moved then the previous position cannot occur again, 
                    // meaning it is pointless to continue
                    return repeatedPositions;
                }
                this.UndoMove(move);
                boardHash = HashBoard.ApplyMove(this, move, boardHash);
                madeMoves.Push(move);
                if (repeatedPositions.ContainsKey(boardHash)) {
                    repeatedPositions[boardHash]++;
                } else {
                    repeatedPositions[boardHash] = 1;
                }
            }
            while (madeMoves.Count != 0) {
                // replay the moves we undid
                this.Move(madeMoves.Pop());
            }
            return repeatedPositions;
        }

        // history is used to check for repeated positions
        public Winner detectWinner(IEnumerable<Move> possibleMoves, IEnumerable<Move> history) {
            var winner = detectWinner(possibleMoves);
            if (winner != Winner.NONE) {
                return winner;
            }

            var repeatedPositions = this.RepeatedPositions(history);

            foreach (var position in repeatedPositions) {
                if (position.Value >= 3) {
                    // if any position has occured more than 3 times then it a draw according to 3 fold repetition
                    return Winner.DRAW;
                }
            }
            return Winner.NONE;
        }

        public Winner detectWinner(IEnumerable<Move> possibleMoves) {
            foreach (var move in possibleMoves) {
                if (IsLegalMove(move)) {
                    return detectWinnerAreThereValidMoves(true);
                }
            }

            return detectWinnerAreThereValidMoves(false);
        }

        // Detect Insufficient material. according to https://www.chessstrategyonline.com/content/tutorials/how-to-play-chess-draws there are 4 options for insufficient material
        // According to the website these combinations are invalid
        // King vs king
        // King and bishop vs king
        // King and knight vs king
        // King and bishop vs king and bishop of the same colour.
        // technically even though unlikely a player can have 2 bishops on the same color, in which case it is still a stalemate
        // This means we need to keep track of if there are more than 2 knights and bishops of more than 1 color
        public bool detectInsufficientMaterial() {
            // the goal is to return as soon as piece is found that indicates it is not a stalemate to increase performance
            bool bishopSquareColor = false;
            bool foundBishop = false;
            bool foundHorse = false;


            bool squareColor = false;
            for (int row = 0; row < 8 * BoardStateOffset.ROW_OFFSET; row += BoardStateOffset.ROW_OFFSET) {
                for (int column = 0; column < 8; column++) {

                    Piece piece = GetPiece(column + row);

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
                            if (foundHorse) {
                                return false;
                            }

                            if (!foundBishop) {
                                bishopSquareColor = squareColor;
                                foundBishop = true;
                            } else {
                                if (bishopSquareColor != squareColor) {
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

        #endregion

        #region Copy
        public Board CreateCopy() {
            Board newBoard = new Board(new byte[BoardStateOffset.BOARD_STATE_SIZE]);
            Buffer.BlockCopy(bytes, 0, newBoard.bytes, 0, BoardStateOffset.BOARD_STATE_SIZE);
            return newBoard;
        }

        public void Copy(Board toBoard) {
            Buffer.BlockCopy(bytes, 0, toBoard.bytes, 0, BoardStateOffset.BOARD_STATE_SIZE);
        }
        #endregion

        #region SAN
        public string StandardAlgebraicNotation(Move move) {
            var moves = GetMoves();

            Board board = this;
            moves = moves.Where(move => board.IsLegalMove(move)).ToList();
            return this.StandardAlgebraicNotation(move, moves);
        }

        // based on rules from https://en.wikipedia.org/wiki/Algebraic_notation_(chess)
        // Notice the parameter legalMoves only contains legal moves
        public string StandardAlgebraicNotation(Move move, List<Move> legalMoves) {
            StringBuilder san = new StringBuilder();
            var piece = GetPiece(move.fromPosition);
            if (((MoveFlags)move.moveFlags & MoveFlags.CASTLING) == MoveFlags.CASTLING) {
                if (move.targetPosition < move.fromPosition) {
                    // castle queen side
                    san.Append("O-O-O");
                } else {
                    // castle king side
                    san.Append("O-O");
                }
            } else {
                var isPawn = (piece & Piece.PIECE_MASK) == Piece.PAWN;

                if (!isPawn) {
                    // all pieces other pawns display their piece name
                    var pChar = PieceParser.ToChar(piece);
                    san.Append(pChar.ToString().ToUpper());
                }


                int fromRow = move.fromPosition / BoardStateOffset.ROW_OFFSET;
                int fromColumn = move.fromPosition - (fromRow * BoardStateOffset.ROW_OFFSET);

                bool sameColumns = false;
                bool sameRows = false;
                bool isAmbigious = false;
                foreach (var possibleMove in legalMoves) {
                    // check all other moves of the same piece type if they can move to the same position
                    if (possibleMove.targetPosition == move.targetPosition // check if it can reach the same square
                        && possibleMove.fromPosition != move.fromPosition // check if not the same piece
                        && piece == GetPiece(possibleMove.fromPosition)) { // check it is the same type of piece

                        isAmbigious = true;

                        int possibleFromRow = possibleMove.fromPosition / BoardStateOffset.ROW_OFFSET;
                        int possibleFromColumn = possibleMove.fromPosition - (possibleFromRow * BoardStateOffset.ROW_OFFSET);
                        if (possibleFromColumn == fromColumn) {
                            sameColumns = true;
                        }
                        if (possibleFromRow == fromRow) {
                            sameRows = true;
                        }
                    }
                }
                if (isPawn) {
                    if ((Piece)move.capturedPiece != Piece.EMPTY) {
                        // when pawn captures always specify the starting column
                        san.Append(Convert.ToChar('a' + (fromColumn)));
                    }
                } else {
                    if (isAmbigious) {
                        // disambiguating moves
                        if (sameColumns && sameRows) {
                            san.Append(Convert.ToChar('a' + (fromColumn)));
                            san.Append((fromRow + 1).ToString());
                        } else if (sameColumns) {
                            san.Append((fromRow + 1).ToString());
                        } else {
                            san.Append(Convert.ToChar('a' + (fromColumn)));
                        }
                    }
                }
                if ((Piece)move.capturedPiece != Piece.EMPTY) {
                    san.Append("x");
                }

                int targetRow = move.targetPosition / BoardStateOffset.ROW_OFFSET;
                int targetColumn = move.targetPosition - (targetRow * BoardStateOffset.ROW_OFFSET);
                san.Append(Convert.ToChar('a' + (targetColumn))).Append((targetRow + 1).ToString());

                Piece promotion = (Piece)move.promotion;
                if (promotion != Piece.EMPTY) {
                    san.Append("=").Append(PieceParser.ToChar(promotion).ToString().ToUpper());
                }
            }



            Move(move);

            // check for checkmate
            // after the move is played check if the current players king is attacked
            if (Attacked(GetKingPosition(IsWhiteTurn), IsWhiteTurn)) {
                var winner = detectWinner(GetMoves());

                if (winner == Winner.WINNER_BLACK || winner == Winner.WINNER_WHITE) {
                    //san += "#";
                    san.Append("#");
                } else {
                    //san += "+";
                    san.Append("+");
                }
            }

            UndoMove(move);

            return san.ToString();
        }
        #endregion


    }
}
