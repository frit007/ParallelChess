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
}

export interface GameProgress {
  state: ChessState;
  gameId: number;
}

@Injectable({ providedIn: "root" })
export class PlayAIService {
  constructor(private http: HttpClient) {}

  startGame(): Observable<GameProgress> {
    return this.http
      .post<GameProgress>("api/PlayAi/StartGame", {})
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
    switch (piece.piece.toLowerCase()) {
      case "r":
        return "assets/images/pieces/rook.svg";
      case "k":
        return "assets/images/pieces/king.svg";
      case "p":
        return "assets/images/pieces/pawn.svg";
      case "n":
        return "assets/images/pieces/knight.svg";
      case "q":
        return "assets/images/pieces/queen.svg";
      case "b":
        return "assets/images/pieces/bishop.svg";
    }
  }

  loadGame() {}
}
