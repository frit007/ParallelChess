using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using ParallelChess;
using System.Linq;

namespace ParallelChessTests.BaseChess {
    class IsValidMoveTest {
        //[Test]
        //public void RunOutOfBoardLeft() {
        //    Assert.IsFalse(Board.IsValidPosition(BoardStateOffset.A1, -1, 0));
        //}

        //[Test]
        //public void RunOutOfBoardRight() {
        //    Assert.IsFalse(Board.IsValidPosition(BoardStateOffset.H8, 1, 0));
        //}

        //[Test]
        //public void RunOutOfBoardTop() {
        //    Assert.IsFalse(Board.IsValidPosition(BoardStateOffset.A8, 0, 1));
        //}

        //[Test]
        //public void RunOutOfBoardBottom() {
        //    Assert.IsFalse(Board.IsValidPosition(BoardStateOffset.A1, 0, -1));
        //}

        //[Test]
        //public void ValidateHorseMoveFromA2() {
        //    Assert.IsTrue(Board.IsValidPosition(BoardStateOffset.B1, 1, 2));
        //    Assert.False(Board.IsValidPosition(BoardStateOffset.B1, 1, -2));


        //    Assert.True(Board.IsValidPosition(BoardStateOffset.B1, -1, 2));
        //    Assert.IsFalse(Board.IsValidPosition(BoardStateOffset.B1, -1, -2));


        //    Assert.IsFalse(Board.IsValidPosition(BoardStateOffset.B1, -2, 1));
        //    Assert.IsFalse(Board.IsValidPosition(BoardStateOffset.B1, -2, -1));

        //    Assert.IsTrue(Board.IsValidPosition(BoardStateOffset.B1, 2, 1));
        //    Assert.IsFalse(Board.IsValidPosition(BoardStateOffset.B1, 2, -1));
        //}
        [Test]
        public void HorseProtectKingFromStraightMove() {
            /*
             * Start position (White to play)
                +---------------+
                |_ _ _ _ k _ _ _| 8
                |_ _ _ _ q _ _ _| 7
                |_ _ _ _ _ _ _ _| 6
                |_ _ _ _ _ _ _ _| 5
                |_ _ _ _ _ _ _ _| 4
                |_ _ _ _ _ _ _ _| 3
                |_ _ N _ _ _ _ _| 2
                |_ _ _ Q K _ _ _| 1
                +---------------+
                 A B C D E F G H

                The knight should have have 4 invalid move options(marked X) and one valid (marked Y)
                +---------------+
                |_ _ _ _ k _ _ _| 8
                |_ _ _ _ q _ _ _| 7
                |_ _ _ _ _ _ _ _| 6
                |_ _ _ _ _ _ _ _| 5
                |_ X _ X _ _ _ _| 4
                |X _ _ _ Y _ _ _| 3
                |_ _ N _ _ _ _ _| 2
                |X _ _ Q K _ _ _| 1
                +---------------+
                 A B C D E F G H
             */
            Board board = Chess.LoadBoardFromFen("4k3/4q3/8/8/8/8/2N5/3QK3 w - - 0 1");

            List<Move> moves = BoardHelper.GetMovesForPosition(board, BoardStateOffset.C2);

            Assert.AreEqual(5, moves.Count, "The knight should have 5 move options");
            //Move move = moves.FindTargetPosition(BoardStateOffset.E3);

            Assert.IsTrue(BoardHelper.IsLegalMove(board, moves.FindTargetPosition(BoardStateOffset.E3)), "Knight can move to E3 to protect the king");

            Assert.IsFalse(BoardHelper.IsLegalMove(board, moves.FindTargetPosition(BoardStateOffset.D4)), "Knight cannot move here because the king is under attack");
            Assert.IsFalse(BoardHelper.IsLegalMove(board, moves.FindTargetPosition(BoardStateOffset.B4)), "Knight cannot move here because the king is under attack");
            Assert.IsFalse(BoardHelper.IsLegalMove(board, moves.FindTargetPosition(BoardStateOffset.A3)), "Knight cannot move here because the king is under attack");
            Assert.IsFalse(BoardHelper.IsLegalMove(board, moves.FindTargetPosition(BoardStateOffset.A1)), "Knight cannot move here because the king is under attack");
        }

        [Test]
        public void HorseProtectKingFromSlantedMove() {
            /*
             * Start position (White to play)
                +---------------+
                |_ _ _ _ k _ _ _| 8
                |_ _ _ _ _ _ _ _| 7
                |_ _ _ _ _ _ _ _| 6
                |_ _ _ _ _ _ _ _| 5
                |_ _ _ _ _ _ _ q| 4
                |_ _ _ N _ _ _ _| 3
                |_ _ _ _ _ _ _ _| 2
                |_ _ _ Q K _ _ _| 1
                +---------------+
                 A B C D E F G H

                The knight should have have 6 invalid move options(marked X) and one valid (marked Y)
                +---------------+
                |_ _ _ _ k _ _ _| 8
                |_ _ _ _ _ _ _ _| 7
                |_ _ _ _ _ _ _ _| 6
                |_ _ X _ X _ _ _| 5
                |_ X _ _ _ X _ q| 4
                |_ _ _ N _ _ _ _| 3
                |_ X _ _ _ Y _ _| 2
                |_ _ X Q K _ _ _| 1
                +---------------+
                 A B C D E F G H
             */
            Board board = Chess.LoadBoardFromFen("4k3/8/8/8/7q/3N4/8/3QK3 w - - 0 1");

            List<Move> moves = BoardHelper.GetMovesForPosition(board, BoardStateOffset.D3);

            Assert.AreEqual(7, moves.Count, "The knight should have 7 move options");
            Move move = moves.FindTargetPosition(BoardStateOffset.E3);

            Assert.IsTrue(BoardHelper.IsLegalMove(board, moves.FindTargetPosition(BoardStateOffset.F2)), "Knight can move to E3 to protect the king");

            Assert.IsFalse(BoardHelper.IsLegalMove(board, moves.FindTargetPosition(BoardStateOffset.F4)), "Knight cannot move To F4 because the king is under attack");
            Assert.IsFalse(BoardHelper.IsLegalMove(board, moves.FindTargetPosition(BoardStateOffset.E5)), "Knight cannot move To E3 because the king is under attack");
            Assert.IsFalse(BoardHelper.IsLegalMove(board, moves.FindTargetPosition(BoardStateOffset.C5)), "Knight cannot move To C4 because the king is under attack");
            Assert.IsFalse(BoardHelper.IsLegalMove(board, moves.FindTargetPosition(BoardStateOffset.B4)), "Knight cannot move To B3 because the king is under attack");
            Assert.IsFalse(BoardHelper.IsLegalMove(board, moves.FindTargetPosition(BoardStateOffset.B2)), "Knight cannot move To B1 because the king is under attack");
            Assert.IsFalse(BoardHelper.IsLegalMove(board, moves.FindTargetPosition(BoardStateOffset.C1)), "Knight cannot move To C1 because the king is under attack");
        }

        [Test]
        public void QueenProtectsKing() {
            /*
            * Start position (White to play)
              +---------------+
              |r _ _ _ q k _ r| 8
              |_ _ _ _ _ _ _ _| 7
              |_ _ _ _ _ _ _ _| 6
              |_ _ _ Q _ _ _ _| 5
              |_ _ _ _ _ _ _ _| 4
              |_ _ _ _ _ _ _ _| 3
              |_ _ _ _ _ _ _ R| 2
              |R _ _ _ K _ _ _| 1
              +---------------+
               A B C D E F G H

               the Queen should have 24 invalid move options (marked X) and 3 valid (marked Y)
              +---------------+
              |X _ _ X q k X r| 8
              |_ X _ X _ X _ _| 7
              |_ _ X X Y _ _ _| 6
              |X X X Q Y X X X| 5
              |_ _ X X Y _ _ _| 4
              |_ X _ X _ X _ _| 3
              |X _ _ X _ _ X R| 2
              |R _ _ X K _ _ X| 1
              +---------------+
               A B C D E F G H
            */
            Board board = Chess.LoadBoardFromFen("r3qk1r/8/8/3Q4/8/8/7R/R3K3 w Qkq - 0 1");

            List<Move> moves = BoardHelper.GetMovesForPosition(board, BoardStateOffset.D5);

            Assert.AreEqual(27, moves.Count);

            // valid moves
            Assert.IsTrue(BoardHelper.IsLegalMove(board, moves.FindTargetPosition(BoardStateOffset.E4)));
            Assert.IsTrue(BoardHelper.IsLegalMove(board, moves.FindTargetPosition(BoardStateOffset.E5)));
            Assert.IsTrue(BoardHelper.IsLegalMove(board, moves.FindTargetPosition(BoardStateOffset.E6)));

            // invalid moves
            Assert.IsFalse(BoardHelper.IsLegalMove(board, moves.FindTargetPosition(BoardStateOffset.A8)));
            Assert.IsFalse(BoardHelper.IsLegalMove(board, moves.FindTargetPosition(BoardStateOffset.G2)));
            Assert.IsFalse(BoardHelper.IsLegalMove(board, moves.FindTargetPosition(BoardStateOffset.A2)));
            Assert.IsFalse(BoardHelper.IsLegalMove(board, moves.FindTargetPosition(BoardStateOffset.G8)));
            Assert.IsFalse(BoardHelper.IsLegalMove(board, moves.FindTargetPosition(BoardStateOffset.F3)));
            // ... there are more invalid moves
        }

        [Test]
        public void ProtectAgainstHorse() {
            /*
             * Start position (White to Play)
             +---------------+
             |_ _ _ _ _ _ k _| 8
             |_ _ _ _ _ p p p| 7
             |_ _ _ _ _ _ _ _| 6
             |_ _ _ _ _ _ _ _| 5
             |_ _ _ _ _ _ _ _| 4
             |_ _ Q _ _ n _ _| 3
             |_ _ _ _ _ _ _ _| 2
             |_ _ _ _ K _ _ _| 1
             +---------------+
              A B C D E F G H
             
             find 20 invalid moves(marked X) for Queen and only 1 valid(marked Y)
             +---------------+
             |_ _ X _ _ _ k _| 8
             |_ _ X _ _ p X p| 7
             |_ _ X _ _ X _ _| 6
             |X _ X _ X _ _ _| 5
             |_ X X X _ _ _ _| 4
             |X X Q X X Y _ _| 3
             |_ X X X _ _ _ _| 2
             |X _ X _ K _ _ _| 1
             +---------------+
              A B C D E F G H
              */
            Board board = Chess.LoadBoardFromFen("6k1/5ppp/8/8/8/2Q2n2/8/4K3 w - - 0 1");

            List<Move> moves = BoardHelper.GetMovesForPosition(board, BoardStateOffset.C3);

            Assert.AreEqual(21, moves.Count());

            Assert.IsTrue(BoardHelper.IsLegalMove(board, moves.FindTargetPosition(BoardStateOffset.F3)), "Can protect the king against Knight");
            Assert.IsFalse(BoardHelper.IsLegalMove(board, moves.FindTargetPosition(BoardStateOffset.C8)), "The king is under attack from Knight on F3");
        }

        [Test]
        public void PieceDoNotAttackTheirOwnColor() {
            /*
             * Start position (White to Play)
             +---------------+
             |_ _ _ _ _ _ k _| 8
             |_ _ _ _ _ p p p| 7
             |_ _ _ _ _ _ _ _| 6
             |_ _ _ _ _ _ _ _| 5
             |_ _ _ _ _ _ _ _| 4
             |_ _ _ _ _ _ _ _| 3
             |_ _ _ _ _ _ _ _| 2
             |_ _ Q _ K _ _ _| 1
             +---------------+
              A B C D E F G H

             
             +---------------+
             |_ _ Y _ _ _ k _| 8
             |_ _ Y _ _ p p p| 7
             |_ _ Y _ _ _ _ Y| 6
             |_ _ Y _ _ _ Y _| 5
             |_ _ Y _ _ Y _ _| 4
             |Y _ Y _ Y _ _ _| 3
             |_ Y Y Y _ _ _ _| 2
             |Y Y Q Y K _ _ _| 1
             +---------------+
              A B C D E F G H
            */
            Board board = Chess.LoadBoardFromFen("6k1/5ppp/8/8/8/8/8/2Q1K3 w - - 0 1");

            var moves = BoardHelper.GetMovesForPosition(board, BoardStateOffset.C1);

            Assert.IsTrue(BoardHelper.IsLegalMove(board, moves.FindTargetPosition(BoardStateOffset.A1)));
            Assert.IsTrue(BoardHelper.IsLegalMove(board, moves.FindTargetPosition(BoardStateOffset.C8)));
            Assert.IsTrue(BoardHelper.IsLegalMove(board, moves.FindTargetPosition(BoardStateOffset.C3)));
        }

        [Test]
        public void KingCannotWalkIntoAnotherKingsTeritory() {
            /*
             * Start position (White to Play)
             +---------------+
             |_ _ _ _ _ _ _ _| 8
             |_ _ _ _ _ _ _ _| 7
             |_ _ _ _ _ _ _ _| 6
             |_ _ _ _ _ _ _ _| 5
             |_ _ _ _ _ _ _ _| 4
             |_ _ _ _ _ k _ _| 3
             |_ _ _ _ _ _ _ _| 2
             |_ r _ _ _ K _ _| 1
             +---------------+
              A B C D E F G H

             The king is in checkmate and has only illegal moves (Marked X).
             +---------------+
             |_ _ _ _ _ _ _ _| 8
             |_ _ _ _ _ _ _ _| 7
             |_ _ _ _ _ _ _ _| 6
             |_ _ _ _ _ _ _ _| 5
             |_ _ _ _ _ _ _ _| 4
             |_ _ _ _ _ k _ _| 3
             |_ _ _ _ X X X _| 2
             |_ r _ _ X K X _| 1
             +---------------+
              A B C D E F G H
              */
            Board board = Chess.LoadBoardFromFen("8/8/8/8/8/5k2/8/1r3K2 w - - 0 1");

            var moves = BoardHelper.GetMovesForPosition(board, BoardStateOffset.F1);

            Assert.AreEqual(5, moves.Count());

            Assert.IsFalse(BoardHelper.IsLegalMove(board, moves.FindTargetPosition(BoardStateOffset.E1)));
            Assert.IsFalse(BoardHelper.IsLegalMove(board, moves.FindTargetPosition(BoardStateOffset.E2)));
            Assert.IsFalse(BoardHelper.IsLegalMove(board, moves.FindTargetPosition(BoardStateOffset.F2)));
            Assert.IsFalse(BoardHelper.IsLegalMove(board, moves.FindTargetPosition(BoardStateOffset.G1)));
            Assert.IsFalse(BoardHelper.IsLegalMove(board, moves.FindTargetPosition(BoardStateOffset.G2)));
        }

        [Test]
        public void WhiteQueenDefendsKingFromPawn() {
            /*
             * Start position (White to play) 
             +---------------+
             |_ _ k _ _ _ _ _| 8
             |_ p p p _ _ _ _| 7
             |_ _ _ _ _ _ _ _| 6
             |_ _ _ _ _ _ _ _| 5
             |_ _ _ _ Q _ _ _| 4
             |_ _ _ _ _ _ _ _| 3
             |_ _ _ _ p _ _ _| 2
             |_ _ _ _ _ K _ _| 1
             +---------------+
              A B C D E F G H

             White must defend king. Invalid moves marked with X, valid moves marked with Y
             +---------------+
             |_ _ k _ X _ _ _| 8
             |_ X p p X _ _ X| 7
             |_ _ X _ X _ X _| 6
             |_ _ _ X X X _ _| 5
             |X X X X Q X X X| 4
             |_ _ _ X X X _ _| 3
             |_ _ X _ Y _ X _| 2
             |_ X _ _ _ K _ X| 1
             +---------------+
              A B C D E F G H
             */
            var board = Chess.LoadBoardFromFen("2k5/1ppp4/8/8/4Q3/8/4p3/5K2 w - - 0 1");

            var moves = BoardHelper.GetMovesForPosition(board, BoardStateOffset.E4);

            Assert.IsTrue(BoardHelper.IsLegalMove(board, moves.FindTargetPosition(BoardStateOffset.E2)));
            Assert.IsFalse(BoardHelper.IsLegalMove(board, moves.FindTargetPosition(BoardStateOffset.E8)));
        }

        [Test]
        public void BlackQueenDefendsKingFromPawn() {
            /*
             * Start position (Black to play)
             +---------------+
             |_ _ _ k _ _ _ _| 8
             |_ _ P _ _ _ _ _| 7
             |_ _ q _ _ _ _ _| 6
             |_ _ _ _ _ _ _ _| 5
             |_ _ _ _ _ _ _ _| 4
             |_ _ _ _ _ _ _ _| 3
             |_ _ _ _ P P P _| 2
             |_ _ _ _ _ K _ _| 1
             +---------------+
              A B C D E F G H

             Black must defend king. Invalid moves marked with X, valid moves marked with Y
             +---------------+
             |X _ _ k X _ _ _| 8
             |_ X Y X _ _ _ _| 7
             |X X q X X X X X| 6
             |_ X X X _ _ _ _| 5
             |X _ X _ X _ _ _| 4
             |_ _ X _ _ X _ _| 3
             |_ _ X _ P P X _| 2
             |_ _ X _ _ K _ _| 1
             +---------------+
              A B C D E F G H
             */
            var board = Chess.LoadBoardFromFen("3k4/2P5/2q5/8/8/8/4PPP1/5K2 b - - 0 1");

            var moves = BoardHelper.GetMovesForPosition(board, BoardStateOffset.C6);

            Assert.IsTrue(BoardHelper.IsLegalMove(board, moves.FindTargetPosition(BoardStateOffset.C7)));
            Assert.IsFalse(BoardHelper.IsLegalMove(board, moves.FindTargetPosition(BoardStateOffset.C1)));
        }
    }
}
