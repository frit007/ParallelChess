using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChessApi.Services {
    public class PieceOption {
        public int column { get; set; }
        public int row { get; set; }
        public bool isCastle { get; set; }
        public bool isEnpassant { get; set; }
        // use san(standard algebraic notation) to uniquely identify a move
        public string san { get; set; }
    }

    public class ChessPiece {
        public string piece { get; set; }
        public List<PieceOption> options { get; set; } = new List<PieceOption>();
        public int column { get; set; }
        public int row { get; set; }
        public bool isWhite {get;set;}
    }

    public class ChessState {
        public List<ChessPiece> pieces { get; set; } = new List<ChessPiece>();
    }
}
