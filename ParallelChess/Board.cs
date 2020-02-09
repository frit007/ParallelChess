using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ParallelChess {


    [StructLayout(LayoutKind.Explicit)]
    public struct Board {
        // 
        //public const int BOARD_STATE_SIZE_IN_BYTES = 75;

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

        //[FieldOffset(68)]
        //public byte enPasasntTarget;

        public byte HalfTurnCounter { 
            get { return bytes[BoardStateOffset.HALF_TURN_COUNTER]; } 
            set { bytes[BoardStateOffset.HALF_TURN_COUNTER] = value; }
        }
        //[FieldOffset(69)]
        //public byte half_turn_counter;
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

        //[FieldOffset(72)]
        //public byte virtual_level;
        public byte VirtualLevel { 
            get { return bytes[BoardStateOffset.VIRTUAL_LEVEL]; } 
            set { bytes[BoardStateOffset.VIRTUAL_LEVEL] = value; } 
        }

        //[FieldOffset(73)]
        //public byte isWhitesTurn;
        public byte IsWhiteTurn { 
            get { return bytes[BoardStateOffset.IS_WHITE_TURN]; }
            set { bytes[BoardStateOffset.IS_WHITE_TURN] = value; }
        }
        public bool IsWhiteTurnBool {
            get { return 0 != bytes[BoardStateOffset.IS_WHITE_TURN]; }
            set { bytes[BoardStateOffset.IS_WHITE_TURN] = (byte)(value ? 1 : 0); }
        }

        public string FEN {
            get { return Chess.BoardToFen(this); }
        }
        public string StandardAlgebraicNotation(Move move) {
            var moves = BoardHelper.GetMoves(this);

            Board board = this;
            moves = moves.Where(move => BoardHelper.IsLegalMove(board, move)).ToList();
            return this.StandardAlgebraicNotation(move, moves);
        }

        // based on rules from https://en.wikipedia.org/wiki/Algebraic_notation_(chess)
        // Notice the parameter legalMoves only contains legal moves
        public string StandardAlgebraicNotation(Move move, List<Move> legalMoves) {
            StringBuilder san = new StringBuilder();
            //var san = "";
            var piece = GetPiece(move.fromPosition);
            if(((MoveFlags)move.moveFlags & MoveFlags.CASTLING) == MoveFlags.CASTLING) {
                if (move.targetPosition < move.fromPosition) {
                    // castle queen side
                    //san += "O-O-O";
                    san.Append("O-O-O");
                } else {
                    // castle king side
                    //san += "O-O";
                    san.Append("O-O");
                }
            } else {
                var isPawn = (piece & Piece.PIECE_MASK) == Piece.PAWN;

                if (!isPawn) {
                    // all pieces other pawns display their piece name
                    var pChar = PieceParse.ToChar(piece);
                    //san += pChar.ToString().ToUpper();
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
                        //san += Convert.ToChar('a' + (fromColumn));
                        san.Append(Convert.ToChar('a' + (fromColumn)));
                    }
                } else {
                    if (isAmbigious) {
                        // disambiguating moves
                        if (sameColumns && sameRows) {
                            //san += Convert.ToChar('a' + (fromColumn));
                            //san += (fromRow + 1).ToString();
                            san.Append(Convert.ToChar('a' + (fromColumn)));
                            san.Append((fromRow + 1).ToString());
                        } else if (sameRows) {
                            //san += (fromRow + 1).ToString();
                            san.Append((fromRow + 1).ToString());
                        } else {
                            //san += Convert.ToChar('a' + (fromColumn));
                            san.Append(Convert.ToChar('a' + (fromColumn)));
                        }
                    }
                }
                if ((Piece)move.capturedPiece != Piece.EMPTY) {
                    //san += "x";
                    san.Append("x");
                }

                int targetRow = move.targetPosition / BoardStateOffset.ROW_OFFSET;
                int targetColumn = move.targetPosition - (targetRow * BoardStateOffset.ROW_OFFSET);
                //san += Convert.ToChar('a' + (targetColumn)) + (targetRow + 1).ToString();
                san.Append(Convert.ToChar('a' + (targetColumn))).Append((targetRow + 1).ToString());

                Piece promotion = (Piece)move.promotion;
                if (promotion != Piece.EMPTY) {
                    //san += "=" + PieceParse.ToChar(promotion).ToString().ToUpper();
                    san.Append("=").Append(PieceParse.ToChar(promotion).ToString().ToUpper());
                }
            }

           

            BoardHelper.MakeMove(this, move);

            // check for checkmate
            // after the move is played check if the current players king is attacked
            if (BoardHelper.Attacked(this, GetKingPosition(IsWhiteTurn), IsWhiteTurn)) {
                var winner = BoardHelper.detectWinner(this, BoardHelper.GetMoves(this));

                if (winner == Winner.WINNER_BLACK || winner == Winner.WINNER_WHITE) {
                    //san += "#";
                    san.Append("#");
                } else {
                    //san += "+";
                    san.Append("+");
                }
            }

            BoardHelper.UndoMove(this, move);

            return san.ToString();
        }


        //// contains king positions
        //// 0 -> black king position
        //// 1 -> white king position
        //[FieldOffset(74)]
        //public fixed byte kingPosition[2];

        //[FieldOffset(74)]
        //public byte blackKingPosition;

        //[FieldOffset(75)]
        //public byte whiteKingPosition;


    }
}
