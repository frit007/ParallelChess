﻿using ParallelChess;
using ParallelChess.AI.worker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ParallelChess.AI.MinMaxAI;

namespace FightEval {
    class Program {

        public static void Main(string[] args) {
            MainAsync(args).GetAwaiter().GetResult();
        }

        public static async Task MainAsync(string[] args) {
            Board.initThreadStaticVariables();
            var ai = new AIWorkerManager();
            ai.spawnWorkers(4);
            var board = Chess.LoadBoardFromFen();
            bool hasCheated = false;
            int difficulty = 5;
            bool debug = true;
            do {
                Console.WriteLine(Chess.AsciiBoard(board));
                var moves = Board.GetMoves(board);
                var winner = Board.detectWinner(board, moves);
                if(winner == Winner.WINNER_WHITE) {
                    Console.WriteLine("White wins");
                    break;
                } else if(winner == Winner.WINNER_BLACK) {
                    Console.WriteLine("Black wins");
                    break;
                } else if(winner == Winner.DRAW) {
                    Console.WriteLine("Its a draw!");
                    break;
                }
                do {
                    try {
                        Console.WriteLine("Make a move (example: \"A1 A3)\"");
                        var readLine = Console.ReadLine().ToUpper().Split(" ");
                        

                        switch (readLine[0]) {
                            case "":
                            case "HELP":
                                Console.WriteLine(@"Available commands are:
 - help              * help
 - Cheat [depth=5]   * uses eval to find best available move(at least according to AI)
 - moves             * lists all available moves
 - board             * Draw the board
 - debug [y/n=y]       * enable / disable debugging
 - difficulty [number=5]    * sets difficulty (above 6 is not recommended because it is slow)
 - switch            * Switch sides
 - [from=D5]         * list moves for a specific field
 - [from=E2] [to=e4] * play a move
 - [from=E7] [to=e8] [promotion=Q/R/B/N] * play a move and choose which piece you want to promote to");
                                continue;
                            case "MOVES":
                                Console.WriteLine("All available moves");
                                var allMoves = Board.GetMoves(board).Where(move => Board.IsLegalMove(board,move)).ToList();
                                foreach (var move in allMoves) {
                                    Console.WriteLine($" - {MoveHelper.ReadableMove(move)}");
                                }
                                Console.WriteLine(Chess.AsciiBoard(board, allMoves, true));
                                continue;
                            case "BOARD":
                                Console.WriteLine(Chess.AsciiBoard(board));
                                continue;
                            case "BOTEZ_GAMBIT":
                                Console.WriteLine("https://www.youtube.com/watch?v=RetIukw56T0");
                                throw new NotImplementedException("not implemented yet, finds a way to somehow lose the queen");
                                continue;
                            case "DIFFICULTY":
                                difficulty = 5;
                                if (readLine.Length > 1) {
                                    difficulty = int.Parse(readLine[1]);
                                }
                                Console.WriteLine($"AI will now look {difficulty} moves ahead");
                                continue;
                            case "SWITCH":
                                goto switchSides; // the almighty goto to skip current move and the the ai make the next move, which also switches sides as a side effect
                            case "DEBUG":
                                debug = true;
                                if (readLine.Length > 1 && readLine[1] == "N") {
                                    debug = false;
                                }
                                continue;
                            case "CHEAT":
                                Console.WriteLine("NOTE everything after the best move is probably not accurate");
                                int depth = 5;
                                if (readLine.Length > 1) {
                                    depth = int.Parse(readLine[1]);
                                }

                                hasCheated = true;

                                List<BestMove> cheatMoves;
                                using (var progressbar = new ProgressBar()) {
                                    cheatMoves = await ai.analyzeBoard(board, depth, (progress) => {
                                        progressbar.Report((double)((double)progress.progress / (double)progress.total));
                                    });
                                }
                                //var cheatMoves = ParallelChess.AI.MinMaxAI.MinMaxList(board, depth);
                                foreach (var cheat in cheatMoves) {
                                    Console.WriteLine($" - {MoveHelper.ReadableMove(cheat.move)} (score: {cheat.score})");
                                }
                                continue;
                            default:
                                break;
                        }

                        var fromPosition = Board.AlgebraicPosition(readLine[0]);
                        if (readLine.Length == 1) {
                            var positionMoves = Board.GetMovesForPosition(board, fromPosition);
                            Console.WriteLine(Chess.AsciiBoard(board, positionMoves));
                            Console.WriteLine("Legal moves:");
                            foreach (var move in positionMoves) {
                                if (Board.IsLegalMove(board, move)) {
                                    Console.WriteLine($" - {MoveHelper.ReadableMove(move)}");
                                }
                            }
                        }
                        var toPosition = Board.AlgebraicPosition(readLine[1]);
                        var promotion = Piece.EMPTY;
                        if (readLine.Length > 2) {
                            switch (readLine[2]) {
                                case "Q":
                                    promotion = Piece.QUEEN;
                                    break;
                                case "R":
                                    promotion = Piece.ROOK;
                                    break;
                                case "N":
                                    promotion = Piece.KNIGHT;
                                    break;
                                case "B":
                                    promotion = Piece.BISHOP;
                                    break;
                                default:
                                    Console.WriteLine("You can only promote to Queen(Q), Rook(R), Knight(N) or Bishop(B)");
                                    continue;
                            }
                        }
                        var moveMade = Chess.MakeMove(board, fromPosition, toPosition, promotion);
                        if (MoveHelper.isValidMove(moveMade)) {
                            break;
                        } else {
                            Console.WriteLine("invalid Move");
                        }
                    } catch (Exception e) {
                        Console.WriteLine(e.Message);
                    }

                } while(true);
                
                Console.WriteLine("Moved to");
                Console.WriteLine(Chess.AsciiBoard(board));
                switchSides:  moves = Board.GetMoves(board);
                winner = Board.detectWinner(board, moves);
                if (winner == Winner.WINNER_WHITE) {
                    if (hasCheated) {
                        Console.WriteLine("Cheater :D");
                    }
                    Console.WriteLine("White wins");
                    break;
                } else if (winner == Winner.WINNER_BLACK) {
                    Console.WriteLine("Black wins");
                    break;
                } else if (winner == Winner.DRAW) {
                    Console.WriteLine("Its a draw!");
                    break;
                }

                Console.WriteLine("AI Finding move...");
                //var bestMove = ParallelChess.AI.MinMaxAI.MinMaxList(board, 5)[0];

                if (debug) {
                    await ai.analyzeBoard(board, difficulty, (progress) => {
                        Console.WriteLine($"{progress.progress}/{progress.total} foundScore: {progress.foundScore} on move {MoveHelper.ReadableMove(progress.move.move.move)}, found by {progress.move.solvedByThread}");
                    });
                } else {
                    using (var progressbar = new ProgressBar()) {
                        await ai.analyzeBoard(board, difficulty, (progress) => {
                            progressbar.Report((double)((double)progress.progress / (double)progress.total));
                        });
                    }
                }


                var bestMove = ai.GetBestMove();

                Board.MakeMove(board, bestMove.move);

                Console.WriteLine($"AI found move with score {bestMove.score}");
                Console.WriteLine($"AI will play {MoveHelper.ReadableMove(bestMove.move)}");
                Console.WriteLine($"FEN: {Chess.BoardToFen(board)}");
            } while (true);


            Console.WriteLine("End of game!");
            
            Console.ReadKey();
        }
    }
}
