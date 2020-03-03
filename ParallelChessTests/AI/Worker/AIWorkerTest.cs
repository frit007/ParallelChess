using NUnit.Framework;
using ParallelChess;
using ParallelChess.MinMax;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParallelChessTests.AI.Worker {
    class AIWorkerTest {
        [Test]
        public static void simple1Workers() {
            var board = BoardFactory.LoadBoardFromFen();

            var ai = new AIWorkerManager();

            EvalBoard.initThreadStaticVariables();
            ai.spawnWorkers(1);
            ai.analyzeBoard(board, 2).GetAwaiter().GetResult();
            Assert.IsTrue(MoveHelper.isValidMove(ai.GetBestMove().move));
        }

        [Test]
        public static void oneWorkerFindCheckMate() {


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
            var board = BoardFactory.LoadBoardFromFen("rnbk2r1/pppp1pBp/3q4/8/2B3Q1/8/P5PP/R3R1K1 w - - 0 1");

            var moves = board.GetMoves();

            var ai = new AIWorkerManager();

            ai.spawnWorkers(1);
            ai.analyzeBoard(board, 3).GetAwaiter().GetResult();
            //BestMove foundMove = MinMaxAI.MinMaxList(board, 3)[0];

            Assert.AreEqual(BoardStateOffset.F6, ai.GetBestMove().move.targetPosition);
            //var board = BoardFactory.LoadBoardFromFen();


            Assert.IsTrue(MoveHelper.isValidMove(ai.GetBestMove().move));
        }

        [Test]
        public static void fourWorkersFindCheckMate() {


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
            var board = BoardFactory.LoadBoardFromFen("rnbk2r1/pppp1pBp/3q4/8/2B3Q1/8/P5PP/R3R1K1 w - - 0 1");

            var moves = board.GetMoves();

            var ai = new AIWorkerManager();

            ai.spawnWorkers(1);
            ai.analyzeBoard(board, 3).GetAwaiter().GetResult();
            //BestMove foundMove = MinMaxAI.MinMaxList(board, 3)[0];

            Assert.AreEqual(BoardStateOffset.F6, ai.GetBestMove().move.targetPosition);
            //var board = BoardFactory.LoadBoardFromFen();


            Assert.IsTrue(MoveHelper.isValidMove(ai.GetBestMove().move));
        }

        [Test]
        [Category("slow")]
        public static void FourWorkersfindMateIn3() {
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
            var board = BoardFactory.LoadBoardFromFen("r1b1n2r/1q1nNpbk/1p1p2p1/p2NpPPp/2P1P2P/3BB3/PP6/R2QK2R w - - 0 1");

            var ai = new AIWorkerManager();

            ai.spawnWorkers(4);
            ai.analyzeBoard(board, 6).GetAwaiter().GetResult();

            var bestMove = ai.GetBestMove().move;
            // it turns out there is another solution which is 1. fxg6+ fxg6 2. Qxh5 bhx6 3. g6#
            if (!(bestMove.targetPosition == BoardStateOffset.H5 || bestMove.targetPosition == BoardStateOffset.G6)) {
                Assert.Fail($"incorrect move found {MoveHelper.ReadableMove(bestMove)}");
            }
            //Assert.AreEqual(BoardStateOffset.H5, ai.GetBestMove().move.targetPosition);
        }

        [Test]
        public static void simple2Workers() {
            var board = BoardFactory.LoadBoardFromFen();

            var ai = new AIWorkerManager();
            ai.spawnWorkers(2);
            ai.analyzeBoard(board, 2).GetAwaiter().GetResult();
            Assert.IsTrue(MoveHelper.isValidMove(ai.GetBestMove().move));
        }

        [Test]
        public static void simple4Workers() {
            var board = BoardFactory.LoadBoardFromFen();

            var ai = new AIWorkerManager();
            ai.spawnWorkers(4);
            ai.analyzeBoard(board, 2).GetAwaiter().GetResult();
            Assert.IsTrue(MoveHelper.isValidMove(ai.GetBestMove().move));
        }

        [Test]
        public static void simple4WorkersDepth5() {
            var board = BoardFactory.LoadBoardFromFen();

            var ai = new AIWorkerManager();

            ai.spawnWorkers(4);
            ai.analyzeBoard(board, 5).GetAwaiter().GetResult();
            Assert.IsTrue(MoveHelper.isValidMove(ai.GetBestMove().move));
        }


        [Test]
        public static void findValidMove() {
            var board = BoardFactory.LoadBoardFromFen();

            var ai = new AIWorkerManager();

            ai.spawnWorkers(1);
            ai.analyzeBoard(board, 2).GetAwaiter().GetResult();
            Assert.IsTrue(MoveHelper.isValidMove(ai.GetBestMove().move));
        }

        //test removed because it is just too slow
       [Test]
       [Category("slow")]
        public static void CompleteTest() {
            // for some reason this board would not return a answer when on difficulty 6
            var board = BoardFactory.LoadBoardFromFen("rnb1kbnr/pppp1ppp/4p3/8/3PP2q/8/PPP2PPP/RNBQKBNR w KQkq - 1 3");
            var ai = new AIWorkerManager();

            ai.spawnWorkers(3);
            ai.analyzeBoard(board, 6).GetAwaiter().GetResult();
            Assert.IsTrue(MoveHelper.isValidMove(ai.GetBestMove().move));
        }
    }
}
