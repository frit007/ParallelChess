using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelChess.MinMax {
    public class AIWorkerManager {
        List<AIWorker> workers = new List<AIWorker>();
        private object stateLock = new object();
        public List<SolvedMove> solvedMoves = new List<SolvedMove>();
        public int workId;
        Random random = new Random();

        public void spawnWorkers(int? number = null) {
            if(number == null) {
                number = Environment.ProcessorCount;
            }
            for (int i = 0; i < number; i++) {
                var worker = new AIWorker();

                Thread thread = new Thread(() => {
                    EvalBoard.initThreadStaticVariables();
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


        public void killWorkers() {
            foreach (var worker in workers) {
                worker.kill();
            }
            lock (stateLock) {
                workers.Clear();
            }
        }

        public EvaluatedMove GetBestMove() {
            return solvedMoves.OrderBy(move => move.move.score).Reverse().ToList().First().move;
        }

        public HashSet<ulong> findTiedBoard(Board board, Stack<Move> history) {
            Dictionary<ulong, int> occurredPositions = new Dictionary<ulong, int>();
            HashSet<ulong> tiedPositions = new HashSet<ulong>();

            // take copies so we do not modify the original collection
            var boardCopy = board.CreateCopy();
            Stack<Move> historyCopy = new Stack<Move>(history);
            var hash = HashBoard.hash(board);
            while (historyCopy.Count != 0) {
                if(!occurredPositions.ContainsKey(hash)) {
                    occurredPositions.Add(hash, 0);
                }
                // count how many times the position has occured
                occurredPositions[hash]++;
                var move = historyCopy.Pop();
                boardCopy.UndoMove(move);
                hash = HashBoard.ApplyMove(board, move, hash);
            }

            foreach (var position in occurredPositions) {
                if (position.Value >= 2) {
                    // when a position occurs 3 times 
                    tiedPositions.Add(position.Key);
                }
            }

            return tiedPositions;
        }

        public async Task<List<EvaluatedMove>> analyzeBoard(Board board, int depth, Stack<Move> history = null, Action<AIProgress> onProgress = null) {
            EvalBoard.initThreadStaticVariables();

            HashSet<ulong> tiedBoards;
            if (history != null) {
                tiedBoards = this.findTiedBoard(board, history);
            } else {
                tiedBoards = new HashSet<ulong>();
            }

            var minmax = new MinMaxAI();
            // get shallow minMax to figure out a initial ordering, because at low depth the thread overhead is going to cost more than it gains
            var moves = minmax.MinMaxList(board, 2);
            
            solvedMoves = moves.Select(move => new SolvedMove() { 
                move = move,
            }).ToList();

            var combinedMoves = moves.ToList();
            var oldMoves = new List<EvaluatedMove>();
            totalFound = 0;
            totalMoves = (depth - 2) * moves.Count; 


            for (int currentDepth = 3; currentDepth <= depth; currentDepth++) {
                moves = await delegateToWorkers(board, combinedMoves, currentDepth, tiedBoards, onProgress);


                combinedMoves.Clear();

                for (int i = 0; i < moves.Count; i++) {
                    // mix the last 2 move orders to try and find the best more as early as possible
                    EvaluatedMove nextMove;
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

        private async Task<List<EvaluatedMove>> delegateToWorkers(Board board, List<EvaluatedMove> moves, int depth, HashSet<ulong> tiedBoards, Action<AIProgress> onProgress = null) {
            EvalBoard.initThreadStaticVariables();
            int workerWorkId = random.Next();
            lock (stateLock) {
                workId = workerWorkId;
            }
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


            object alreadyCompletedLock = new object();
            try {
                var task = new Task(() => {
                    for (int i = 0; i < workers.Count; i++) {
                        var worker = workers[i];
                        var aiTask = new AITask() {
                            taskId = i,
                            board = board.CreateCopy(),
                            moves = workerMoves,
                            depth = depth,
                            tiedPositions = tiedBoards,
                            onMoveComplete = (solvedMove) => {
                                int count = 0;
                                bool isNew = false;
                                lock (stateLock) {
                                    if(workId != workerWorkId) {
                                        // if the worker returned to late then refuse the task
                                        return;
                                    }
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
                    // max wait for 3 minutes
                    Task.Delay(1000 * 60 * 3).Wait();
                    cancelationSource.Cancel();
                });

                task.Start();
                //while (!alreadyComplete) {
                //    Task.Delay(10).GetAwaiter().GetResult();
                //}
                //lock(alreadyCompleteLock) {
                //    wasComplated = alreadyComplete;
                //}
                //if(!wasComplated) {

                task.Wait(cancelationtoken);
                //}
            } catch (OperationCanceledException) {
                // intentional cancel
            }


            return solvedMoves.Select(move => move.move).OrderBy(move => move.score).Reverse().ToList();
        }
    }
}
