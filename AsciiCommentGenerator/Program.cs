using ParallelChess;
using System;
using System.Collections.Generic;

namespace AsciiCommentGenerator {
    class Program {
        static void Main(string[] args) {
            while (true) {
                Console.WriteLine("press \"1\" for PGN \"2\" for FEN builder ");
                var key = Console.ReadKey();
                switch (key.KeyChar) {
                    case '1':
                        GameToChessComment(CreateGameFromPGN());
                        return ;
                    case'2':
                        GameToChessComment(CreateGameLinePerLine());
                        return ;
                    default:
                        break;
                }
            }
        }

        static ChessGame CreateGameFromPGN() {
            Console.WriteLine("please paste pgn (2 empty lines marks the end of the pgn)");

            string pgn = "";
            bool lastLineWasEmpty = false;
            while(true) {
                string line = Console.ReadLine();
                if (lastLineWasEmpty) {
                    if(line.Trim() == "") {
                        break;
                    } else {
                        lastLineWasEmpty = false;
                    }
                } else {
                    if(line.Trim() == "") {
                        lastLineWasEmpty = true;
                    }
                }
                pgn += line + "\n";
            }

            return PGNParser.parse(pgn);
        }
        static ChessGame CreateGameLinePerLine() {

            ChessGame game = null;
            while (game == null) {
                try {
                    Console.WriteLine("Please enter a FEN");

                    var fen = Console.ReadLine();

                    game = ChessGame.ContinueFromFEN(fen);
                } catch (Exception e) {
                    Console.WriteLine(e.Message);
                }
            }

            do {
                Console.WriteLine(ChessOutput.AsciiBoard(game.board));
                Console.WriteLine("Do you want to add moves? for each move add in the format \"A2 A4\" if you want no more moves press space, you can specify promotion with by adding R,N,B,Q like \"A7 A8 Q\" or san");
                var read = Console.ReadLine();
                if (read == "") {
                    break;
                } else {
                    try {
                        var split = read.Split(" ");
                        Move boardMove;
                        Piece promotionPiece = Piece.EMPTY;
                        if (split.Length == 1) {
                            var chessMove = game.FindMoveFromSan(split[0]);
                            boardMove = chessMove.move;
                        } else {
                            if (split.Length == 3) {
                                switch (split[2].ToUpper()) {
                                    case "Q":
                                        promotionPiece = Piece.QUEEN;
                                        break;
                                    case "R":
                                        promotionPiece = Piece.ROOK;
                                        break;
                                    case "B":
                                        promotionPiece = Piece.BISHOP;
                                        break;
                                    case "N":
                                        promotionPiece = Piece.KNIGHT;
                                        break;
                                };
                            }
                            boardMove = game.board.FindMove(BoardPosition.ArrayPosition(split[0]), BoardPosition.ArrayPosition(split[1]), promotionPiece);
                        }
                        if (!MoveHelper.isValidMove(boardMove)) {
                            continue;
                        }

                        game.Move(boardMove);
                    } catch (Exception e) {

                        Console.WriteLine("move could not be found");
                    }

                }
            } while (true);

            return game;
        }

        static void GameToChessComment(ChessGame game) {

            ChessGame copy = game.Copy();
            copy.UndoAll();

            var color = copy.board.IsWhiteTurnBool ? "White" : "Black";
            
            var fen = copy.FEN;

            Console.WriteLine(
                "/*\n" +
                $" * Starting position ({color} to play)");

            Console.WriteLine($"{ChessOutput.AsciiBoard(game)}");
            
            while(copy.CanRedo()) {
                var chessMove = copy.Redo();
                var move = chessMove.move;
                var promotion = "";
                
                if ((Piece)move.promotion != Piece.EMPTY) {
                    promotion = $"(Promote to {PieceParser.ToReadable((Piece)move.promotion)})";
                }

                Console.WriteLine($"{BoardPosition.ReadablePosition(move.fromPosition)} -> {BoardPosition.ReadablePosition(move.targetPosition)} {promotion}");
                Console.WriteLine($"{ChessOutput.AsciiBoard(copy)}");
            }
            Console.WriteLine(" */\n" +
                $"var board = BoardFactory.LoadBoardFromFen(\"{fen}\");\n\n" +
                $"var moves = board.GetMoves();");

            copy.UndoAll();
            while(copy.CanRedo()) {
                var move = copy.Redo().move;

                var promotion = "EMPTY";
                switch ((Piece)move.promotion) {
                    case Piece.QUEEN:
                        promotion = "QUEEN";
                        break;
                    case Piece.ROOK:
                        promotion = "ROOK";
                        break;
                    case Piece.BISHOP:
                        promotion = "BISHOP";
                        break;
                    case Piece.KNIGHT:
                        promotion = "Knight";
                        break;
                };

                Console.WriteLine($"board.MakeMove(BoardStateOffset.{BoardPosition.ReadablePosition(move.fromPosition).ToUpper()},BoardStateOffset.{BoardPosition.ReadablePosition(move.targetPosition).ToUpper()}, Piece.{promotion});");
            }
        }
    }
}
