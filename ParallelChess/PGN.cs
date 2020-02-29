using System;
using System.Collections.Generic;
using System.Text;


namespace ParallelChess {
    // based on http://en.chessbase.com/support-kb/content/details/1157/Tips_on_the_PGN_format
    // minimum implementation
    public class PGN {
        // name of the tournament, or other possible event
        public string Event = "none";
        // location the game was played
        public string Site = "?";
        // given in the YYYY.MM.DD format
        // if only the year is known by not the day and month then the unknowns are marked with question marks
        // example 2019.??.??
        public string Date = "????.??.??";
        public string Round = "?";
        // name of white player
        public string White = "Unkown";
        // name of black palyerTest
        public string Black = "Unkown";
        // possibilities 
        // - "1-0"
        // - "1/2-1/2"
        // - "0-1"
        // - "*"
        public string Result = "*";

        public string FEN = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
    }
}
