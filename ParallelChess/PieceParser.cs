using System;
using System.Collections.Generic;
using System.Text;

namespace ParallelChess {
    public static class PieceParser {
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

            if ((piece & Piece.IS_WHITE) == Piece.IS_WHITE) {
                c = Char.ToUpper(c);
            }

            return c;
        }

        public static string ToReadable(Piece piece, bool includeColor = false) {
            if(Piece.EMPTY == piece) {
                return "_blank_";
            }

            String name = (piece & Piece.PIECE_MASK) switch
            {
                Piece.PAWN => "Pawn",
                Piece.ROOK => "Rook",
                Piece.KNIGHT => "Knight",
                Piece.BISHOP => "Bishop",
                Piece.QUEEN => "Queen",
                Piece.KING => "King",
                Piece.EMPTY => "",
                _ => throw new ArgumentException($"Invalid piece {piece}")
            };

            if(includeColor) {
                if ((piece & Piece.IS_WHITE) == Piece.IS_WHITE) {
                    name = "White " + name;
                } else {
                    name = "Black " + name;
                }
            }

            return name;
        }
    }
}
