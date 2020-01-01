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
                new Thread(() => {
                    Board.initThreadStaticVariables();
                    var worker = new AIWorker();

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
            // therefor the workers are given one of what is believed to be the best move in order
            for (int workerIndex = 0; workerIndex < workerMoves.Count; workerIndex++) {
                for (int i = 0; i < moves.Count; i++) {
                    var move = moves[(workerIndex + i) % moves.Count];
                    workerMoves[workerIndex].Add(move.move);
                }
            }

            CancellationTokenSource cancelationSource = new CancellationTokenSource();
            var cancelationtoken = cancelationSource.Token;
            
            for (int i = 0; i < workers.Count; i++) {
                var worker = workers[i];
                var aiTask = new AITask() {
                    taskId = i,
                    board = board,
                    moves = workerMoves[i],
                    depth = depth,
                    onMoveComplete = (solvedMove) => {
                        int moveCount = workerMoves.First().Count;
                        int count = 0;
                        lock (stateLock) {
                            var exists = solvedMoves.Count(move => move.move.Equals(solvedMove.move));
                            if (exists == 0) {
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
                        if(onProgress != null) {
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
