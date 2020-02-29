#pragma once

#include <string>
#include <iostream>
#include "Board.h"
#include "PieceParser.h"

static class ChessOutput {
    public:

    //static std::string AsciiBoard(ChessGame game, List<Move> moves* = null, bool displayCount = false) {

    //    return AsciiBoard(game.board, moves, displayCount);
    //}

    static std::string AsciiBoard(Board board) {

        std::string ascii = "";

        ascii += "+---------------+\n";
        for (int row = 7; row >= 0; row--) {
            ascii += "|";
            for (int column = 0; column < 8; column++) {
                auto position = row * ROW_OFFSET + column;
                auto piece = board.GetPiece(position);

                ascii += PieceParser::ToChar(piece);
                
                if (column != 7) {
                    ascii += " ";
                }
            }
            
            ascii += "| "+ std::to_string(row + 1) + "\n";
        }
        ascii += "+---------------+\n";

        ascii += " A B C D E F G H";
        return ascii;
    }


    //static String BoardToFen(Board board, int move = 0) {
    //    StringBuilder fen = new StringBuilder();

    //    for (int row = 7; row >= 0; row--) {
    //        int count = 0;
    //        for (int column = 0; column < 8; column++) {
    //            var position = row * BoardStateOffset.ROW_OFFSET + column;
    //            var piece = board.GetPiece(position);
    //            //var piece = Board.GetPiece(board, position);
    //            char c = PieceParser.ToChar(piece);
    //            if (c == '_') {
    //                count++;
    //            }
    //            else {
    //                if (count != 0) {
    //                    fen.Append(count);
    //                    count = 0;
    //                }
    //                fen.Append(c);
    //            }
    //        }
    //        if (count != 0) {
    //            fen.Append(count);
    //        }

    //        if (row != 0) {
    //            fen.Append("/");
    //        }
    //    }
    //    fen.Append(" ");
    //    fen.Append(board.IsWhiteTurnBool ? "w" : "b");
    //    fen.Append(" ");
    //    CastlingBits castlingBits = board.CastlingBits;
    //    if (castlingBits == CastlingBits.EMPTY) {
    //        fen.Append("-");
    //    }
    //    else {
    //        if ((castlingBits & CastlingBits.WHITE_KING_SIDE_CASTLE) != CastlingBits.EMPTY) {
    //            fen.Append("K");
    //        }
    //        if ((castlingBits & CastlingBits.WHITE_QUEEN_SIDE_CASTLE) != CastlingBits.EMPTY) {
    //            fen.Append("Q");
    //        }
    //        if ((castlingBits & CastlingBits.BLACK_KING_SIDE_CASTLE) != CastlingBits.EMPTY) {
    //            fen.Append("k");
    //        }
    //        if ((castlingBits & CastlingBits.BLACK_QUEEN_SIDE_CASTLE) != CastlingBits.EMPTY) {
    //            fen.Append("q");
    //        }
    //    }
    //    fen.Append(" ");
    //    fen.Append(board.EnPassantTarget != EnPassant.NO_ENPASSANT ? BoardPosition.ReadablePosition(board.EnPassantTarget) : "-");
    //    fen.Append(" ");
    //    fen.Append(board.HalfTurnCounter);
    //    fen.Append(" ");
    //    fen.Append(move);

    //    return fen.ToString();
    //}
};

