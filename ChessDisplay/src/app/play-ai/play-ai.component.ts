import { Component, OnInit } from "@angular/core";
import { PlayAIService, ChessState } from "./play-ai.service";
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: "play-ai",
  templateUrl: "./play-ai.component.html",
  styleUrls: ["./play-ai.component.less"]
})
export class PlayAiComponent implements OnInit {
  constructor(private playAi: PlayAIService, private route: ActivatedRoute) {}

  state: ChessState;
  gameId;
  winningMessage;

  difficulty: number;

  ngOnInit() {
    this.route.queryParams.subscribe(params => {
      this.difficulty = +params['difficulty'],
      this.playAi.startGame(this.difficulty).subscribe(this.onNewState.bind(this));
    })
  }

  startGame() {
    
  }

  onNewState(progress) {
    this.state = progress.state;
    this.gameId = progress.gameId;
    if (this.state.whiteWins) {
      this.winningMessage = "White won!";
    } else if (this.state.isDraw) {
      this.winningMessage = "It is a draw!";
    } else if (this.state.blackWins) {
      this.winningMessage = "Black won!";
    }
  }

  madeMove(move) {
    this.playAi
      .playMove(this.gameId, move)
      .subscribe(this.onNewState.bind(this));
  }
}
