using System;

namespace ParallelChess {
    class Program {
        static void Main() {
            Console.WriteLine(CastlingHelper.castleLookup[BoardOffset.A1]);
            //byte[] board = Chess.LoadBoardFromFen();
            byte[] board = Chess.LoadBoardFromFen("1k6/6pp/8/8/8/8/6PP/1K6 w - - 0 1");
            Console.WriteLine(Chess.AsciiBoard(board));
            //Piece piece = Board.GetPiece(board, BoardOffset.H7);
            //Piece piece2 = Board.GetPiece(board, BoardOffset.H8);

            //if((Piece.PIECE_MASK & Piece.PAWN) == Piece.PAWN) {
            //    Console.WriteLine("IS IS A PAWN");
            //}

            //Console.WriteLine("Hello World!");
        }
    }
}
