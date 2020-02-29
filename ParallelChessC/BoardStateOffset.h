#pragma once
const int A1 = 0;
const int B1 = 1;
const int C1 = 2;
const int D1 = 3;
const int E1 = 4;
const int F1 = 5;
const int G1 = 6;
const int H1 = 7;

const int A2 = 16;
const int B2 = 17;
const int C2 = 18;
const int D2 = 19;
const int E2 = 20;
const int F2 = 21;
const int G2 = 22;
const int H2 = 23;

const int A3 = 32;
const int B3 = 33;
const int C3 = 34;
const int D3 = 35;
const int E3 = 36;
const int F3 = 37;
const int G3 = 38;
const int H3 = 39;

const int A4 = 48;
const int B4 = 49;
const int C4 = 50;
const int D4 = 51;
const int E4 = 52;
const int F4 = 53;
const int G4 = 54;
const int H4 = 55;

const int A5 = 64;
const int B5 = 65;
const int C5 = 66;
const int D5 = 67;
const int E5 = 68;
const int F5 = 69;
const int G5 = 70;
const int H5 = 71;

const int A6 = 80;
const int B6 = 81;
const int C6 = 82;
const int D6 = 83;
const int E6 = 84;
const int F6 = 85;
const int G6 = 86;
const int H6 = 87;

const int A7 = 96;
const int B7 = 97;
const int C7 = 98;
const int D7 = 99;
const int E7 = 100;
const int F7 = 101;
const int G7 = 102;
const int H7 = 103;

const int A8 = 112;
const int B8 = 113;
const int C8 = 114;
const int D8 = 115;
const int E8 = 116;
const int F8 = 117;
const int G8 = 118;
const int H8 = 119;

const int ROW_OFFSET = 16;

// -----------Ox88 wasted space------------
// every second row on the board is wasted since they are invalid positions.
// We can use this space by placing all the extra data we need in those empty spaces

// -------------- FULL Turn Counter -------------- 
// (DEPRECATED) Turn counter moved to Chess
// counter is increased after BLACKs turn 
// since we likely need more than a byte to store the game length we dedicate 2 bytes to this.
//public const int TURN_COUNTER_1_FROM_RIGHT = 9;
//public const int TURN_COUNTER_2_FROM_RIGHT = 8;

//public const int TURN_COUNTER_FROM_SHORT = TURN_COUNTER_1_FROM_RIGHT / 2;

const int CASTLING_OFFSET = 10;


// contains field that a pawn skipped over. 
// this means this field can be attacked by an enemy pawn
// this field should be reset when a move is made, because en passant is no longer possible
const int EN_PASSANT_FIELD_OFFSET = 11;

// --------------- STALEMATE ------------------
// after 50 turns without any piece being taken or pawns being moved the game is a stalemate
// we count half turns since it avoid having to keep track of who started the stalemate
const int HALF_TURN_COUNTER_OFFSET = 12;

// --------------- VIRTULIZATION LEVEL ---------------
// keep track of how many levels the game is being simulated
const int VIRTUAL_LEVEL_OFFSET = 13;

// -------------- HALF TURN --------------
// boolean true for WHITE and false for BLACK
const int IS_WHITE_TURN_OFFSET = 14;

// -------------- KING POSITIONS ----------------
// because we need to know where the king is we store that information
const int BLACK_KING_POSITION_OFFSET = 24;
const int WHITE_KING_POSITION_OFFSET = 25;




const int BOARD_STATE_SIZE = 120;