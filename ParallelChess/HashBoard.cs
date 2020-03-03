using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace ParallelChess {

    public static class RandomVariables {

    }
    // based on https://en.wikipedia.org/wiki/Zobrist_hashing
    public static class HashBoard {

        public static int pieceLength = (int)Piece.ENPASSANT_TARGET;

        // because the pieces do not have low numeric value from 0 to 12(instead it is something like 0-25) and the boardSize is 120 instead of 64 
        // this hash table is going to be bigger than it needs to be, which means it will fill more than it needs to,
        // but it doesn't really matter since we only create it once.
        // the board could have been smaller, but then we would need to translate piece and board numbers into optimized values which takes time
        // instead we create a lookup table for their natual format, which should give us a performance boost
        // as long as we are not working on a memory constrained system then it should be fine.
        // this could be anoying for cache misses
        public static ulong[] hashTable = new ulong[BoardStateOffset.BOARD_STATE_SIZE * pieceLength + pieceLength];
        public static ulong[] enpassantHashTable = new ulong[EnPassant.NO_ENPASSANT];
        public static ulong[] castlingOptions = new ulong[(int)CastlingBits.CAN_ALL + 1];
        public static ulong whiteTurn;

        private static Piece[] pieceVariations = {
            Piece.EMPTY,

            Piece.PAWN | Piece.IS_WHITE,
            Piece.ROOK|Piece.IS_WHITE,
            Piece.KNIGHT|Piece.IS_WHITE,
            Piece.BISHOP |Piece.IS_WHITE,
            Piece.KING | Piece.IS_WHITE,
            Piece.QUEEN | Piece.IS_WHITE,

            Piece.PAWN,
            Piece.ROOK,
            Piece.KNIGHT,
            Piece.BISHOP,
            Piece.KING,
            Piece.QUEEN,

            Piece.ENPASSANT_TARGET,
        };



        private static Random random = new Random();

        static HashBoard() {
            for (int column = 0; column < 8; column++) {
                for (int row = 0; row < 8 * BoardStateOffset.ROW_OFFSET; row += BoardStateOffset.ROW_OFFSET) {
                    int position = column + row;
                    foreach (var variation in pieceVariations) {
                        int offset = position * pieceLength + (int)variation;
                        hashTable[offset] = LongRandom(random);
                    }
                }
            }


            for (int i = 0; i < enpassantHashTable.Length; i++) {
                enpassantHashTable[i] = LongRandom(random);
            }

            for (int i = 0; i < castlingOptions.Length; i++) {
                castlingOptions[i] = LongRandom(random);
            }

            whiteTurn = LongRandom(random);
        }

        public static ulong pieceHash(int position, Piece piece) {
            return hashTable[position * pieceLength + (byte)piece];
        }
        public static ulong enpassantHash(int position) {
            return enpassantHashTable[position];
        }


        public static ulong ApplyMove(Board board, Move move, ulong boardHash) {
            var fromPiece = board.GetPiece(move.fromPosition);
            var toPiece = board.GetPiece(move.targetPosition);
            var toPosition = move.targetPosition;
            var fromPosition = move.fromPosition;

            var promotion = ((Piece)move.promotion == Piece.EMPTY) ? fromPiece : (Piece)move.promotion | (fromPiece & Piece.IS_WHITE);


            // remove the from piece
            boardHash ^= pieceHash(fromPosition, fromPiece);
            // add nothing to fromPosition
            boardHash ^= pieceHash(fromPosition, Piece.EMPTY);
            // remove the previous piece at the target location
            boardHash ^= pieceHash(toPosition, toPiece);
            // add the moved piece to the target position
            boardHash ^= pieceHash(toPosition, promotion);
            // switch turn
            boardHash ^= whiteTurn;

            MoveFlags moveFlags = (MoveFlags)move.moveFlags;


            if ((moveFlags & MoveFlags.BIG_PAWN_MOVE) == MoveFlags.BIG_PAWN_MOVE) {
                // toPosition -16 if white and + 16 when black.
                boardHash ^= enpassantHash(toPosition + BoardStateOffset.ROW_OFFSET - BoardStateOffset.ROW_OFFSET * 2 * board.IsWhiteTurn);
            }

            if (board.EnPassantTarget != EnPassant.NO_ENPASSANT) {
                // if there is a enpassant target on the board then unmark the enpassant target
                boardHash ^= enpassantHash(board.EnPassantTarget);
            }

            if (((MoveFlags)move.moveFlags & MoveFlags.ENPASSANT) == MoveFlags.ENPASSANT) {
                int enpassantSpawnPosition = move.targetPosition - BoardStateOffset.ROW_OFFSET + 2 * BoardStateOffset.ROW_OFFSET * board.IsWhiteTurn;

                // toPosition -16 if white and + 16 when black.
                int takenPosition = toPosition + BoardStateOffset.ROW_OFFSET - BoardStateOffset.ROW_OFFSET * 2 * board.IsWhiteTurn;
                boardHash ^= pieceHash(takenPosition, Piece.EMPTY);
                boardHash ^= pieceHash(takenPosition, board.GetPiece(takenPosition));
            }

            var nextCastlingBits = board.CastlingBits
                & CastlingHelper.castleLookup[toPosition]
                & CastlingHelper.castleLookup[fromPosition];

            if (nextCastlingBits != board.CastlingBits) {
                // remove the previous castlingOptions;
                boardHash ^= castlingOptions[(int)board.CastlingBits];
                // apply the new castlingOptions
                boardHash ^= castlingOptions[(int)nextCastlingBits];
            }

            if ((moveFlags & MoveFlags.CASTLING) == MoveFlags.CASTLING) {
                // if the target move is less than the kingsposition it is queenside castling, 
                // otherwise it is kingside castle 
                if (toPosition < fromPosition) {
                    // remove the rook
                    boardHash ^= pieceHash(fromPosition - 4, board.GetPiece(fromPosition - 4)); // remove the rook
                    boardHash ^= pieceHash(fromPosition - 4, Piece.EMPTY); // place nothing where the rook once was
                    // add the rook
                    boardHash ^= pieceHash(fromPosition - 1, board.GetPiece(fromPosition - 4)); // place the rook
                    boardHash ^= pieceHash(fromPosition - 1, Piece.EMPTY); // remove the nothing
                } else {
                    // remove the rook
                    boardHash ^= pieceHash(fromPosition + 3, board.GetPiece(fromPosition + 3)); // remove the rook
                    boardHash ^= pieceHash(fromPosition + 3, Piece.EMPTY); // place nothing where the rook once was
                    // add the rook
                    boardHash ^= pieceHash(fromPosition + 1, board.GetPiece(fromPosition + 3)); // place the rook
                    boardHash ^= pieceHash(fromPosition + 1, Piece.EMPTY); // remove the nothing
                }
            }

            return boardHash;
        }


        public static ulong hash(Board board) {
            ulong hash = 0;
            for (int column = 0; column < 8; column++) {
                for (int row = 0; row < 8 * BoardStateOffset.ROW_OFFSET; row += BoardStateOffset.ROW_OFFSET) {
                    int position = column + row;
                    Piece piece = board.GetPiece(position);
                    ulong bitString = hashTable[position * pieceLength + (int)piece];
                    hash ^= bitString;
                }
            }

            hash ^= board.IsWhiteTurn * whiteTurn;

            hash ^= castlingOptions[(int)board.CastlingBits];

            if (board.EnPassantTarget != EnPassant.NO_ENPASSANT) {
                hash ^= enpassantHash(board.EnPassantTarget);
            }

            return hash;
        }

        public static ulong LongRandom(Random rand) {
            ulong result = (ulong)rand.Next(int.MinValue, int.MaxValue);
            result = result << 32;
            result = result | (ulong)rand.Next(int.MinValue, int.MaxValue);

            return result;
        }
    }
}
