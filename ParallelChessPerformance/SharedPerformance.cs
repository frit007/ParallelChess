using BenchmarkDotNet.Attributes;
using ParallelChess;
using ParallelChess.AI;
using ParallelChess.AI.worker;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ParallelChessPerformance {
    [SimpleJob]
    public class SharedPerformance {

        //String fen = "rnbqkb1r/ppp1pppp/5n2/3p4/3P1B2/5N2/PPP1PPPP/RN1QKB1R b KQkq - 4 3";
        // "r1b2rk1/ppq1bppp/2nppn2/8/2B1PP2/1NN1B3/PPP3PP/R2Q1RK1 w - - 0 1"
        [Params("r1b2rk1/ppq1bppp/2nppn2/8/2B1PP2/1NN1B3/PPP3PP/R2Q1RK1 w - - 0 1")]
        public String fen = "rnbqkb1r/ppp1pppp/5n2/3p4/3P1B2/5N2/PPP1PPPP/RN1QKB1R b KQkq - 4 3";

        [Params(5)]
        public int depth { get; set; }

        [Params(1, 4, 7, 8)]
        public int workers { get; set; }

        Board board;

        [GlobalSetup]
        public void setup() {
            board = BoardFactory.LoadBoardFromFen(fen);
        }

        //[Benchmark]
        //public void solvePositionJustMinMax() {
        //    var minmax = new MinMaxAI();
        //    minmax.MinMaxList(board, depth);
        //}

        //[Benchmark]
        //public void solvePosition1Worker() {
        //    var ai = new AIWorkerManager();
        //    ai.spawnWorkers(1);
        //    Task.Run(async () => {
        //        await ai.analyzeBoard(board, depth);
        //    }).GetAwaiter().GetResult();
        //    ai.killWorkers();
        //}

        //[Benchmark]
        //public void solvePosition2Worker() {
        //    var ai = new AIWorkerManager();
        //    ai.spawnWorkers(2);
        //    Task.Run(async () => {
        //        await ai.analyzeBoard(board, depth);
        //    }).GetAwaiter().GetResult();
        //    ai.killWorkers();
        //}
        [Benchmark]
        public void solvePositionWorker() {
            var ai = new AIWorkerManager();
            ai.spawnWorkers(workers);
            Task.Run(async () => {
                await ai.analyzeBoard(board, depth);
            }).GetAwaiter().GetResult();
            ai.killWorkers();
        }
        //[Benchmark]
        //public void solvePosition4Worker() {
        //    var ai = new AIWorkerManager();
        //    ai.spawnWorkers(4);
        //    Task.Run(async () => {
        //        await ai.analyzeBoard(board, depth);
        //    }).GetAwaiter().GetResult();
        //    ai.killWorkers();
        //}

        //[Benchmark]
        //public void solvePosition8Worker() {
        //    var ai = new AIWorkerManager();
        //    ai.spawnWorkers(8);
        //    Task.Run(async () => {
        //        await ai.analyzeBoard(board, depth);
        //    }).GetAwaiter().GetResult();
        //    ai.killWorkers();
        //}

        //[Benchmark]
        //public void solvePosition7Worker() {
        //    var ai = new AIWorkerManager();
        //    ai.spawnWorkers(7);
        //    Task.Run(async () => {
        //        await ai.analyzeBoard(board, depth);
        //    }).GetAwaiter().GetResult();
        //    ai.killWorkers();
        //}

        //[Benchmark]
        //public void solvePosition4Worker() {
        //    var ai = new AIWorkerManager();
        //    ai.spawnWorkers(4);
        //    Task.Run(async () => {
        //        await ai.analyzeBoard(board, depth);
        //    }).GetAwaiter().GetResult();
        //    ai.killWorkers();
        //}
    }
}
