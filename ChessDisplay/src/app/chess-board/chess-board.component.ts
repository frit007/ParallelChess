import { Component, OnInit, Output, EventEmitter, Input } from "@angular/core";
import { Piece, ChessState, PieceOption } from "../play-ai/play-ai.service";

@Component({
  selector: "chess-board",
  templateUrl: "./chess-board.component.html",
  styleUrls: ["./chess-board.component.less"]
})
export class ChessBoardComponent implements OnInit {
  constructor() {}

  ROW_OFFSET = 8;

  board = [];

  // pieces = [];

  @Input() pieces: Piece[] = [];

  @Output() moveMade = new EventEmitter();

  @Input()
  set state(newState: ChessState) {
    if (newState) {
      this.pieces = newState.pieces;
      console.log("new pieces", this.pieces);
    }
  }

  selectedPiece = null;
  dragHighlightSquare = { column: 0, row: 0 };
  // dropTargets = [];

  // used for displaying the data
  // reordering the board is needed to display information
  private _displayBoard = new Array(64);

  ngOnInit() {
    this.board = new Array(64);

    let isWhite = false;
    for (let i = 0; i < 64; i++) {
      let showText = false;
      if (i % 8 == 0) {
        isWhite = !isWhite;
        showText = true;
      }

      var row = Math.floor(i / 8);
      var column = i - row * 8;

      this.board[i] = {
        background: isWhite ? "white-tile" : "black-tile",
        showText: row == 0 || column == 0,
        // showText: true,
        location: this.readablePosition(i)
      };
      isWhite = !isWhite;
    }

    this._displayBoard = [...this.board];

    // this.pieces = [
    //   {
    //     piece: "Queen",
    //     column: 1,
    //     row: 2,
    //     image: "assets/images/pieces/queen.svg",
    //     options: [
    //       { row: 1, column: 0 },
    //       { row: 1, column: 1 },
    //       { row: 2, column: 2 },
    //       { row: 3, column: 3 },
    //       { row: 4, column: 4 },
    //       { row: 5, column: 5 },
    //       { row: 6, column: 6 },
    //       { row: 7, column: 7 },
    //       { row: 0, column: 1 }
    //     ]
    //   }
    // ];
  }

  public readablePosition(numericPosition: number) {
    let row = Math.floor(numericPosition / this.ROW_OFFSET);
    let column = numericPosition - row * this.ROW_OFFSET;
    let position =
      String.fromCharCode("a".charCodeAt(0) + column) + (row + 1).toString();
    return position;
  }
  get tiles() {
    for (let i = 0; i < 64; i++) {
      var row = Math.floor(i / 8);
      var column = i - row * 8;
      let targetRow = 7 - row;
      this._displayBoard[targetRow * 8 + column] = this.board[i];
    }
    return this._displayBoard;
  }

  onPieceDragStart(piece, event) {
    this.dragHighlightSquare.column = piece.column;
    this.dragHighlightSquare.row = piece.row;

    this.selectedPiece = piece;
    // console.log("start", piece, event);
    // console.log(this.selectedPiece.options);
    // console.log(this);

    // var crt = event.srcElement.cloneNode(true);

    // place the image outside the screen to
    // crt.style.position = "absolute";
    // crt.style.display = "block";
    // crt.style.left = "-80000px";

    // we need to create an actual element so we can style it
    // let e = document.body.appendChild(crt);
    let bounding = event.srcElement.getBoundingClientRect();

    event.dataTransfer.setDragImage(
      event.srcElement,
      bounding.width / 2,
      bounding.height / 2
    );

    // setTimeout(() => {
    //   // what
    //   e.parentNode.removeChild(e);
    // }, 100);
  }

  onPieceDragStop(piece, event) {
    this.selectedPiece = null;
    // console.log("stop", piece, event);
  }

  onDropPiece(event, droptarget: PieceOption) {
    // console.log("dropped it!");
    this.selectedPiece.column = droptarget.column;
    this.selectedPiece.row = droptarget.row;
    // console.log(droptarget);
    this.moveMade.emit(droptarget.san);
  }

  allowDrop(ev, dropTarget) {
    this.dragHighlightSquare.column = dropTarget.column;
    this.dragHighlightSquare.row = dropTarget.row;

    ev.preventDefault();
  }

  onDragoverHighlightTile($event, position) {
    var row = Math.floor(position / 8);
    var column = position - row * 8;
    let targetRow = 7 - row;
    this.dragHighlightSquare.column = column;
    this.dragHighlightSquare.row = targetRow;
  }
}
