using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using ParallelChess;

namespace ParallelChessTests.BaseChess {
    class AsciiTest {

        [Test]
        public void SimpleBoard() {
            var board = Board.LoadBoardFromFen();

            string ascii = ChessOutput.AsciiBoard(board);

            Console.WriteLine(ascii);
        }
    }
}
