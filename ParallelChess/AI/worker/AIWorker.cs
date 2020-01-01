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
        private float min = float.MaxValue;
        private float max = float.MinValue;
        private HashSet<Move> alreadySolved = new HashSet<Move>();

        public ConcurrentQueue<AITask> tasks = new ConcurrentQueue<AITask>();

        public bool alive = true;

        public class AITask {
            public int taskId;
            public BoardState board;
            public List<Move> moves;
            public int depth;
            public Action<SolvedMove> onMoveComplete;
        }

        public struct SolvedMove {
            public BestMove move;
            public float min;
            public float max;
            //public ulong boardHash;
            public int solvedByThread;
        }
        internal void WaitForTask() {
            AITask aiTask;
            while (alive) {
                while (tasks.TryDequeue(out aiTask)) {
                    MinMaxWorker(
                        aiTask
                    );
                }
                Thread.Sleep(1);
            }
        }

        internal void kill() {
            alive = false;
        }

        internal void MinMaxWorker(AITask aiTask) {
            BoardState board = aiTask.board; 
            List< Move > moves = aiTask.moves;
            int depth = aiTask.depth;
            Action<SolvedMove> onMoveComplete = aiTask.onMoveComplete;

            lock (stateLock) {
                max = float.MaxValue;
                min = float.MinValue;
                alreadySolved.Clear();
            }

            float bestScore = float.MinValue;

            ulong boardHash = HashBoard.hash(board);

            foreach (var move in moves) {
                boardHash = HashBoard.ApplyMove(board, move, boardHash);
                ulong currentBoardHash = boardHash;
                lock (stateLock) {
                    if(alreadySolved.Contains(move)) {
                        // if the hash has board has already been analyzed then skip
                        boardHash = HashBoard.ApplyMove(board, move, boardHash);
                        continue;
                    }
                    if (min > bestScore) {
                        bestScore = min;
                    }
                }
                Board.MakeMove(board, move);

                var detectWinnerMoves = Board.GetMoves(board);
                // check for winner
                var winner = Board.detectWinner(board, detectWinnerMoves);
                if (winner != Winner.NONE) {
                    float score = 0;
                    if ((winner == Winner.WINNER_WHITE || winner == Winner.WINNER_BLACK)) {

                        // if a checkmate is found then no deeper moves matter since we are going to play that move
                        score = float.MinValue + board.VirtualLevel;

                    } else if (winner == Winner.DRAW) {
                        score = 0;
                    }
                    lock (stateLock) {
                        alreadySolved.Add(move);

                        min = Math.Max(score, min);
                    }
                    onMoveComplete(new SolvedMove() {
                        min = min,
                        max = max,

                        move = new BestMove() {
                            move = move,
                            score = score
                        },
                        solvedByThread = aiTask.taskId,
                    });
                    boardHash = HashBoard.ApplyMove(board, move, boardHash);
                    continue;
                }


                board.VirtualLevel++;
                var minmax = new MinMaxAI();
                var moveScore = minmax.MinMax(board, depth, false, min, max);
                board.VirtualLevel--;
                Board.UndoMove(board, move);
                boardHash = HashBoard.ApplyMove(board, move, boardHash);
                lock (stateLock) {
                    if(alreadySolved.Contains(move)) {
                        // if the hash has board has already been analyzed before this thread managed to do it, then skip
                        continue;
                    }

                    // optimize for player
                    if (moveScore > bestScore) {
                        bestScore = moveScore;
                    }

                    min = Math.Max(moveScore, min);


                    alreadySolved.Add(move);
                }
                onMoveComplete(new SolvedMove() {
                    min = min,
                    max = max,

                    move = new BestMove() {
                        move = move,
                        score = moveScore
                    },
                });
            }
        }


        public void moveSolved(SolvedMove solvedMove) {
            lock (stateLock) {
                if(solvedMove.min < min) {
                    Console.WriteLine($"Found better min! before {min} after {solvedMove.min}");
                    min = solvedMove.min;
                }
                if(solvedMove.max < max) {
                    max = solvedMove.max;
                }
                alreadySolved.Add(solvedMove.move.move);
            }
        }
    }
}
