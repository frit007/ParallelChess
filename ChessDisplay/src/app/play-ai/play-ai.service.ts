import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable, pipe } from "rxjs";
import { map } from "rxjs/operators";

export interface PieceOption {
  column: number;
  row: number;
  isCastle: number;
  isEnpassant: number;
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

@Injectable({ providedIn: "root" })
export class PlayAIService {
  constructor(private http: HttpClient) {}

  startGame(difficulty: number): Observable<GameProgress> {
    return this.http
      .post<GameProgress>("api/PlayAi/StartGame", {
        difficulty: difficulty || 5
      })
      .pipe<GameProgress>(
        map(progress => {
          this.attachImageToChessState(progress.state);
          return progress;
        })
      );
    // return
    //   .pipe(progress => {progress.});
  }
  playMove(gameId, moveSAN: string) {
    return this.http
      .post<GameProgress>(`api/PlayAI/Play/${gameId}`, {
        san: moveSAN
      })
      .pipe<GameProgress>(
        map(progress => {
          this.attachImageToChessState(progress.state);
          return progress;
        })
      );
  }

  attachImageToChessState(state: ChessState) {
    state.pieces.forEach(piece => (piece.image = this.pieceImage(piece)));
  }

  pieceImage(piece: Piece) {
    switch (piece.piece) {
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

  loadGame() {}
}
