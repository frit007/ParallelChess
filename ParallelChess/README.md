# ChessGame
ChessGame is an easy to use interface which keeps track of the history necessary to check for winners.
```
// start a game
var game = ChessGame.StartGame();
// get all possible moves
var moves = game.Moves();
// play the move pawn to e4
game.Move("e4");
// check if anybody won
Winner winner = game.Winner();
```
## Continue an existing game
```
// get moves previously made from a database or anywhere you want
var SANmoves = List<String>();
// optionally specify a FEN (Forsyth-Edwards Notation) to continue from a specific position
var fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
var game = ChessGame.LoadGame(SANmoves, fen);
```
## load a PGN (Portable game notation)
```
var pgn = "load PGN from somewhere";
var game = ChessGame.LoadPGN(pgn);
```

# Board
Board is the internal minimal representation of the chess board. It relies on ChessGame to remember history used for the 3 fold repetition. 


Board should only be used if you need to do something efficiently, for example it is used by the minmax AI since it is a lot faster then ChessGame.
