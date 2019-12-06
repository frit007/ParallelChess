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

            //Board.UndoMove(copy, moveMade);

            Assert.AreEqual(original.F4, copy.F4);
            Assert.AreEqual(original.C7, copy.C7);
            //everyThingIsEqual(original, copy);
        }
    }
}
