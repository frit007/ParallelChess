using NUnit.Framework;
using ParallelChess;
using ParallelChess.AI;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParallelChessTests.AI.Easy {
    class FindMateIn1Test {
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

            MinMaxAI.MinMax(board, 3);
            //EvalBoard.evalBoard(board, )
        }
    }
}
