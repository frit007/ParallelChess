#pragma once

#include <map>
#include <stack>
#include <vector>
#include <iostream>

#include "BoardPosition.h"
#include "BoardStateOffset.h"
#include "CastlingBits.h"
#include "Piece.h"
#include "MoveOption.h"
#include "MoveFlags.h"
#include "Move.h";
#include "MoveHelper.h"
#include "StringHelpers.h"
#include "Enpassant.h"
#include "Winner.h"
#include "ChessException.h"

const int kingMoves[] = {
    ROW_OFFSET * 1 + 1,
    ROW_OFFSET * -1 + 1,
    ROW_OFFSET * 1 + -1,
    ROW_OFFSET * -1 + -1,
    ROW_OFFSET,
    -ROW_OFFSET,
    1,
    -1
};

const int straightMoves[] = {
    ROW_OFFSET,
    -ROW_OFFSET,
    1,
    -1
};

const int slantedMoves[] = {
    ROW_OFFSET * 1 + 1,
    ROW_OFFSET * -1 + 1,
    ROW_OFFSET * 1 + -1,
    ROW_OFFSET * -1 + -1,
};

const int knightMoves[] = {
           -1 + ROW_OFFSET * 2 , 1 + ROW_OFFSET * 2,
    -2 + ROW_OFFSET * 1,              2 + ROW_OFFSET * 1,
    -2 + ROW_OFFSET * -1,              2 + ROW_OFFSET * -1,
           -1 + ROW_OFFSET * -2 , 1 + ROW_OFFSET * -2,
};

struct Board {
    char* bytes;

    Board(char arg_bytes[BOARD_STATE_SIZE]) {
        bytes = arg_bytes;
        zeroOutBytes();
    }

    void zeroOutBytes() {
        for (size_t i = 0; i < BOARD_STATE_SIZE; i++) {
            bytes[i] = 0;
        }
    }

    Piece GetPiece(int position) {
        return (Piece)bytes[position];
    }

    void SetPiece(int position, Piece piece) {
        bytes[position] = (char)piece;
    }

    CastlingBits getCastlingBits() {
        return (CastlingBits)bytes[CASTLING_OFFSET];
    }

    void addCastlingBit(CastlingBits castlingBits) {
        bytes[CASTLING_OFFSET] |= castlingBits;
    }

    void setCastlingBits(CastlingBits value) {
        bytes[CASTLING_OFFSET] = (char)value;
    }
    char getEnPassantTarget(){
        return bytes[EN_PASSANT_FIELD_OFFSET]; 
    }

    void setEnPassantTarget(char value) { 
        bytes[EN_PASSANT_FIELD_OFFSET] = value; 
    }

    
    char getHalfTurnCounter() { 
        return bytes[HALF_TURN_COUNTER_OFFSET]; 
    }
    
    void setHalfTurnCounter(char value) { 
        bytes[HALF_TURN_COUNTER_OFFSET] = value; 
    }

    void incrementHalfTurnCounter() {
        bytes[HALF_TURN_COUNTER_OFFSET]++;
    }
    
    char GetKingPosition(int isWhite) {
        return bytes[BLACK_KING_POSITION_OFFSET + isWhite];
    }

    void SetKingPosition(int isWhite, char position) {
        bytes[BLACK_KING_POSITION_OFFSET + isWhite] = position;
    }
    
    char getWhiteKingPosition() { 
        return bytes[WHITE_KING_POSITION_OFFSET]; 
    }
    void setWhiteKingPosition(char value) { 
        bytes[WHITE_KING_POSITION_OFFSET] = value; 
    }
    

    char getBlackKingPosition() { 
        return bytes[BLACK_KING_POSITION_OFFSET];
    }

    void setBlackKingPosition(char value) { 
        bytes[BLACK_KING_POSITION_OFFSET] = value;
    }

    char getVirtualLevel(){ 
        return bytes[VIRTUAL_LEVEL_OFFSET]; 
    }
    void setVirtualLevel(char value){ 
        bytes[VIRTUAL_LEVEL_OFFSET] = value; 
    }
    
    char getIsWhiteTurn() { 
        return bytes[IS_WHITE_TURN_OFFSET]; 
    }
    char setIsWhiteTurn(char value) { 
        bytes[IS_WHITE_TURN_OFFSET] = value; 
    }

    void flipIsWhiteTurn() {
        bytes[IS_WHITE_TURN_OFFSET] ^= 1;
    }

    bool IsWhiteTurnBool() { 
        return 0 != bytes[IS_WHITE_TURN_OFFSET]; 
    }

    void setIsWhiteTurnBool(bool value) {
        bytes[IS_WHITE_TURN_OFFSET] = value ? 1 : 0;
    }

    //// Forsyth–Edwards Notation of the current chess board state
    //// the board doesn't keep track of the moves since it doesn't affect any chess rule
    //// that part is stored in the Chess wrapper
    //std::string simplifiedFEN{
    //     return  "";
    //     //get { return ChessOutput.BoardToFen(this, 0); }
    //}

    bool PieceBelongsToMe(Piece piece) {
        // we abuse that Piece stores the information about color in its last bit
        // therefor we can mask that bit away for example the piece 0x07 (white king) & 0x01(isWhitecheck) = 0x01
        // this always produces 1 or 0 which can be directly compared to isWhiteTurn.
        // IsWhite is either 0x01 (true) or 0x00 (false)
        
        return getIsWhiteTurn() == (char)(IS_WHITE & piece);
    }

    bool IsPositionEmpty(int position) {
        return GetPiece(position) == PIECE_EMPTY;
    }

    bool IsPositionEmptyByte(int position) {
        return bytes[position] == 0;
    }

    static bool IsPositionEmptyByte(Board board, int position) {
        return board.bytes[position] == 0;
    }

    static bool IsPositionEmpty(Board board, int position) {
        return board.GetPiece(position) == PIECE_EMPTY;
    }

    MoveOption CanITakeSquare(int position) {
        auto piece = GetPiece(position);
        if (piece == PIECE_EMPTY) {
            return MoveOption::NO_FIGHT;
        }

        // check that the current color is not the same as the moving piece
        if (PieceBelongsToMe(piece)) {
            return MoveOption::INVALID_MOVE;
        }
        else {
            return MoveOption::CAPTURE;
        }
    }



    bool CanICastleQueenSide() {
        CastlingBits castlingBits = getCastlingBits();

        // TODO bitshift optimize?
        if (IsWhiteTurnBool()) {
            return (castlingBits & WHITE_QUEEN_SIDE_CASTLE) == WHITE_QUEEN_SIDE_CASTLE;
        }
        else {
            return (castlingBits & BLACK_QUEEN_SIDE_CASTLE) == BLACK_QUEEN_SIDE_CASTLE;
        }
    }


    bool CanCastleKingSide() {
        CastlingBits castlingBits = getCastlingBits();

        // TODO bitshift optimize?
        if (IsWhiteTurnBool()) {
            return (castlingBits & WHITE_KING_SIDE_CASTLE) == WHITE_KING_SIDE_CASTLE;
        }
        else {
            return (castlingBits & BLACK_KING_SIDE_CASTLE) == BLACK_KING_SIDE_CASTLE;
        }
    }


    void AddPawnMove(int fromPosition, int targetPosition, MoveFlags move, std::vector<Move>* moves) {
        Piece takenPiece;
        
        int row = BoardPosition::PositionRow(targetPosition);
        if ((move & ENPASSANT) == ENPASSANT) {
            takenPiece = PAWN;
        }
        else {
            takenPiece = GetPiece(targetPosition);
        }

        // check if the pawn is going to move into a promotion row.
        // we don't check the color because a pawn can never enter its own promotion area
        if (row == 0 || row == 7) {
            moves->push_back(MoveHelper::CreateMove(targetPosition, fromPosition, takenPiece, QUEEN, move, this));
            moves->push_back(MoveHelper::CreateMove(targetPosition, fromPosition, takenPiece, BISHOP, move, this));
            moves->push_back(MoveHelper::CreateMove(targetPosition, fromPosition, takenPiece, KNIGHT, move, this));
            moves->push_back(MoveHelper::CreateMove(targetPosition, fromPosition, takenPiece, ROOK, move, this));
        }
        else {
            moves->push_back(MoveHelper::CreateMove(targetPosition, fromPosition, takenPiece, PIECE_EMPTY, move, this));
        }
    }

    void AddMove(int fromPosition, int targetPosition, MoveFlags moveBits, std::vector<Move>* moves) {
        Piece takenPiece = GetPiece(targetPosition);
        moves->push_back(MoveHelper::CreateMove(targetPosition, fromPosition, takenPiece, PIECE_EMPTY, moveBits, this));
    }

    
    //void rel() {
    //    for (auto i : A)
    //    {

    //    }
    //}
    template<int N>
    void WalkRelativePaths(int fromPosition, const int movePositions[N], std::vector<Move>* moves) {

        for (size_t i = 0; i < N; i++) {
            int relativePosition = movePositions[i];
            int move = fromPosition;
            do {
                move += relativePosition;
                if (BoardPosition::IsValidPosition(move)) {
                    auto moveOption = CanITakeSquare(move);
                    if (moveOption == MoveOption::NO_FIGHT) {
                        AddMove(fromPosition, move, MOVEFLAG_EMPTY, moves);
                    }
                    else if (moveOption == MoveOption::CAPTURE) {
                        AddMove(fromPosition, move, MOVEFLAG_EMPTY, moves);
                        break;
                    }
                    else {
                        break;
                    }
                }
                else {
                    break;
                }
            } while (true);
        }
    }


    template<int N>
    void RelativePath(int fromPosition, const int relativePaths[N], std::vector<Move>* moves) {
        for (size_t i = 0; i < N; i++)
        {
            int move = relativePaths[i] + fromPosition;
            if (BoardPosition::IsValidPosition(move)) {
                auto moveOption = CanITakeSquare(move);
                if (moveOption != MoveOption::INVALID_MOVE) {
                    AddMove(fromPosition, move, MoveFlags::MOVEFLAG_EMPTY, moves);
                }
            }
        }
    }


    std::vector<Move>* GetMoves(std::vector<Move>* moves = nullptr) {
        if (moves == nullptr) {
            moves = new std::vector<Move>();
        }

        for (int column = 0; column < 8; column++) {
            for (int row = 0; row < 8 * ROW_OFFSET; row += ROW_OFFSET) {
                moves = GetMovesForPosition(column + row, moves);
            }
        }

        return moves;
    }


    std::vector<Move>* GetMovesForPosition(int fromPosition, std::vector<Move>* moves = nullptr) {
        if (moves == nullptr) {
            moves = new std::vector<Move>();
        }

        Piece piece = GetPiece(fromPosition);
        if (!PieceBelongsToMe(piece)) {
            // is the piece of the same color as the current turn
            // TODO: maybe move this check out to a higher level
            return moves;
        }
        Piece justPiece = piece & PIECE_MASK;



        switch (justPiece) {
        case PAWN:{
            bool isWhitesTurn = IsWhiteTurnBool();
            int direction = isWhitesTurn ? 1 : -1;

            // We don't need to check if there is a next row is outside the board.
            // because the pawn is never able to stand on the last because of promotion
            int moveOne = BoardPosition::RelativePosition(fromPosition, 0, direction);
            if (IsPositionEmpty(moveOne)) {
                AddPawnMove(fromPosition, moveOne, PAWN_MOVE, moves);
            }

            // check if the pawn is on the starting position. If it is then assume that it is possible to move forward
            if (isWhitesTurn ? BoardPosition::PositionRow(fromPosition) == 1 : BoardPosition::PositionRow(fromPosition) == 6) {
                int hasToBeEmptyPosition = BoardPosition::RelativePosition(fromPosition, 0, direction);
                int move = BoardPosition::RelativePosition(fromPosition, 0, 2 * direction);
                if (IsPositionEmpty(move) && IsPositionEmpty(hasToBeEmptyPosition)) {
                    AddPawnMove(fromPosition, move, (MoveFlags)(BIG_PAWN_MOVE | PAWN_MOVE), moves);
                }
            }

            int column = BoardPosition::PositionColumn(fromPosition);
            auto enPassantTarget = getEnPassantTarget();
            // check if the pawn is on the right column. 
            // We don't need to check if there is a next row is outside the board.
            // because the pawn is never able to stand on the last because of promotion
            if (column != BoardPosition::H_COLUMN) {
                int move = BoardPosition::RelativePosition(fromPosition, 1, direction);
                bool isEnpassant = enPassantTarget == move;
                if (
                    // targetposition has to either be the enpassant square.
                    isEnpassant
                    || CanITakeSquare(move) == MoveOption::CAPTURE)
                    // or be empty or contain an enemy) {
                    AddPawnMove(fromPosition, move, PAWN_MOVE | (isEnpassant ? ENPASSANT : MOVEFLAG_EMPTY), moves);
            }
            // check if the pawn is on the left column. 
            // We don't need to check if there is a next row is outside the board.
            // because the pawn is never able to stand on the last because of promotion
            if (column != BoardPosition::A_COLUMN) {
                int move = BoardPosition::RelativePosition(fromPosition, -1, direction);
                bool isEnpassant = enPassantTarget == move;
                if (
                    // targetposition has to either be the enpassant square.
                    isEnpassant
                    || CanITakeSquare(move) == MoveOption::CAPTURE)
                    // or be empty or contain an enemy) {
                    AddPawnMove(fromPosition, move, PAWN_MOVE | (isEnpassant ? ENPASSANT : MOVEFLAG_EMPTY), moves);
            }

            break;
        }
        case KING:{
            RelativePath<sizeof(kingMoves)/sizeof(*kingMoves)>(fromPosition, kingMoves, moves);

            char isWhite = getIsWhiteTurn();
            // check if they are allowed to castle
            // 1. Check history if rook or king has moved. This information is stored in the castlingBit
            // 2. Check if the king moves through any square that is under attack
            // 3. Check if the king moves through a square that is not empty
            if (CanCastleKingSide()
                && IsPositionEmpty(fromPosition + 1)
                && IsPositionEmpty(fromPosition + 2)
                && !Attacked(fromPosition + 0, isWhite)
                && !Attacked(fromPosition + 1, isWhite)
                && !Attacked(fromPosition + 2, isWhite)
                ) {
                AddMove(fromPosition, fromPosition + 2, MoveFlags::CASTLING, moves);
            }

            // Do the same queen side
            if (CanICastleQueenSide()
                && IsPositionEmpty(fromPosition - 1)
                && IsPositionEmpty(fromPosition - 2)
                && IsPositionEmpty(fromPosition - 3)
                && !Attacked(fromPosition - 0, isWhite)
                && !Attacked(fromPosition - 1, isWhite)
                && !Attacked(fromPosition - 2, isWhite)
                ) {
                AddMove(fromPosition, fromPosition - 2, MoveFlags::CASTLING, moves);
            }

            }break;
        case KNIGHT:
            RelativePath<sizeof(knightMoves) / sizeof(*knightMoves)>(fromPosition, knightMoves, moves);

            break;
        case QUEEN:
            WalkRelativePaths<sizeof(slantedMoves)/sizeof(*slantedMoves)>(fromPosition, slantedMoves, moves);
            WalkRelativePaths<sizeof(straightMoves) / sizeof(*straightMoves)>(fromPosition, straightMoves, moves);
            break;
        case ROOK:
            WalkRelativePaths<sizeof(straightMoves) / sizeof(*straightMoves)>(fromPosition, straightMoves, moves);
            break;
        case BISHOP:
            WalkRelativePaths<sizeof(slantedMoves) / sizeof(*slantedMoves)>(fromPosition, slantedMoves, moves);
            break;
        case PIECE_EMPTY:
            break;
        default:
            
            throw ChessException("Invalid piece: " + std::to_string((int)piece));
            //throw new exception("invalid piece: " + (int)piece);
            // TOOD handle unknown
            break;
        }

        return moves;
    }



    bool Attacked(int position, char pretendToBeWhite) {
        int theirColor = pretendToBeWhite ^ 1;
        Piece theirColorPiece = (Piece)theirColor;

        for(auto move : slantedMoves) {
            int relativePosition = position;
            // king filter is used to allow kings to attack one square
            // they are disabled are the first rotation
            bool isFirstPosition = true;
            do {
                relativePosition += move;
                if (BoardPosition::IsValidPosition(relativePosition)) {
                    auto piece = GetPiece(relativePosition);
                    if (piece != PIECE_EMPTY) {
                        Piece enemySlantedAttacked = (theirColorPiece | ATTACKS_SLANTED);
                        if ((piece & (ATTACKS_SLANTED | IS_WHITE)) == enemySlantedAttacked) {
                            return true;
                        }
                        if (isFirstPosition) {
                            Piece kingFilter = (theirColorPiece | KING);
                            if ((piece & (PIECE_MASK | IS_WHITE)) == kingFilter) {
                                return true;
                            }
                        }
                        break;
                    }
                }
                else {
                    break;
                }
                isFirstPosition = false;
            } while (true);
        }

        for(auto move : straightMoves) {
            int relativePosition = position;
            // king filter is used to allow kings to attack one square
            // they are disabled are the first square
            bool isFirstPosition = true;
            do {
                relativePosition += move;
                if (BoardPosition::IsValidPosition(relativePosition)) {
                    auto piece = GetPiece(relativePosition);
                    if (piece != PIECE_EMPTY) {
                        Piece enemySlantedAttacked = (theirColorPiece | ATTACKS_STRAIGHT);
                        if ((piece & (ATTACKS_STRAIGHT | IS_WHITE)) == enemySlantedAttacked) {
                            return true;
                        }
                        if (isFirstPosition) {
                            Piece kingFilter = (theirColorPiece | KING);
                            if ((piece & (PIECE_MASK | IS_WHITE)) == kingFilter) {
                                return true;
                            }
                        }
                        break;
                    }
                }
                else {
                    break;
                }
                isFirstPosition = false;
            } while (true);
        }

        for(auto move : knightMoves) {
            int relativePosition = position + move;

            if (BoardPosition::IsValidPosition(relativePosition)) {
                auto piece = GetPiece(relativePosition);
                Piece enemySlantedAttacked = (theirColorPiece | KNIGHT);
                if ((piece & (PIECE_MASK | IS_WHITE)) == enemySlantedAttacked) {
                    return true;
                }
            }
        }

        int leftPawnPosition = position - ROW_OFFSET - 1 + ROW_OFFSET * pretendToBeWhite * 2;
        int rightPawnPosition = position - ROW_OFFSET + 1 + ROW_OFFSET * pretendToBeWhite * 2;
        if (BoardPosition::IsValidPosition(leftPawnPosition)) {
            Piece leftPawn = GetPiece(leftPawnPosition);
            if (leftPawn == (theirColorPiece | PAWN)) {
                return true;
            }
        }

        if (BoardPosition::IsValidPosition(rightPawnPosition)) {
            Piece rightPawn = GetPiece(rightPawnPosition);
            if (rightPawn == (theirColorPiece | PAWN)) {
                return true;
            }
        }

        return false;
    }


    bool IsLegalMove(Move move) {
        char myTurn = getIsWhiteTurn();
        Play(move);

        auto notAttacked = !Attacked(GetKingPosition(myTurn), myTurn);

        UndoMove(move);

        return notAttacked;
    }

    Move FindMove(int from, int to) {
        std::vector<Move>* moves = GetMovesForPosition(from);

        Move targetPosition = MoveHelper::FindTargetPosition(moves, to);
        
        delete moves;

        if (!MoveHelper::isValidMove(targetPosition)) {
            //throw new Exception("Move not found");
            throw ChessException("Move not found");
            //return Move(0, 0, 0, 0, 0, 0, 0, 0);
        }
        if (!IsLegalMove(targetPosition)) {
            //throw new Exception("Illegal move");
            throw ChessException("Illegal move");
            //return Move(0, 0, 0, 0, 0, 0, 0, 0);
        }

        return targetPosition;
    }

    Move FindMove(int from, int to, Piece promotion) {
        std::vector<Move>* moves = GetMovesForPosition(from);

        Move targetPosition = MoveHelper::FindTargetPosition(moves, to, promotion);
        
        delete moves;

        if (!MoveHelper::isValidMove(targetPosition)) {
            //throw new Exception("Move not found");
            throw ChessException("Move not found");
            //return Move(0, 0, 0, 0, 0, 0, 0, 0);
        }

        if (!IsLegalMove(targetPosition)) {
            //throw new Exception("Illegal move");
            throw ChessException("Illegal move");
            //return Move(0, 0, 0, 0, 0, 0, 0, 0);
        }

        return targetPosition;
    }

    Move Play(int from, int to) {
        Move targetPosition = FindMove(from, to);

        Play(targetPosition);

        return targetPosition;
    }

    //Move Play(std::string san) {
    //    san = StringHelpers::trim(san);
    //    auto moves = GetMoves();
    //    for(auto move : *moves) {
    //        if (IsLegalMove(move) && StandardAlgebraicNotation(move) == san) {
    //            Play(move);
    //            delete moves;
    //            return move;
    //        }
    //    }
    //    delete moves;

    //    //throw new Exception("Move not found!");
    //    return Move(0, 0, 0, 0, 0, 0, 0, 0);
    //}

    Move Play(int from, int to, Piece promotion) {
        Move targetPosition = FindMove(from, to, promotion);

        Play(targetPosition);

        return targetPosition;
    }

    void Play(Move move) {
        int toPosition = move.targetPosition;
        int fromPosition = move.fromPosition;
        char isWhiteTurn = getIsWhiteTurn();
        bool isWhitesTurnBool = IsWhiteTurnBool();
        

        Piece piece = GetPiece(fromPosition);
        Piece pieceType = piece & PIECE_MASK;
        Piece takenPiece = GetPiece(toPosition);
        Piece promotion = (Piece)move.promotion;
        MoveFlags moveFlags = (MoveFlags)move.moveFlags;

        if ((moveFlags & BIG_PAWN_MOVE) == BIG_PAWN_MOVE) {
            // when making a big pawn move mark the square behind the moving pawn vulnerable to 
            if (isWhitesTurnBool) {
                setEnPassantTarget((char)(toPosition - ROW_OFFSET));
            }
            else {
                setEnPassantTarget((char)(toPosition + ROW_OFFSET));
            }
        }
        else {
            setEnPassantTarget(NO_ENPASSANT);
        }
        switch (pieceType) {
        case PAWN:
            if ((moveFlags & ENPASSANT) == ENPASSANT) {
                // When taking with enpassant remove the piece
                if (isWhitesTurnBool) {
                    SetPiece(toPosition - ROW_OFFSET, PIECE_EMPTY);
                }
                else {
                    SetPiece(toPosition + ROW_OFFSET, PIECE_EMPTY);
                }
            }

            break;
        case KING:
            if ((moveFlags & CASTLING) == CASTLING) {
                if (BoardPosition::PositionColumn(toPosition) < BoardPosition::E_COLUMN) {
                    // if position is less than E_COLUMN then we are castling queen side
                    // copy the rook from the square so we correctly handle the color
                    SetPiece(toPosition + 1, GetPiece(toPosition - 2));
                    SetPiece(toPosition - 2, PIECE_EMPTY);
                }
                else {
                    // otherwise we are castling king side
                    // copy the rook from the square so we correctly handle the color
                    SetPiece(toPosition - 1, GetPiece(toPosition + 1));
                    SetPiece(toPosition + 1, PIECE_EMPTY);
                }
            }
            SetKingPosition(isWhiteTurn, (char)toPosition);
            break;
        }


        // remove opportunity to castle based on the position on the board
        setCastlingBits(getCastlingBits()
            & castleLookup[toPosition]
            & castleLookup[fromPosition]);


        // move piece to new position
        if (promotion == PIECE_EMPTY) {
            SetPiece(toPosition, piece);
        }
        else {
            SetPiece(toPosition, promotion | (Piece)isWhiteTurn);
        }

        // remove piece from previous position
        SetPiece(fromPosition, PIECE_EMPTY);

        if (pieceType == PAWN || ((takenPiece & PIECE_MASK) != PIECE_EMPTY)) {
            setHalfTurnCounter(0);
        }
        else {
            incrementHalfTurnCounter();
        }

        // flip turn
        flipIsWhiteTurn();
    }


    void UndoMove(Move move) {
        int targetPosition = move.targetPosition;
        int fromPosition = move.fromPosition;
        int theirColor = getIsWhiteTurn();
        int ourColor = getIsWhiteTurn() ^ 1;

        bytes[fromPosition] = bytes[targetPosition];

        bytes[targetPosition] = move.capturedPiece;

        Piece movedPiece = GetPiece(fromPosition);

        CastlingBits previous = (CastlingBits)move.previousCastlingBits;
        bytes[HALF_TURN_COUNTER_OFFSET] = move.previousHalfMove;
        bytes[CASTLING_OFFSET] = move.previousCastlingBits;
        bytes[EN_PASSANT_FIELD_OFFSET] = move.previousEnpassant;
        MoveFlags moveFlags = (MoveFlags)move.moveFlags;

        if ((moveFlags & ENPASSANT) == ENPASSANT) {
            // when undoing a enpassant move spawn their pawn back
            // we abuse that isWhite is a integer which is 1 on whites turn and 0 and blacks turn
            // if it was black move then we have to spawn it one row above
            // if it was whites move we spawn it one move below
            // keep in mind the IsWhiteTurn is currently opposite of who made the move
            int enpassantSpawnPosition = targetPosition - ROW_OFFSET + 2 * ROW_OFFSET * getIsWhiteTurn();
            SetPiece(enpassantSpawnPosition, PAWN | (Piece)theirColor);

            // when capturing with enpassant don't place the captured piece back since it was taken from another square
            bytes[targetPosition] = (char)PIECE_EMPTY;
        }
        if (move.promotion != 0) {
            bytes[fromPosition] = (char)(PAWN | (Piece)ourColor);
        }

        // if black made a move decrement the turn counter
        // we abuse that isWhite is a integer which is 1 on whites turn and 0 and blacks turn
        //TurnCounter -= IsWhiteTurn;

        if ((movedPiece & PIECE_MASK) == KING) {
            if ((moveFlags & CASTLING) == CASTLING) {
                // if the target move is less than the kingsposition it is queenside castling, 
                // otherwise it is kingside castle 
                if (targetPosition < fromPosition) {
                    // copy the rook back to its starting position 
                    bytes[fromPosition - 4] = bytes[fromPosition - 1];
                    bytes[fromPosition - 1] = 0;
                }
                else {
                    bytes[fromPosition + 3] = bytes[fromPosition + 1];
                    bytes[fromPosition + 1] = 0;
                }
            }
            SetKingPosition(ourColor, (char)fromPosition);
        }

        
        //IsWhiteTurn = (byte)ourColor;
        flipIsWhiteTurn();
    }

    Winner detectWinnerAreThereValidMoves(bool areThereValidMoves) {
        if (!areThereValidMoves) {
            // check if king is under attack
            if (Attacked(GetKingPosition(getIsWhiteTurn()), getIsWhiteTurn())) {
                if (IsWhiteTurnBool()) {
                    return WINNER_BLACK;
                }
                else {
                    return WINNER_WHITE;
                }
            }
            else {
                return DRAW;
            }
        }

        if (hasInsufficientMaterialOrTimeLimit()) {
            return DRAW;
        }

        return NO_WINNER;
    }

    bool hasInsufficientMaterialOrTimeLimit() {
        return getHalfTurnCounter() >= 50 || detectInsufficientMaterial();
    }

    //std::map<unsigned long, int> RepeatedPositions(std::vector<Move> history) {
    //    std::stack<Move> madeMoves = std::stack<Move>();

    //    auto repeatedPositions = new std::map<unsigned long, int>();
    //    auto boardHash = HashBoard.hash(this);
    //    repeatedPositions[boardHash] = 1;
    //    for(auto move : history.Reverse()) {
    //        if ((Piece)move.capturedPiece != Piece.EMPTY || (MoveFlags)move.moveFlags == MoveFlags.PAWN_MOVE) {
    //            while (madeMoves.Count != 0) {
    //                // replay the moves we undid
    //                this.Move(madeMoves.Pop());
    //            }
    //            // if a piece was captured or a pawn was moved then the previous position cannot occur again, 
    //            // meaning it is pointless to continue
    //            return repeatedPositions;
    //        }
    //        this.UndoMove(move);
    //        boardHash = HashBoard.ApplyMove(this, move, boardHash);
    //        madeMoves.Push(move);
    //        if (repeatedPositions.ContainsKey(boardHash)) {
    //            repeatedPositions[boardHash]++;
    //        }
    //        else {
    //            repeatedPositions[boardHash] = 1;
    //        }
    //    }
    //    while (madeMoves.Count != 0) {
    //        // replay the moves we undid
    //        this.Move(madeMoves.Pop());
    //    }
    //    return repeatedPositions;
    //}

    // history is used to check for repeated positions
    Winner detectWinner(std::vector<Move>* possibleMoves, std::vector<Move>* history) {
        auto winner = detectWinner(possibleMoves);
        if (winner != NO_WINNER) {
            return winner;
        }

        //var repeatedPositions = this.RepeatedPositions(history);

        //foreach(var position in repeatedPositions) {
        //    if (position.Value >= 3) {
        //        // if any position has occured more than 3 times then it a draw according to 3 fold repetition
        //        return Winner.DRAW;
        //    }
        //}
        return NO_WINNER;
    }

    Winner detectWinner(std::vector<Move>* possibleMoves) {
        for(auto move : *possibleMoves) {
            if (IsLegalMove(move)) {
                return detectWinnerAreThereValidMoves(true);
            }
        }

        return detectWinnerAreThereValidMoves(false);
    }

    // Detect Insufficient material. according to https://www.chessstrategyonline.com/content/tutorials/how-to-play-chess-draws there are 4 options for insufficient material
    // According to the website these combinations are invalid
    // King vs king
    // King and bishop vs king
    // King and knight vs king
    // King and bishop vs king and bishop of the same colour.
    // technically even though unlikely a player can have 2 bishops on the same color, in which case it is still a stalemate
    // This means we need to keep track of if there are more than 2 knights and bishops of more than 1 color
    bool detectInsufficientMaterial() {
        // the goal is to return as soon as piece is found that indicates it is not a stalemate to increase performance
        bool bishopSquareColor = false;
        bool foundBishop = false;
        bool foundHorse = false;


        bool squareColor = false;
        for (int row = 0; row < 8 * ROW_OFFSET; row += ROW_OFFSET) {
            for (int column = 0; column < 8; column++) {

                Piece piece = GetPiece(column + row);

                switch (piece & PIECE_MASK) {
                case PIECE_EMPTY:
                case KING:
                    // ignore king and empty since they don't matter
                    // the king doesn't matter because it is always there
                    break;
                case KNIGHT:
                    // if there are 2 horses of any color then it is not insufficient material
                    if (!foundHorse && !foundBishop) {
                        foundHorse = true;
                    }
                    else {
                        return false;
                    }
                    break;
                case BISHOP:
                    if (foundHorse) {
                        return false;
                    }

                    if (!foundBishop) {
                        bishopSquareColor = squareColor;
                        foundBishop = true;
                    }
                    else {
                        if (bishopSquareColor != squareColor) {
                            return false;
                        }
                    }
                    break;
                default:
                    return false;
                }

                // flip the square color
                squareColor = !squareColor;
            }
        }

        // if we get to this point there are only 2 kings left, or found which is a stalemate
        return true;
    }

    Board CreateCopy() {
        Board newBoard = Board(new char[BOARD_STATE_SIZE]);
        //Buffer.BlockCopy(bytes, 0, newBoard.bytes, 0, BOARD_STATE_SIZE);
        memcpy(bytes, newBoard.bytes, sizeof(char) * BOARD_STATE_SIZE);
        return newBoard;
    }

    void Copy(Board toBoard) {
        //Buffer.BlockCopy(bytes, 0, toBoard.bytes, 0, BOARD_STATE_SIZE);
        memcpy(bytes, toBoard.bytes, sizeof(char) * BOARD_STATE_SIZE);
    }

    //std::string StandardAlgebraicNotation(Move move) {
    //    auto moves = GetMoves();

    //    moves = moves.Where(move = > board.IsLegalMove(move)).ToList();

    //    return this.StandardAlgebraicNotation(move, moves);
    //}

    //// based on rules from https://en.wikipedia.org/wiki/Algebraic_notation_(chess)
    //// Notice the parameter legalMoves only contains legal moves
    //public string StandardAlgebraicNotation(Move move, List<Move> legalMoves) {
    //    StringBuilder san = new StringBuilder();
    //    var piece = GetPiece(move.fromPosition);
    //    if (((MoveFlags)move.moveFlags & MoveFlags.CASTLING) == MoveFlags.CASTLING) {
    //        if (move.targetPosition < move.fromPosition) {
    //            // castle queen side
    //            san.Append("O-O-O");
    //        }
    //        else {
    //            // castle king side
    //            san.Append("O-O");
    //        }
    //    }
    //    else {
    //        var isPawn = (piece & Piece.PIECE_MASK) == Piece.PAWN;

    //        if (!isPawn) {
    //            // all pieces other pawns display their piece name
    //            var pChar = PieceParser.ToChar(piece);
    //            san.Append(pChar.ToString().ToUpper());
    //        }


    //        int fromRow = move.fromPosition / BoardStateOffset.ROW_OFFSET;
    //        int fromColumn = move.fromPosition - (fromRow * BoardStateOffset.ROW_OFFSET);

    //        bool sameColumns = false;
    //        bool sameRows = false;
    //        bool isAmbigious = false;
    //        foreach(var possibleMove in legalMoves) {
    //            // check all other moves of the same piece type if they can move to the same position
    //            if (possibleMove.targetPosition == move.targetPosition // check if it can reach the same square
    //                && possibleMove.fromPosition != move.fromPosition // check if not the same piece
    //                && piece == GetPiece(possibleMove.fromPosition)) { // check it is the same type of piece

    //                isAmbigious = true;

    //                int possibleFromRow = possibleMove.fromPosition / BoardStateOffset.ROW_OFFSET;
    //                int possibleFromColumn = possibleMove.fromPosition - (possibleFromRow * BoardStateOffset.ROW_OFFSET);
    //                if (possibleFromColumn == fromColumn) {
    //                    sameColumns = true;
    //                }
    //                if (possibleFromRow == fromRow) {
    //                    sameRows = true;
    //                }
    //            }
    //        }
    //        if (isPawn) {
    //            if ((Piece)move.capturedPiece != Piece.EMPTY) {
    //                // when pawn captures always specify the starting column
    //                san.Append(Convert.ToChar('a' + (fromColumn)));
    //            }
    //        }
    //        else {
    //            if (isAmbigious) {
    //                // disambiguating moves
    //                if (sameColumns && sameRows) {
    //                    san.Append(Convert.ToChar('a' + (fromColumn)));
    //                    san.Append((fromRow + 1).ToString());
    //                }
    //                else if (sameColumns) {
    //                    san.Append((fromRow + 1).ToString());
    //                }
    //                else {
    //                    san.Append(Convert.ToChar('a' + (fromColumn)));
    //                }
    //            }
    //        }
    //        if ((Piece)move.capturedPiece != Piece.EMPTY) {
    //            san.Append("x");
    //        }

    //        int targetRow = move.targetPosition / BoardStateOffset.ROW_OFFSET;
    //        int targetColumn = move.targetPosition - (targetRow * BoardStateOffset.ROW_OFFSET);
    //        san.Append(Convert.ToChar('a' + (targetColumn))).Append((targetRow + 1).ToString());

    //        Piece promotion = (Piece)move.promotion;
    //        if (promotion != Piece.EMPTY) {
    //            san.Append("=").Append(PieceParser.ToChar(promotion).ToString().ToUpper());
    //        }
    //    }



    //    Move(move);

    //    // check for checkmate
    //    // after the move is played check if the current players king is attacked
    //    if (Attacked(GetKingPosition(IsWhiteTurn), IsWhiteTurn)) {
    //        var winner = detectWinner(GetMoves());

    //        if (winner == Winner.WINNER_BLACK || winner == Winner.WINNER_WHITE) {
    //            //san += "#";
    //            san.Append("#");
    //        }
    //        else {
    //            //san += "+";
    //            san.Append("+");
    //        }
    //    }

    //    UndoMove(move);

    //    return san.ToString();
    //}


};








