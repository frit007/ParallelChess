using System;
using System.Collections.Generic;
using System.Text;

namespace ParallelChess {

    public static class BoardStateOffset {
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
        // (DEPRECATED) Turn counter moved to Chess
        // counter is increased after BLACKs turn 
        // since we likely need more than a byte to store the game length we dedicate 2 bytes to this.
        //public const int TURN_COUNTER_1_FROM_RIGHT = 9;
        //public const int TURN_COUNTER_2_FROM_RIGHT = 8;

        //public const int TURN_COUNTER_FROM_SHORT = TURN_COUNTER_1_FROM_RIGHT / 2;

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

        public static List<(Piece piece, int position)> listPieces(this Board board) {
            List<(Piece, int)> items = new List<(Piece, int)>();

            for (int column = 0; column < 8; column++) {
                for (int row = 0; row < 8 * BoardStateOffset.ROW_OFFSET; row += BoardStateOffset.ROW_OFFSET) {
                    var position = column + row;
                    items.Add((board.GetPiece(position), position));
                }
            }

            return items;
        }
    }
}
