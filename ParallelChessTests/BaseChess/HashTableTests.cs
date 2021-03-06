﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using ParallelChess;

namespace ParallelChessTests.BaseChess {
    class HashTableTests {

        private Board incrementalUpdate(string fen, int fromPosition, int toPosition, Piece promotion = Piece.EMPTY) {
            var board = BoardFactory.LoadBoardFromFen(fen);

            var boardHash = HashBoard.hash(board);

            var move = board.FindMove(fromPosition, toPosition, promotion);

            var nextBoardHash = HashBoard.ApplyMove(board, move, boardHash);

            board.Move(move);

            var expectedHash = HashBoard.hash(board);

            Assert.AreEqual(expectedHash, nextBoardHash);

            return board;
        }

        [Test]
        public void hasDifferentBitStrings() {
            Assert.AreNotEqual(HashBoard.pieceHash(0, Piece.EMPTY), HashBoard.pieceHash(1, Piece.EMPTY));
        }

        [Test]
        public void hashDifferentBoards() {
            var first = BoardFactory.LoadBoardFromFen("rnbqkbnr/pppp1ppp/8/4p3/2B1P3/8/PPPP1PPP/RNBQK1NR b KQkq - 2 2");
            var second = BoardFactory.LoadBoardFromFen("rnbqkbnr/pppp1ppp/8/4p3/2B1PP2/8/PPPP2PP/RNBQK1NR b KQkq - 2 2");
            
            Assert.AreNotEqual(HashBoard.hash(first), HashBoard.hash(second));
        }

        [Test]
        public void blackBigPawnMoveBecomesSamePosition() {
            /*
             * Starting position (Black to play)
            +---------------+
            |r n b q k b n r| 8
            |p p p p _ p p p| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ p _ _ _| 5
            |_ _ B _ P _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |P P P P _ P P P| 2
            |R N B Q K _ N R| 1
            +---------------+
             A B C D E F G H
            F7 -> F5
            +---------------+
            |r n b q k b n r| 8
            |p p p p _ _ p p| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ p p _ _| 5
            |_ _ B _ P _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |P P P P _ P P P| 2
            |R N B Q K _ N R| 1
            +---------------+
             A B C D E F G H
             */
            var board = BoardFactory.LoadBoardFromFen("rnbqkbnr/pppp1ppp/8/4p3/2B1P3/8/PPPP1PPP/RNBQK1NR b KQkq - 2 2");

            var boardHash = HashBoard.hash(board);

            var moves = board.GetMoves();

            var move = moves.FindTargetPosition(BoardStateOffset.F5);

            var nextBoardHash = HashBoard.ApplyMove(board, move, boardHash);

            board.Move(move);

            var expectedHash = HashBoard.hash(board);

            Assert.AreEqual(expectedHash, nextBoardHash);
        }

        [Test]
        public void whiteBigPawnMoveBecomesSamePosition() {
            /*
             * Starting position (White to play)
            +---------------+
            |r n b q k b n r| 8
            |p p p p _ p p p| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ p _ _ _| 5
            |_ _ B _ P _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |P P P P _ P P P| 2
            |R N B Q K _ N R| 1
            +---------------+
             A B C D E F G H
            F2 -> F4
            +---------------+
            |r n b q k b n r| 8
            |p p p p _ p p p| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ p _ _ _| 5
            |_ _ B _ P P _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |P P P P _ _ P P| 2
            |R N B Q K _ N R| 1
            +---------------+
             A B C D E F G H
             */
            var board = BoardFactory.LoadBoardFromFen("rnbqkbnr/pppp1ppp/8/4p3/2B1P3/8/PPPP1PPP/RNBQK1NR w KQkq - 2 2");

            var moves = board.GetMoves();

            var boardHash = HashBoard.hash(board);

            var move = moves.FindTargetPosition(BoardStateOffset.F5);

            var nextBoardHash = HashBoard.ApplyMove(board, move, boardHash);

            board.Move(move);

            var expectedHash = HashBoard.hash(board);

            Assert.AreEqual(expectedHash, nextBoardHash);
        }



        [Test]
        public void loseCastlingOptions() {
            /*
             * Starting position (White to play)
            +---------------+
            |r _ _ k _ _ _ r| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |R _ _ K _ _ _ R| 1
            +---------------+
             A B C D E F G H
            D1 -> D2
            +---------------+
            |r _ _ k _ _ _ r| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ K _ _ _ _| 2
            |R _ _ _ _ _ _ R| 1
            +---------------+
             A B C D E F G H
             */
            incrementalUpdate("r2k3r/8/8/8/8/8/8/R2K3R w KQkq - 0 1", BoardStateOffset.D1, BoardStateOffset.D2);
        }

        [Test]
        public void whiteCastleKingSide() {
            /*
             * Starting position (White to play)
            +---------------+
            |r _ _ _ k _ _ r| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |R _ _ _ K _ _ R| 1
            +---------------+
             A B C D E F G H
            E1 -> G1
            +---------------+
            |r _ _ _ k _ _ r| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |R _ _ _ _ R K _| 1
            +---------------+
             A B C D E F G H
             */
            incrementalUpdate("r3k2r/8/8/8/8/8/8/R3K2R w KQkq - 0 1", BoardStateOffset.E1, BoardStateOffset.G1);
        }

        [Test]
        public void whiteCastleQueenSide() {
            /*
             * Starting position (White to play)
            +---------------+
            |r _ _ _ k _ _ r| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |R _ _ _ K _ _ R| 1
            +---------------+
             A B C D E F G H
            E1 -> G1
            +---------------+
            |r _ _ _ k _ _ r| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |_ _ K R _ _ _ R| 1
            +---------------+
             A B C D E F G H
             */
            incrementalUpdate("r3k2r/8/8/8/8/8/8/R3K2R w KQkq - 0 1", BoardStateOffset.E1, BoardStateOffset.C1);
        }

        [Test]
        public void blackCastleKingSide() {
            /*
             * Starting position (White to play)
            +---------------+
            |r _ _ _ k _ _ r| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |R _ _ _ K _ _ R| 1
            +---------------+
             A B C D E F G H
            E8 -> G8
            +---------------+
            |r _ _ _ _ r k _| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |R _ _ _ K _ _ R| 1
            +---------------+
             A B C D E F G H
             */
            incrementalUpdate("r3k2r/8/8/8/8/8/8/R3K2R b KQkq - 0 1", BoardStateOffset.E8, BoardStateOffset.G8);
        }

        [Test]
        public void blackCastleQueenSide() {
            /*
             * Starting position (Black to play)
            +---------------+
            |r _ _ _ k _ _ r| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |R _ _ _ K _ _ R| 1
            +---------------+
             A B C D E F G H
            E8 -> C8
            +---------------+
            |_ _ k r _ _ _ r| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |R _ _ _ K _ _ R| 1
            +---------------+
             A B C D E F G H
             */
            incrementalUpdate("r3k2r/8/8/8/8/8/8/R3K2R b KQkq - 0 1", BoardStateOffset.E8, BoardStateOffset.C8);
        }

        [Test]
        public void blackEnPassant() {
            /*
             * Starting position (Black to play)
            +---------------+
            |r _ _ _ k _ _ r| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ p P _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |R _ _ _ K _ _ R| 1
            +---------------+
             A B C D E F G H
            C4 -> D3
            +---------------+
            |r _ _ _ k _ _ r| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ p _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |R _ _ _ K _ _ R| 1
            +---------------+
             A B C D E F G H
             */
            incrementalUpdate("r3k2r/8/8/8/2pP4/8/8/R3K2R b KQkq d3 0 1", BoardStateOffset.C4, BoardStateOffset.D3);
        }

        [Test]
        public void whiteEnPassant() {
            /*
             * Starting position (White to play)
            +---------------+
            |r _ _ _ k _ _ r| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ p P _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |R _ _ _ K _ _ R| 1
            +---------------+
             A B C D E F G H
            D5 -> C6
            +---------------+
            |r _ _ _ k _ _ r| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ P _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |R _ _ _ K _ _ R| 1
            +---------------+
             A B C D E F G H
            */
            incrementalUpdate("r3k2r/8/8/2pP4/8/8/8/R3K2R w KQkq c6 0 2", BoardStateOffset.D5, BoardStateOffset.C6);
        }

        [Test]
        public void promoteToQueen() {
            /*
             * Starting position (White to play)
            +---------------+
            |_ _ _ _ k _ _ _| 8
            |_ P _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |_ _ _ _ K _ _ _| 1
            +---------------+
             A B C D E F G H
            b7 -> b8 (Promote to Queen)
            +---------------+
            |_ Q _ _ k _ _ _| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |_ _ _ _ K _ _ _| 1
            +---------------+
             A B C D E F G H
             */
            incrementalUpdate("4k3/1P6/8/8/8/8/8/4K3 w - - 0 1", BoardStateOffset.B7, BoardStateOffset.B8, Piece.QUEEN);
        }

        [Test]
        public void promoteToBishop() {
            /*
             * Starting position (White to play)
            +---------------+
            |_ _ _ _ k _ _ _| 8
            |_ P _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |_ _ _ _ K _ _ _| 1
            +---------------+
             A B C D E F G H
            b7 -> b8 (Promote to Queen)
            +---------------+
            |_ B _ _ k _ _ _| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |_ _ _ _ K _ _ _| 1
            +---------------+
             A B C D E F G H
             */
            incrementalUpdate("4k3/1P6/8/8/8/8/8/4K3 w - - 0 1", BoardStateOffset.B7, BoardStateOffset.B8, Piece.BISHOP);
        }

        [Test]
        public void promoteToKnight() {
            /*
             * Starting position (White to play)
            +---------------+
            |_ _ _ _ k _ _ _| 8
            |_ P _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |_ _ _ _ K _ _ _| 1
            +---------------+
             A B C D E F G H
            b7 -> b8 (Promote to Queen)
            +---------------+
            |_ N _ _ k _ _ _| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |_ _ _ _ K _ _ _| 1
            +---------------+
             A B C D E F G H
             */
            incrementalUpdate("4k3/1P6/8/8/8/8/8/4K3 w - - 0 1", BoardStateOffset.B7, BoardStateOffset.B8, Piece.KNIGHT);
        }


        [Test]
        public void promoteToRook() {
            /*
             * Starting position (White to play)
            +---------------+
            |_ _ _ _ k _ _ _| 8
            |_ P _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |_ _ _ _ K _ _ _| 1
            +---------------+
             A B C D E F G H
            b7 -> b8 (Promote to Queen)
            +---------------+
            |_ R _ _ k _ _ _| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |_ _ _ _ K _ _ _| 1
            +---------------+
             A B C D E F G H
             */
            incrementalUpdate("4k3/1P6/8/8/8/8/8/4K3 w - - 0 1", BoardStateOffset.B7, BoardStateOffset.B8, Piece.ROOK);
        }


        // in theory the same function can be used to undo a move since we xor everything
        [Test]
        public void canUndoEnpassant() {
            /*
             * Starting position (White to play)
            +---------------+
            |r _ _ _ k _ _ r| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ p P _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |R _ _ _ K _ _ R| 1
            +---------------+
             A B C D E F G H
            D5 -> C6
            +---------------+
            |r _ _ _ k _ _ r| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ P _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |R _ _ _ K _ _ R| 1
            +---------------+
             A B C D E F G H
            */
            var board = BoardFactory.LoadBoardFromFen("r3k2r/8/8/2pP4/8/8/8/R3K2R w KQkq c6 0 2");

            var moves = board.GetMoves();

            var boardHash = HashBoard.hash(board);

            var move = moves.FindTargetPosition(BoardStateOffset.D5, BoardStateOffset.C6);

            var nextBoardHash = HashBoard.ApplyMove(board, move, boardHash);

            board.Move(move);
            board.UndoMove(move);
            var previousHash = HashBoard.ApplyMove(board, move, nextBoardHash);

            Assert.AreEqual(boardHash, previousHash);
        }

        [Test]
        public void canPromote() {
            /*
             * Starting position (White to play)
            +---------------+
            |r _ _ _ k _ _ r| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ p P _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |R _ _ _ K _ _ R| 1
            +---------------+
             A B C D E F G H
            D5 -> C6
            +---------------+
            |r _ _ _ k _ _ r| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ P _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |R _ _ _ K _ _ R| 1
            +---------------+
             A B C D E F G H
            */
            var board = BoardFactory.LoadBoardFromFen("r3k2r/8/8/2pP4/8/8/8/R3K2R w KQkq c6 0 2");

            var moves = board.GetMoves();

            var boardHash = HashBoard.hash(board);

            var move = moves.FindTargetPosition(BoardStateOffset.D5, BoardStateOffset.C6);

            var nextBoardHash = HashBoard.ApplyMove(board, move, boardHash);

            board.Move(move);
            board.UndoMove(move);
            var previousHash = HashBoard.ApplyMove(board, move, nextBoardHash);

            Assert.AreEqual(boardHash, previousHash);
        }

        [Test]
        public void blackAndWhiteDontHashToTheSameValue() {
            var whiteBoard = BoardFactory.LoadBoardFromFen("r3k2r/8/8/2pP4/8/8/8/R3K2R w KQkq c6 0 2");
            var blackBoard = BoardFactory.LoadBoardFromFen("r3k2r/8/8/2pP4/8/8/8/R3K2R b KQkq c6 0 2");


            Assert.AreNotEqual(HashBoard.hash(whiteBoard), HashBoard.hash(blackBoard));
        }

        [Test]
        public void enpassantTargetMakesADifference() {
            var noEnpassant = BoardFactory.LoadBoardFromFen("rnb1kbnr/1ppp1ppp/p3p3/8/1P1PP2q/5N2/P1P2PPP/RNBQKB1R b KQkq - 1 0");
            var enpassant = BoardFactory.LoadBoardFromFen("rnb1kbnr/1ppp1ppp/p3p3/8/1P1PP2q/5N2/P1P2PPP/RNBQKB1R b KQkq b3 1 0");

            var enpassantHash = HashBoard.hash(enpassant);
            Assert.AreNotEqual(HashBoard.hash(noEnpassant), enpassantHash);
        }

        
        [Test]
        public void enpassantTargetUpdates() {
            //var noEnpassant = BoardFactory.LoadBoardFromFen("");

            incrementalUpdate("rnb1kbnr/pppp1ppp/4p3/8/P2PP2q/8/1PP2PPP/RNBQKBNR b KQkq a3 0 1", BoardStateOffset.A7, BoardStateOffset.A6);
        }


    }
}
