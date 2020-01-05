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

                Thread thread = new Thread(() => {
                    Board.initThreadStaticVariables();
                    worker.WaitForTask();
                });
                thread.Start();
                lock (stateLock) {
                    workers.Add(worker);
                }
            }
        }
        int totalFound = 0;
        int totalMoves = 0;
        public class AIProgress{
            public int total;
            public int progress;
            public float foundScore;
            public int depth;
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
            var minmax = new MinMaxAI();
            // get shallow minMax to figure out a initial ordering, because at low depth the thread overhead is going to cost more than it gains
            var moves = minmax.MinMaxList(board, 2);

            var combinedMoves = moves.ToList();
            var oldMoves = new List<BestMove>();
            totalFound = 0;
            totalMoves = (depth - 2) * moves.Count; 


            for (int currentDepth = 3; currentDepth <= depth; currentDepth++) {
                moves = await delegateToWorkers(board, combinedMoves, currentDepth, onProgress);


                combinedMoves.Clear();

                for (int i = 0; i < moves.Count; i++) {
                    // mix the last 2 move orders to try and find the best more as early as possible
                    BestMove nextMove;
                    if (oldMoves.Count > i) {
                        nextMove = oldMoves[i];
                        if (combinedMoves.Find(existingMove => existingMove.move.Equals(nextMove.move)) == null) {
                            combinedMoves.Add(nextMove);
                        }
                    }

                    nextMove = moves[i];
                    if (combinedMoves.Find(existingMove => existingMove.move.Equals(nextMove.move)) == null) {
                        combinedMoves.Add(nextMove);
                    }

                }
                oldMoves = moves.ToList();
            }

            return moves;
        }

        private async Task<List<BestMove>> delegateToWorkers(BoardState board, List<BestMove> moves, int depth, Action<AIProgress> onProgress = null) {
            Board.initThreadStaticVariables();

            solvedMoves.Clear();
            //var minmax = new MinMaxAI();
            // get shallow minMax to figure out a initial ordering, which will be used to share out 
            //var moves = minmax.MinMaxList(board, 2);
            //var moves = Board.GetMoves(board)
            //    .Where(move => Board.IsLegalMove(board, move))
            //    .Select(move => new BestMove() { move = move, score = 10 })
            //    .ToList();
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
                        if (onProgress != null && isNew) {
                            totalFound++;
                            onProgress(new AIProgress() {
                                foundScore = solvedMove.move.score,
                                total = totalMoves,
                                progress = totalFound,
                                move = solvedMove,
                                depth = depth
                            });
                        }
                    }
                };
                worker.tasks.Enqueue(aiTask);
            }

            try {
                var task = new Task(() => {
                    // max wait for 3 minutes
                    Task.Delay(1000 * 60 * 3);
                    cancelationSource.Cancel();
                });
                task.Wait(cancelationtoken);
            } catch (OperationCanceledException) {
                // intentional cancel
            }


            return solvedMoves.Select(move => move.move).OrderBy(move => move.score).Reverse().ToList();
        }
    }
}
