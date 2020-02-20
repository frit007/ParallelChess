using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace ParallelChess {
    public static class BoardPosition {



        public const int A_COLUMN = 0;
        public const int B_COLUMN = 1;
        public const int C_COLUMN = 2;
        public const int D_COLUMN = 3;
        public const int E_COLUMN = 4;
        public const int F_COLUMN = 5;
        public const int G_COLUMN = 6;
        public const int H_COLUMN = 7;

        // position is expected be in the format "a1" or "b5"
        // converts it to a number from 0 to 119, which is the number system used internally
        // examples
        // a1 -> 0
        // a3 -> 2
        // h8 -> 119
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ArrayPosition(string readablePosition) {
            readablePosition = readablePosition.ToLower();

            // abuse the ascii system by converting the row to a number from 0 to 7
            // subtract 'a' because then the number will start from 0
            int algebraicPosition = (readablePosition[0] - 'a');

            // add the column to the positon, subtract one because
            // multiply by the rowoffset to get the correct row.
            algebraicPosition += (int.Parse(readablePosition.Substring(1, 1)) - 1) * BoardStateOffset.ROW_OFFSET;

            return algebraicPosition;
        }

        // convert from internal number format to a readble form
        // examples
        // 0 -> a1
        // 2 -> a3
        // 119 -> h8
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ReadablePosition(int arrayPosition) {
            int row = arrayPosition / BoardStateOffset.ROW_OFFSET;
            int column = arrayPosition - (row * BoardStateOffset.ROW_OFFSET);
            string move = Convert.ToChar('a' + (column)) + (row + 1).ToString();
            return move;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsValidPosition(int position) {
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int RelativePosition(int position, int relativeColumn, int relativeRow) {
            return position + relativeRow * BoardStateOffset.ROW_OFFSET + relativeColumn;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int PositionRow(int position) {
            return position / BoardStateOffset.ROW_OFFSET;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int PositionColumn(int position) {
            return position - (PositionRow(position) * BoardStateOffset.ROW_OFFSET);
        }
    }
}
