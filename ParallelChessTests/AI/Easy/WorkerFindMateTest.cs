using NUnit.Framework;
using ParallelChess;
using ParallelChess.AI;
using ParallelChess.AI.worker;
using System;
using System.Collections.Generic;
using System.Text;
using static ParallelChess.AI.MinMaxAI;

namespace ParallelChessTests.AI.Easy {
    class WorkerFindMateTest {
        [Test]
        public static void findQueenMate() {
            /*
             * Starting position (White to play)
            +---------------+
            |_ k _ _ _ _ _ _| 8
            |p p p p _ p p p| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ Q _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |_ _ K _ _ _ _ _| 1
            +---------------+
             A B C D E F G H
             */
            var board = Chess.LoadBoardFromFen("1k6/pppp1ppp/8/4Q3/8/8/8/2K5 w - - 0 1");

            var moves = Board.GetMoves(board);
            var bestMove = moves.FindTargetPosition(BoardStateOffset.E8);

            var aiWorkers = new AIWorkerManager();
            aiWorkers.spawnWorkers(3);
            aiWorkers.analyzeBoard(board, 1).Wait();
            aiWorkers.killWorkers();
            BestMove foundMove = aiWorkers.GetBestMove();

            Assert.AreEqual(bestMove.targetPosition, foundMove.move.targetPosition);
        }

        [Test]
        public static void findKnightMate() {
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
             */
            var board = Chess.LoadBoardFromFen("rkr5/ppp2ppp/8/4N3/8/8/8/2K5 w - - 0 1");

            var moves = Board.GetMoves(board);
            //var minmax = new MinMaxAI();
            //BestMove foundMove = minmax.MinMaxList(board, 1)[0];


            var aiWorkers = new AIWorkerManager();
            aiWorkers.spawnWorkers(3);
            aiWorkers.analyzeBoard(board, 1).Wait();
            aiWorkers.killWorkers();
            BestMove foundMove = aiWorkers.GetBestMove();

            Assert.AreEqual(BoardStateOffset.D7, foundMove.move.targetPosition);
        }

        [Test]
        public static void defendAgainstMate() {
            /*
             * Starting position (White to play)
            +---------------+
            |_ _ _ _ _ _ k _| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ r _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ p _ p _| 3
            |_ _ _ _ P _ P _| 2
            |_ _ N _ _ _ K _| 1
            +---------------+
             A B C D E F G H
            C1 -> D3 (expected line to protect against mate on D1)
            +---------------+
            |_ _ _ _ _ _ k _| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ r _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ N p _ p _| 3
            |_ _ _ _ P _ P _| 2
            |_ _ _ _ _ _ K _| 1
            +---------------+
             A B C D E F G H
            D5 -> D3
            +---------------+
            |_ _ _ _ _ _ k _| 8
            |_ _ _ _ _ _ _ _| 7
            |_ _ _ _ _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ r p _ p _| 3
            |_ _ _ _ P _ P _| 2
            |_ _ _ _ _ _ K _| 1
            +---------------+
             A B C D E F G H
             */
            var board = Chess.LoadBoardFromFen("6k1/8/8/3r4/8/4p1p1/4P1P1/2N3K1 w - - 0 1");

            var moves = Board.GetMoves(board);
            var aiWorkers = new AIWorkerManager();
            aiWorkers.spawnWorkers(3);
            aiWorkers.analyzeBoard(board, 3).Wait();
            aiWorkers.killWorkers();
            BestMove foundMove = aiWorkers.GetBestMove();
            // the rook has to move to e3 to defend against mate(d4->e2)
            Assert.AreEqual(BoardStateOffset.D3, foundMove.move.targetPosition);
        }

        [Test]
        [Category("slow")]
        public static void findMateIn2() {
            /*
             * Starting position (White to play)
            +---------------+
            |r n b k _ _ r _| 8
            |p p p p _ p B p| 7
            |_ _ _ q _ _ _ _| 6
            |_ _ _ _ _ _ _ _| 5
            |_ _ B _ _ _ Q _| 4
            |_ _ _ _ _ _ _ _| 3
            |P _ _ _ _ _ P P| 2
            |R _ _ _ R _ K _| 1
            +---------------+
             A B C D E F G H
             */
            var board = Chess.LoadBoardFromFen("rnbk2r1/pppp1pBp/3q4/8/2B3Q1/8/P5PP/R3R1K1 w - - 0 1");

            var moves = Board.GetMoves(board);

            var aiWorkers = new AIWorkerManager();
            aiWorkers.spawnWorkers(3);
            aiWorkers.analyzeBoard(board, 3).Wait();
            aiWorkers.killWorkers();
            BestMove foundMove = aiWorkers.GetBestMove();

            Assert.AreEqual(BoardStateOffset.F6, foundMove.move.targetPosition);
        }

        [Test]
        [Category("slow")]
        public static void findMateIn3() {
            /*
             * Starting position (White to play)
            +---------------+
            |r _ b _ n _ _ r| 8
            |_ q _ n N p b k| 7
            |_ p _ p _ _ p _| 6
            |p _ _ N p P P p| 5
            |_ _ P _ P _ _ P| 4
            |_ _ _ B B _ _ _| 3
            |P P _ _ _ _ _ _| 2
            |R _ _ Q K _ _ R| 1
            +---------------+
             A B C D E F G H
            D1 -> H5
            +---------------+
            |r _ b _ n _ _ r| 8
            |_ q _ n N p b k| 7
            |_ p _ p _ _ p _| 6
            |p _ _ N p P P Q| 5
            |_ _ P _ P _ _ P| 4
            |_ _ _ B B _ _ _| 3
            |P P _ _ _ _ _ _| 2
            |R _ _ _ K _ _ R| 1
            +---------------+
             A B C D E F G H
            G6 -> H5
            +---------------+
            |r _ b _ n _ _ r| 8
            |_ q _ n N p b k| 7
            |_ p _ p _ _ _ _| 6
            |p _ _ N p P P p| 5
            |_ _ P _ P _ _ P| 4
            |_ _ _ B B _ _ _| 3
            |P P _ _ _ _ _ _| 2
            |R _ _ _ K _ _ R| 1
            +---------------+
             A B C D E F G H
            G5 -> G6
            +---------------+
            |r _ b _ n _ _ r| 8
            |_ q _ n N p b k| 7
            |_ p _ p _ _ P _| 6
            |p _ _ N p P _ p| 5
            |_ _ P _ P _ _ P| 4
            |_ _ _ B B _ _ _| 3
            |P P _ _ _ _ _ _| 2
            |R _ _ _ K _ _ R| 1
            +---------------+
             A B C D E F G H
            F7 -> G6
            +---------------+
            |r _ b _ n _ _ r| 8
            |_ q _ n N _ b k| 7
            |_ p _ p _ _ p _| 6
            |p _ _ N p P _ p| 5
            |_ _ P _ P _ _ P| 4
            |_ _ _ B B _ _ _| 3
            |P P _ _ _ _ _ _| 2
            |R _ _ _ K _ _ R| 1
            +---------------+
             A B C D E F G H
            F5 -> G6 (Mate)
            +---------------+
            |r _ b _ n _ _ r| 8
            |_ q _ n N _ b k| 7
            |_ p _ p _ _ P _| 6
            |p _ _ N p _ _ p| 5
            |_ _ P _ P _ _ P| 4
            |_ _ _ B B _ _ _| 3
            |P P _ _ _ _ _ _| 2
            |R _ _ _ K _ _ R| 1
            +---------------+
             A B C D E F G H
             */
            var board = Chess.LoadBoardFromFen("r1b1n2r/1q1nNpbk/1p1p2p1/p2NpPPp/2P1P2P/3BB3/PP6/R2QK2R w - - 0 1");

            var aiWorkers = new AIWorkerManager();
            aiWorkers.spawnWorkers(3);
            aiWorkers.analyzeBoard(board, 5).Wait();
            aiWorkers.killWorkers();
            BestMove foundMove = aiWorkers.GetBestMove();

            if (!(foundMove.move.targetPosition == BoardStateOffset.H5|| foundMove.move.targetPosition == BoardStateOffset.G6)) {
                Assert.Fail($"found move {MoveHelper.ReadableMove(foundMove.move)}");
            }
        }

        [Test]
        public void keepPlayingEvenWhenLost() {
            /*
             * Starting position (Black to play)
            +---------------+
            |B _ _ _ _ _ _ _| 8
            |_ _ _ _ _ P R p| 7
            |_ _ _ _ N _ _ k| 6
            |_ _ _ p _ _ p _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ P _ P _| 3
            |P _ _ _ _ _ K _| 2
            |_ _ _ _ _ _ _ _| 1
            +---------------+
             A B C D E F G H
             */
            var board = Chess.LoadBoardFromFen("B7/5PRp/4N2k/3p2p1/8/4P1P1/P5K1/8 b - - 0 2");

            var moves = Board.GetMoves(board);


            var aiWorkers = new AIWorkerManager();
            aiWorkers.spawnWorkers(3);
            aiWorkers.analyzeBoard(board, 5).Wait();
            aiWorkers.killWorkers();
            BestMove foundMove = aiWorkers.GetBestMove();

            Assert.IsTrue(MoveHelper.isValidMove(foundMove.move));
        }

        [Test]
        public void keepPlayingEvenWhenLostSimpler() {
            /*
             * Starting position (Black to play)
            +---------------+
            |_ _ _ _ _ _ _ k| 8
            |p R p _ _ _ p _| 7
            |P p P _ _ _ P _| 6
            |_ P _ _ _ _ p _| 5
            |_ _ _ _ _ _ _ _| 4
            |_ _ _ _ _ _ _ _| 3
            |_ _ _ _ _ _ _ _| 2
            |_ _ _ _ _ _ _ _| 1
            +---------------+
             A B C D E F G H
             */
            var board = Chess.LoadBoardFromFen("7k/pRp3p1/PpP3P1/1P4p1/8/8/8/8 b - - 0 1");

            var moves = Board.GetMoves(board);

            var aiWorkers = new AIWorkerManager();
            aiWorkers.spawnWorkers(3);
            aiWorkers.analyzeBoard(board, 2).Wait();
            aiWorkers.killWorkers();
            BestMove foundMove = aiWorkers.GetBestMove();

            Assert.IsTrue(MoveHelper.isValidMove(foundMove.move));
        }

    }
}
