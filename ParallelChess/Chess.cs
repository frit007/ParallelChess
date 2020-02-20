using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParallelChess {

    public class ChessMove {
        public string san { get; set; }
        public Move move { get; set; }
    }

    // chess is supposed to be an easy to use interface which is less performant than the core game, but handles the chess history necessary for 3 fold repetition
    public class Chess {

        public Stack<ChessMove> previousMoves { get; private set; } = new Stack<ChessMove>();
        public Stack<ChessMove> nextMoves { get; private set; } = new Stack<ChessMove>();
        public Board board { get; private set; } = BoardFactory.LoadBoardFromFen();
        public string FEN { get { return board.FEN; } }

        // portable game notation
        public PGN pgn = new PGN();

        private Chess() {
            // hide the default chess constructor, you have to use a factory method
            // - LoadGame 
            // - StartGame
            // - ContinueFromFen
        }

        public static Chess StartGame() {
            return new Chess();
        }

        // continue from FEN(Forsyth–Edwards Notation) position
        public static Chess ContinueFromFEN(string fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1") {
            return new Chess() {
                board = BoardFactory.LoadBoardFromFen(fen)
            };
        }

        public static Chess LoadGame(IEnumerable<String> SANlist, string fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1") {
            var game = ContinueFromFEN(fen);

            foreach (var san in SANlist) {
                game.Move(san);
            }

            return game;
        }

        // loads PGN(Portable Game Notation)
        public static Chess LoadPGN(string pgn) {
            return PGNParser.parse(pgn);
        }

        public ChessMove FindMoveFromSan(string SAN) {
            var moves = getMoves();
            foreach (var move in moves) {
                if(move.san == SAN) {
                    return move;
                }
            }
            throw new ArgumentException("Move could not be found");
        }

        public Winner Winner() {
            return board.detectWinner(board.GetMoves(),previousMoves.Select(chessMove => chessMove.move).Reverse());
        }

        public Stack<ChessMove> GetPreviousMoves() {
            return previousMoves;
        }

        public Board GetBoard() {
            return board;
        }

        public List<ChessMove> getMoves() {
            return board.GetMoves()
                .Where((move) => board.IsLegalMove(move))
                .Select((move) => new ChessMove() {
                    move = move,
                    san = board.StandardAlgebraicNotation(move)
                }).ToList();
        }

        public ChessMove Move(string san) {
            var move = FindMoveFromSan(san);

            return Move(move.move);
        }
        public ChessMove Move(int from, int to, Piece promotion = Piece.EMPTY) {
            var move = board.FindMove(from, to, promotion);

            return Move(move);
        }

        public ChessMove Move(Move move, bool clearNextMoves = true) {
            var san = board.StandardAlgebraicNotation(move);

            var chessMove = new ChessMove() {
                san = san,
                move = move
            };
            previousMoves.Push(chessMove);
            
            if(clearNextMoves) {
                // unless explictly specified clear the next moves so nobody accidentally tries redo a move that doesn't make sense any more.
                nextMoves.Clear();
            }

            board.Move(move);

            return chessMove;
        }

        public ChessMove Redo() {
            if(CanRedo()) {
                var move = nextMoves.Pop();
                Move(move.move, clearNextMoves: false);
                return move;
            }
            return null;
        }
        public void UndoAll() {
            while(CanUndo()) {
                Undo();
            }
        }
        public bool CanRedo() {
            return nextMoves.Count != 0;
        }
        public bool CanUndo() {
            return previousMoves.Count != 0;
        }
        // undo single move
        public bool Undo() {
            if(CanUndo()) {
                var move = previousMoves.Pop();
                board.UndoMove(move.move);
                nextMoves.Push(move);
                return true;
            }
            return false;
        }

        // undo turn undoes two moves
        public bool UndoTurn() {
            // undo twice
            Undo();
            return Undo();
        }

        public Stack<Move> moveHistory() {
            return new Stack<Move>(previousMoves.Select(move => move.move));
        }


        public Chess Copy() {

            return new Chess() {
                board = board.CreateCopy(),
                // we have to reverse the stack, because when copying the stack this way it reverses the order.
                previousMoves = Helper.CloneStack(previousMoves),
                nextMoves = Helper.CloneStack(nextMoves),
            };
        }



    }


}
