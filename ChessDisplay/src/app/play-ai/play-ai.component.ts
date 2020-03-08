import { Component, OnInit } from "@angular/core";
import { PlayAIService } from "./play-ai.service";
import { ActivatedRoute, Router } from '@angular/router';
import { ChessState } from '../helpers/chess-helper';

@Component({
  selector: "play-ai",
  templateUrl: "./play-ai.component.html",
  styleUrls: ["./play-ai.component.less"]
})
export class PlayAiComponent implements OnInit {
  constructor(
    private playAi: PlayAIService, 
    private route: ActivatedRoute,
    private router: Router
    ) {}

  state: ChessState;
  gameId;
  winningMessage;
  difficulty: number;
  autoQueen = localStorage.getItem("autoQueen") == "true";

  ngOnInit() {
    this.route.queryParams.subscribe(params => {
      this.startGame(+ params['difficulty']);
    })
    // this.autoQueen = ;
  }

  setAutoQueen(autoQueen) {
    console.log("set auto queen")
    this.autoQueen = autoQueen;
    localStorage.setItem("autoQueen", autoQueen)
  }

  startGame(difficulty:number) {
    this.difficulty = difficulty,
    this.playAi.startGame(this.difficulty)
    .subscribe(this.onNewState.bind(this));
  }

  onNewState(progress) {
    this.state = progress.state;
    this.gameId = progress.gameId;
    this.endOfGame(progress.state)
  }

  winningMessageFromState(state :ChessState) {
    if (this.state.whiteWins) {
      return "White won!";
    } else if (this.state.isDraw) {
      return "It is a draw!";
    } else if (this.state.blackWins) {
      return "Black won!";
    }
    return "";
  }

  endOfGame(state: ChessState) {
    this.winningMessage = this.winningMessageFromState(state);
  }

  rematch() {
    this.startGame(this.difficulty)
    this.winningMessage = "";
  }

  back() {
    this.router.navigate(["/play"]);
  }

  madeMove(move) {
    this.playAi
      .playMove(this.gameId, move)
      .subscribe((progress) => {
        if(progress.gameId == this.gameId) {
          this.onNewState(progress)
        }
      });
  }
}
