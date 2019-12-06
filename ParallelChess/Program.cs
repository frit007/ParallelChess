﻿using System;

namespace ParallelChess {
    class Program {
        static void Main() {
            Console.WriteLine("Please enter a FEN");
            
            //BoardState board = Chess.LoadBoardFromFen("4k3/8/8/8/7q/3N4/8/3QK3 w - - 0 1");
            BoardState board = Chess.LoadBoardFromFen(Console.ReadLine());

            //Chess.MakeMove(board, BoardStateOffset.B8, BoardStateOffset.B7);

            Console.WriteLine(Chess.AsciiBoard(board));
        }
    }
}