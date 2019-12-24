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

        public static void initThreadStaticVariables() {
            layeredLists = new List<Move>[100];
            for (int i = 0; i < layeredLists.Length; i++) {
                layeredLists[i] = new List<Move>();
            }
        }

        static MinMaxAI() {
            initThreadStaticVariables();
        }

        public static BestMove MinMax(BoardState board, int depth, bool maximizing = true, float min = float.MinValue, float max = float.MaxValue) {
            var optimizeForColor = maximizing ? 1 : 0;
            var minimizeForColor = optimizeForColor ^ 1;



            var moveList = layeredLists[board.VirtualLevel];
            moveList.Clear();
            var moves = Board.GetMoves(board, moveList);

            BestMove bestMove = new BestMove() {
                score = maximizing ? float.MinValue : float.MaxValue,
            };
            

            var winner = Board.detectWinner(board, moves);
            if ((winner == Winner.WINNER_WHITE || winner == Winner.WINNER_BLACK)) {
                if(maximizing) {
                    bestMove.score = float.MinValue + board.VirtualLevel;
                    // if a checkmate is found then no deeper moves matter since we are going to play that move
                    return bestMove;
                } else {
                    bestMove.score = float.MaxValue - board.VirtualLevel;
                    return bestMove;
                }
            } else if (winner == Winner.DRAW) {
                bestMove.score = 0;
                return bestMove;
            }

            if (board.VirtualLevel == depth) {
                var score = EvalBoard.evalBoard(board, moves);
                if(!maximizing) {
                    // if the score is not for the optimized player flip the score.
                    score *= -1;
                }
                return new BestMove() {
                    score = score,
                };
            }


            
            foreach (var move in moves) {
                byte myTurn = board.IsWhiteTurn;

                Board.MakeMove(board, move);

                movesEvaluated++;

                board.VirtualLevel++;

                var attacked = Board.Attacked(board, board.GetKingPosition(myTurn), myTurn);
                if(attacked) {
                    // if the king is under attack after making the move then it is not a valid move, in which case ignore the move
                    board.VirtualLevel--;
                    Board.UndoMove(board, move);
                    continue;
                }
                BestMove moveScore = MinMax(board, depth, !maximizing, min, max);
                moveScore.move = move;
                board.VirtualLevel--;
                Board.UndoMove(board, move);
                
                if(maximizing) {
                    // optimize for player
                    if(moveScore.score > bestMove.score) {
                        bestMove = moveScore;
                    }
                    min = Math.Max(moveScore.score, min);
                    if (min >= max) {
                        return bestMove;
                    }
                } else {
                    if (moveScore.score < bestMove.score) {
                        bestMove = moveScore;
                    }
                    max = Math.Min(moveScore.score, max);
                    if(min >= max) {
                        return bestMove;
                    }
                }

                
                //Console.WriteLine("-------------undo----------------");
                //Console.WriteLine(Chess.AsciiBoard(board));

                //for (int row = 7; row >= 0; row--) {
                //    for (int column = 0; column < 8; column++) {
                //        var position = row * BoardStateOffset.ROW_OFFSET + column;
                //        if (board.bytes[position] == (byte)Piece.PAWN) {
                //            blackPawnCount++;
                //        }
                //    }
                //}
                //if (blackPawnCount == 9) {
                //    Console.WriteLine("SOMETHING MESSED UP");
                //}

            }


            return bestMove;
        }
    }
}
