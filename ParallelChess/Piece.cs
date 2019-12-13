using System;
using System.Collections.Generic;
using System.Text;

namespace ParallelChess {
    // Pieces are represented using 1 byte that means different parts of the byte is used to store different information
    // piece information
    // 0000 cppp
    // p = used to store which kind piece it is
    // 0 = unused space
    [Flags]
    public enum Piece {

        EMPTY   = 0b0000_0000,

        PAWN    = 0b1000_0010,
        KNIGHT  = 0b1000_0100,
        KING    = 0b1000_0110,
        ROOK    = 0b1000_1000,
        BISHOP  = 0b1001_0000,
        QUEEN   = 0b1001_1000,

        // used to only get what piece it is
        // use the first bit to mark pieces, this is not strictly necessary but it makes the error messages nicer
        // otherwise ROOK and ATTACKS_STRAIGHT have the same color pattern and for some reason is prefers ATTACKS_STRAIGHT
        PIECE_MASK = 0b1001_1110,

        // we are using the 3rd and 4th bit to store information about if the piece attacks slanted or straight(or both in case of the queen)
        // this is done to speed up attacked checks
        ATTACKS_STRAIGHT = 0b000_1000,
        ATTACKS_SLANTED = 0b001_0000,

        // use the furthest right as as isWhite because we can compare is directly to isWhite then
        IS_WHITE = 0b0000_0001,
        // IS_BLACK would be 0000_0000 but it gives anoying error messages, therefor if something is not white it is implied to be black
        //IS_BLACK = 0b0000_0000,
    }

    public static class PieceParse {
        public static Piece FromChar(char c) {
            char lowercaseCharacter = Char.ToLower(c);
            // black pieces are represented with lowercase characters
            // therefor if the charcter changed when 
            bool isWhite = lowercaseCharacter != c;

            Piece piece = isWhite ? Piece.IS_WHITE : Piece.EMPTY; 

            piece |= lowercaseCharacter switch
            {
                'p' => Piece.PAWN,
                'r' => Piece.ROOK,
                'n' => Piece.KNIGHT,
                'b' => Piece.BISHOP,
                'q' => Piece.QUEEN,
                'k' => Piece.KING,
                _ => throw new ArgumentException($"The piece {c} is not a valid FEN piece. It has to be one of p,r,n,b,q,j,k"),
            };
            return piece;
        }

        public static char ToChar(Piece piece) {
            char c = (piece & Piece.PIECE_MASK) switch
            {
                Piece.PAWN => 'p',
                Piece.ROOK => 'r',
                Piece.KNIGHT => 'n',
                Piece.BISHOP => 'b',
                Piece.QUEEN => 'q',
                Piece.KING => 'k',
                Piece.EMPTY => '_',
                _ => throw new ArgumentException($"Invalid piece {piece}")
            };

            if((piece & Piece.IS_WHITE) == Piece.IS_WHITE) {
                c = Char.ToUpper(c);
            }

            return c;
        }
    }
}
