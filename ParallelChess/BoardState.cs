using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace ParallelChess {

    public static class BoardStateOffset{
        public const int A1 = 0;
        public const int B1 = 1;
        public const int C1 = 2;
        public const int D1 = 3;
        public const int E1 = 4;
        public const int F1 = 5;
        public const int G1 = 6;
        public const int H1 = 7;

        public const int A2 = 8;
        public const int B2 = 9;
        public const int C2 = 10;
        public const int D2 = 11;
        public const int E2 = 12;
        public const int F2 = 13;
        public const int G2 = 14;
        public const int H2 = 15;

        public const int A3 = 16;
        public const int B3 = 17;
        public const int C3 = 18;
        public const int D3 = 19;
        public const int E3 = 20;
        public const int F3 = 21;
        public const int G3 = 22;
        public const int H3 = 23;

        public const int A4 = 24;
        public const int B4 = 25;
        public const int C4 = 26;
        public const int D4 = 27;
        public const int E4 = 28;
        public const int F4 = 29;
        public const int G4 = 30;
        public const int H4 = 31;

        public const int A5 = 32;
        public const int B5 = 33;
        public const int C5 = 34;
        public const int D5 = 35;
        public const int E5 = 36;
        public const int F5 = 37;
        public const int G5 = 38;
        public const int H5 = 39;

        public const int A6 = 40;
        public const int B6 = 41;
        public const int C6 = 42;
        public const int D6 = 43;
        public const int E6 = 44;
        public const int F6 = 45;
        public const int G6 = 46;
        public const int H6 = 47;

        public const int A7 = 48;
        public const int B7 = 49;
        public const int C7 = 50;
        public const int D7 = 51;
        public const int E7 = 52;
        public const int F7 = 53;
        public const int G7 = 54;
        public const int H7 = 55;

        public const int A8 = 56;
        public const int B8 = 57;
        public const int C8 = 58;
        public const int D8 = 59;
        public const int E8 = 60;
        public const int F8 = 61;
        public const int G8 = 62;
        public const int H8 = 63;

        public const int CASTLING = 64;
        //public const int CASTLING_INT = CASTLING / 4;

        public const int ROW = 8;

        // contains field that a pawn skipped over. 
        // this means this field can be attacked by an enemy pawn
        // this field should be reset when a move is made, because en passant is no longer possible
        public const int EN_PASSANT_FIELD = 68;

        // --------------- STALEMATE ------------------
        // after 50 turns without any piece being taken or pawns being moved the game is a stalemate
        // we count half turns since it avoid having to keep track of who started the stalemate
        public const int HALF_TURN_COUNTER = 69;

        // --------------- Virtualiztion level ---------------
        // keep track of how many levels the game is being simulated
        public const int VIRTUAL_LEVEL = 70;

        // -------------- TURN --------------
        // boolean true for WHITE and false for BLACK
        public const int IS_WHITE_TURN = 71;

        // -------------- KING POSITIONS ----------------
        // because we need to know where the king is we store that information
        public const int BLACK_KING_POSITION = 72;
        public const int WHITE_KING_POSITION = 73;

        // -------------- FULL --------------
        // counter is increased after BLACKs turn 
        // since we likely need more than a byte to store the game length we dedicate 2 bytes to this.
        public const int TURN_COUNTER_1_FROM_RIGHT = 74;
        public const int TURN_COUNTER_2_FROM_RIGHT = 75;

        public const int TURN_COUNTER_FROM_SHORT = TURN_COUNTER_1_FROM_RIGHT / 2;


        public const int BOARD_STATE_SIZE = 75;
    }

    [StructLayout(LayoutKind.Explicit)]
    public unsafe struct BoardState {
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
        public byte getKingPosition(int isWhite) {
            return bytes[BoardStateOffset.BLACK_KING_POSITION + isWhite];
        }

        public void setKingPosition(int isWhite, byte position) {
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
