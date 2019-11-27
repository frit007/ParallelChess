using System;
using System.Collections.Generic;
using System.Text;

namespace ParallelChess {
    // Used to document what is at which position in boardStateArray.
    // The board state contains more than just the current board because certain moves rely on historical information like castling.
    // which requires the rook and the king to never have moved. 
    // Therefor boardstate also stores information necessary to determine if special moves like castling is possible
    public static class BoardOffset {
        /*
         TODO : Delete code for generating positions
         result = ""
        for(var i = 0; i < 64; i++) {
	        row = Math.floor(i / 8);
	        column = i - row * 8
	        letter = String.fromCharCode('A'.charCodeAt(0) + column)
            // console.log(row,column, letter)
            result+= `public const int ${letter}${row+1} = ${i};\n`;
            if(column == 7) {
                result += "\n"
            }
	

        }
        */
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
        public const int H8 = 63;

        // it is often usefull to go one row forward or backwards
        // for example when checking enpassant rules we need to get the piece behind a pawn.
        public const int ROW = 8;

        // ----------- CASTLING -----------
        // Castling is a move in the game of chess involving a player's king and either of the player's original rooks. 
        // It is the only move in chess in which a player moves two pieces in the same move, 
        // and it is the only move aside from the knight's move where a piece can be said to "jump over" another (https://en.wikipedia.org/wiki/Castling)
        /*
         * Example Queen side castle initial position
         * _│_│_│_│_│_│_│_
         * _│_│_│_│_│_│_│_
         * _│_│_│_│_│_│_│_
         * _│_│_│_│_│_│_│_
         * _│_│_│_│_│_│_│_
         * _│_│_│_│_│_│_│_
         * _│_│_│_│_│_│_│_
         * R│_│_│_│K│_│_│_
         * 
         * After the move it looks like this
         * _│_│_│_│_│_│_│_
         * _│_│_│_│_│_│_│_
         * _│_│_│_│_│_│_│_
         * _│_│_│_│_│_│_│_
         * _│_│_│_│_│_│_│_
         * _│_│_│_│_│_│_│_
         * _│_│_│_│_│_│_│_
         * _│_│K│R│_│_│_│_
         *
         * You cannot castle if the kings is either in check or has to move through check to get to his final position.
         * The rook can be attacked 
         */

        // Historical information is reqiured to know if you are allowed to castle
        // you need to know if either the king or the rook has already moved. 
        // If either of them have moved you are not allowed to castle

        // The bits below are used to indicate if you are allowed to castle
        // ---- BLACK ----
        // 0001 black queen side castle
        // 0010 black king side castle
        // ---- WHITE ----
        // 0100 white queen side castle
        // 1000 white king side castle
        // This is also shown in the class CastlingBits
        public const int CASTLING = 64;



        // ----------- EN PASSANT -----------
        // En passant is a move in chess. 
        // It is a special pawn capture that can only occur immediately after a pawn makes a move of two squares from its starting square, 
        // and it could have been captured by an enemy pawn had it advanced only one square. 
        // The opponent captures the just-moved pawn "as it passes" through the first square. 
        // The result is the same as if the pawn had advanced only one square and the enemy pawn had captured it normally. (https://en.wikipedia.org/wiki/En_passant)

        /*
        * Example
        * 
        * initial position
        * _│_│_│_│_│_│_│_
        * _│_│_│_│_│_│_│_
        * _│_│_│_│_│_│_│_
        * _│_│_│_│_│_│_│_
        * _│_│p│_│_│_│_│_
        * _│_│_│_│_│_│_│_
        * _│_│_│P│_│_│_│_
        * _│_│_│_│_│_│_│_
        * 
        * white moves 2 places forward. black now has 2 option (marked with *)
        * _│_│_│_│_│_│_│_
        * _│_│_│_│_│_│_│_
        * _│_│_│_│_│_│_│_
        * _│_│_│_│_│_│_│_
        * _│_│p│P│_│_│_│_
        * _│_│*│*│_│_│_│_
        * _│_│_│_│_│_│_│_
        * _│_│_│_│_│_│_│_
        * 
        * En passant allows black pawn to take the white pawn by moving to the square where white would have stood if they had moved 1 square forward
        * _│_│_│_│_│_│_│_
        * _│_│_│_│_│_│_│_
        * _│_│_│_│_│_│_│_
        * _│_│_│_│_│_│_│_
        * _│_│_│_│_│_│_│_
        * _│_│_│p│_│_│_│_
        * _│_│_│_│_│_│_│_
        * _│_│_│_│_│_│_│_
        *
        */
        // contains field that a pawn skipped over. 
        // this means this field can be attacked by an enemy pawn
        // this field should be reset when a move is made, because en passant is no longer possible
        public const int EN_PASSANT_FIELD = 65;

        // --------------- STALEMATE ------------------
        // after 50 turns without any piece being taken or pawns being moved the game is a stalemate
        // we count half turns since it avoid having to keep track of who started the stalemate
        public const int HALF_TURN_COUNTER = 66;

        // --------------- Virtualiztion level ---------------
        // keep track of how many levels the game is being simulated
        public const int VIRTUAL_LEVEL = 67;

        // -------------- TURN --------------
        // boolean true for WHITE and false for BLACK
        public const int IS_WHITE_TURN = 68;

        // -------------- KING POSITIONS ----------------
        // because we need to know where the king is we store that information
        public const int BLACK_KING_POSITION = 69;
        public const int WHITE_KING_POSITION = 70;

        // -------------- FULL --------------
        // counter is increased after BLACKs turn 
        // since we likely need more than a byte to store the game length we dedicate 2 bytes to this.
        public const int COUNTER_1_FROM_RIGHT = 71;
        public const int COUNTER_2_FROM_RIGHT = 72;


        public const int BOARD_STATE_SIZE = 73;

    }
}
