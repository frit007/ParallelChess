using ParallelChess;
using ParallelChess.AI;
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

        public static bool botezGambit(Board board, Stack<Move> movesMade) {

            var moves = board.GetMoves();

            foreach (var move in moves) {
                if((board.GetPiece(move.fromPosition) & Piece.PIECE_MASK) == Piece.QUEEN) {
                    if(board.Attacked(move.targetPosition, board.IsWhiteTurn)) {
                        board.MakeMove(move);
                        movesMade.Push(move);
                        return true;
                    }
                }
            }

            return false;
        }

        public static async Task MainAsync(string[] args) {
            Board.initThreadStaticVariables();
            var ai = new AIWorkerManager();
            var history = new Stack<Move>();



            ai.spawnWorkers(3);

            var board = BoardFactory.LoadBoardFromFen();
            bool hasCheated = false;
            int difficulty = 5;
            bool debug = false;
            do {
                Console.WriteLine(ChessOutput.AsciiBoard(board));
                var moves = board.GetMoves();
                var winner = board.detectWinner(moves);
                if(winner == Winner.WINNER_WHITE) {
                    Console.WriteLine("you lost even though you cheated? :D");
                    Console.WriteLine("White wins");
                    break;
                } else if(winner == Winner.WINNER_BLACK) {
                    Console.WriteLine("you lost even though you cheated? :D");
                    Console.WriteLine("Black wins");
                    break;
                } else if(winner == Winner.DRAW) {
                    Console.WriteLine("Its a draw!");
                    break;
                }
                do {
                    try {
                        Console.WriteLine("Make a move (example: \"A1 A3)\"");
                        var readLineOriginal = Console.ReadLine();
                        var readLine = readLineOriginal.ToUpper().Split(" ").ToList();
                        

                        switch (readLine[0]) {
                            case "":
                            case "HELP":
                                Console.WriteLine(@"Available commands are:
 - help                     * help
 - Cheat [depth=5]          * uses eval to find best available move(at least according to AI)
 - moves                    * lists all available moves
 - board                    * Draw the board
 - debug [y/n=y]            * enable / disable debugging
 - difficulty [number=5]    * sets difficulty (above 6 is not recommended because it is slow)
 - switch                   * Switch sides
 - botez gambit             * the best gambit
 - fen [fen]                * load Forsyth-Edwards Notation 
 - undo                     * Undo the last move
 - eval                     * get the current evaluation of the board
 - workers [workerCount=2]  * Specify how many worker threads the computer should use (should be less than the amount of processors you have)
 - [from=D5]                * list moves for a specific field
 - [from=E2] [to=e4]        * play a move
 - [from=E7] [to=e8] [promotion=Q/R/B/N] * play a move and choose which piece you want to promote to");
                                continue;
                            case "MOVES":
                                Console.WriteLine("All available moves");
                                var allMoves = board.GetMoves().Where(move => board.IsLegalMove(move)).ToList();
                                foreach (var move in allMoves) {
                                    Console.WriteLine($" - {MoveHelper.ReadableMove(move)}");
                                }
                                Console.WriteLine(ChessOutput.AsciiBoard(board, allMoves, true));
                                continue;
                            case "BOARD":
                                Console.WriteLine(ChessOutput.AsciiBoard(board));
                                continue;
                            case "BOTEZ": // alias fall through 
                            case "BOTEZ_GAMBIT":
                                if (botezGambit(board, history)) {
                                    Console.WriteLine("Botez gambit found!");
                                    Console.WriteLine(ChessOutput.AsciiBoard(board));
                                    goto switchSides;
                                } else {
                                    Console.WriteLine("Botez gambit unavailable");
                                    continue;
                                }
                                //Console.WriteLine("https://www.youtube.com/watch?v=RetIukw56T0");
                                //throw new NotImplementedException("not implemented yet, finds a way to somehow lose the queen");
                                continue;
                            case "DIFFICULTY":
                                if (readLine.Count() > 1) {
                                    difficulty = int.Parse(readLine[1]);
                                    Console.WriteLine($"AI will now look {difficulty} moves ahead");
                                } else {
                                    Console.WriteLine($"Current difficulty is {difficulty}");
                                    continue;
                                }
                                continue;
                            case "SWITCH":
                                goto switchSides; // the almighty goto to skip current move and the the ai make the next move, which also switches sides as a side effect
                            case "EVAL":
                                var evalMoves = board.GetMoves().Where(move => board.IsLegalMove(move)).ToList();
                                var score = EvalBoard.evalBoard(board, evalMoves);
                                Console.WriteLine($"current score is: {score}");
                                continue;
                            case "FEN":
                                var fenList = readLineOriginal.Split(" ").ToList();
                                fenList.RemoveAt(0);
                                var fen = String.Join(" ", fenList);
                                board = BoardFactory.LoadBoardFromFen(fen);
                                Console.WriteLine($"Loaded board {fen}");
                                Console.WriteLine(ChessOutput.AsciiBoard(board));
                                continue;
                            case "DEBUG":
                                debug = true;
                                if (readLine.Count() > 1 && readLine[1] == "N") {
                                    debug = false;
                                    Console.WriteLine("disabled debug");
                                } else {
                                    Console.WriteLine("enabled debug");
                                }
                                continue;
                            case "WORKERS":
                                ai.killWorkers();
                                int workerCount = 2;
                                if (readLine.Count() > 1) {
                                    workerCount = int.Parse(readLine[1]);
                                }
                                Console.WriteLine($"Using {workerCount} workers");
                                ai.spawnWorkers(workerCount);
                                continue;
                            case "UNDO":
                                hasCheated = true;
                                if(history.Count != 0) {
                                    board.UndoMove(history.Pop());
                                } else {
                                    Console.WriteLine("There is not history to undo, sorry");
                                    continue;
                                }

                                if (history.Count != 0) {
                                    board.UndoMove(history.Pop());
                                }
                                Console.WriteLine("Undo done (cheater :p)");
                                Console.WriteLine(ChessOutput.AsciiBoard(board));
                                continue;
                            case "CHEAT":
                                Console.WriteLine("NOTE everything after the best move is probably not accurate");
                                int depth = 5;
                                if (readLine.Count() > 1) {
                                    depth = int.Parse(readLine[1]);
                                }

                                hasCheated = true;

                                List<EvaluatedMove> cheatMoves;
                                using (var progressbar = new ProgressBar()) {
                                    cheatMoves = await ai.analyzeBoard(board, depth, history, onProgress: (progress) => {
                                        progressbar.Report((double)((double)progress.progress / (double)progress.total));
                                    });
                                }

                                foreach (var cheat in cheatMoves) {
                                    Console.WriteLine($" - {MoveHelper.ReadableMove(cheat.move)} (score: {cheat.score})");
                                }
                                continue;

                            case "CHEAT2":
                                Console.WriteLine("NOTE everything after the best move is probably not accurate");
                                int depth2 = 5;
                                if (readLine.Count() > 1) {
                                    depth2 = int.Parse(readLine[1]);
                                }

                                hasCheated = true;


                                
                                //using (var progressbar = new ProgressBar()) {
                                //    cheatMoves2 = await ai.analyzeBoard(board, depth2, (progress) => {
                                //        progressbar.Report((double)((double)progress.progress / (double)progress.total));
                                //    });
                                //}
                                var minmax = new MinMaxAI();
                                List<EvaluatedMove> cheatMoves2 = minmax.MinMaxList(board, depth2);
                                //var cheatMoves = ParallelChess.AI.MinMaxAI.MinMaxList(board, depth);
                                foreach (var cheat in cheatMoves2) {
                                    Console.WriteLine($" - {MoveHelper.ReadableMove(cheat.move)} (score: {cheat.score})");
                                }
                                continue;
                            default:
                                break;
                        }

                        var fromPosition = Board.ArrayPosition(readLine[0]);
                        if (readLine.Count() == 1) {
                            var positionMoves = board.GetMovesForPosition(fromPosition);
                            Console.WriteLine(ChessOutput.AsciiBoard(board, positionMoves));
                            Console.WriteLine("Legal moves:");
                            foreach (var move in positionMoves) {
                                if (board.IsLegalMove(move)) {
                                    Console.WriteLine($" - {MoveHelper.ReadableMove(move)}");
                                }
                            }
                            continue;
                        }
                        var toPosition = Board.ArrayPosition(readLine[1]);
                        var promotion = Piece.EMPTY;
                        if (readLine.Count() > 2) {
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
                        var moveMade = board.MakeMove(fromPosition, toPosition, promotion);
                        if (MoveHelper.isValidMove(moveMade)) {
                            history.Push(moveMade);
                            break;
                        } else {
                            Console.WriteLine("invalid Move");
                        }
                    } catch (Exception e) {
                        Console.WriteLine(e.Message);
                    }

                } while(true);
                
                Console.WriteLine("Moved to");
                Console.WriteLine(ChessOutput.AsciiBoard(board));
                switchSides:  moves = board.GetMoves();
                winner = board.detectWinner(moves);
                if (winner == Winner.WINNER_WHITE) {
                    if (hasCheated) {
                        Console.WriteLine("Cheater :D");
                    }
                    Console.WriteLine("White wins");
                    break;
                } else if (winner == Winner.WINNER_BLACK) {
                    if (hasCheated) {
                        Console.WriteLine("Cheater :D");
                    }
                    Console.WriteLine("Black wins");
                    break;
                } else if (winner == Winner.DRAW) {
                    Console.WriteLine("Its a draw!");
                    break;
                }

                Console.WriteLine("AI Finding move...");
                //var bestMove = ParallelChess.AI.MinMaxAI.MinMaxList(board, 5)[0];

                if (debug) {
                    await ai.analyzeBoard(board, difficulty, history, onProgress: (progress) => {
                        Console.WriteLine($"[depth {progress.depth}] {progress.progress}/{progress.total} foundScore: {progress.foundScore} on move {MoveHelper.ReadableMove(progress.move.move.move)}, found by worker {progress.move.taskId} {{ duration {progress.move.durationMS}ms }} began from score {progress.move.startFromMin}");
                    });
                } else {
                    using (var progressbar = new ProgressBar()) {
                        await ai.analyzeBoard(board, difficulty, history, onProgress: (progress) => {
                            progressbar.Report((double)((double)progress.progress / (double)progress.total));
                        });
                    }
                }


                var bestMove = ai.GetBestMove();
                history.Push(bestMove.move);
                board.MakeMove(bestMove.move);

                Console.WriteLine($"AI found move with score {bestMove.score}");
                Console.WriteLine($"AI will play {MoveHelper.ReadableMove(bestMove.move)}");
                Console.WriteLine($"FEN: {board.FEN}");
            } while (true);


            Console.WriteLine("End of game!");
            
            Console.ReadKey();
        }
    }
}
