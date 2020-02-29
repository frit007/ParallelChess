using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ParallelChess {
    // a simple PGN parser to handle the output websites like lichess or chess.com generate
    // it does not handle comments or variations of the game.
    public class PGNParser {

        public static ChessGame parse(string pgnString) {
            var pgn = new PGN();
            var lines = pgnString.Split("\n");
            ChessGame game = null;

            bool headerSection = true;
            var body = "";
            foreach (var line in lines) {
                if(line.Trim() == "") {
                    if(headerSection == false) {
                        return game;
                    }
                    headerSection = false;
                    game = ChessGame.ContinueFromFEN(pgn.FEN);
                    game.pgn = pgn;
                    continue;
                }
                if(headerSection) {
                    parseHeader(line, pgn);
                } else {
                    body += line + "\n";
                }
            }

            parseBody(body, game);


            return game;
        }

        public static String removeComments(string body) {
            int startPosition = -1;
            while((startPosition = body.IndexOf("{")) != -1) {
                int endPosition = body.IndexOf("}", startPosition);
                body = body.Remove(startPosition, endPosition - startPosition + 1);
            }
            return body;
        }

        // this parsing is too simplistic to catch all the nuances of proper PGN, but it works for simple examples, 
        // and it might even get complex examples correct(but it shouldn't relied upon)
        public static void parseBody(string body, ChessGame game) {
            body = removeComments(body);
            body = body.Replace("\n", " ");
            body = body.Replace("\r", " ");
            var parts = body.Split(" ");
            var moves = game.Moves();
            foreach (var part in parts) {
                if (part.Contains(".")) {
                    continue;
                }
                var move = moves.Find(move => move.san == part.Trim());
                if(move != null) {
                    game.Move(move.move);
                    moves = game.Moves();
                }
            }
        }

        public static void parseHeader(string line, PGN pgn) {
            var parsed = parseHeaderFormat(line);
            switch (parsed.key) {
                case "Event":
                    pgn.Event = parsed.value;
                    break;
                case "Date":
                    pgn.Date = parsed.value;
                    break;
                case "Site":
                    pgn.Site = parsed.value;
                    break;
                case "FEN":
                    pgn.FEN = parsed.value;
                    break;
                case "Round":
                    pgn.Round = parsed.value;
                    break;
                case "White":
                    pgn.White = parsed.value;
                    break;
                case "Black":
                    pgn.Black = parsed.value;
                    break;
                case "Result":
                    pgn.Result = parsed.value;
                    break;
                default:
                    break;
            }
        }

        public static (string key, string value) parseHeaderFormat(string line) {
            // trim line for safety
            line = line.Trim();

            if (line[0] != '[') {
                // unrecognized header
                return ("","");
            }
            // remove the beginning and trailing []
            //line = line.Substring(1, line.Length - 2);

            int firstQuote = line.IndexOf("\"");
            int secondQuote = line.IndexOf("\"", firstQuote + 1);

            if (firstQuote == -1 || secondQuote == -1) {
                // unrecognizedHeader
                return ("", "");
            }
            // start from one to skip first "["
            // subtract one to avoid the starting quote
            string key = line.Substring(1, firstQuote -1).Trim();
            // add 1 to skip starting quote 
            // subtract 1 to avoid closing quote
            string value = line.Substring(firstQuote + 1, secondQuote - firstQuote - 1 );
            return (key, value);
        }

        public static List<ChessGame> ParseFile(string path) {
            List<ChessGame> games = new List<ChessGame>();

            System.IO.StreamReader file = new System.IO.StreamReader(path);

            string line;
            bool isHeaderSection = true;
            StringBuilder pgn = new StringBuilder();

            string pgnString = "";
            while((line = file.ReadLine()) != null) {
                pgn.Append(line).Append("\n");
                if(line.Trim() == "") {
                    if(isHeaderSection) {
                        if(pgn.Length != 0) {
                            // the first empty line is used to indicate the end of the header section, the next empty section is used to indicate the end of the game
                            isHeaderSection = false;
                        }
                    } else {
                        try {
                            isHeaderSection = true;
                            pgnString = pgn.ToString();
                            pgn.Clear();
                            games.Add(parse(pgnString));
                            //Console.WriteLine(games.Count);
                        } catch(Exception e) {
                            Console.WriteLine("Could not parse");
                            Console.WriteLine(pgnString);
                            Console.WriteLine(e.Message);
                            Console.WriteLine(e.StackTrace);
                        }
                    }
                }
            }

            return games;
        }
    }
}
