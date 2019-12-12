using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using ParallelChess;

namespace ParallelChessTests.MoveCompression {
    class EnPassantWhite {
        [Test]
        public static void CompactFirstWhite() {
            /* position
            +---------------+
            |_ _ _ _ k _ _ _| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |p P p _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |_ _ _ _ K _ _ _| 1
            +---------------+
             A B C D E F G H
             */
            BoardState board = Chess.LoadBoardFromFen("4k3/8/8/8/pPp5/8/8/4K3 w - B3 0 1");

            Assert.AreEqual(1, MoveHelper.compactEnpassantTarget(board));
        }
        [Test]
        public static void CompactSecondWhite() {
            /* position
            +---------------+
            |_ _ _ _ k _ _ _| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |p P P p _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |_ _ _ _ K _ _ _| 1
            +---------------+
             A B C D E F G H
             */
            BoardState board = Chess.LoadBoardFromFen("4k3/8/8/8/pPPp4/8/8/4K3 w - C3 0 1");

            Assert.AreEqual(2, MoveHelper.compactEnpassantTarget(board));
        }

        [Test]
        public static void CompactFourthWhite() {
            /* position
            +---------------+
            |_ _ _ _ k _ _ _| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |P p P P p P P p| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |_ _ _ _ K _ _ _| 1
            +---------------+
             A B C D E F G H
             */
            BoardState board = Chess.LoadBoardFromFen("4k3/8/8/8/PpPPpPPp/8/8/4K3 w - F3 0 1");

            Assert.AreEqual(4, MoveHelper.compactEnpassantTarget(board));
        }

        [Test]
        public static void CompactNoEnpassantWhite() {
            /* position
            +---------------+
            |_ _ _ _ k _ _ _| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |P p P P p P P p| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |_ _ _ _ K _ _ _| 1
            +---------------+
             A B C D E F G H
             */
            BoardState board = Chess.LoadBoardFromFen("4k3/8/8/8/PpPPpPPp/8/8/4K3 w - - 0 1");

            Assert.AreEqual(0, MoveHelper.compactEnpassantTarget(board));
        }

        [Test]
        public static void UnCompactFirstWhite() {
            /* position
            +---------------+
            |_ _ _ _ k _ _ _| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |p P p _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |_ _ _ _ K _ _ _| 1
            +---------------+
             A B C D E F G H
             */
            BoardState board = Chess.LoadBoardFromFen("4k3/8/8/8/pPp5/8/8/4K3 w - B3 0 1");

            Assert.AreEqual(BoardStateOffset.B3, MoveHelper.decompactEnpassantTarget(board, MoveHelper.compactEnpassantTarget(board)));
        }

        [Test]
        public static void UnCompactSecondWhite() {
            /* position
            +---------------+
            |_ _ _ _ k _ _ _| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |p P P p _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |_ _ _ _ K _ _ _| 1
            +---------------+
             A B C D E F G H
             */
            BoardState board = Chess.LoadBoardFromFen("4k3/8/8/8/pPPp4/8/8/4K3 w - C3 0 1");

            Assert.AreEqual(BoardStateOffset.C3, MoveHelper.decompactEnpassantTarget(board, MoveHelper.compactEnpassantTarget(board)));
        }

        [Test]
        public static void UnCompactFourthWhite() {
            /* position
            +---------------+
            |_ _ _ _ k _ _ _| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |P p P P p P P p| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |_ _ _ _ K _ _ _| 1
            +---------------+
             A B C D E F G H
             */
            BoardState board = Chess.LoadBoardFromFen("4k3/8/8/8/PpPPpPPp/8/8/4K3 w - F3 0 1");

            Assert.AreEqual(BoardStateOffset.F3, MoveHelper.decompactEnpassantTarget(board, MoveHelper.compactEnpassantTarget(board)));
        }

        [Test]
        public static void UnCompactNoEnpassantWhite() {
            /* position
            +---------------+
            |_ _ _ _ k _ _ _| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |P p P P p P P p| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |_ _ _ _ K _ _ _| 1
            +---------------+
             A B C D E F G H
             */
            BoardState board = Chess.LoadBoardFromFen("4k3/8/8/8/PpPPpPPp/8/8/4K3 w - - 0 1");

            Assert.AreEqual(EnPassant.NO_ENPASSANT, MoveHelper.decompactEnpassantTarget(board, MoveHelper.compactEnpassantTarget(board)));
        }
    }
}
