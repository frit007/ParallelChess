import { Component, OnInit } from "@angular/core";
import { PlayAIService, ChessState } from "./play-ai.service";

@Component({
  selector: "play-ai",
  templateUrl: "./play-ai.component.html",
  styleUrls: ["./play-ai.component.less"]
})
export class PlayAiComponent implements OnInit {
  constructor(private playAi: PlayAIService) {}

  private state: ChessState;
  private gameId;

  ngOnInit() {
    this.startGame();
  }

  startGame() {
    this.playAi.startGame().subscribe(progress => {
      // console.log("state", state);
      this.state = progress.state;
      this.gameId = progress.gameId;
    });
  }

  madeMove(move) {
    console.log(move);
    this.playAi.playMove(this.gameId, move).subscribe(progress => {
      this.state = progress.state;
      this.gameId = progress.gameId;
    });
  }
}
