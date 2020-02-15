using ParallelChess.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ParallelChess {


    [StructLayout(LayoutKind.Explicit)]
    public struct Board {
        public static void initThreadStaticVariables() {
            EvalBoard.initThreadStaticVariables();
        }



        public const int A_COLUMN = 0;
        public const int B_COLUMN = 1;
        public const int C_COLUMN = 2;
        public const int D_COLUMN = 3;
        public const int E_COLUMN = 4;
        public const int F_COLUMN = 5;
        public const int G_COLUMN = 6;
        public const int H_COLUMN = 7;


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
                    if ((parsedPiece & Piece.PIECE_MASK) == Piece.KING) {
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
                board.EnPassantTarget = (byte)Board.ArrayPosition(enPassantAttackedSquare);
            }

            //Board.SetHalfTurnCounter(board, int.Parse(halfMoveClock));
            board.HalfTurnCounter = (byte)int.Parse(halfMoveClock);

            //Board.SetFullMoveClock(board, int.Parse(fullMoveClock));
            board.TurnCounter = (short)int.Parse(fullMoveClock);

            return board;
        }

        [FieldOffset(0)]
        public byte[] bytes;
        [FieldOffset(0)]
        public int[] ints;
        [FieldOffset(0)]
        public short[] shorts;

        public Piece GetPiece(int position) {
            return (Piece) bytes[position];
        }
        public void SetPiece(int position, Piece piece) {
            bytes[position] = (byte) piece;
        }
        #region positions
        public Piece A1 {
            get { return (Piece)bytes[BoardStateOffset.A1]; }
            set { bytes[BoardStateOffset.A1] = (byte)value; }
        }
        public Piece B1 {
            get { return (Piece)bytes[BoardStateOffset.B1]; }
            set { bytes[BoardStateOffset.B1] = (byte)value; }
        }
        public Piece C1 {
            get { return (Piece)bytes[BoardStateOffset.C1]; }
            set { bytes[BoardStateOffset.C1] = (byte)value; }
        }
        public Piece D1 {
            get { return (Piece)bytes[BoardStateOffset.D1]; }
            set { bytes[BoardStateOffset.D1] = (byte)value; }
        }
        public Piece E1 {
            get { return (Piece)bytes[BoardStateOffset.E1]; }
            set { bytes[BoardStateOffset.E1] = (byte)value; }
        }
        public Piece F1 {
            get { return (Piece)bytes[BoardStateOffset.F1]; }
            set { bytes[BoardStateOffset.F1] = (byte)value; }
        }
        public Piece G1 {
            get { return (Piece)bytes[BoardStateOffset.G1]; }
            set { bytes[BoardStateOffset.G1] = (byte)value; }
        }
        public Piece H1 {
            get { return (Piece)bytes[BoardStateOffset.H1]; }
            set { bytes[BoardStateOffset.H1] = (byte)value; }
        }

        public Piece A2 {
            get { return (Piece)bytes[BoardStateOffset.A2]; }
            set { bytes[BoardStateOffset.A2] = (byte)value; }
        }
        public Piece B2 {
            get { return (Piece)bytes[BoardStateOffset.B2]; }
            set { bytes[BoardStateOffset.B2] = (byte)value; }
        }
        public Piece C2 {
            get { return (Piece)bytes[BoardStateOffset.C2]; }
            set { bytes[BoardStateOffset.C2] = (byte)value; }
        }
        public Piece D2 {
            get { return (Piece)bytes[BoardStateOffset.D2]; }
            set { bytes[BoardStateOffset.D2] = (byte)value; }
        }
        public Piece E2 {
            get { return (Piece)bytes[BoardStateOffset.E2]; }
            set { bytes[BoardStateOffset.E2] = (byte)value; }
        }
        public Piece F2 {
            get { return (Piece)bytes[BoardStateOffset.F2]; }
            set { bytes[BoardStateOffset.F2] = (byte)value; }
        }
        public Piece G2 {
            get { return (Piece)bytes[BoardStateOffset.G2]; }
            set { bytes[BoardStateOffset.G2] = (byte)value; }
        }
        public Piece H2 {
            get { return (Piece)bytes[BoardStateOffset.H2]; }
            set { bytes[BoardStateOffset.H2] = (byte)value; }
        }

        public Piece A3 {
            get { return (Piece)bytes[BoardStateOffset.A3]; }
            set { bytes[BoardStateOffset.A3] = (byte)value; }
        }
        public Piece B3 {
            get { return (Piece)bytes[BoardStateOffset.B3]; }
            set { bytes[BoardStateOffset.B3] = (byte)value; }
        }
        public Piece C3 {
            get { return (Piece)bytes[BoardStateOffset.C3]; }
            set { bytes[BoardStateOffset.C3] = (byte)value; }
        }
        public Piece D3 {
            get { return (Piece)bytes[BoardStateOffset.D3]; }
            set { bytes[BoardStateOffset.D3] = (byte)value; }
        }
        public Piece E3 {
            get { return (Piece)bytes[BoardStateOffset.E3]; }
            set { bytes[BoardStateOffset.E3] = (byte)value; }
        }
        public Piece F3 {
            get { return (Piece)bytes[BoardStateOffset.F3]; }
            set { bytes[BoardStateOffset.F3] = (byte)value; }
        }
        public Piece G3 {
            get { return (Piece)bytes[BoardStateOffset.G3]; }
            set { bytes[BoardStateOffset.G3] = (byte)value; }
        }
        public Piece H3 {
            get { return (Piece)bytes[BoardStateOffset.H3]; }
            set { bytes[BoardStateOffset.H3] = (byte)value; }
        }

        public Piece A4 {
            get { return (Piece)bytes[BoardStateOffset.A4]; }
            set { bytes[BoardStateOffset.A4] = (byte)value; }
        }
        public Piece B4 {
            get { return (Piece)bytes[BoardStateOffset.B4]; }
            set { bytes[BoardStateOffset.B4] = (byte)value; }
        }
        public Piece C4 {
            get { return (Piece)bytes[BoardStateOffset.C4]; }
            set { bytes[BoardStateOffset.C4] = (byte)value; }
        }
        public Piece D4 {
            get { return (Piece)bytes[BoardStateOffset.D4]; }
            set { bytes[BoardStateOffset.D4] = (byte)value; }
        }
        public Piece E4 {
            get { return (Piece)bytes[BoardStateOffset.E4]; }
            set { bytes[BoardStateOffset.E4] = (byte)value; }
        }
        public Piece F4 {
            get { return (Piece)bytes[BoardStateOffset.F4]; }
            set { bytes[BoardStateOffset.F4] = (byte)value; }
        }
        public Piece G4 {
            get { return (Piece)bytes[BoardStateOffset.G4]; }
            set { bytes[BoardStateOffset.G4] = (byte)value; }
        }
        public Piece H4 {
            get { return (Piece)bytes[BoardStateOffset.H4]; }
            set { bytes[BoardStateOffset.H4] = (byte)value; }
        }

        public Piece A5 {
            get { return (Piece)bytes[BoardStateOffset.A5]; }
            set { bytes[BoardStateOffset.A5] = (byte)value; }
        }
        public Piece B5 {
            get { return (Piece)bytes[BoardStateOffset.B5]; }
            set { bytes[BoardStateOffset.B5] = (byte)value; }
        }
        public Piece C5 {
            get { return (Piece)bytes[BoardStateOffset.C5]; }
            set { bytes[BoardStateOffset.C5] = (byte)value; }
        }
        public Piece D5 {
            get { return (Piece)bytes[BoardStateOffset.D5]; }
            set { bytes[BoardStateOffset.D5] = (byte)value; }
        }
        public Piece E5 {
            get { return (Piece)bytes[BoardStateOffset.E5]; }
            set { bytes[BoardStateOffset.E5] = (byte)value; }
        }
        public Piece F5 {
            get { return (Piece)bytes[BoardStateOffset.F5]; }
            set { bytes[BoardStateOffset.F5] = (byte)value; }
        }
        public Piece G5 {
            get { return (Piece)bytes[BoardStateOffset.G5]; }
            set { bytes[BoardStateOffset.G5] = (byte)value; }
        }
        public Piece H5 {
            get { return (Piece)bytes[BoardStateOffset.H5]; }
            set { bytes[BoardStateOffset.H5] = (byte)value; }
        }

        public Piece A6 {
            get { return (Piece)bytes[BoardStateOffset.A6]; }
            set { bytes[BoardStateOffset.A6] = (byte)value; }
        }
        public Piece B6 {
            get { return (Piece)bytes[BoardStateOffset.B6]; }
            set { bytes[BoardStateOffset.B6] = (byte)value; }
        }
        public Piece C6 {
            get { return (Piece)bytes[BoardStateOffset.C6]; }
            set { bytes[BoardStateOffset.C6] = (byte)value; }
        }
        public Piece D6 {
            get { return (Piece)bytes[BoardStateOffset.D6]; }
            set { bytes[BoardStateOffset.D6] = (byte)value; }
        }
        public Piece E6 {
            get { return (Piece)bytes[BoardStateOffset.E6]; }
            set { bytes[BoardStateOffset.E6] = (byte)value; }
        }
        public Piece F6 {
            get { return (Piece)bytes[BoardStateOffset.F6]; }
            set { bytes[BoardStateOffset.F6] = (byte)value; }
        }
        public Piece G6 {
            get { return (Piece)bytes[BoardStateOffset.G6]; }
            set { bytes[BoardStateOffset.G6] = (byte)value; }
        }
        public Piece H6 {
            get { return (Piece)bytes[BoardStateOffset.H6]; }
            set { bytes[BoardStateOffset.H6] = (byte)value; }
        }

        public Piece A7 {
            get { return (Piece)bytes[BoardStateOffset.A7]; }
            set { bytes[BoardStateOffset.A7] = (byte)value; }
        }
        public Piece B7 {
            get { return (Piece)bytes[BoardStateOffset.B7]; }
            set { bytes[BoardStateOffset.B7] = (byte)value; }
        }
        public Piece C7 {
            get { return (Piece)bytes[BoardStateOffset.C7]; }
            set { bytes[BoardStateOffset.C7] = (byte)value; }
        }
        public Piece D7 {
            get { return (Piece)bytes[BoardStateOffset.D7]; }
            set { bytes[BoardStateOffset.D7] = (byte)value; }
        }
        public Piece E7 {
            get { return (Piece)bytes[BoardStateOffset.E7]; }
            set { bytes[BoardStateOffset.E7] = (byte)value; }
        }
        public Piece F7 {
            get { return (Piece)bytes[BoardStateOffset.F7]; }
            set { bytes[BoardStateOffset.F7] = (byte)value; }
        }
        public Piece G7 {
            get { return (Piece)bytes[BoardStateOffset.G7]; }
            set { bytes[BoardStateOffset.G7] = (byte)value; }
        }
        public Piece H7 {
            get { return (Piece)bytes[BoardStateOffset.H7]; }
            set { bytes[BoardStateOffset.H7] = (byte)value; }
        }

        public Piece A8 {
            get { return (Piece)bytes[BoardStateOffset.A8]; }
            set { bytes[BoardStateOffset.A8] = (byte)value; }
        }
        public Piece B8 {
            get { return (Piece)bytes[BoardStateOffset.B8]; }
            set { bytes[BoardStateOffset.B8] = (byte)value; }
        }
        public Piece C8 {
            get { return (Piece)bytes[BoardStateOffset.C8]; }
            set { bytes[BoardStateOffset.C8] = (byte)value; }
        }
        public Piece D8 {
            get { return (Piece)bytes[BoardStateOffset.D8]; }
            set { bytes[BoardStateOffset.D8] = (byte)value; }
        }
        public Piece E8 {
            get { return (Piece)bytes[BoardStateOffset.E8]; }
            set { bytes[BoardStateOffset.E8] = (byte)value; }
        }
        public Piece F8 {
            get { return (Piece)bytes[BoardStateOffset.F8]; }
            set { bytes[BoardStateOffset.F8] = (byte)value; }
        }
        public Piece G8 {
            get { return (Piece)bytes[BoardStateOffset.G8]; }
            set { bytes[BoardStateOffset.G8] = (byte)value; }
        }
        public Piece H8 {
            get { return (Piece)bytes[BoardStateOffset.H8]; }
            set { bytes[BoardStateOffset.H8] = (byte)value; }
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

        public void ActivateCastlingBit(CastlingBits addCastlingBits) {
            this.CastlingBits |= addCastlingBits;
        }

        public void RemoveCastlingBit(CastlingBits removeCasltingBit) {
            // example 
            // current: 0111
            // input: 0010
            // not Input: ~ 0010 = 1101
            // current and not Input: 0111 & 1101 = 0101
            this.CastlingBits &= ~removeCasltingBit;
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

        public short TurnCounter { 
            get { return shorts[BoardStateOffset.TURN_COUNTER_FROM_SHORT]; }
            set { shorts[BoardStateOffset.TURN_COUNTER_FROM_SHORT] = value; } 
        }

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
        public string FEN {
            get { return ChessOutput.BoardToFen(this); }
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

        public Move MakeMove(int from, int to) {
            Move targetPosition = FindMove(from, to);

            MakeMove(targetPosition);

            return targetPosition;
        }

        public Move MakeMove(string san) {
            foreach (var move in GetMoves()) {
                if (IsLegalMove(move) && StandardAlgebraicNotation(move) == san.Trim()) {
                    MakeMove(move);
                    return move;
                }
            }

            throw new Exception("Move not found!");
        }

        public Move MakeMove(int from, int to, Piece promotion) {
            Move targetPosition = FindMove(from, to, promotion);

            MakeMove(targetPosition);

            return targetPosition;
        }

        public void MakeMove(Move move) {
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
                        switch (toPosition) {
                            case BoardStateOffset.C1:
                                D1 = A1;
                                A1 = Piece.EMPTY;
                                break;
                            case BoardStateOffset.G1:
                                F1 = H1;
                                H1 = Piece.EMPTY;
                                break;
                            case BoardStateOffset.C8:
                                D8 = A8;
                                A8 = Piece.EMPTY;
                                break;
                            case BoardStateOffset.G8:
                                F8 = H8;
                                H8 = Piece.EMPTY;
                                break;
                        }
                    }
                    SetKingPosition(IsWhiteTurn, (byte)toPosition);
                    break;
            }


            var castleBit = CastlingBits;
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


            if (!isWhitesTurn) {
                // increment fullMoveClock after blacks turn
                TurnCounter++;
            }

            // flip turn
            IsWhiteTurn = (byte)(IsWhiteTurn ^ 1);
        }
        #endregion

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
            if(((MoveFlags)move.moveFlags & MoveFlags.CASTLING) == MoveFlags.CASTLING) {
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
                    var pChar = PieceParse.ToChar(piece);
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
                    san.Append("=").Append(PieceParse.ToChar(promotion).ToString().ToUpper());
                }
            }

           

            MakeMove( move);

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

        public Winner detectWinner(IEnumerable<Move> moves) {
            //int count = history.Count();
            List<Move> validMoves = new List<Move>();
            foreach (var move in moves) {
                if (IsLegalMove(move)) {
                    validMoves.Add(move);
                }
            }

            return detectWinnerAreThereValidMoves(validMoves.Count() != 0);
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

        public Board CreateCopyBoard() {
            Board newBoard = new Board {
                bytes = new byte[BoardStateOffset.BOARD_STATE_SIZE],
            };
            Buffer.BlockCopy(bytes, 0, newBoard.bytes, 0, BoardStateOffset.BOARD_STATE_SIZE);
            return newBoard;
        }

        public void CopyBoard(Board toBoard) {
            Buffer.BlockCopy(bytes, 0, toBoard.bytes, 0, BoardStateOffset.BOARD_STATE_SIZE);
        }

        // position is expected be in the format "a1" or "b5"
        // converts it to a number from 0 to 119, which is the number system used internally
        // examples
        // a1 -> 0
        // a3 -> 2
        // h8 -> 119
        public static int ArrayPosition(string readablePosition) {
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
        public static string ReadablePosition(int arrayPosition) {
            int row = arrayPosition / BoardStateOffset.ROW_OFFSET;
            int column = arrayPosition - (row * BoardStateOffset.ROW_OFFSET);
            string move = Convert.ToChar('a' + (column)) + (row + 1).ToString();
            return move;
        }


        // me refers to the current player
        public bool PieceBelongsToMe(Piece piece) {
            // we abuse that Piece stores the information about color in its last bit
            // therefor we can mask that bit away for example the piece 0x07 (white king) & 0x01(isWhitecheck) = 0x01
            // this always produces 1 or 0 which can be directly compared to isWhiteTurn.
            // IsWhite is either 0x01 (true) or 0x00 (false)
            return IsWhiteTurn == (byte)(piece & Piece.IS_WHITE);
        }

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

        public bool IsPositionEmpty(int position) {
            return GetPiece(position) == Piece.EMPTY;
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

        #region GetMoves

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

        private void AddPawnMove(int fromPosition, int targetPosition, MoveFlags move, List<Move> moves) {
            Piece takenPiece;
            int row = PositionRow(targetPosition);
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

        private void AddMove(int fromPosition, int targetPosition, MoveFlags moveBits, List<Move> moves) {
            Piece takenPiece = GetPiece(targetPosition);
            moves.Add(MoveHelper.CreateMove(targetPosition, fromPosition, takenPiece, Piece.EMPTY, moveBits, this));
        }

        private void WalkRelativePaths(int fromPosition, int[] movePositions, List<Move> moves) {
            foreach (var relativePosition in movePositions) {
                int move = fromPosition;
                do {
                    move += relativePosition;
                    if (IsValidPosition(move)) {
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
                    int moveOne = RelativePosition(fromPosition, 0, direction);
                    if (IsPositionEmpty(moveOne)) {
                        AddPawnMove(fromPosition, moveOne, MoveFlags.PAWN_MOVE, moves);
                    }

                    // check if the pawn is on the starting position. If it is then assume that it is possible to move forward
                    if (isWhitesTurn ? PositionRow(fromPosition) == 1 : PositionRow(fromPosition) == 6) {
                        int hasToBeEmptyPosition = RelativePosition(fromPosition, 0, direction);
                        int move = RelativePosition(fromPosition, 0, 2 * direction);
                        if (IsPositionEmpty(move) && IsPositionEmpty(hasToBeEmptyPosition)) {
                            AddPawnMove(fromPosition, move, MoveFlags.BIG_PAWN_MOVE | MoveFlags.PAWN_MOVE, moves);
                        }
                    }

                    int column = PositionColumn(fromPosition);
                    // check if the pawn is on the right column. 
                    // We don't need to check if there is a next row is outside the board.
                    // because the pawn is never able to stand on the last because of promotion
                    if (column != H_COLUMN) {
                        int move = RelativePosition(fromPosition, 1, direction);
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
                    if (column != A_COLUMN) {
                        int move = RelativePosition(fromPosition, -1, direction);
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
                    foreach (var relativeMove in kingMoves) {
                        int move = relativeMove + fromPosition;
                        if (IsValidPosition(move)) {
                            var moveOption = CanITakeSquare(move);
                            if (moveOption != MoveOption.INVALID) {
                                AddMove(fromPosition, move, MoveFlags.EMPTY, moves);
                            }
                        }
                    }
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
                    foreach (var relativeMove in knightMoves) {
                        int move = fromPosition + relativeMove;
                        if (IsValidPosition(move)) {
                            var moveOption = CanITakeSquare(move);
                            if (moveOption != MoveOption.INVALID) {
                                AddMove(fromPosition, move, MoveFlags.EMPTY, moves);
                            }
                        }
                    }
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
                    if (IsValidPosition(relativePosition)) {
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
                    if (IsValidPosition(relativePosition)) {
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

                if (IsValidPosition(relativePosition)) {
                    var piece = GetPiece(relativePosition);
                    Piece enemySlantedAttacked = (theirColorPiece | Piece.KNIGHT);
                    if ((piece & (Piece.PIECE_MASK | Piece.IS_WHITE)) == enemySlantedAttacked) {
                        return true;
                    }
                }
            }

            int leftPawnPosition = position - BoardStateOffset.ROW_OFFSET - 1 + BoardStateOffset.ROW_OFFSET * pretendToBeWhite * 2;
            int rightPawnPosition = position - BoardStateOffset.ROW_OFFSET + 1 + BoardStateOffset.ROW_OFFSET * pretendToBeWhite * 2;
            if (IsValidPosition(leftPawnPosition)) {
                Piece leftPawn = GetPiece(leftPawnPosition);
                if (leftPawn == (theirColorPiece | Piece.PAWN)) {
                    return true;
                }
            }

            if (IsValidPosition(rightPawnPosition)) {
                Piece rightPawn = GetPiece(rightPawnPosition);
                if (rightPawn == (theirColorPiece | Piece.PAWN)) {
                    return true;
                }
            }

            return false;
        }


        public bool IsLegalMove(Move move) {
            byte myTurn = IsWhiteTurn;
            MakeMove(move);

            var notAttacked = !Attacked(GetKingPosition(myTurn), myTurn);

            UndoMove(move);

            return notAttacked;
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
            TurnCounter -= IsWhiteTurn;

            if ((movedPiece & Piece.KING) == Piece.KING) {
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


    }
}
