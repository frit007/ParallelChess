using ParallelChess.AI.worker;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using static ParallelChess.AI.MinMaxAI;

namespace ParallelChess.AI {
    public class AIWorker {

        private object stateLock = new object();
        private float min = float.MinValue;
        private float max = float.MaxValue;
        private HashSet<Move> alreadySolved = new HashSet<Move>();
        private HashSet<Move> begunMoves = new HashSet<Move>();

        public ConcurrentQueue<AITask> tasks = new ConcurrentQueue<AITask>();

        public bool alive = true;
        AITask currentTask;



        internal void WaitForTask() {
            AITask aiTask;
            bool amIAlive = true;
            while (amIAlive) {
                while (tasks.TryDequeue(out aiTask)) {
                    MinMaxWorker(
                        aiTask
                    );
                }
                Thread.Sleep(1);
                lock (stateLock) {
                    amIAlive = alive;
                }
            }
        }

        internal void kill() {
            lock (stateLock) {
                alive = false;
            }
        }

        ulong boardHash = 0;

        float bestScore = float.MinValue;

        // start by solving your own threads problems 
        // afterwards start solving moves that belong to other that have not been started yet
        // if there are only started moves left then start working a move that is already beeing worked on (they might have a worse starting minimum score)
        internal void MinMaxWorker(AITask aiTask) {
            currentTask = aiTask;
            Board board = aiTask.board; 
            List< Move> moves = aiTask.moves[aiTask.taskId];
            int depth = aiTask.depth;
            Action<SolvedMove> onMoveComplete = aiTask.onMoveComplete;

            lock (stateLock) {
                max = float.MaxValue;
                min = float.MinValue;
                alreadySolved.Clear();
                begunMoves.Clear();
            }

            bestScore = float.MinValue;

            boardHash = HashBoard.hash(board);
            
            SolvedMove lastSolvedMove = null;

            // ------------ start working on this threads moves ------------------
            foreach (var move in moves) {
                // check that the other threads have not begun the next move yet
                if (!alreadySolved.Contains(move) && !begunMoves.Contains(move)) {
                    lastSolvedMove = MinMaxMove(aiTask, move, lastSolvedMove);
                }
            }

            // ------------ start searching for other threads unfinished work --------------
            IEnumerable<Move> availableMoves = new List<Move>();
            Random random = new Random();
            do {
                availableMoves = aiTask.moves.SelectMany((threadsMoves) => {
                    return threadsMoves.Where(move => !alreadySolved.Contains(move) && !begunMoves.Contains(move));
                }).ToList();

                // use random to avoid conflicts
                var nextMove = availableMoves.RandomElementUsing(random);
                if (MoveHelper.isValidMove(nextMove)) {
                    lastSolvedMove = MinMaxMove(aiTask, nextMove, lastSolvedMove);
                }
            } while (availableMoves.Count() > 0);

            // ------------ start searching for other threads begun but unfinished work --------------
            do {
                availableMoves = aiTask.moves.SelectMany((threadsMoves) => {
                    return threadsMoves.Where(move => !alreadySolved.Contains(move));
                }).ToList();

                // use random to avoid conflicts
                var nextMove = availableMoves.RandomElementUsing(random);
                if(MoveHelper.isValidMove(nextMove)) { 
                    lastSolvedMove = MinMaxMove(aiTask, nextMove, lastSolvedMove);
                }

            } while (availableMoves.Count() > 0);

            if (lastSolvedMove != null) {
                aiTask.onMoveComplete(lastSolvedMove);
            }
        }

        // should only be called by MinMaxWorker, since it initializes a bunch of stuff
        private SolvedMove MinMaxMove(AITask aiTask, Move move, SolvedMove lastSolvedMove) {
            if (lastSolvedMove != null) {
                lastSolvedMove.startSolvingMove = move;
                aiTask.onMoveComplete(lastSolvedMove);
            }
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            boardHash = HashBoard.ApplyMove(aiTask.board, move, boardHash);
            ulong currentBoardHash = boardHash;
            lock (stateLock) {
                if (alreadySolved.Contains(move)) {
                    // if the hash has board has already been analyzed then skip
                    boardHash = HashBoard.ApplyMove(aiTask.board, move, boardHash);
                    return null;
                }
                if (min > bestScore) {
                    bestScore = min;
                }
            }
            aiTask.board.MakeMove(move);
            var startedFromMin = min;
            var detectWinnerMoves = aiTask.board.GetMoves();
            // check for winner
            var winner = aiTask.board.detectWinner(detectWinnerMoves);
            if (winner != Winner.NONE) {
                float score = 0;
                if ((winner == Winner.WINNER_WHITE || winner == Winner.WINNER_BLACK)) {
                    // if a checkmate is found then no deeper moves matter since we are going to play that move
                    score = float.MaxValue - aiTask.board.VirtualLevel;
                } else if (winner == Winner.DRAW) {
                    score = 0;
                }
                lock (stateLock) {

                    alreadySolved.Add(move);

                    min = Math.Max(score, min);
                }
                stopWatch.Stop();
                return new SolvedMove() {
                    startFromMin = min,
                    min = min,
                    max = max,
                    durationMS = stopWatch.ElapsedMilliseconds,
                    move = new EvaluatedMove() {
                        move = move,
                        score = score
                    },
                    taskId = currentTask.taskId,
                };
            }
            if(aiTask.tiedPositions.Contains(currentBoardHash)) {
                float score = 0;
                lock (stateLock) {

                    alreadySolved.Add(move);

                    min = Math.Max(score, min);
                }
                return new SolvedMove() {
                    startFromMin = min,
                    min = min,
                    max = max,
                    durationMS = stopWatch.ElapsedMilliseconds,
                    taskId = currentTask.taskId,
                    move = new EvaluatedMove() {
                        move= move,
                        score = score,
                    },
                };
            }


            aiTask.board.VirtualLevel++;
            var minmax = new MinMaxAI();
            var moveScore = minmax.MinMax(aiTask.board, aiTask.depth, aiTask.tiedPositions, false, min, max);
            aiTask.board.VirtualLevel--;
            aiTask.board.UndoMove(move);
            boardHash = HashBoard.ApplyMove(aiTask.board, move, boardHash);
            lock (stateLock) {
                if (alreadySolved.Contains(move)) {
                    //Console.WriteLine($"collision! refound found move! {MoveHelper.ReadableMove(move)}");
                    // if the hash has board has already been analyzed before this thread managed to do it, then skip
                    return null;
                }

                // optimize for player
                if (moveScore > bestScore) {
                    bestScore = moveScore;
                }

                min = Math.Max(moveScore, min);


                alreadySolved.Add(move);
            }
            // outside of stateLock because otherwise it will trigger will trigger the lock in moveSolved
            Thread t = Thread.CurrentThread;

            stopWatch.Stop();
            return new SolvedMove() {
                startFromMin = startedFromMin,
                min = min,
                max = max,
                durationMS = stopWatch.ElapsedMilliseconds,
                move = new EvaluatedMove() {
                    move = move,
                    score = moveScore
                },
                taskId = currentTask.taskId
            };
        }


        public void moveSolved(SolvedMove solvedMove) {
            lock (stateLock) {
                int taskId = 999;
                if (currentTask != null) {
                    taskId = currentTask.taskId;
                }
                //if (solvedMove.min > min) {
                //    Console.WriteLine($"Found better min! before {min} after {solvedMove.min} to task {taskId} from task {solvedMove.taskId}");
                //    //min = solvedMove.min;
                //}
                min = Math.Max(solvedMove.min, min);
                max = Math.Min(solvedMove.max, max);
                //if (solvedMove.max < max) {
                //    max = solvedMove.max;
                //}
                begunMoves.Add(solvedMove.startSolvingMove);
                
                alreadySolved.Add(solvedMove.move.move);
            }
        }
    }
}
