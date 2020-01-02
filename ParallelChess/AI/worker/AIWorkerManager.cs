using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static ParallelChess.AI.AIWorker;
using static ParallelChess.AI.MinMaxAI;

namespace ParallelChess.AI.worker {
    public class AIWorkerManager {
        List<AIWorker> workers = new List<AIWorker>();
        private object stateLock = new object();
        public List<SolvedMove> solvedMoves = new List<SolvedMove>();

        public void spawnWorkers(int? number = null) {
            if(number == null) {
                number = Environment.ProcessorCount;
            }
            for (int i = 0; i < number; i++) {
                var worker = new AIWorker();
                new Thread(() => {
                    Board.initThreadStaticVariables();

                    lock (stateLock) {
                        workers.Add(worker);
                    }
                    worker.WaitForTask();
                }).Start();
            }
        }

        public class AIProgress{
            public int total;
            public int progress;
            public float foundScore;
            public SolvedMove move;
        }

        public void killWorkers() {
            foreach (var worker in workers) {
                worker.kill();
            }
            lock (stateLock) {
                workers.Clear();
            }
        }

        public BestMove GetBestMove() {
            return solvedMoves.OrderBy(move => move.move.score).Reverse().ToList().First().move;
        }

        public async Task<List<BestMove>> analyzeBoard(BoardState board, int depth, Action<AIProgress> onProgress = null) {
            Board.initThreadStaticVariables();

            solvedMoves.Clear();
            var minmax = new MinMaxAI();
            // get shallow minMax to figure out a initial ordering, which will be used to share out 
            var moves = minmax.MinMaxList(board, 2);
            // create a list of moves foreach worker
            List<List<Move>> workerMoves = workers.Select((worker) => new List<Move>()).ToList();

            // the idea here is that every worker gets the complete list of moves to try but they start at different points
            // the goal is to find the best move as soon as possible as possible and then share it to all other threads
            // therefor the moves are sorted first and then given out so the first worker
            // worker 1: 1,4,7,10,     
            // worker 2: 2,5,8,11,
            // worker 3: 3,6,9,12,
            for (int i = 0; i < moves.Count; i++) {
                var move = moves[i];
                workerMoves[i % workerMoves.Count].Add(move.move);
            }

            CancellationTokenSource cancelationSource = new CancellationTokenSource();
            var cancelationtoken = cancelationSource.Token;

            int moveCount = moves.Count;

            for (int i = 0; i < workers.Count; i++) {
                var worker = workers[i];
                var aiTask = new AITask() {
                    taskId = i,
                    board = Board.CreateCopyBoard(board),
                    moves = workerMoves,
                    depth = depth,
                    onMoveComplete = (solvedMove) => {
                        int count = 0;
                        bool isNew = false;
                        lock (stateLock) {
                            var exists = solvedMoves.Count(move => move.move.move.Equals(solvedMove.move.move));
                            if (exists == 0) {
                                isNew = true;
                                solvedMoves.Add(solvedMove);
                                foreach (var otherWorker in workers) {
                                    if (worker != otherWorker) {
                                        otherWorker.moveSolved(solvedMove);
                                    }
                                }
                                count = solvedMoves.Count;
                                if (solvedMoves.Count >= moveCount) {
                                    cancelationSource.Cancel();
                                }
                            }

                        }
                        if(onProgress != null && isNew) {
                            onProgress(new AIProgress() {
                                foundScore = solvedMove.move.score,
                                total = moveCount,
                                progress = count,
                                move = solvedMove
                            });
                        }
                    }
                };
                worker.tasks.Enqueue(aiTask);
            }

            try {
                var task = new Task(() => {
                    // max wait for 3 minutes
                    Task.Delay(1000*60*3);
                });
                task.Wait(cancelationtoken);
            } catch (OperationCanceledException) {
                // intentional cancel
            }
            

            return solvedMoves.Select(move => move.move).OrderBy(move => move.score).Reverse().ToList();
        }
    }
}
