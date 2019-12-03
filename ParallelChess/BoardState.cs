using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace ParallelChess {

    public static class BoardStateOffset{
        // ------------0x88-------------
        // 0x88 is method used checking if location is out of bound very fast
        // it is done by creating an array and leaving every second row blank.
        // We can then check if a position is in a invalid row by anding a position with 0x88
        // for example if you tried to move one right from H3(39) + 1 = 40. 40 is a invalid position 
        // which can be caught by checking 0x88 & 40 = 8. Any invalid position will result in a non zero value
        // the potential disadvantage to this approach is that the boardState is larger, which will take longer to copy and 
        // might mean that the array is no longer cached in CPU cache.
        public const int A1 = 0;
        public const int B1 = 1;
        public const int C1 = 2;
        public const int D1 = 3;
        public const int E1 = 4;
        public const int F1 = 5;
        public const int G1 = 6;
        public const int H1 = 7;

        public const int A2 = 16;
        public const int B2 = 17;
        public const int C2 = 18;
        public const int D2 = 19;
        public const int E2 = 20;
        public const int F2 = 21;
        public const int G2 = 22;
        public const int H2 = 23;

        public const int A3 = 32;
        public const int B3 = 33;
        public const int C3 = 34;
        public const int D3 = 35;
        public const int E3 = 36;
        public const int F3 = 37;
        public const int G3 = 38;
        public const int H3 = 39;

        public const int A4 = 48;
        public const int B4 = 49;
        public const int C4 = 50;
        public const int D4 = 51;
        public const int E4 = 52;
        public const int F4 = 53;
        public const int G4 = 54;
        public const int H4 = 55;

        public const int A5 = 64;
        public const int B5 = 65;
        public const int C5 = 66;
        public const int D5 = 67;
        public const int E5 = 68;
        public const int F5 = 69;
        public const int G5 = 70;
        public const int H5 = 71;

        public const int A6 = 80;
        public const int B6 = 81;
        public const int C6 = 82;
        public const int D6 = 83;
        public const int E6 = 84;
        public const int F6 = 85;
        public const int G6 = 86;
        public const int H6 = 87;

        public const int A7 = 96;
        public const int B7 = 97;
        public const int C7 = 98;
        public const int D7 = 99;
        public const int E7 = 100;
        public const int F7 = 101;
        public const int G7 = 102;
        public const int H7 = 103;

        public const int A8 = 112;
        public const int B8 = 113;
        public const int C8 = 114;
        public const int D8 = 115;
        public const int E8 = 116;
        public const int F8 = 117;
        public const int G8 = 118;
        public const int H8 = 119;

        public const int ROW_OFFSET = 16;

        // -----------Ox88 wasted space------------
        // every second row on the board is wasted since they are invalid positions.
        // We can use this space by placing all the extra data we need in those empty spaces

        // -------------- FULL Turn Counter --------------
        // counter is increased after BLACKs turn 
        // since we likely need more than a byte to store the game length we dedicate 2 bytes to this.
        public const int TURN_COUNTER_1_FROM_RIGHT = 8;
        public const int TURN_COUNTER_2_FROM_RIGHT = 9;

        public const int TURN_COUNTER_FROM_SHORT = TURN_COUNTER_1_FROM_RIGHT / 2;

        public const int CASTLING = 10;


        // contains field that a pawn skipped over. 
        // this means this field can be attacked by an enemy pawn
        // this field should be reset when a move is made, because en passant is no longer possible
        public const int EN_PASSANT_FIELD = 11;

        // --------------- STALEMATE ------------------
        // after 50 turns without any piece being taken or pawns being moved the game is a stalemate
        // we count half turns since it avoid having to keep track of who started the stalemate
        public const int HALF_TURN_COUNTER = 12;

        // --------------- VIRTULIZATION LEVEL ---------------
        // keep track of how many levels the game is being simulated
        public const int VIRTUAL_LEVEL = 13;

        // -------------- HALF TURN --------------
        // boolean true for WHITE and false for BLACK
        public const int IS_WHITE_TURN = 14;

        // -------------- KING POSITIONS ----------------
        // because we need to know where the king is we store that information
        public const int BLACK_KING_POSITION = 24;
        public const int WHITE_KING_POSITION = 25;




        public const int BOARD_STATE_SIZE = 120;
        //public const int BOARD_STATE_SIZE = 1000;
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
