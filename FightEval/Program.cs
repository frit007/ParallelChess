using ParallelChess;
using System;
using System.Linq;

namespace FightEval {
    class Program {
        static void Main(string[] args) {
            Console.WriteLine("");
            var board = Chess.LoadBoardFromFen();
            bool hasCheated = false;
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
                        Console.WriteLine("Make a move (eksample: \"A1 A3)\"");
                        var readLine = Console.ReadLine().ToUpper().Split(" ");
                        

                        switch (readLine[0]) {
                            case "":
                            case "HELP":
                                Console.WriteLine(@"Available commands are:
 - help              * help
 - Cheat [depth=5]   * uses eval to find best available move(at least according to AI)
 - moves             * lists all available moves
 - board             * Draw the board
 - [from=D5]         * list moves for a specific field
 - [from=E2] [to=e4] * play a move
 - [from=E7] [to=e8] [promotion=Q/R/B/N] * play a move and choose which piece you want to promote to"

);
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
                            case "CHEAT":
                                int depth = 5;
                                if (readLine.Length > 1) {
                                    depth = int.Parse(readLine[1]);
                                }
                                hasCheated = true;
                                var cheatMoves = ParallelChess.AI.MinMaxAI.MinMax(board, depth);
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
                moves = Board.GetMoves(board);
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
                var bestMove = ParallelChess.AI.MinMaxAI.MinMax(board, 5)[0];

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
