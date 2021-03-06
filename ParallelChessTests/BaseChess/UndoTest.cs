﻿using NUnit.Framework;
using ParallelChess;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParallelChessTests.BaseChess {
    class UndoTest {
        public void everyThingIsEqual(Board original, Board copy, Move move) {
            for (int i = 0; i < BoardStateOffset.BOARD_STATE_SIZE; i++) {
                Assert.AreEqual(original.bytes[i], copy.bytes[i], $"The boards are not equal at offset {i}, after playing the move {MoveHelper.ReadableMove(move)}");
            }
        }

        [Test]
        public void undoMove() {
            /*
             * Start position (White to play)
             +---------------+
             |r n b q k b n r| 8
             |p p p p p p p p| 7
             |_ _ _ _ _ _ _ _| 6
             |_ _ _ _ _ _ _ _| 5
             |_ _ _ _ _ _ _ _| 4
             |_ _ _ _ _ _ _ _| 3
             |P P P P P P P P| 2
             |R N B Q K B N R| 1
             +---------------+
              A B C D E F G H
              Play E2->E4
             +---------------+
             |r n b q k b n r| 8
             |p p p p p p p p| 7
             |_ _ _ _ _ _ _ _| 6
             |_ _ _ _ _ _ _ _| 5
             |_ _ _ _ P _ _ _| 4
             |_ _ _ _ _ _ _ _| 3
             |P P P P _ P P P| 2
             |R N B Q K B N R| 1
             +---------------+
              A B C D E F G H
              Undo
            */
            var original = BoardFactory.LoadBoardFromFen();
            var copy = original.CreateCopy();
            var moveMade = copy.Move(BoardStateOffset.E2, BoardStateOffset.E4);

            copy.UndoMove(moveMade);

            Assert.AreEqual(original.GetPiece(BoardStateOffset.E2), copy.GetPiece(BoardStateOffset.E2));
            Assert.AreEqual(original.GetPiece(BoardStateOffset.E4), copy.GetPiece(BoardStateOffset.E4));
            //everyThingIsEqual(original, copy);
        }

        [Test]
        public void undoCapture() {
            /*
             * Start position (White to play)
             +---------------+
             |r n b q k b _ r| 8
             |p p p _ p p _ p| 7
             |_ _ _ _ _ n p _| 6
             |_ _ _ p _ _ _ _| 5
             |_ _ P P _ B _ _| 4
             |_ _ N _ _ _ _ _| 3
             |P P _ _ P P P P| 2
             |R _ _ Q K B N R| 1
             +---------------+
              A B C D E F G H

              Play F4->C7
             +---------------+
             |r n b q k b _ r| 8
             |p p B _ p p _ p| 7
             |_ _ _ _ _ n p _| 6
             |_ _ _ p _ _ _ _| 5
             |_ _ P P _ _ _ _| 4
             |_ _ N _ _ _ _ _| 3
             |P P _ _ P P P P| 2
             |R _ _ Q K B N R| 1
             +---------------+
              A B C D E F G H
              Undo
            */
            var original = BoardFactory.LoadBoardFromFen("rnbqkb1r/ppp1pp1p/5np1/3p4/2PP1B2/2N5/PP2PPPP/R2QKBNR w KQkq - 2 4");
            var copy = original.CreateCopy();

            var moveMade = copy.Move(BoardStateOffset.F4, BoardStateOffset.C7);

            copy.UndoMove(moveMade);

            Assert.AreEqual(original.GetPiece(BoardStateOffset.F4), copy.GetPiece(BoardStateOffset.F4));
            Assert.AreEqual(original.GetPiece(BoardStateOffset.C7), copy.GetPiece(BoardStateOffset.C7));
        }

        [Test]
        public void undoHalfTurnCounter() {
            /*
             * Start position (White to play)
             +---------------+
             |r n b q k b _ r| 8
             |p p p _ p p _ p| 7
             |_ _ _ _ _ n p _| 6
             |_ _ _ p _ _ _ _| 5
             |_ _ P P _ B _ _| 4
             |_ _ N _ _ _ _ _| 3
             |P P _ _ P P P P| 2
             |R _ _ Q K B N R| 1
             +---------------+
              A B C D E F G H

              Play F4->C7
             +---------------+
             |r n b q k b _ r| 8
             |p p B _ p p _ p| 7
             |_ _ _ _ _ n p _| 6
             |_ _ _ p _ _ _ _| 5
             |_ _ P P _ _ _ _| 4
             |_ _ N _ _ _ _ _| 3
             |P P _ _ P P P P| 2
             |R _ _ Q K B N R| 1
             +---------------+
              A B C D E F G H
              Undo
            */
            var original = BoardFactory.LoadBoardFromFen("rnbqkb1r/ppp1pp1p/5np1/3p4/2PP1B2/2N5/PP2PPPP/R2QKBNR w KQkq - 2 4");
            var copy = original.CreateCopy();

            var moveMade = copy.Move(BoardStateOffset.F4, BoardStateOffset.C7);

            copy.UndoMove(moveMade);


            Assert.AreEqual(original.HalfTurnCounter, copy.HalfTurnCounter);
        }

        [Test]
        public void undoTurnCounter() {
            /*
             * Start position (black to play)
            +---------------+
            |_ q _ _ k _ _ _| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ P P P _ _| 2
            |_ _ _ _ K _ _ _| 1
            +---------------+
             A B C D E F G H

              Play B8->B1
            +---------------+
            |_ _ _ _ k _ _ _| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ P P P _ _| 2
            |_ q _ _ K _ _ _| 1
            +---------------+
             A B C D E F G H
              Undo
            */
            var original = ChessGame.ContinueFromFEN("1q2k3/8/8/8/8/8/3PPP2/4K3 b - - 0 1");

            var copy = original.Copy();

            copy.Move(BoardStateOffset.B8, BoardStateOffset.B1);

            copy.Undo();

            Assert.AreEqual(original.TurnCounter, copy.TurnCounter);
        }
        [Test]
        public void undoTurnCounterDoesNotUndoWhenWhitePlayer() {
            /*
             * Start position (white to play)
            +---------------+
            |_ q _ _ k _ _ _| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ P P P _ _| 2
            |_ _ _ _ K _ _ _| 1
            +---------------+
             A B C D E F G H

              Play D2->D3
            +---------------+
            |_ q _ _ k _ _ _| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ P _ _ _ _| 3
            |_ _ _ _ P P _ _| 2
            |_ _ _ _ K _ _ _| 1
            +---------------+
             A B C D E F G H
              Undo
            */
            var original = ChessGame.ContinueFromFEN("1q2k3/8/8/8/8/8/3PPP2/4K3 w - - 0 1");

            var copy = original.Copy();

            var moveMade = copy.Move(BoardStateOffset.D2, BoardStateOffset.D3);

            copy.Undo();

            Assert.AreEqual(original.TurnCounter, copy.TurnCounter);
        }

        [Test]
        public void undoRevertsWhichTurnItIs() {
            /*
             * Start position (black to play)
            +---------------+
            |_ q _ _ k _ _ _| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ P P P _ _| 2
            |_ _ _ _ K _ _ _| 1
            +---------------+
             A B C D E F G H

              Play B8->B1
            +---------------+
            |_ _ _ _ k _ _ _| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ P P P _ _| 2
            |_ q _ _ K _ _ _| 1
            +---------------+
             A B C D E F G H
              Undo
            */
            var original = BoardFactory.LoadBoardFromFen("1q2k3/8/8/8/8/8/3PPP2/4K3 b - - 0 1");

            var copy = original.CreateCopy();

            var moveMade = copy.Move(BoardStateOffset.B8, BoardStateOffset.B1);

            copy.UndoMove(moveMade);

            Assert.AreEqual(original.IsWhiteTurn, copy.IsWhiteTurn);
        }

        [Test]
        public void undoCastlingWhiteQueenSide() {
            /*
             * Start position (White to play)
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

              Play E1->C1
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
              Undo
            */
            var original = BoardFactory.LoadBoardFromFen("r3k2r/8/8/8/8/8/8/R3K2R w KQkq - 0 1");
            var copy = original.CreateCopy();

            var move = copy.Move(BoardStateOffset.E1, BoardStateOffset.C1);

            copy.UndoMove(move);

            // king position checks
            Assert.AreEqual(original.GetPiece(BoardStateOffset.E1), copy.GetPiece(BoardStateOffset.E1));
            Assert.AreEqual(original.GetPiece(BoardStateOffset.C1), copy.GetPiece(BoardStateOffset.C1));

            // check rooks moved back to its starting position
            Assert.AreEqual(original.GetPiece(BoardStateOffset.D1), copy.GetPiece(BoardStateOffset.D1));
            Assert.AreEqual(original.GetPiece(BoardStateOffset.A1), copy.GetPiece(BoardStateOffset.A1));
            Assert.AreEqual(original.CastlingBits, copy.CastlingBits);

        }

        [Test]
        public void undoCastlingWhiteKingSide() {
            /*
             * Start position (White to play)
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

              Play E1->G1
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
              Undo
            */
            var original = BoardFactory.LoadBoardFromFen("r3k2r/8/8/8/8/8/8/R3K2R w KQkq - 0 1");
            var copy = original.CreateCopy();

            var move = copy.Move(BoardStateOffset.E1, BoardStateOffset.G1);

            copy.UndoMove(move);

            // king position checks
            Assert.AreEqual(original.GetPiece(BoardStateOffset.E1), copy.GetPiece(BoardStateOffset.E1));
            Assert.AreEqual(original.GetPiece(BoardStateOffset.G1), copy.GetPiece(BoardStateOffset.G1));

            // check rooks moved back to its starting position
            Assert.AreEqual(original.GetPiece(BoardStateOffset.F1), copy.GetPiece(BoardStateOffset.F1));
            Assert.AreEqual(original.GetPiece(BoardStateOffset.H1), copy.GetPiece(BoardStateOffset.H1));
            Assert.AreEqual(original.CastlingBits, copy.CastlingBits);

        }

        [Test]
        public void undoCastlingBlackQueenSide() {
            /*
             * Start position (Black to play)
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

              Play E8->C8
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
              Undo
            */
            var original = BoardFactory.LoadBoardFromFen("r3k2r/8/8/8/8/8/8/R3K2R b KQkq - 0 1");
            var copy = original.CreateCopy();

            var move = copy.Move(BoardStateOffset.E8, BoardStateOffset.C8);

            copy.UndoMove(move);

            // king position checks
            Assert.AreEqual(original.GetPiece(BoardStateOffset.E8), copy.GetPiece(BoardStateOffset.E8));
            Assert.AreEqual(original.GetPiece(BoardStateOffset.C8), copy.GetPiece(BoardStateOffset.C8));

            // check rooks moved back to its starting position
            Assert.AreEqual(original.GetPiece(BoardStateOffset.D8), copy.GetPiece(BoardStateOffset.D8));
            Assert.AreEqual(original.GetPiece(BoardStateOffset.A8), copy.GetPiece(BoardStateOffset.A8));
            Assert.AreEqual(original.CastlingBits, copy.CastlingBits);

        }

        [Test]
        public void undoCastlingBlackKingSide() {
            /*
             * Start position (White to play)
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

              Play E1->G1
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
              Undo
            */
            var original = BoardFactory.LoadBoardFromFen("r3k2r/8/8/8/8/8/8/R3K2R b KQkq - 0 1");
            var copy = original.CreateCopy();

            var move = copy.Move(BoardStateOffset.E8, BoardStateOffset.G8);

            copy.UndoMove(move);

            // king position checks
            Assert.AreEqual(original.GetPiece(BoardStateOffset.E8), copy.GetPiece(BoardStateOffset.E8));
            Assert.AreEqual(original.GetPiece(BoardStateOffset.G8), copy.GetPiece(BoardStateOffset.G8));

            // check rooks moved back to its starting position
            Assert.AreEqual(original.GetPiece(BoardStateOffset.F8), copy.GetPiece(BoardStateOffset.F8));
            Assert.AreEqual(original.GetPiece(BoardStateOffset.H8), copy.GetPiece(BoardStateOffset.H8));
            Assert.AreEqual(original.CastlingBits, copy.CastlingBits);

        }

        [Test]
        public void undoWhiteKingPosition() {
            /*
             * Start position (White to play)
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

              Play E1->C1
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
              Undo
            */
            var original = BoardFactory.LoadBoardFromFen("r3k2r/8/8/8/8/8/8/R3K2R w KQkq - 0 1");
            var copy = original.CreateCopy();

            var move = copy.Move(BoardStateOffset.E1, BoardStateOffset.C1);

            copy.UndoMove(move);

            // king position checks
            Assert.AreEqual(original.WhiteKingPosition, copy.WhiteKingPosition);
        }

        [Test]
        public void undoBlackKingPosition() {
            /*
             * Start position (Black to play)
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

              Play E8->C8
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
              Undo
            */
            var original = BoardFactory.LoadBoardFromFen("r3k2r/8/8/8/8/8/8/R3K2R b KQkq - 0 1");
            var copy = original.CreateCopy();

            var move = copy.Move(BoardStateOffset.E8, BoardStateOffset.C8);

            copy.UndoMove(move);

            // king position checks
            Assert.AreEqual(original.BlackKingPosition, copy.BlackKingPosition);
        }


        [Test]
        public void UndoWhiteEnpassant() {
            /*
             * Start position (White to play)
            +---------------+
            |_ _ _ k _ _ _ _| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ p _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ P _ _ _ _| 2
            |_ _ _ K _ _ _ _| 1
            +---------------+
             A B C D E F G H
             White D2 -> D4 (invite for en passant)
            +---------------+
            |_ _ _ k _ _ _ _| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ P p _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |_ _ _ K _ _ _ _| 1
            +---------------+
             A B C D E F G H
             Black E4->D3 (en passant)
            +---------------+
            |_ _ _ k _ _ _ _| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ p _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |_ _ _ K _ _ _ _| 1
            +---------------+
             A B C D E F G H
             Undo twice to get back to starting position
            */
            var original = BoardFactory.LoadBoardFromFen("3k4/8/8/8/4p3/8/3P4/3K4 w - - 0 1");
            var copy = original.CreateCopy();

            var firstMove = copy.Move(BoardStateOffset.D2, BoardStateOffset.D4);
            var secondMove = copy.Move(BoardStateOffset.E4, BoardStateOffset.D3);

            copy.UndoMove(secondMove);
            copy.UndoMove(firstMove);

            Assert.AreEqual(original.GetPiece(BoardStateOffset.E4), copy.GetPiece(BoardStateOffset.E4));
            Assert.AreEqual(original.GetPiece(BoardStateOffset.D2), copy.GetPiece(BoardStateOffset.D2));
            Assert.AreEqual(original.GetPiece(BoardStateOffset.D3), copy.GetPiece(BoardStateOffset.D3));

            everyThingIsEqual(original, copy, firstMove);
        }

        [Test]
        public void UndoBlackEnpassant() {
            /*
             * Start position (Black to play)
            +---------------+
            |_ _ _ k _ _ _ _| 8
            |_ _ _ p _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ P _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |_ _ _ K _ _ _ _| 1
            +---------------+
             A B C D E F G H
             Black D7 -> D5 (invite for en passant)
            +---------------+
            |_ _ _ k _ _ _ _| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ p P _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |_ _ _ K _ _ _ _| 1
            +---------------+
             A B C D E F G H
             White E5->D6 (en passant)
            +---------------+
            |_ _ _ k _ _ _ _| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ _ P _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |_ _ _ K _ _ _ _| 1
            +---------------+
             A B C D E F G H
             Undo twice to get back to starting position
            */
            var original = BoardFactory.LoadBoardFromFen("3k4/3p4/8/4P3/8/8/8/3K4 b - - 0 1");
            var copy = original.CreateCopy();

            var firstMove = copy.Move(BoardStateOffset.D7, BoardStateOffset.D5);
            var secondMove = copy.Move(BoardStateOffset.E5, BoardStateOffset.D6);

            copy.UndoMove(secondMove);
            copy.UndoMove(firstMove);

            Assert.AreEqual(original.GetPiece(BoardStateOffset.D7), copy.GetPiece(BoardStateOffset.D7));
            Assert.AreEqual(original.GetPiece(BoardStateOffset.E5), copy.GetPiece(BoardStateOffset.E5));
            Assert.AreEqual(original.GetPiece(BoardStateOffset.D6), copy.GetPiece(BoardStateOffset.D6));

            everyThingIsEqual(original, copy, firstMove);
        }

        [Test]
        public void undoWhitePromoteToBishop() {
            /* Starting position
            +---------------+
            |r _ b _ _ _ _ _| 8
            |p k p P _ _ _ _| 7
            |N p r _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |R _ _ K _ _ _ _| 1
            +---------------+
             A B C D E F G H

            D7->C8 promote to Bishop
            +---------------+
            |r _ B _ _ _ _ _| 8
            |p k p _ _ _ _ _| 7
            |N p r _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |R _ _ K _ _ _ _| 1
            +---------------+
             A B C D E F G H
             */

            var board = BoardFactory.LoadBoardFromFen("r1b5/pkpP4/Npr5/8/8/8/8/R2K4 w - - 0 1");

            var moves = board.GetMovesForPosition(BoardStateOffset.D7);
            var move = moves.FindTargetPosition(BoardStateOffset.C8, Piece.BISHOP);
            board.Move(move);
            board.UndoMove(move);

            Assert.AreEqual(Piece.BISHOP, board.GetPiece(BoardStateOffset.C8));
            Assert.AreEqual(Piece.PAWN | Piece.IS_WHITE, board.GetPiece(BoardStateOffset.D7));
        }

        [Test]
        public void undoWhitePromoteToQueen() {
            /* Starting position
            +---------------+
            |r _ b _ _ _ _ _| 8
            |p k p P _ _ _ _| 7
            |N p r _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |R _ _ K _ _ _ _| 1
            +---------------+
             A B C D E F G H

            D7->C8 promote to Queen
            +---------------+
            |r _ Q _ _ _ _ _| 8
            |p k p _ _ _ _ _| 7
            |N p r _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |R _ _ K _ _ _ _| 1
            +---------------+
             A B C D E F G H
             */

            var board = BoardFactory.LoadBoardFromFen("r1b5/pkpP4/Npr5/8/8/8/8/R2K4 w - - 0 1");

            var moves = board.GetMovesForPosition(BoardStateOffset.D7);
            var move = moves.FindTargetPosition(BoardStateOffset.C8, Piece.QUEEN);
            board.Move(move);
            board.UndoMove(move);

            Assert.AreEqual(Piece.BISHOP, board.GetPiece(BoardStateOffset.C8));
            Assert.AreEqual(Piece.PAWN | Piece.IS_WHITE, board.GetPiece(BoardStateOffset.D7));
        }

        [Test]
        public void undoWhitePromoteToRook() {
            /* Starting position
            +---------------+
            |r _ b _ _ _ _ _| 8
            |p k p P _ _ _ _| 7
            |N p r _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |R _ _ K _ _ _ _| 1
            +---------------+
             A B C D E F G H

            D7->C8 promote to Rook
            +---------------+
            |r _ R _ _ _ _ _| 8
            |p k p _ _ _ _ _| 7
            |N p r _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |R _ _ K _ _ _ _| 1
            +---------------+
             A B C D E F G H
             */

            var board = BoardFactory.LoadBoardFromFen("r1b5/pkpP4/Npr5/8/8/8/8/R2K4 w - - 0 1");
            var original = board.CreateCopy();
            var moves = board.GetMovesForPosition(BoardStateOffset.D7);
            var move = moves.FindTargetPosition(BoardStateOffset.C8, Piece.ROOK);
            board.Move(move);
            board.UndoMove(move);

            Assert.AreEqual(Piece.BISHOP, board.GetPiece(BoardStateOffset.C8));
            Assert.AreEqual(Piece.PAWN | Piece.IS_WHITE, board.GetPiece(BoardStateOffset.D7));
            everyThingIsEqual(original, board, move);
        }

        [Test]
        public void undoWhitePromoteToKnight() {
            /* Starting position
            +---------------+
            |r _ b _ _ _ _ _| 8
            |p k p P _ _ _ _| 7
            |N p r _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |R _ _ K _ _ _ _| 1
            +---------------+
             A B C D E F G H

            D7->C8 promote to Knight
            +---------------+
            |r _ N _ _ _ _ _| 8
            |p k p _ _ _ _ _| 7
            |N p r _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |R _ _ K _ _ _ _| 1
            +---------------+
             A B C D E F G H
             */

            var board = BoardFactory.LoadBoardFromFen("r1b5/pkpP4/Npr5/8/8/8/8/R2K4 w - - 0 1");

            var moves = board.GetMovesForPosition(BoardStateOffset.D7);
            var move = moves.FindTargetPosition(BoardStateOffset.C8, Piece.KNIGHT);
            board.Move(move);
            board.UndoMove(move);

            Assert.AreEqual(Piece.BISHOP, board.GetPiece(BoardStateOffset.C8));
            Assert.AreEqual(Piece.PAWN | Piece.IS_WHITE, board.GetPiece(BoardStateOffset.D7));
        }

        [Test]
        public void undoEnpassantTarget() {
            /*
             * Start position (White to play)
            +---------------+
            |_ _ _ k _ _ _ _| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ p _| 3
            |_ _ _ _ _ _ P P| 2
            |_ _ _ K _ _ _ _| 1
            +---------------+
             A B C D E F G H
             Play H2->H4
            +---------------+
            |_ _ _ k _ _ _ _| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ P| 4
            |_ _ _ _ _ _ p _| 3
            |_ _ _ _ _ _ P _| 2
            |_ _ _ K _ _ _ _| 1
            +---------------+
             A B C D E F G H
             Undo
             */
            var board = BoardFactory.LoadBoardFromFen("3k4/8/8/8/8/6p1/6PP/3K4 w - - 0 1");
            var original = board.CreateCopy();

            var move = board.Move(BoardStateOffset.H2, BoardStateOffset.H4);
            board.UndoMove(move);
            Assert.AreEqual(original.EnPassantTarget, board.EnPassantTarget);
        }

        [Test]
        public void undoStressTest1() {
            var board = BoardFactory.LoadBoardFromFen();
            var original = board.CreateCopy();

            foreach (var move in board.GetMoves()) {
                board.Move(move);
                board.UndoMove(move);
                everyThingIsEqual(original, board, move);
            }
        }

        [Test]
        public void undoStressTest2() {
            var board = BoardFactory.LoadBoardFromFen("r2qkb1r/pp3ppp/2n2n2/6B1/3pN1b1/P7/1PP1BPPP/R2QK1NR b KQkq - 0 10");
            var original = board.CreateCopy();

            foreach (var move in board.GetMoves()) {
                board.Move(move);
                board.UndoMove(move);
                everyThingIsEqual(original, board, move);
            }
        }

        [Test]
        public void undoStressTest3() {
            var board = BoardFactory.LoadBoardFromFen("r2qkb1r/ppP2ppp/2n2n2/6B1/3pN1b1/P7/2P1BPPP/R2QK1NR b KQkq - 0 10");
            var original = board.CreateCopy();

            foreach (var move in board.GetMoves()) {
                board.Move(move);
                board.UndoMove(move);
                everyThingIsEqual(original, board, move);
            }
        }

        [Test]
        public void undoStressTest4() {
            var board = BoardFactory.LoadBoardFromFen("r2qkb1r/ppP2ppp/2n2n2/6B1/3pN1b1/P7/2P1BPPP/R2QK1NR w KQkq - 0 10");
            var original = board.CreateCopy();

            foreach (var move in board.GetMoves()) {
                board.Move(move);
                board.UndoMove(move);
                everyThingIsEqual(original, board, move);
            }
        }
        [Test]
        public void undoStressTest5() {
            var board = BoardFactory.LoadBoardFromFen("r5k1/5ppp/1p6/p1p5/7b/1PPrqPP1/1PQ4P/R4R1K b - - 0 1");
            var original = board.CreateCopy();

            foreach (var move in board.GetMoves()) {
                board.Move(move);
                board.UndoMove(move);
                everyThingIsEqual(original, board, move);
            }
        }

        [Test]
        public void BlackPromotionStressUndoTest() {
            var board = BoardFactory.LoadBoardFromFen("1nbqkbnr/rppppppp/p7/8/1P6/P1P5/1p1PPPPP/R1BQKBNR b KQk - 0 3");
            var original = board.CreateCopy();
            var moves = board.GetMovesForPosition(BoardStateOffset.B2);
            foreach (var move in moves) {
                board.Move(move);
                board.UndoMove(move);
                everyThingIsEqual(original, board, move);
            }
        }

        [Test]
        public void UndoKingMovePosition() {
            /*
             * Starting position (White to play)
            +---------------+
            |r k r _ _ _ _ _| 8
            |p p p _ _ p p p| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ N _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |_ _ K _ _ _ _ _| 1
            +---------------+
             A B C D E F G H
            C1 -> C2
            +---------------+
            |r k r _ _ _ _ _| 8
            |p p p _ _ p p p| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ N _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ K _ _ _ _ _| 2
            |_ _ _ _ _ _ _ _| 1
            +---------------+
             A B C D E F G H
             */
            var board = BoardFactory.LoadBoardFromFen("rkr5/ppp2ppp/8/4N3/8/8/8/2K5 w - - 0 1");
            var original = board.CreateCopy();
            var moves = board.GetMoves();

            var move = board.Move(BoardStateOffset.C1, BoardStateOffset.C2, Piece.EMPTY);
            board.UndoMove(move);

            Assert.AreEqual(original.BlackKingPosition, board.BlackKingPosition);
        }

    }
}
