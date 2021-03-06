﻿using BenchmarkDotNet.Attributes;
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
            var board = BoardFactory.LoadBoardFromFen("Bn1N3R/ppPpNR1r/BnBr1NKR/k3pP2/3PR2R/N7/3P2P1/4Q2R w - e6 0 1");

            for (int i = 0; i < 10000; i++) {
                var moves = board.GetMoves();

                var legalMoves = board.GetMoves()
                    .Where(move => board.IsLegalMove(move))
                    .ToList();

                foreach (var move in moves) {
                    board.StandardAlgebraicNotation(move, legalMoves);
                }
            }
        }
    }
}
