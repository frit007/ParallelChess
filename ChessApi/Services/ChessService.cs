using ChessApi.Config;
using Microsoft.Extensions.Configuration;
using ParallelChess;
using ParallelChess.MinMax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChessApi.Services {
    public class ChessService {
        ChessConfig chessConfig;
        public ChessService(IConfiguration config) {
            //this.config = config;
            chessConfig = new ChessConfig();
            config.GetSection("Chess").Bind(chessConfig);
        }


        public ChessGame ReplayMoves(List<ChessApi.Models.Move> moves) {
            return ChessGame.LoadGame(moves.Select(move => move.SAN));
        }

        public async Task<Move> GetAiMove(int diffculty, ChessGame game) {
            var ai = new AIWorkerManager();

            ai.spawnWorkers(chessConfig.Threads);

            await ai.analyzeBoard(game.GetBoard(), diffculty, new Stack<Move>());

            var move = ai.GetBestMove();

            ai.killWorkers();

            return move.move;
        }
    }
}
