using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParallelChess {

    // wrapper around move to include san
    public class ChessMove {
        public string san { get; set; }
        public Move move { get; set; }
    }

    // chess is supposed to be an easy to use interface which is less performant than the core game, but handles the chess history necessary for 3 fold repetition
    public class ChessGame {
        

        public Stack<ChessMove> previousMoves { get; private set; } = new Stack<ChessMove>();
        public Stack<ChessMove> nextMoves { get; private set; } = new Stack<ChessMove>();
        public Board board { get; private set; } = BoardFactory.LoadBoardFromFen();
        public string FEN { get { return ChessOutput.BoardToFen(board, TurnCounter); } }

        // store the turnCounter in Chess since it doesn't affect any chess rules and is only used by humans
        // which means it is wasteful during simulation
        public int TurnCounter = 1;

        // portable game notation
        public PGN pgn = new PGN();

        private ChessGame() {
            // hide the default chess constructor, you have to use a factory method
            // - LoadGame 
            // - StartGame
            // - ContinueFromFen
        }

        public static ChessGame StartGame() {
            return new ChessGame();
        }

        // continue from FEN(Forsyth–Edwards Notation) position
        public static ChessGame ContinueFromFEN(string fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1") {
            var parts = fen.Split();

            var board = BoardFactory.LoadBoardFromFen(out int turnCounter, fen);
            
            var game = new ChessGame() {
                board = board
            };

            game.TurnCounter = turnCounter;

            return game;
        }

        public static ChessGame LoadGame(IEnumerable<String> SANlist, string fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1") {
            var game = ContinueFromFEN(fen);

            foreach (var san in SANlist) {
                game.Move(san);
            }

            return game;
        }

        // loads PGN(Portable Game Notation)
        public static ChessGame LoadPGN(string pgn) {
            return PGNParser.parse(pgn);
        }

        public ChessMove FindMoveFromSan(string SAN) {
            var moves = Moves();
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

        public List<ChessMove> Moves() {
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
            




            board.Move(move);

            if (board.IsWhiteTurnBool) {
                // increment fullMoveClock after blacks turn
                TurnCounter++;
            }

            previousMoves.Push(chessMove);
            if (clearNextMoves) {
                // unless explictly specified clear the next moves so nobody accidentally tries redo a move that doesn't make sense any more.
                nextMoves.Clear();
            }

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

                // decrement the turn counter if it is blacks turn now(the turn counter is incremented after whites turn)
                if(!board.IsWhiteTurnBool) {
                    TurnCounter--;
                }

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


        public ChessGame Copy() {

            return new ChessGame() {
                board = board.CreateCopy(),
                // we have to reverse the stack, because when copying the stack this way it reverses the order.
                previousMoves = Helper.CloneStack(previousMoves),
                nextMoves = Helper.CloneStack(nextMoves),
            };
        }

        public Dictionary<ulong, int> RepeatedPositions() {
            var repeated = board.RepeatedPositions(previousMoves.Select(move => move.move));

            return repeated;
        }

        public HashSet<ulong> AboutToTiePositions() {

            return new HashSet<ulong>(RepeatedPositions()
                // if a position has occurred more than twice then it is about to tie
                // when a position occurs three times it is a time.
                .Where(position => position.Value >= 2)
                .Select(position => position.Key));


        }

        public HashSet<ulong> AboutToTieDirect() {
            HashSet<ulong> aboutToTieHashes = new HashSet<ulong>();

            foreach (var position in RepeatedPositions()) {
                if (position.Value >= 2) {
                    aboutToTieHashes.Add(position.Key);
                }
            }

            return aboutToTieHashes;
        }


    }


}
