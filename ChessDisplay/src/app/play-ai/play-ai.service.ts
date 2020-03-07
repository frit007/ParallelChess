import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable, pipe } from "rxjs";
import { map, catchError } from "rxjs/operators";
import { GameProgress, ChessState, pieceImage } from '../helpers/chess-helper';


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
        }),
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
    state.pieces.forEach(piece => (piece.image = pieceImage(piece)));
  }


  loadGame() {}
}
