#pragma once

#include "Piece.h"
#include <string>

static class PieceParser {
    public:

    static Piece FromChar(char c) {
        //char lowercaseCharacter = Char.ToLower(c);
        char lowercaseCharacter = tolower(c);
        // black pieces are represented with lowercase characters
        // therefor if the charcter changed when 
        bool isWhite = lowercaseCharacter != c;

        Piece isPieceWhite = isWhite ? IS_WHITE : PIECE_EMPTY;


        switch (lowercaseCharacter)
        {
        case 'p':
            return PAWN | isPieceWhite;
        case 'r':
            return ROOK | isPieceWhite;
        case 'n':
            return KNIGHT | isPieceWhite;
        case 'b':
            return BISHOP | isPieceWhite;
        case 'q':
            return QUEEN | isPieceWhite;
        case'k':
            return KING | isPieceWhite;
        default:
            throw ChessException("The piece" + std::to_string(c) + "is not a valid FEN piece. It has to be one of p,r,n,b,q,j,k");
            //return PIECE_EMPTY;
        }
        //piece |= lowercaseCharacter switch
        //{
        //    'p' = > Piece.PAWN,
        //        'r' = > Piece.ROOK,
        //        'n' = > Piece.KNIGHT,
        //        'b' = > Piece.BISHOP,
        //        'q' = > Piece.QUEEN,
        //        'k' = > Piece.KING,
        //        _ = > throw new ArgumentException($"The piece {c} is not a valid FEN piece. It has to be one of p,r,n,b,q,j,k"),
        //};
        //return piece;
    }

    static char ToChar(Piece piece) {
        char c = '_';
        auto mask = piece & PIECE_MASK;
        switch (mask)
        {
        case PAWN:
            c = 'p';
            break;
        case KNIGHT:
            c = 'n';
            break;
        case KING:
            c = 'k';
            break;
        case ROOK:
            c = 'r';
            break;
        case BISHOP:
            c = 'b';
            break;
        case QUEEN:
            c = 'q';
            break;
        case PIECE_EMPTY:
            c = '_';
            break;
        default:
            throw ChessException("Invalid piece " + std::to_string(piece));
            break;
        }
        //char c = (piece & PIECE_MASK) switch
        //{
        //    Piece.PAWN = > 'p',
        //        Piece.ROOK = > 'r',
        //        Piece.KNIGHT = > 'n',
        //        Piece.BISHOP = > 'b',
        //        Piece.QUEEN = > 'q',
        //        Piece.KING = > 'k',
        //        Piece.EMPTY = > '_',
        //        _ = > throw new ArgumentException($"Invalid piece {piece}")
        //};

        if ((piece & IS_WHITE) == IS_WHITE) {
            //c = Char.ToUpper(c);
            c = toupper(c);
        }

        return c;
    }

    static std::string ToReadable(Piece piece, bool includeColor = false) {
        if (PIECE_EMPTY == piece) {
            return "_blank_";
        }

        //std::string name = (piece & PIECE_MASK) switch
        //{
        //    Piece.PAWN = > "Pawn",
        //        Piece.ROOK = > "Rook",
        //        Piece.KNIGHT = > "Knight",
        //        Piece.BISHOP = > "Bishop",
        //        Piece.QUEEN = > "Queen",
        //        Piece.KING = > "King",
        //        Piece.EMPTY = > "",
        //        _ = > throw new ArgumentException($"Invalid piece {piece}")
        //};
        std::string name = "";

        switch (piece)
        {
        case PAWN:
            name = 'Pawn';
            break;
        case KNIGHT:
            name = "Knight";
            break;
        case KING:
            name = "King";
            break;
        case ROOK:
            name = "Rook";
            break;
        case BISHOP:
            name = "Bishop";
            break;
        case QUEEN:
            name = "Queen";
            break;
        case PIECE_EMPTY:
            name = "";
            break;
        default:
            throw ChessException("Invalid piece " + std::to_string(piece));
            //name = "UNKNOWN";
            break;
        }


        if (includeColor) {
            if ((piece & IS_WHITE) == IS_WHITE) {
                name = "White " + name;
            }
            else {
                name = "Black " + name;
            }
        }

        return name;
    }
};

