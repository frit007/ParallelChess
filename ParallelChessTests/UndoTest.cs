using NUnit.Framework;
using ParallelChess;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParallelChessTests {
    class UndoTest {
        public void everyThingIsEqual(BoardState original, BoardState copy) {
            for (int i = 0; i < BoardStateOffset.BOARD_STATE_SIZE; i++) {
                Assert.AreEqual(original.bytes[i], copy.bytes[i], $"The boards are not equal at offset {i}");
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
            var original = Chess.LoadBoardFromFen();
            var copy = Board.CreateCopyBoard(original);
            var moveMade = Chess.MakeMove(copy, BoardStateOffset.E2, BoardStateOffset.E4);

            Board.UndoMove(copy, moveMade);

            Assert.AreEqual(original.E2, copy.E2);
            Assert.AreEqual(original.E4, copy.E4);
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
            var original = Chess.LoadBoardFromFen("rnbqkb1r/ppp1pp1p/5np1/3p4/2PP1B2/2N5/PP2PPPP/R2QKBNR w KQkq - 2 4");
            var copy = Board.CreateCopyBoard(original);

            var moveMade = Chess.MakeMove(copy, BoardStateOffset.F4, BoardStateOffset.C7);

            Board.UndoMove(copy, moveMade);

            Assert.AreEqual(original.F4, copy.F4);
            Assert.AreEqual(original.C7, copy.C7);
            //everyThingIsEqual(original, copy);
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
            var original = Chess.LoadBoardFromFen("rnbqkb1r/ppp1pp1p/5np1/3p4/2PP1B2/2N5/PP2PPPP/R2QKBNR w KQkq - 2 4");
            var copy = Board.CreateCopyBoard(original);

            var moveMade = Chess.MakeMove(copy, BoardStateOffset.F4, BoardStateOffset.C7);

            Board.UndoMove(copy, moveMade);


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
            var original = Chess.LoadBoardFromFen("1q2k3/8/8/8/8/8/3PPP2/4K3 b - - 0 1");

            var copy = Board.CreateCopyBoard(original);

            var moveMade = Chess.MakeMove(copy, BoardStateOffset.B8, BoardStateOffset.B1);

            Board.UndoMove(copy, moveMade);

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
            var original = Chess.LoadBoardFromFen("1q2k3/8/8/8/8/8/3PPP2/4K3 w - - 0 1");

            var copy = Board.CreateCopyBoard(original);

            var moveMade = Chess.MakeMove(copy, BoardStateOffset.D2, BoardStateOffset.D3);

            Board.UndoMove(copy, moveMade);

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
            var original = Chess.LoadBoardFromFen("1q2k3/8/8/8/8/8/3PPP2/4K3 b - - 0 1");

            var copy = Board.CreateCopyBoard(original);

            var moveMade = Chess.MakeMove(copy, BoardStateOffset.B8, BoardStateOffset.B1);

            Board.UndoMove(copy, moveMade);

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
            var original = Chess.LoadBoardFromFen("r3k2r/8/8/8/8/8/8/R3K2R w KQkq - 0 1");
            var copy = Board.CreateCopyBoard(original);

            var move = Chess.MakeMove(copy, BoardStateOffset.E1, BoardStateOffset.C1);

            Board.UndoMove(copy, move);

            // king position checks
            Assert.AreEqual(original.E1, copy.E1);
            Assert.AreEqual(original.C1, copy.C1);

            // check rooks moved back to its starting position
            Assert.AreEqual(original.D1, copy.D1);
            Assert.AreEqual(original.A1, copy.A1);
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
            var original = Chess.LoadBoardFromFen("r3k2r/8/8/8/8/8/8/R3K2R w KQkq - 0 1");
            var copy = Board.CreateCopyBoard(original);

            var move = Chess.MakeMove(copy, BoardStateOffset.E1, BoardStateOffset.G1);

            Board.UndoMove(copy, move);

            // king position checks
            Assert.AreEqual(original.E1, copy.E1);
            Assert.AreEqual(original.G1, copy.G1);

            // check rooks moved back to its starting position
            Assert.AreEqual(original.F1, copy.F1);
            Assert.AreEqual(original.H1, copy.H1);
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
            var original = Chess.LoadBoardFromFen("r3k2r/8/8/8/8/8/8/R3K2R b KQkq - 0 1");
            var copy = Board.CreateCopyBoard(original);

            var move = Chess.MakeMove(copy, BoardStateOffset.E8, BoardStateOffset.C8);

            Board.UndoMove(copy, move);

            // king position checks
            Assert.AreEqual(original.E8, copy.E8);
            Assert.AreEqual(original.C8, copy.C8);

            // check rooks moved back to its starting position
            Assert.AreEqual(original.D8, copy.D8);
            Assert.AreEqual(original.A8, copy.A8);
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
            var original = Chess.LoadBoardFromFen("r3k2r/8/8/8/8/8/8/R3K2R b KQkq - 0 1");
            var copy = Board.CreateCopyBoard(original);

            var move = Chess.MakeMove(copy, BoardStateOffset.E8, BoardStateOffset.G8);

            Board.UndoMove(copy, move);

            // king position checks
            Assert.AreEqual(original.E8, copy.E8);
            Assert.AreEqual(original.G8, copy.G8);

            // check rooks moved back to its starting position
            Assert.AreEqual(original.F8, copy.F8);
            Assert.AreEqual(original.H8, copy.H8);
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
            var original = Chess.LoadBoardFromFen("r3k2r/8/8/8/8/8/8/R3K2R w KQkq - 0 1");
            var copy = Board.CreateCopyBoard(original);

            var move = Chess.MakeMove(copy, BoardStateOffset.E1, BoardStateOffset.C1);

            Board.UndoMove(copy, move);

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
            var original = Chess.LoadBoardFromFen("r3k2r/8/8/8/8/8/8/R3K2R b KQkq - 0 1");
            var copy = Board.CreateCopyBoard(original);

            var move = Chess.MakeMove(copy, BoardStateOffset.E8, BoardStateOffset.C8);

            Board.UndoMove(copy, move);

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
            var original = Chess.LoadBoardFromFen("3k4/8/8/8/4p3/8/3P4/3K4 w - - 0 1");
            var copy = Board.CreateCopyBoard(original);

            var firstMove = Chess.MakeMove(copy, BoardStateOffset.D2, BoardStateOffset.D4);
            var secondMove = Chess.MakeMove(copy, BoardStateOffset.E4, BoardStateOffset.D3);

            Board.UndoMove(copy, secondMove);
            Board.UndoMove(copy, firstMove);

            Assert.AreEqual(original.E4, copy.E4);
            Assert.AreEqual(original.D2, copy.D2);
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
            var original = Chess.LoadBoardFromFen("3k4/3p4/8/4P3/8/8/8/3K4 b - - 0 1");
            var copy = Board.CreateCopyBoard(original);

            var firstMove = Chess.MakeMove(copy, BoardStateOffset.D7, BoardStateOffset.D5);
            var secondMove = Chess.MakeMove(copy, BoardStateOffset.E5, BoardStateOffset.D6);

            Board.UndoMove(copy, secondMove);
            Board.UndoMove(copy, firstMove);

            Assert.AreEqual(original.D7, copy.D7);
            Assert.AreEqual(original.E5, copy.E5);
        }

    }
}
