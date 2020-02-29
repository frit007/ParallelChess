﻿using System;
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

            var chess = chessService.ReplayMoves(new List<Models.Move>());

            var progress = new GameProgress() {
                GameId = game.Entity.Id,
                State = CoordinateOutput.GameToState(chess),
            };

            return progress;
        }

        [HttpGet("View/{id}")]
        public GameProgress View(int id) {
            var game = context.Game.Include(game => game.Moves).Where(game => game.Id == id).First();
            
            var chess = chessService.ReplayMoves(game.Moves.ToList());

            return new GameProgress() {
                GameId = game.Id,
                State = CoordinateOutput.GameToState(chess),
            };
        }

        public class PlayDTO {
            public string SAN { get; set; }
        }

        [HttpPost("Play/{id}")]
        public async Task<GameProgress> PlayMove(PlayDTO play, int id) {
            var game = context.Game.Include(game => game.Moves).Where(game => game.Id == id).First();

            var chess = chessService.ReplayMoves(game.Moves.ToList());

            var move = chess.Move(play.SAN);

            var boardState = CoordinateOutput.GameToState(chess);

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

            var aiMove = await chessService.GetAiMove(chess);

            var aiMoveSan = chess.board.StandardAlgebraicNotation(aiMove);

            chess.Move(aiMove);

            if(MoveHelper.isValidMove(move.move)) {
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
                State = CoordinateOutput.GameToState(chess)
            };
        }



    }
}