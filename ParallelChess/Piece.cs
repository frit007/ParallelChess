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

        EMPTY               = 0b0000_0000,

        PAWN                = 0b0000_0010,
        KNIGHT              = 0b0000_0100,
        KING                = 0b0000_0110,
        ROOK                = 0b0000_1000,
        BISHOP              = 0b0001_0000,
        QUEEN               = 0b0001_1000,

        // used to only get what piece it is
        // use the first bit to mark pieces, this is not strictly necessary but it makes the error messages nicer
        // otherwise ROOK and ATTACKS_STRAIGHT have the same color pattern and for some reason is prefers ATTACKS_STRAIGHT
        PIECE_MASK          = 0b0001_1110,

        // we are using the 4th and 5th bit to store information about if the piece attacks slanted or straight(or both in case of the queen)
        // this is done to speed up attacked checks
        ATTACKS_STRAIGHT    = 0b000_1000,
        ATTACKS_SLANTED     = 0b001_0000,

        // use the furthest right as as isWhite because we can compare isWhite directly to other things that also follow this convention
        IS_WHITE            = 0b0000_0001,
        // IS_BLACK would be 0000_0000 but it gives anoying error messages(everything is marked as IS_BLACK because it matches every piece), therefor if something is not white it is implied to be black
        //IS_BLACK = 0b0000_0000,

        ENPASSANT_TARGET    = 0b00100_0000

    }


}
