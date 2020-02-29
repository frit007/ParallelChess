#pragma once

#include<string>

struct Coordinate {
public:
    int row;
    int column;
};


class BoardPosition {

public:
    static const int A_COLUMN = 0;
    static const int B_COLUMN = 1;
    static const int C_COLUMN = 2;
    static const int D_COLUMN = 3;
    static const int E_COLUMN = 4;
    static const int F_COLUMN = 5;
    static const int G_COLUMN = 6;
    static const int H_COLUMN = 7;

    // position is expected be in the format "a1" or "b5"
    // converts it to a number from 0 to 119, which is the number system used internally
    // examples
    // a1 -> 0
    // a3 -> 2
    // h8 -> 119
    static int ArrayPosition(std::string readablePosition);

    // convert from internal number format to a readble form
    // examples
    // 0 -> a1
    // 2 -> a3
    // 119 -> h8
    static std::string ReadablePosition(int arrayPosition);

    // ------------0x88-------------
    // 0x88 is method used checking if location is out of bound very fast
    // it is done by creating an array and leaving every second row blank.
    // We can then check if a position is in a invalid row by anding a position with 0x88
    // for example if you tried to move one right from H3(39) + 1 = 40. 40 is a invalid position 
    // which can be caught by checking 0x88 & 40 = 8. Any invalid position will result in a non zero value
    // the potential disadvantage to this approach is that the boardState is larger, which will take longer to copy and 
    // might mean that the array is no longer cached in CPU cache.
    static bool IsValidPosition(int position);

    static int RelativePosition(int position, int relativeColumn, int relativeRow);

    static int PositionRow(int position);

    static int PositionColumn(int position);



    static Coordinate x88PositionToCoordinate(int x88Position);
};