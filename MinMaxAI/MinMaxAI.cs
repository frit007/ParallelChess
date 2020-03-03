using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParallelChess.MinMax {
    // based on https://en.wikipedia.org/wiki/Alpha%E2%80%93beta_pruning
    // use alpha beta prunning to reduce the amount of notes searched, in best case scenario this can remove over half of the searched nodes


    struct EvaluatedPosition {
        public float score;
        public byte distanceToEndSearch;
        public byte depth;
    }

    struct MoveAndEvaluatedPosition {
        public Move move;
        public EvaluatedPosition evaluatedPosition;
        public bool hasEvaluatedMove;
    }

    public class MinMaxAI {
        //public static long movesEvaluated = 0;


        // To avoid creating too many lists and destroying them have a list ready for each depth layer which is then cleared
        
        public List<Move>[] layeredLists = new List<Move>[100];
        
        private ulong boardHash = 0;

        private Dictionary<ulong, EvaluatedPosition> moveScores = new Dictionary<ulong, EvaluatedPosition>();


        public MinMaxAI() {
            layeredLists = new List<Move>[100];
            for (int i = 0; i < layeredLists.Length; i++) {
                layeredLists[i] = new List<Move>();
            }
            boardHash = 0;
        }

        public List<EvaluatedMove> MinMaxList(Board board, int depth, HashSet<ulong> tiedPositions = null, bool maximizing = true, float min = float.MinValue, float max = float.MaxValue) {
            boardHash = HashBoard.hash(board);

            if(tiedPositions == null) {
                tiedPositions = new HashSet<ulong>();
            }

            List<EvaluatedMove> movePoints = new List<EvaluatedMove>();
            //var bestMove = MinMaxInternal(board, depth, maximizing, min, max);
            var bestMove = maximizing ? float.MinValue : float.MaxValue;

            var moveList = layeredLists[board.VirtualLevel];
            moveList.Clear();
            
            var moves = board.GetMoves(moveList);

            var winner = board.detectWinner(moves);


            foreach (var move in moves) {
                byte myTurn = board.IsWhiteTurn;
                boardHash = HashBoard.ApplyMove(board, move, boardHash);
                board.Move(move);

                board.VirtualLevel++;

                var attacked = board.Attacked(board.GetKingPosition(myTurn), myTurn);
                if (attacked) {
                    // if the king is under attack after making the move then it is not a valid move, in which case ignore the move
                    board.VirtualLevel--;
                    board.UndoMove(move);
                    boardHash = HashBoard.ApplyMove(board, move, boardHash);
                    continue;
                }
                var moveScore = MinMax(board, depth, tiedPositions, !maximizing, min, max);
                movePoints.Add(new EvaluatedMove() {
                    move = move,
                    score = moveScore,
                });

                board.VirtualLevel--;
                board.UndoMove(move);
                boardHash = HashBoard.ApplyMove(board, move, boardHash);

                if (maximizing) {
                    // optimize for player
                    if (moveScore > bestMove) {
                        bestMove = moveScore;
                    }
                    min = Math.Max(moveScore, min);
                } else {
                    if (moveScore < bestMove) {
                        bestMove = moveScore;
                    }
                    max = Math.Min(moveScore, max);
                }
            }

            // sort the moves in descending order
            //movePoints.Sort((a, b) => (a.score < b.score) ? 1 : -1);
            movePoints = movePoints.OrderBy(move => move.score).Reverse().ToList();

            return movePoints;
        }

        private MoveAndEvaluatedPosition GetMoveAndEvaluatedPositionFromMove(Move move) {
            EvaluatedPosition existingScore;
            if (moveScores.TryGetValue(boardHash, out existingScore)) {
                return new MoveAndEvaluatedPosition() {
                    move = move,
                    evaluatedPosition = existingScore,
                    hasEvaluatedMove = true
                };
            }
            return new MoveAndEvaluatedPosition() {
                //evaluatedPosition = null,
                move = move,
                hasEvaluatedMove = false,
            };
        }

        private float OrderMoveAndEvaluatedPositionFromMove(MoveAndEvaluatedPosition move) {
            if(move.hasEvaluatedMove) {
                return move.evaluatedPosition.score;
            }
            if(move.move.capturedPiece != 0) {
                return -1000000 + move.move.capturedPiece;
            }
            return -100000000;
        }

        public float MinMax(Board board, int depth, HashSet<ulong> tiedPositions, bool maximizing = true, float min = float.MinValue, float max = float.MaxValue) {
            // this function 
            boardHash = HashBoard.hash(board);
            return MinMaxInteral(board, depth, tiedPositions, maximizing, min, max);
        }
        protected float MinMaxInteral(Board board, int depth, HashSet<ulong> tiedPositions, bool maximizing = true, float min = float.MinValue, float max = float.MaxValue) {
            var optimizeForColor = maximizing ? 1 : 0;
            var minimizeForColor = optimizeForColor ^ 1;

            float bestMove = maximizing ? float.MinValue : float.MaxValue;

            if(tiedPositions.Contains(boardHash)) {
                // if the position is tied due to repetition then return 0;
                return 0;
            }

            var moveList = layeredLists[board.VirtualLevel];
            moveList.Clear();
            //var moves = Board.GetMoves(board, moveList);
            
            var moves = board.GetMoves(moveList);


            

            if (board.VirtualLevel >= depth) {
                // if we have reached max depth assign a score.
                var winner = board.detectWinner(moves);
                if ((winner == Winner.WINNER_WHITE || winner == Winner.WINNER_BLACK)) {
                    if (maximizing) {
                        // if a checkmate is found then no deeper moves matter since we are going to play that move
                        return float.MinValue + board.VirtualLevel;
                    } else {
                        return float.MaxValue - board.VirtualLevel;
                    }
                } else if (winner == Winner.DRAW) {
                    return 0;
                }

                float score= EvalBoard.evalBoard(board, moves);

                int isWhite = ((board.IsWhiteTurn ^ depth) & 1);
                
                if (isWhite != 1) {
                    // if black started the query then optimize for black
                    score *= -1;
                }
                return score;
            }

            // because detectWinner requires checking for valid moves, which is slow only do it for end nodes
            // for all other cases reimplement reimplement the logic locally
            if (board.hasInsufficientMaterialOrTimeLimit()) {
                return 0;
            }



            // hasValidMove is used to track if the player has a valid move they can play, 
            // if not this is used to declare a winner
            bool foundValidMove = false;
            foreach (MoveAndEvaluatedPosition moveAndEvluated in moves
                    .Select(GetMoveAndEvaluatedPositionFromMove)
                    .OrderByDescending(OrderMoveAndEvaluatedPositionFromMove)) {

                var move = moveAndEvluated.move;
                //foreach (var move in moves) { 
                byte myTurn = board.IsWhiteTurn;

                //var calculatedBeforeHash = HashBoard.hash(board);
                //var beforeHash = boardHash;
                boardHash = HashBoard.ApplyMove(board, move, boardHash);
                board.Move(move);
                //var expectedHash = HashBoard.hash(board);
                //if (expectedHash != boardHash) {
                //    Console.WriteLine("ARG");
                //    Console.WriteLine(calculatedBeforeHash);
                //    Console.WriteLine(beforeHash);
                //}

                board.VirtualLevel++;

                var attacked = board.Attacked(board.GetKingPosition(myTurn), myTurn);
                if (attacked) {
                    // if the king is under attack after making the move then it is not a valid move, in which case ignore the move
                    board.VirtualLevel--;
                    board.UndoMove(move);
                    boardHash = HashBoard.ApplyMove(board, move, boardHash);
                    continue;
                }
                foundValidMove = true;

                EvaluatedPosition existingScore = moveAndEvluated.evaluatedPosition;
                bool useCache = false;
                float moveScore = 0;
                int movesFromMaxDepth = depth - board.VirtualLevel;

                if (moveAndEvluated.hasEvaluatedMove) {
                    // subtract the difference between the 2 depths to give previous best moves a disadvantage
                    // because a previous move that was explored at less depth has less knowlegde than the current best
                    if (existingScore.distanceToEndSearch - depth + existingScore.depth >= movesFromMaxDepth) {
                        useCache = true;
                        moveScore = existingScore.score;
                    }
                }
                if (!useCache) {
                    moveScore = MinMax(board, depth, tiedPositions, !maximizing, min, max);

                    moveScores[boardHash] = new EvaluatedPosition() {
                        distanceToEndSearch = (byte)movesFromMaxDepth,
                        score = moveScore,
                        depth = (byte)depth,
                        //fen = board.simplifiedFEN,
                    };
                }

                board.VirtualLevel--;
                board.UndoMove(move);
                // undo previous hash
                boardHash = HashBoard.ApplyMove(board, move, boardHash);

                if (maximizing) {
                    // optimize for player
                    if (moveScore > bestMove) {
                        bestMove = moveScore;
                    }
                    min = Math.Max(moveScore, min);
                    if (min > max) {
                        return bestMove;
                    }
                } else {
                    if (moveScore < bestMove) {
                        bestMove = moveScore;
                    }
                    max = Math.Min(moveScore, max);
                    if (min > max) {
                        return bestMove;
                    }
                }
            }
            if (!foundValidMove) {
                if (maximizing) {
                    return float.MinValue + board.VirtualLevel;
                    // if a checkmate is found then no deeper moves matter since we are going to play that move
                } else {
                    return float.MaxValue - board.VirtualLevel;
                }
            }


            return bestMove;
        }
    }
}
