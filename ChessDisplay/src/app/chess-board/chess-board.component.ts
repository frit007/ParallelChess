import { Component, OnInit, Output, EventEmitter, Input } from "@angular/core";
import { Piece, ChessState, PieceOption } from "../play-ai/play-ai.service";
import { identifierModuleUrl } from "@angular/compiler";

interface PieceWithId extends Piece {
  id: number;
}

@Component({
  selector: "chess-board",
  templateUrl: "./chess-board.component.html",
  styleUrls: ["./chess-board.component.less"]
})
export class ChessBoardComponent implements OnInit {
  constructor() {}

  ROW_OFFSET = 8;

  board = [];

  pieces: PieceWithId[] = [];

  @Output() moveMade = new EventEmitter();
  
  selectedPiece = null;

  dragHighlightSquare = { column: 0, row: 0 };

  dragging = false;
  // dropTargets = [];

  // used for displaying the data
  // reordering the board is needed to display information
  private _displayBoard = new Array(64);

  @Input()
  set state(newState: ChessState) {
    if (newState) {
      let newPieces = newState.pieces as PieceWithId[];

      this.assignIds(newPieces);

      this.pieces = newPieces;

      this.pieces
        .filter(piece => piece.piece == "n" && piece.column > 2)
        .forEach(piece => {
          console.log(piece.id);
        });
    }
  }

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
    this.preload(
      "/assets/images/possibilityDot.svg"
      )
  }

  
  preloadedImages = new Array();
  preload(...args: any[]):void {
    for (var i = 0; i < args.length; i++) {
      this.preloadedImages[i] = new Image();
      this.preloadedImages[i].src = args[i];
      console.log('loaded: ' + args[i]);
    }
  }

  findPieceOnPosition(pieces: PieceWithId[], column, row): PieceWithId {
    return pieces.find(piece => piece.row == row && piece.column == column);
  }

  // Try to get ids from last last position (pieces that have not moved)
  // assume that any piece not on a previous position
  assignIds(newPieces: PieceWithId[]) {
    let unassignedPieces = this.pieces.map(piece => piece);
    newPieces.forEach(piece => {
      let existingPiece = this.findPieceOnPosition(
        unassignedPieces,
        piece.column,
        piece.row
      );
      if (existingPiece && existingPiece.piece == piece.piece) {
        piece.id = existingPiece.id;
        unassignedPieces = unassignedPieces.filter(
          piece => piece != existingPiece
        );
      }
    });

    newPieces.forEach(piece => {
      if (!piece.id) {
        // console.log("no id yet", piece);
        //find the first unassigned piece of the same type
        let index = unassignedPieces.findIndex(
          unassignedPiece => piece.piece == unassignedPiece.piece
        );
        if (index != -1) {
          let unassignedPiece = unassignedPieces[index];
          // console.log(unassignedPiece, index);
          unassignedPieces.splice(index, 1);
          piece.id = unassignedPiece.id;
        }
      }
    });

    newPieces.forEach(piece => {
      if (!piece.id) {
        // if there is no ids then generate new ones
        piece.id = Math.random() * 100000000000000000;
      }
    });

    // for some reason we have to
    newPieces = newPieces.sort((a, b) => a.id - b.id);
  }

  trackById(i, obj) {
    return obj.id;
  }




  removeAllOptions() {
    for (const piece of this.pieces) {
      piece.options = [];
    }
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

    this.dragging = true;
    this.selectedPiece = piece;
    // console.log("start", piece, event);
    // console.log(this.selectedPiece.options);
    // console.log(this);

    // var crt = event.srcElement.cloneNode(true);

    // place the image outside the screen to
    // crt.style.position = "absolute";
    // crt.style.display = "block";
    // crt.style.left = "-800";
    // crt.style.cursor = "move";
    // crt.class = "piece-white";
    // crt.style.background = "red";
    //crt.style.filter =
    //"invert(73%) sepia(62%) saturate(420%) hue-rotate(22deg) brightness(94%) contrast(93%)";

    // we need to create an actual element so we can style it
    // let e = document.body.appendChild(crt);
    let bounding = event.srcElement.getBoundingClientRect();

    // target.style.cursor = "move"; // You can do this or use a css class to change the cursor
    event.dataTransfer.setDragImage(
      event.srcElement,
      bounding.width / 2,
      bounding.height / 2
    );
    // event.dataTransfer.dropEffect = "copy";
    // event.dataTransfer.effectAllowed = "copy"
    // setTimeout(() => {
    //   console.log(crt);
    //   // what
    //   e.parentNode.removeChild(e);
    // }, 100);
  }

  onPieceDragStop(piece, event) {
    // this.selectedPiece = null;
    this.dragging = false;
    // console.log("stop", piece, event);
  }

  makeMove(san) {
    this.applyMoveEffects(san);

    this.removeAllOptions();

    this.moveMade.emit(san);
  }

  findPieceFromSan(san) {
    return this.pieces.find(
      piece => piece.options.find(move => move.san == san) !== undefined
    );
  }

  removePiece(column, row, exceptPiece) {
    this.pieces = this.pieces.filter(
      piece =>
        piece.row != row || piece.column != column || piece == exceptPiece
      // console.log(piece.row != row)
    );
  }

  applyMoveEffects(san) {
    var piece = this.findPieceFromSan(san);
    var move = piece.options.find(move => move.san == san);

    piece.column = move.column;
    piece.row = move.row;
    if (move.isCastle) {
      if (move.column > 4) {
        // castling king side
        let rook = this.findPieceOnPosition(this.pieces, 7, piece.row);
        if (rook) {
          rook.column -= 2;
        }
      } else {
        // castling queen side
        let rook = this.findPieceOnPosition(this.pieces, 0, piece.row);
        if (rook) {
          rook.column += 3;
        }
      }
    }

    if (move.isEnpassant) {
      if (move.row > 4) {
        let enemyPawn = this.findPieceOnPosition(
          this.pieces,
          move.column,
          move.row - 1
        );
        this.pieces = this.pieces.filter(piece => piece != enemyPawn);
      } else {
        let enemyPawn = this.findPieceOnPosition(
          this.pieces,
          move.column,
          move.row + 1
        );
        this.pieces = this.pieces.filter(piece => piece != enemyPawn);
      }
    }
    this.removePiece(move.column, move.row, piece);
  }

  onDropPiece(event, droptarget: PieceOption) {
    this.makeMove(droptarget.san);
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
  
  onPieceDragOver(piece: PieceWithId, $event) {
    this.dragHighlightSquare.column = piece.column;
    this.dragHighlightSquare.row = piece.row;
  }

  onPieceMouseDown(piece: PieceWithId, $event) {
    
    this.selectedPiece = piece;
  }
}
