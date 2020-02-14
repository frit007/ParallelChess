using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChessApi.Models;
using ChessApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParallelChess;
using Microsoft.EntityFrameworkCore;

namespace ChessApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayAIController : ControllerBase
    {
        private readonly ChessContext context;
        private readonly ChessService chessService;
        public PlayAIController(ChessContext context) {
            this.context = context;
            this.chessService = new ChessService();
        }

        public class GameProgress {
            public ChessState State { get; set; }
            public int GameId { get; set; }
        }

        [HttpPost("StartGame")]
        public GameProgress PostStartGame() {
            var game = context.Game.Add(new Game() {
                Name = "frit",
                Opponent = "AI"
            });
            context.SaveChanges();

            var board = Chess.LoadBoardFromFen();

            var progress = new GameProgress() {
                GameId = game.Entity.Id,
                State = chessService.BoardToState(board),
            };

            return progress;
        }

        [HttpGet("View/{id}")]
        public GameProgress View(int id) {
            var game = context.Game.Include(game => game.Moves).Where(game => game.Id == id).First();
            
            var board = chessService.ReplayMoves(game.Moves.ToList());

            return new GameProgress() {
                GameId = game.Id,
                State = chessService.BoardToState(board),
            };
        }

        public class PlayDTO {
            public string SAN { get; set; }
        }

        [HttpPost("Play/{id}")]
        public async Task<GameProgress> PlayMove(PlayDTO play, int id) {
            var game = context.Game.Include(game => game.Moves).Where(game => game.Id == id).First();

            var board = chessService.ReplayMoves(game.Moves.ToList());

            var boardState = chessService.BoardToState(board);
            
            var move = Chess.MakeMove(board, play.SAN);

            if (boardState.whiteWins || boardState.isDraw || boardState.blackWins) {
                var moveRow = new Models.Move() {
                    SAN = play.SAN,
                    Game = game,
                    CreatedAt = DateTime.Now
                };
                context.Move.Add(moveRow);

                context.SaveChanges();
                return new GameProgress() {
                    GameId = id,
                    State = boardState
                };
            }

            var aiMove = await chessService.GetAiMove(board);

            var aiMoveSan = board.StandardAlgebraicNotation(aiMove);

            BoardHelper.MakeMove(board, aiMove);

            if(MoveHelper.isValidMove(move)) {
                var moveRow = new Models.Move() {
                    SAN = play.SAN,
                    Game = game,
                    CreatedAt = DateTime.Now
                };

                var aiMoveRow = new Models.Move() {
                    SAN = aiMoveSan,
                    Game = game,
                    CreatedAt = DateTime.Now
                };

                context.Move.Add(moveRow);

                context.Move.Add(aiMoveRow);

                context.SaveChanges();
            }
            
            return new GameProgress() {
                GameId = id,
                State = chessService.BoardToState(board)
            };
        }



    }
}