using System;

namespace ParallelChess {
    class Program {
        static void Main() {
            Console.WriteLine(CastlingHelper.castleLookup[BoardStateOffset.A1]);
            //BoardState board = Chess.LoadBoardFromFen();

            BoardState board = Chess.LoadBoardFromFen("4k3/4q3/8/8/8/8/2N5/3QK3 w - - 0 1");

            //Chess.MakeMove(board, BoardStateOffset.B8, BoardStateOffset.B7);

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
