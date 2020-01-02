using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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
        public class AITask {
            public int taskId;
            public BoardState board;
            public List<List<Move>> moves;
            public int depth;
            public Action<SolvedMove> onMoveComplete;
        }

        public class SolvedMove {
            public BestMove move;
            public Move startingMove;
            public float min;
            public float max;
            //public ulong boardHash;
            public int taskId;
        }
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
            BoardState board = aiTask.board; 
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
                lastSolvedMove = MinMaxMove(aiTask, move, lastSolvedMove);
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
                lastSolvedMove.startingMove = move;
                aiTask.onMoveComplete(lastSolvedMove);
            }
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
            Board.MakeMove(aiTask.board, move);

            var detectWinnerMoves = Board.GetMoves(aiTask.board);
            // check for winner
            var winner = Board.detectWinner(aiTask.board, detectWinnerMoves);
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
                return new SolvedMove() {
                    min = min,
                    max = max,

                    move = new BestMove() {
                        move = move,
                        score = score
                    },
                    taskId = currentTask.taskId,
                };
                //boardHash = HashBoard.ApplyMove(aiTask.board, move, boardHash);
                //return;
            }


            aiTask.board.VirtualLevel++;
            var minmax = new MinMaxAI();
            var moveScore = minmax.MinMax(aiTask.board, aiTask.depth, false, min, max);
            aiTask.board.VirtualLevel--;
            Board.UndoMove(aiTask.board, move);
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


            return new SolvedMove() {
                min = min,
                max = max,

                move = new BestMove() {
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
                begunMoves.Add(solvedMove.startingMove);
                alreadySolved.Add(solvedMove.move.move);
            }
        }
    }
}
