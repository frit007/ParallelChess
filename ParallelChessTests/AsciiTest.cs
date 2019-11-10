using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using ParallelChess;

namespace ParallelChessTests {
    class AsciiTest {

        [Test]
        public void SimpleBoard() {
            var board = Chess.LoadBoardFromFen();

            string ascii = Chess.AsciiBoard(board);

            Console.WriteLine(ascii);
        }
    }
}
