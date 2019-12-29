using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using ParallelChess;
using System.Threading;

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
            List<Move> moves = new List<Move>();
            for (int i = 0; i < 1000000; i++) {
                moves.Clear();
                moves = Board.GetMoves(board, moves);
            }
        }

        [Benchmark]
        public void MakeAllMoves() {
            BoardState board = Chess.LoadBoardFromFen();
            BoardState virtualBoard = Board.CreateCopyBoard(board);
            List<Move> moves = new List<Move>();
            for (int i = 0; i < 1000000; i++) {
                moves.Clear();
                moves = Board.GetMoves(board, moves);
                foreach (var move in moves) {
                    Board.CopyBoard(board, virtualBoard);
                    Board.MakeMove(virtualBoard, move);
                }
            }
        }

        [Benchmark]
        public void FindValidMoves() {
            BoardState board = Chess.LoadBoardFromFen();
            BoardState virtualBoard = Board.CreateCopyBoard(board);
            List<Move> moves = new List<Move>();
            for (int i = 0; i < 1000000; i++) {
                moves.Clear();
                moves = Board.GetMoves(board, moves);
                foreach (var move in moves) {
                    Board.IsLegalMove(virtualBoard, move);
                }
            }
        }

        [Benchmark]
        public void FindValidMovesThreads() {
            List<Thread> threads = new List<Thread>();
            int count = 4;
            for (int i = 0; i < count; i++) {
                var thread = new Thread(() => {
                    Board.initThreadStaticVariables();

                    for (int i = 0; i < (1000000 / count); i++) {
                        BoardState board = Chess.LoadBoardFromFen();
                        BoardState virtualBoard = Board.CreateCopyBoard(board);
                        List<Move> moves = new List<Move>();
                        moves.Clear();
                        moves = Board.GetMoves(board, moves);
                        foreach (var move in moves) {
                            //Board.CopyBoard(board, virtualBoard);
                            //Board.MakeMove(virtualBoard, move);
                            Board.IsLegalMove(virtualBoard, move);
                        }
                    }
                });
                thread.Start();
                threads.Add(thread);
            }

            foreach (var thread in threads) {
                thread.Join();
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

        [Benchmark]
        public void realisticTest() {
            /*
             * Board was chosen because it looks like a realistic midgame
             * Starting position (White to play)
            +---------------+
            |r n b q k b _ r| 8
            |_ p _ _ _ p p p| 7
            |p _ _ p _ n _ _| 6
            |_ _ _ _ p _ _ _| 5
            |_ _ _ N P _ _ _| 4
            |_ _ N _ _ _ _ _| 3
            |P P P _ _ P P P| 2
            |R _ B Q K B _ R| 1
            +---------------+
             A B C D E F G H
             */
            BoardState board = Chess.LoadBoardFromFen("rnbqkb1r/1p3ppp/p2p1n2/4p3/3NP3/2N5/PPP2PPP/R1BQKB1R w KQkq - 1 6");
            List<Move> moves = new List<Move>();
            for (int i = 0; i < 1000000; i++) {
                moves.Clear();
                moves = Board.GetMoves(board, moves);
                foreach (var move in moves) {
                    Board.IsLegalMove(board, move);
                }
            }
        }
    }
}
