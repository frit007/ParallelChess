using BenchmarkDotNet.Attributes;
using ParallelChess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParallelChessPerformance {
    [SimpleJob]
    public class SanStringBuilder {


        [Benchmark]
        public void generateSan() {
            var board = Chess.LoadBoardFromFen("Bn1N3R/ppPpNR1r/BnBr1NKR/k3pP2/3PR2R/N7/3P2P1/4Q2R w - e6 0 1");

            for (int i = 0; i < 10000; i++) {
                var moves = BoardHelper.GetMoves(board);

                var legalMoves = BoardHelper.GetMoves(board)
                    .Where(move => BoardHelper.IsLegalMove(board, move))
                    .ToList();

                foreach (var move in moves) {
                    board.StandardAlgebraicNotation(move, legalMoves);
                }
            }
        }
    }
}
