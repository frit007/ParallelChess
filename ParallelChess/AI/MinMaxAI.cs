﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParallelChess.AI {
    // based on https://en.wikipedia.org/wiki/Alpha%E2%80%93beta_pruning
    // use alpha beta prunning to reduce the amount of notes searched, in best case scenario this can remove over half of the searched nodes
    public static class MinMaxAI {
        public static long movesEvaluated = 0;
        public struct BestMove {
            public Move move;
            public float score;
        }

        // To avoid creating too many lists and destroying them have a list ready for each depth layer which is then cleared
        [ThreadStatic]
        public static List<Move>[] layeredLists = new List<Move>[100];
        [ThreadStatic]
        private static ulong boardHash = 0;

        public static void initThreadStaticVariables() {
            layeredLists = new List<Move>[100];
            for (int i = 0; i < layeredLists.Length; i++) {
                layeredLists[i] = new List<Move>();
            }
        }

        static MinMaxAI() {
            initThreadStaticVariables();
        }

        public static List<BestMove> MinMax(BoardState board, int depth, bool maximizing = true, float min = float.MinValue, float max = float.MaxValue) {
            boardHash = HashBoard.hash(board);
            List<BestMove> movePoints = new List<BestMove>();
            //var bestMove = MinMaxInternal(board, depth, maximizing, min, max);
            var bestMove = maximizing ? float.MinValue : float.MaxValue;

            var moveList = layeredLists[board.VirtualLevel];
            moveList.Clear();
            
            var moves = Board.GetMoves(board, moveList);

            var winner = Board.detectWinner(board, moves);

            foreach (var move in moves) {
                byte myTurn = board.IsWhiteTurn;
                boardHash = HashBoard.ApplyMove(board, move, boardHash);
                Board.MakeMove(board, move);

                movesEvaluated++;

                board.VirtualLevel++;

                var attacked = Board.Attacked(board, board.GetKingPosition(myTurn), myTurn);
                if (attacked) {
                    // if the king is under attack after making the move then it is not a valid move, in which case ignore the move
                    board.VirtualLevel--;
                    Board.UndoMove(board, move);
                    boardHash = HashBoard.ApplyMove(board, move, boardHash);
                    continue;
                }
                var moveScore = MinMaxInternal(board, depth, !maximizing, min, max);
                movePoints.Add(new BestMove() {
                    move = move,
                    score = moveScore,
                });

                board.VirtualLevel--;
                Board.UndoMove(board, move);
                boardHash = HashBoard.ApplyMove(board, move, boardHash);

                if (maximizing) {
                    // optimize for player
                    if (moveScore > bestMove) {
                        bestMove = moveScore;
                    }
                    min = Math.Max(moveScore, min);
                } else {
                    if (moveScore < bestMove) {
                        bestMove = moveScore;
                    }
                    max = Math.Min(moveScore, max);
                }
            }

            // sort the moves in order of
            movePoints.Sort((a, b) => (a.score < b.score) ? 1 : -1);

            return movePoints;
        }

        private static float MinMaxInternal(BoardState board, int depth, bool maximizing = true, float min = float.MinValue, float max = float.MaxValue) {
            var optimizeForColor = maximizing ? 1 : 0;
            var minimizeForColor = optimizeForColor ^ 1;

            float bestMove = maximizing ? float.MinValue : float.MaxValue;

            var moveList = layeredLists[board.VirtualLevel];
            moveList.Clear();
            var moves = Board.GetMoves(board, moveList);


            if (board.VirtualLevel == depth) {
                var winner = Board.detectWinner(board, moves);
                if ((winner == Winner.WINNER_WHITE || winner == Winner.WINNER_BLACK)) {
                    if (maximizing) {
                        // if a checkmate is found then no deeper moves matter since we are going to play that move
                        return float.MinValue + board.VirtualLevel;
                    } else {
                        return float.MaxValue - board.VirtualLevel;
                    }
                } else if (winner == Winner.DRAW) {
                    return 0;
                }

                var score = EvalBoard.evalBoard(board, moves);
                if(!maximizing) {
                    // if the score is not for the optimized player flip the score.
                    score *= -1;
                }
                return score;
            }

            // because detectWinner requires checking for valid moves, which is slow only do it for end nodes
            // for all other cases reimplement reimplement the logic locally
            if (Board.hasInsufficientMaterialOrTimeLimit(board)) {
                return 0;
            }
            // hasValidMove is used to track if the player has a valid move they can play, 
            // if not this is used to declare a winner
            bool foundValidMove = false;
            foreach (var move in moves) {
                byte myTurn = board.IsWhiteTurn;

                boardHash = HashBoard.ApplyMove(board, move, boardHash);
                Board.MakeMove(board, move);

                movesEvaluated++;

                board.VirtualLevel++;

                var attacked = Board.Attacked(board, board.GetKingPosition(myTurn), myTurn);
                if(attacked) {
                    // if the king is under attack after making the move then it is not a valid move, in which case ignore the move
                    board.VirtualLevel--;
                    Board.UndoMove(board, move);
                    boardHash = HashBoard.ApplyMove(board, move, boardHash);
                    continue;
                }
                foundValidMove = true;
                var moveScore = MinMaxInternal(board, depth, !maximizing, min, max);
                board.VirtualLevel--;
                Board.UndoMove(board, move);
                boardHash = HashBoard.ApplyMove(board, move, boardHash);

                if (maximizing) {
                    // optimize for player
                    if(moveScore > bestMove) {
                        bestMove = moveScore;
                    }
                    min = Math.Max(moveScore, min);
                    if (min >= max) {
                        return bestMove;
                    }
                } else {
                    if (moveScore < bestMove) {
                        bestMove = moveScore;
                    }
                    max = Math.Min(moveScore, max);
                    if (min >= max) {
                        return bestMove;
                    }
                }
            }

            if (!foundValidMove) {
                if (maximizing) {
                    return float.MinValue + board.VirtualLevel;
                    // if a checkmate is found then no deeper moves matter since we are going to play that move
                } else {
                    return float.MaxValue - board.VirtualLevel;
                }
            }


            return bestMove;
        }
    }
}
