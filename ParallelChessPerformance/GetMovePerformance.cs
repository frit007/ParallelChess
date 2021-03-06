﻿using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using ParallelChess;
using System.Threading;
using ParallelChess.MinMax;

namespace ParallelChessPerformance {
    [SimpleJob]
    public class GetMovePerformance {
        //[Benchmark]
        //public void PawnMoveFromStart() {
        //    byte[] board = BoardFactory.LoadBoardFromFen();

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

        //[Benchmark]
        //public void PawnPerformanceTest() {
        //    Board board = BoardFactory.LoadBoardFromFen();

        //    for (int i = 0; i < 1000000; i++) {
        //        BoardHelper.GetMovesForPosition(board, BoardStateOffset.E2);
        //    }
        //}

        //[Benchmark]
        //public void GetAllMoves() {
        //    Board board = BoardFactory.LoadBoardFromFen();
        //    List<Move> moves = new List<Move>();
        //    for (int i = 0; i < 1000000; i++) {
        //        moves.Clear();
        //        moves = BoardHelper.GetMoves(board, moves);
        //    }
        //}

        //[Benchmark]
        //public void MakeAllMoves() {
        //    Board board = BoardFactory.LoadBoardFromFen();
        //    Board virtualBoard = BoardHelper.CreateCopyBoard(board);
        //    List<Move> moves = new List<Move>();
        //    for (int i = 0; i < 1000000; i++) {
        //        moves.Clear();
        //        moves = BoardHelper.GetMoves(board, moves);
        //        foreach (var move in moves) {
        //            BoardHelper.CopyBoard(board, virtualBoard);
        //            BoardHelper.MakeMove(virtualBoard, move);
        //        }
        //    }
        //}

        //[Benchmark]
        //public void FindValidMoves() {
        //    Board board = BoardFactory.LoadBoardFromFen();
        //    Board virtualBoard = BoardHelper.CreateCopyBoard(board);
        //    List<Move> moves = new List<Move>();
        //    for (int i = 0; i < 1000000; i++) {
        //        moves.Clear();
        //        moves = BoardHelper.GetMoves(board, moves);
        //        foreach (var move in moves) {
        //            BoardHelper.IsLegalMove(virtualBoard, move);
        //        }
        //    }
        //}

        //[Benchmark]
        //public void FindValidMovesThreads() {
        //    List<Thread> threads = new List<Thread>();
        //    int count = 4;
        //    for (int i = 0; i < count; i++) {
        //        var thread = new Thread(() => {
        //            BoardHelper.initThreadStaticVariables();

        //            for (int i = 0; i < (1000000 / count); i++) {
        //                Board board = BoardFactory.LoadBoardFromFen();
        //                Board virtualBoard = BoardHelper.CreateCopyBoard(board);
        //                List<Move> moves = new List<Move>();
        //                moves.Clear();
        //                moves = BoardHelper.GetMoves(board, moves);
        //                foreach (var move in moves) {
        //                    //Board.CopyBoard(board, virtualBoard);
        //                    //Board.MakeMove(virtualBoard, move);
        //                    BoardHelper.IsLegalMove(virtualBoard, move);
        //                }
        //            }
        //        });
        //        thread.Start();
        //        threads.Add(thread);
        //    }

        //    foreach (var thread in threads) {
        //        thread.Join();
        //    }

        //}

        //[Benchmark]
        //public void KnightPerformanceTest() {
        //    Board board = BoardFactory.LoadBoardFromFen();

        //    for (int i = 0; i < 1000000; i++) {
        //        BoardHelper.GetMovesForPosition(board, BoardStateOffset.B1);
        //    }
        //}

        //[Benchmark]
        //public void KingPerformanceTest() {
        //    Board board = BoardFactory.LoadBoardFromFen();

        //    for (int i = 0; i < 1000000; i++) {
        //        BoardHelper.GetMovesForPosition(board, BoardStateOffset.E1);
        //    }
        //}

        //[Benchmark]
        //public void QueenPerformanceTest() {
        //    Board board = BoardFactory.LoadBoardFromFen();


        //    for (int i = 0; i < 1000000; i++) {
        //        BoardHelper.GetMovesForPosition(board, BoardStateOffset.D1);
        //    }
        //}

        //[Benchmark]
        //public void QueenWorstCasePerformanceTest() {
        //    // Basically a board where the queen stands in the midle of the board which means she has to check the entire board.
        //    Board board = BoardFactory.LoadBoardFromFen("r2qk2r/8/8/3Q4/8/8/8/R3K2R w KQkq - 0 1");

        //    for (int i = 0; i < 1000000; i++) {
        //        BoardHelper.GetMovesForPosition(board, BoardStateOffset.D5);
        //    }
        //}


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
        //Board board = BoardFactory.LoadBoardFromFen("rnbqkb1r/1p3ppp/p2p1n2/4p3/3NP3/2N5/PPP2PPP/R1BQKB1R w KQkq - 1 6");
        //List<Move> moves = new List<Move>();
        //[Benchmark]
        //public void realisticTest() {

        //    for (int i = 0; i < 1000000; i++) {
        //        moves.Clear();
        //        foreach (var move in board.GetMoves(moves)) {

        //            if (board.IsLegalMove(move)) {
        //                board.Move(move);
        //                board.UndoMove(move);
        //            }
        //        }
        //    }

        //}
        
        /**
         * Starting position white to play
        +---------------+
        |r n b _ k b n r| 8
        |p p p p _ p p p| 7
        |_ _ _ _ p _ _ _| 6
        |_ _ _ _ _ _ _ _| 5
        |_ _ _ P P _ _ q| 4
        |_ _ _ _ _ _ _ _| 3
        |P P P _ _ P P P| 2
        |R N B Q K B N R| 1
        +---------------+
        A B C D E F G H
        */
        Board slowBoard = BoardFactory.LoadBoardFromFen("rnb1kbnr/pppp1ppp/4p3/8/3PP2q/8/PPP2PPP/RNBQKBNR w KQkq - 1 3");
        AIWorkerManager ai;
        [GlobalSetup]
        public void setup() {

        }

        [Benchmark]
        public void slowTest() {
            ai = new AIWorkerManager();
            ai.spawnWorkers(1);

            ai.analyzeBoard(slowBoard, 6).GetAwaiter().GetResult();
            ai.killWorkers();

        }
    }
}
