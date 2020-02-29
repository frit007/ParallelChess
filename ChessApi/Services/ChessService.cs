using ParallelChess;
using ParallelChess.MinMax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChessApi.Services {
    public class ChessService {

        public ChessGame ReplayMoves(List<ChessApi.Models.Move> moves) {
            return ChessGame.LoadGame(moves.Select(move => move.SAN));
        }

        public async Task<Move> GetAiMove(ChessGame game) {
            var ai = new AIWorkerManager();

            ai.spawnWorkers(3);

            await ai.analyzeBoard(game.GetBoard(), 5, new Stack<Move>());

            var move = ai.GetBestMove();

            return move.move;
        }
    }
}
