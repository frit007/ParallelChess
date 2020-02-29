#pragma once

#include <string>
#include "Board.h"
#include "StringHelpers.h"
#include "BoardStateOffset.h"
#include "PieceParser.h"

class BoardFactory {
public:
    static Board LoadBoardFromFen(std::string fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1") {
        int discard = 0;
        return LoadBoardFromFen(discard, fen);
    }

    // Loads a board from the FEN notation(Forsyth–Edwards Notation)
    // Useful for getting to certain chess positions quickly. 
    // Use the website below to generate positions
    // https://lichess.org/editor/8/1QQ2QQ1/QqqQQqqQ/QqqqqqqQ/QqqqqqqQ/1QqqqqQ1/2QqqQ2/3QQ3_b_-_-_0_1
    // FEN consists of 6 parts which are sepperated by space
    // 1. Position
    //  - Describes what pieces are at which positions
    //  - The pieces are described from left to right with a "/" to indicate row changes
    //  - a single letter is used to represent a piece
    //    - P = Pawn
    //    - N = Knight
    //    - B = Bishop
    //    - R = Rook
    //    - Q = Queen
    //    - K = King
    //  - Capital letters indicate White pieces, while lowercase letter indicate Black pieces
    // 2. active color
    //  - Either "w" or "b", which indicates whose turn it is.
    // 3. castling options
    //  - stores information if about who is allowed to castle.
    //  - Capital letters indicate White side lowercase letters are blacks options
    //  - the letters K and Q mean queen or kingside castle.
    //  - If no castling options it is marked with a "-"
    // 4. En passant target
    //  - marks which square is current possible to attack with en passant.
    // 5. Half move clock
    //  - Stores how many half moves have been made since the last capture or pawn move
    //  - This is used for declaring stalemate
    // 6. Fullmove number
    //  - Counts how many full moves have been made
    //  - full moves are not a part of Board since it is not required to play legal chess, it will be handled by the Chess class
    static Board LoadBoardFromFen(int& move, std::string fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1") {
        Board board = Board(new char[BOARD_STATE_SIZE]);

        std::vector<std::string> sections;

        int sectionCount = StringHelpers::split(fen, sections, ' ');

        if (sections.size() != sectionCount) {
            throw ChessException("FEN has to contain 6 sections");
            //throw new ArgumentException("FEN has to contanin 6 sections");
            //return board;
        }

        std::string positions = sections[0];
        std::string activeColor = sections[1];
        std::string castlingOptions = sections[2];
        std::string enPassantAttackedSquare = sections[3];
        std::string halfMoveClock = sections[4];
        std::string fullMoveClock = sections[5];

        int square = A8;
        
        for (auto i = 0; i < positions.length(); i++) {
            char piece = positions[i];
            if (piece == '/') {
                square -= 24;
            }
            else if (isdigit(piece)) {
                
                //square += int.Parse(piece.ToString());

                square += piece - '0';
            }
            else {
                Piece parsedPiece = PieceParser::FromChar(piece);
                //Board.PutPiece(board, square, parsedPiece);
                board.SetPiece(square, parsedPiece);
                if ((parsedPiece & PIECE_MASK) == KING) {
                    //Board.SetKingPosition(board, (parsedPiece & Piece.IS_WHITE) == Piece.IS_WHITE, square);
                    board.SetKingPosition((int)(parsedPiece & IS_WHITE), (char)square);
                }
                square++;
            }
        }

        //Board.SetIsWhitesTurn(board, activeColor == "w");
        board.setIsWhiteTurnBool(activeColor == "w");

        if (castlingOptions.find("K") != std::string::npos && board.getWhiteKingPosition() == E1 && board.GetPiece(H1) == (ROOK | IS_WHITE)) {
            //Board.SetCastleBit(board, CastlingBits.WHITE_KING_SIDE_CASTLE, true);
            //board.CastlingBits |= WHITE_KING_SIDE_CASTLE;
            board.addCastlingBit(WHITE_KING_SIDE_CASTLE);
        }
        if (castlingOptions.find("Q") != std::string::npos && board.getWhiteKingPosition() == E1 && board.GetPiece(A1) == (ROOK | IS_WHITE)) {
            //Board.SetCastleBit(board, WHITE_QUEEN_SIDE_CASTLE, true);
            //board.CastlingBits |= WHITE_QUEEN_SIDE_CASTLE;
            board.addCastlingBit(WHITE_QUEEN_SIDE_CASTLE);
        }
        if (castlingOptions.find("k") != std::string::npos && board.getBlackKingPosition() == E8 && board.GetPiece(H8) == ROOK) {
            //Board.SetCastleBit(board, BLACK_KING_SIDE_CASTLE, true);
            //board.CastlingBits |= BLACK_KING_SIDE_CASTLE;
            board.addCastlingBit(BLACK_KING_SIDE_CASTLE);
        }
        if (castlingOptions.find("q") != std::string::npos && board.getBlackKingPosition() == E8 && board.GetPiece(A8) == ROOK) {
            //Board.SetCastleBit(board, BLACK_QUEEN_SIDE_CASTLE, true);
            //board.CastlingBits |= BLACK_QUEEN_SIDE_CASTLE;
            board.addCastlingBit(BLACK_QUEEN_SIDE_CASTLE);
        }

        if (enPassantAttackedSquare == "-") {
            //Board.SetEnPassantAttackedSquare(board, EnPassant.NO_ENPASSANT);
            //board.EnPassantTarget = EnPassant.NO_ENPASSANT;
            board.setEnPassantTarget(NO_ENPASSANT);
        }
        else {
            //Board.SetEnPassantAttackedSquare(board, Board.AlgebraicPosition(enPassantAttackedSquare));
            //board.EnPassantTarget = (byte)BoardPosition.ArrayPosition(enPassantAttackedSquare);
            board.setEnPassantTarget((char)BoardPosition::ArrayPosition(enPassantAttackedSquare));
        }

        //Board.SetHalfTurnCounter(board, int.Parse(halfMoveClock));
        //board.HalfTurnCounter = (byte)int.Parse(halfMoveClock);
        board.setHalfTurnCounter(std::stoi(halfMoveClock));

        //Board.SetFullMoveClock(board, int.Parse(fullMoveClock));
        //move = (short)int.Parse(fullMoveClock);
        move = std::stoi(fullMoveClock);
        return board;
    }

};

