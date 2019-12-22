using ParallelChess;
using System;
using System.Collections.Generic;

namespace AsciiCommentGenerator {
    class Program {
        static void Main(string[] args) {
            Console.WriteLine("Please enter a FEN");

            var fen = Console.ReadLine();
            BoardState board = Chess.LoadBoardFromFen(fen);

            var color = board.IsWhiteTurnBool ? "White" : "Black";
            
            Console.WriteLine(Chess.AsciiBoard(board));

            var moves = new List<string>();

            do {
                Console.WriteLine("Do you want to add moves? for each move add in the format \"A2 A4\" if you want no more moves press space, you can specify promotion with by adding R,N,B,Q like \"A7 A8 Q\"");
                var read = Console.ReadLine().ToUpper();
                if(read == "") {
                    break;
                } else {
                    moves.Add(read);
                }
            } while (true);
            
            Console.WriteLine(
                "/*\n" +
                $" * Starting position ({color} to play)");

            Console.WriteLine($"{Chess.AsciiBoard(board)}");
            foreach (var move in moves) {
                var split = move.Split(" ");
                var promotion = "";
                Piece promotionPiece = Piece.EMPTY;
                
                if(split.Length == 3) {
                    switch (split[2]) {
                        case "Q":
                            promotionPiece = Piece.QUEEN;
                            promotion = "(Promote to Queen)";
                            break;
                        case "R":
                            promotionPiece = Piece.ROOK;
                            promotion = "(Promote to Rook)";
                            break;
                        case "B":
                            promotionPiece = Piece.BISHOP;
                            promotion = "(Promote to Bishop)";
                            break;
                        case "N":
                            promotionPiece = Piece.KNIGHT;
                            promotion = "(Promote to Knight)";
                            break;
                    };
                }

                Chess.MakeMove(board, Board.AlgebraicPosition(split[0]), Board.AlgebraicPosition(split[1]), promotionPiece);
                Console.WriteLine($"{split[0]} -> {split[1]} {promotion}");
                Console.WriteLine($"{Chess.AsciiBoard(board)}");
            }
            Console.WriteLine(" */\n" +
                $"var board = Chess.LoadBoardFromFen(\"{fen}\");\n\n"+
                $"var moves = Board.GetMoves(board);");

            foreach (var move in moves) {
                var split = move.Split(" ");
                var promotion = "EMPTY";
                if (split.Length == 3) {
                    switch (split[2]) {
                        case "Q":
                            promotion = "QUEEN";
                            break;
                        case "R":
                            promotion = "ROOK";
                            break;
                        case "B":
                            promotion = "BISHOP";
                            break;
                        case "N":
                            promotion = "Knight";
                            break;
                    };
                }
                Console.WriteLine($"Chess.MakeMove(board, BoardStateOffset.{split[0]},BoardStateOffset.{split[1]}, Piece.{promotion});");
            }
        }
    }
}
