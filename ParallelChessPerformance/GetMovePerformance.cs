using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using ParallelChess;

namespace ParallelChessPerformance {
    [SimpleJob]
    public class GetMovePerformance {
        //[Benchmark]
        //public void PawnMoveFromStart() {
        //    byte[] board = Chess.LoadBoardFromFen();

        //    var moves = Board.GetMovesForPosition(board, BoardOffset.C2);
        //}

        // previous code-ish
        //Action<byte[],int,int,Move,List<Move>> addPawnMove = (byte[] _d1, int _d2, int targetPosition, Move moveBits, List<Move> _d3) => {
        //    Piece takenPiece;
        //    int row = Board.PositionRow(targetPosition);
        //    if ((moveBits & Move.ENPASSANT) == Move.ENPASSANT) {
        //        takenPiece = Piece.PAWN;
        //    } else {
        //        takenPiece = GetPiece(board, targetPosition);
        //    }

        //    // check if the pawn is going to move into a promotion row.
        //    // we don't check the color because a pawn can never enter its own promotion area
        //    byte castlingBit = (byte)Board.GetCastleBit(board);
        //    if (row == 0 || row == 7) {
        //        moves.Add(MoveHelper.CreateMove(targetPosition, fromPosition, takenPiece, Piece.BISHOP, moveBits, castlingBit));
        //        moves.Add(MoveHelper.CreateMove(targetPosition, fromPosition, takenPiece, Piece.BISHOP, moveBits, castlingBit));
        //        moves.Add(MoveHelper.CreateMove(targetPosition, fromPosition, takenPiece, Piece.BISHOP, moveBits, castlingBit));
        //        moves.Add(MoveHelper.CreateMove(targetPosition, fromPosition, takenPiece, Piece.BISHOP, moveBits, castlingBit));
        //    } else {
        //        moves.Add(MoveHelper.CreateMove(targetPosition, fromPosition, takenPiece, Piece.EMPTY, moveBits, castlingBit));
        //    }
        //};

        //Action<byte[], int, int, Move, List<Move>> AddMove = (byte[] _d1, int _d2, int targetPosition, Move moveBits, List<Move> _d3) => {
        //    Piece takenPiece = GetPiece(board, targetPosition);
        //    moves.Add(MoveHelper.CreateMove(targetPosition, fromPosition, takenPiece, Piece.EMPTY, moveBits, (byte)Board.GetCastleBit(board)));
        //};

        //Action<byte[], int, (int, int)[], List<Move>> walkRelativePaths = (byte[] _d1, int _d2, (int column, int row)[] movePositions, List<Move> _d3) => {
        //    foreach (var relativeMove in movePositions) {
        //        int move = 0;
        //        int count = 0;
        //        do {
        //            if (IsValidPosition(fromPosition, relativeMove.column * count, relativeMove.row * count)) {
        //                move = RelativePosition(fromPosition, relativeMove.column * count, relativeMove.row * count);
        //                var moveOption = CanTakeSquare(board, move);
        //                if (moveOption == MoveOption.CAPTURE) {
        //                    AddMove(board, fromPosition, move, Move.EMPTY, moves);
        //                    break;
        //                } else if (moveOption == MoveOption.NO_FIGHT) {
        //                    AddMove(board, fromPosition, move, Move.EMPTY, moves);
        //                } else {
        //                    break;
        //                }
        //            } else {
        //                break;
        //            }
        //        } while (true);
        //    }
        //};

        [Benchmark]
        public void PawnPerformanceTest() {
            BoardState board = Chess.LoadBoardFromFen();

            for (int i = 0; i < 1000000; i++) {
                Board.GetMovesForPosition(board, BoardStateOffset.E2);
            }
        }

        [Benchmark]
        public void GetAllMoves() {
            BoardState board = Chess.LoadBoardFromFen();
            for(int i = 0; i < 1000000;i++) {
                var moves = Board.GetMoves(board);
            }
        }

        [Benchmark]
        public void KnightPerformanceTest() {
            BoardState board = Chess.LoadBoardFromFen();

            for (int i = 0; i < 1000000; i++) {
                Board.GetMovesForPosition(board, BoardStateOffset.B1);
            }
        }

        [Benchmark]
        public void KingPerformanceTest() {
            BoardState board = Chess.LoadBoardFromFen();

            for (int i = 0; i < 1000000; i++) {
                Board.GetMovesForPosition(board, BoardStateOffset.E1);
            }
        }

        [Benchmark]
        public void QueenPerformanceTest() {
            BoardState board = Chess.LoadBoardFromFen();

            for (int i = 0; i < 1000000; i++) {
                Board.GetMovesForPosition(board, BoardStateOffset.D1);
            }
        }

        [Benchmark]
        public void QueenWorstCasePerformanceTest() {
            // Basically a board where the queen stands in the midle of the board which means she has to check the entire board.
            BoardState board = Chess.LoadBoardFromFen("r2qk2r/8/8/3Q4/8/8/8/R3K2R w KQkq - 0 1");

            for (int i = 0; i < 1000000; i++) {
                Board.GetMovesForPosition(board, BoardStateOffset.D5);
            }
        }
    }
}
