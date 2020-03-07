

export interface PieceOption {
    column: number;
    row: number;
    isCastle: boolean;
    isEnpassant: boolean;
    isPromotion: boolean;
    san: string;
  }
  
  export interface Piece {
    piece: string;
    options: PieceOption[];
    column: number;
    image: string;
    row: number;
    isWhite: boolean;
  }
  export interface ChessState {
    pieces: Piece[];
    blackWins: boolean;
    whiteWins: boolean;
    isDraw: boolean;
    fen: string;
  }
  
  export interface GameProgress {
    state: ChessState;
    gameId: number;
  }
  export function pieceImage(piece: Piece | string) {
    let pieceType = piece;
    if(typeof piece == "object") {
        pieceType = piece.piece
    }
    switch (pieceType) {
      case "R":
        return "assets/images/pieces/rook-white.svg";
      case "r":
        return "assets/images/pieces/rook-black.svg";
      case "K":
        return "assets/images/pieces/king-white.svg";
      case "k":
        return "assets/images/pieces/king-black.svg";
      case "P":
        return "assets/images/pieces/pawn-white.svg";
      case "p":
        return "assets/images/pieces/pawn-black.svg";
      case "N":
        return "assets/images/pieces/knight-white.svg";
      case "n":
        return "assets/images/pieces/knight-black.svg";
      case "Q":
        return "assets/images/pieces/queen-white.svg";
      case "q":
        return "assets/images/pieces/queen-black.svg";
      case "B":
        return "assets/images/pieces/bishop-white.svg";
      case "b":
        return "assets/images/pieces/bishop-black.svg";
    }
  }