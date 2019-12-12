using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using ParallelChess;

namespace ParallelChessTests.MoveCompression {
    class EnpassantBlack {
        [Test]
        public static void CompactFirstBlack() {
            /* position
            +---------------+
            |_ _ _ _ k _ _ _| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |P p P _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |_ _ _ _ K _ _ _| 1
            +---------------+
             A B C D E F G H
             */
            BoardState board = Chess.LoadBoardFromFen("4k3/8/8/PpP5/8/8/8/4K3 b - B6 0 1");

            Assert.AreEqual(1, MoveHelper.compactEnpassantTarget(board));
        }
        [Test]
        public static void CompactSecondBlack() {
            /* position
            +---------------+
            |_ _ _ _ k _ _ _| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |P p p P _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |_ _ _ _ K _ _ _| 1
            +---------------+
             A B C D E F G H
             */
            BoardState board = Chess.LoadBoardFromFen("4k3/8/8/PppP/8/8/8/4K3 b - C6 0 1");

            Assert.AreEqual(2, MoveHelper.compactEnpassantTarget(board));
        }

        [Test]
        public static void CompactFourthBlack() {
            /* position
            +---------------+
            |_ _ _ _ k _ _ _| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |p P p p P p p P| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |_ _ _ _ K _ _ _| 1
            +---------------+
             A B C D E F G H
             */
            BoardState board = Chess.LoadBoardFromFen("4k3/8/8/pPppPppP/8/8/8/4K3 b - F6 0 1");

            Assert.AreEqual(4, MoveHelper.compactEnpassantTarget(board));
        }

        [Test]
        public static void CompactNoEnpassantBlack() {
            /* position
            +---------------+
            |_ _ _ _ k _ _ _| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |p P p p P p p P| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |_ _ _ _ K _ _ _| 1
            +---------------+
             A B C D E F G H
             */
            BoardState board = Chess.LoadBoardFromFen("4k3/8/8/pPppPppP/8/8/8/4K3 b - - 0 1");

            Assert.AreEqual(0, MoveHelper.compactEnpassantTarget(board));
        }

        [Test]
        public static void UnCompactFirstBlack() {
            /* position
            +---------------+
            |_ _ _ _ k _ _ _| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |P p P _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |_ _ _ _ K _ _ _| 1
            +---------------+
             A B C D E F G H
             */
            BoardState board = Chess.LoadBoardFromFen("4k3/8/8/PpP5/8/8/8/4K3 b - B6 0 1");

            Assert.AreEqual(BoardStateOffset.B6, MoveHelper.decompactEnpassantTarget(board, MoveHelper.compactEnpassantTarget(board)));
        }

        [Test]
        public static void UnCompactSecondBlack() {
            /* position
            +---------------+
            |_ _ _ _ k _ _ _| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |P p p P _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |_ _ _ _ K _ _ _| 1
            +---------------+
             A B C D E F G H
             */
            BoardState board = Chess.LoadBoardFromFen("4k3/8/8/PppP/8/8/8/4K3 b - C6 0 1");

            Assert.AreEqual(BoardStateOffset.C6, MoveHelper.decompactEnpassantTarget(board, MoveHelper.compactEnpassantTarget(board)));
        }

        [Test]
        public static void UnCompactFourthBlack() {
            /* position
            +---------------+
            |_ _ _ _ k _ _ _| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |p P p p P p p P| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |_ _ _ _ K _ _ _| 1
            +---------------+
             A B C D E F G H
             */
            BoardState board = Chess.LoadBoardFromFen("4k3/8/8/pPppPppP/8/8/8/4K3 b - F6 0 1");

            Assert.AreEqual(BoardStateOffset.F6, MoveHelper.decompactEnpassantTarget(board, MoveHelper.compactEnpassantTarget(board)));
        }

        [Test]
        public static void UnCompactNoEnpassantBlack() {
            /* position
            +---------------+
            |_ _ _ _ k _ _ _| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |p P p p P p p P| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |_ _ _ _ K _ _ _| 1
            +---------------+
             A B C D E F G H
             */
            BoardState board = Chess.LoadBoardFromFen("4k3/8/8/pPppPppP/8/8/8/4K3 b - - 0 1");

            Assert.AreEqual(EnPassant.NO_ENPASSANT, MoveHelper.decompactEnpassantTarget(board, MoveHelper.compactEnpassantTarget(board)));
        }
    }
}
