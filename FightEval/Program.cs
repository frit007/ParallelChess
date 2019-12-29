using ParallelChess;
using System;

namespace FightEval {
    class Program {
        static void Main(string[] args) {
            Console.WriteLine("");
            var board = Chess.LoadBoardFromFen("6k1/pp6/3K1np1/6pp/1P6/2r5/6PP/R7 w - - 2 43");

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
                        var readLine = Console.ReadLine().Split(" ");

                        var fromPosition = Board.AlgebraicPosition(readLine[0]);
                        var toPosition = Board.AlgebraicPosition(readLine[1]);
                        var moveMade = Chess.MakeMove(board, fromPosition, toPosition);
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
                var bestMove = ParallelChess.AI.MinMaxAI.MinMax(board, 7);

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
