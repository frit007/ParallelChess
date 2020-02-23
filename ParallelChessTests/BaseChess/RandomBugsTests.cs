using NUnit.Framework;
using ParallelChess;
using ParallelChess.MinMax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParallelChessTests.BaseChess {
    class RandomBugsTests {
        // this class was created to repeat any random bugs.
        // in theory anything in this class could be deleted, but these tests probably contain some interesting edge cases
        public void everyThingIsEqual(Board original, Board copy) {
            for (int i = 0; i < BoardStateOffset.BOARD_STATE_SIZE; i++) {
                Assert.AreEqual(original.bytes[i], copy.bytes[i], $"The boards are not equal at offset {i}");
            }
        }
        [Test]
        public void thisIsNotWon() {
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

            var moves = board.GetMoves();
            board.Move(BoardStateOffset.C1, BoardStateOffset.C2, Piece.EMPTY);

            var winner = board.detectWinner(moves);

            Assert.AreEqual(Winner.NONE, winner);
        }

        [Test]
        public void pawnTakes() {
            /*
             * Starting position (White to play)
            +---------------+
            |_ _ _ _ _ _ k _| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ n B N| 3
            |_ _ _ B _ P P P| 2
            |_ _ _ _ _ R K R| 1
            +---------------+
             A B C D E F G H
             */
            var board = BoardFactory.LoadBoardFromFen("6k1/8/8/8/8/5nBN/3B1PPP/5RKR w - - 0 1");
            var original = board.CreateCopy();

            var moves = board.GetMoves();

            var move = board.Move(BoardStateOffset.G2, BoardStateOffset.F3);

            board.UndoMove(move);

            everyThingIsEqual(original, board);
        }
        [Test]
        public void pawnTakes____TESTING() {
            /*
             * Starting position (White to play)
            +---------------+
            |_ _ _ _ _ _ k _| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ n B N| 3
            |_ _ _ B _ P P P| 2
            |_ _ _ _ _ R K R| 1
            +---------------+
             A B C D E F G H
             */
            var board = BoardFactory.LoadBoardFromFen("6k1/8/8/8/8/5nBN/3B1PPP/5RKR w - - 0 1");
            var original = board.CreateCopy();

            var moves = board.GetMoves();

            //board.IsWhiteTurn ^= 1;
            //moves.Clear();
            //var theirMoves = Board.GetMoves(board, moves);
            //var theirValidMoves = theirMoves.Where(move => Board.IsLegalMove(board, move)).ToList();
            //// NOTE: the score has to be counted before switching the turns back to normal, 
            //// because otherwise the the linq statement will not evaluate with the correct state.
            //board.IsWhiteTurn ^= 1;

            EvalBoard.evalBoard(board, moves);

            everyThingIsEqual(original, board);
        }
    }
}
