using Newtonsoft.Json;
using ParallelChess;
using ParallelChess.MinMax;
using System;
using System.Threading;

namespace ParallelChessWebassembly {

    public class GlobalChess {
        static ChessGame game = ChessGame.StartGame();
        static bool aiWorking = false;

        static bool startedThread = false;
        static Thread workerThread;

        public static void StartWorker() {
            workerThread = new Thread(AiWorker);

            workerThread.Start();
        }

        private static void AiWorker() {
            while (true) {
                if(aiWorking) {
                    var ai = new MinMaxAI();

                    // do actions on  virtual board for safety
                    var virtualBoard = game.Copy();

                    var aiMoves = ai.MinMaxList(virtualBoard.GetBoard(), 5, virtualBoard.AboutToTiePositions());


                    // trying alternative ways of iteration moves
                    foreach (var move in aiMoves) {
                        Console.WriteLine(game.board.StandardAlgebraicNotation(move.move));
                        game.Move(move.move);
                        break;
                    }

                    aiWorking = false;
                }

                Thread.Sleep(100);
            }
        }
        

        public static bool IsWorking() {
            return aiWorking;
        }

        public static void ContinueFromFen(string fen) {
            game = ChessGame.ContinueFromFEN(fen);
        }

        public static string GetFEN() {
            return game.FEN;
        }

        public static void StartGame() {
            game = ChessGame.StartGame();
        }

        public static String GetState() {
            var state = CoordinateOutput.GameToState(game);
            return JsonConvert.SerializeObject(state);
        }

        public static void PlayMove(string san) {
            game.Move(san);
        }

        public static void AiPlayMove() {
            aiWorking = true;
        }


        public static void PlayMoveAgainstAI(string san) {
            game.Move(san);

            if (game.Winner() != Winner.NONE) {
                return;
            }

            AiPlayMove();
        }

        public static void Undo() {
            game.Undo();
        }

        public static void Redo() {
            game.Redo();
        }
    }
}
