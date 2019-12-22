using System;
using System.Collections.Generic;
using System.Text;

namespace ParallelChess.AI {
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

        public static BestMove MinMax(BoardState board, byte optimizeForColor, int depth, float min = float.MaxValue, float max = float.MaxValue) {
            var theirColor = optimizeForColor ^ 1;

            BestMove bestMove = new BestMove() {
                score = board.IsWhiteTurn == optimizeForColor ? max : min
            };
            var moveList = layeredLists[board.VirtualLevel];
            moveList.Clear();

            var moves = Board.GetMoves(board, moveList);
            if (board.VirtualLevel == depth) {
                var score = EvalBoard.evalBoard(board, moves);
                if(board.IsWhiteTurn != optimizeForColor) {
                    // if the score is not for the optimized player flip the score.
                    score *= -1;
                }
                return new BestMove() {
                    score = score,
                };
            }

            foreach (var move in moves) {

                Board.MakeMove(board, move);
                movesEvaluated++;
                var winner = Board.detectWinner(board, moves);

                // abuse that winner 0 is black and winner 1 white 
                if((byte)winner == optimizeForColor) {
                    bestMove.move = move;
                    bestMove.score = float.MaxValue;
                    Board.UndoMove(board, move);
                    // if a checkmate is found then no deeper moves matter since we are going to play that move
                    return bestMove;
                }else if((byte)winner == theirColor) {
                    bestMove.score = float.MinValue;
                    bestMove.move = move;
                    Board.UndoMove(board, move);
                    return bestMove;
                }else if(winner == Winner.DRAW) {
                    if(optimizeForColor != board.IsWhiteTurn) {
                        // if it is currently their turn it means it was our move which means we will pick the highest of these options
                        if(bestMove.score < 0) {
                            bestMove.score = 0;
                            bestMove.move = move;
                            if (bestMove.score < min) {
                                return bestMove;
                            }
                        }
                    } else {
                        // if it was their move then they will try to minimize the move
                        if (bestMove.score > 0) {
                            bestMove.score = 0;
                            bestMove.move = move;
                            if(bestMove.score > min) {
                                // if this worse than a guaranteed minimum then return
                                return bestMove;
                            }
                        }
                    }
                    Board.UndoMove(board, move);
                    continue;
                }

                board.VirtualLevel++;
                MinMax(board, optimizeForColor, depth);
                board.VirtualLevel--;
                Board.UndoMove(board, move);
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
