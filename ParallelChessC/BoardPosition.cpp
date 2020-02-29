#include "pch.h"
#include <algorithm>
#include <string>
#include <cctype>

#include "BoardPosition.h";
#include "BoardStateOffset.h";


// position is expected be in the format "a1" or "b5"
// converts it to a number from 0 to 119, which is the number system used internally
// examples
// a1 -> 0
// a3 -> 2
// h8 -> 119
int BoardPosition::ArrayPosition(std::string readablePosition) {
    if (readablePosition.length() != 2) {
        // if the string is not 2 chracters long then this method might crash.
        return 0;
    }
    // string to lower case readablePosition.ToLower()
    // WARNING this solution only works for ASCII characters. otherwise this might result in UB acording to SO.
    std::transform(readablePosition.begin(), readablePosition.end(), readablePosition.begin(), std::tolower);

    // abuse the ascii system by converting the row to a number from 0 to 7
    // subtract 'a' because then the number will start from 0
    int algebraicPosition = (readablePosition[0] - 'a');

    // add the column to the positon, subtract one because
    // multiply by the rowoffset to get the correct row.
    algebraicPosition += (readablePosition[1]) * ROW_OFFSET;

    return algebraicPosition;
}

// convert from internal number format to a readble form
// examples
// 0 -> a1
// 2 -> a3
// 119 -> h8
std::string BoardPosition::ReadablePosition(int arrayPosition) {
    int row = arrayPosition / ROW_OFFSET;
    int column = arrayPosition - (row * ROW_OFFSET);
    std::string move = "  ";
    move[0] = 'a' + column;
    // convert number to row
    move[1] = '1' + row;
    return move;
}

bool BoardPosition::IsValidPosition(int position) {
    // ------------0x88-------------
    // 0x88 is method used checking if location is out of bound very fast
    // it is done by creating an array and leaving every second row blank.
    // We can then check if a position is in a invalid row by anding a position with 0x88
    // for example if you tried to move one right from H3(39) + 1 = 40. 40 is a invalid position 
    // which can be caught by checking 0x88 & 40 = 8. Any invalid position will result in a non zero value
    // the potential disadvantage to this approach is that the boardState is larger, which will take longer to copy and 
    // might mean that the array is no longer cached in CPU cache.
    return (0x88 & position) == 0;
}

int BoardPosition::RelativePosition(int position, int relativeColumn, int relativeRow) {
    return position + relativeRow * ROW_OFFSET + relativeColumn;
}

int BoardPosition::PositionRow(int position) {
    return position / ROW_OFFSET;
}

int BoardPosition::PositionColumn(int position) {
    return position - (PositionRow(position) * ROW_OFFSET);
}



Coordinate BoardPosition::x88PositionToCoordinate(int x88Position) {
    int row = x88Position / ROW_OFFSET;
    int column = x88Position - (row * ROW_OFFSET);

    return {
        row = row,
        column = column
    };
}