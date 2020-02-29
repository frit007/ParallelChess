// FightC.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include <iostream>
#include "Board.h"
#include "BoardFactory.h"
#include "ChessOutput.h"
#include <chrono>



Board board = BoardFactory::LoadBoardFromFen("rnbqkb1r/1p3ppp/p2p1n2/4p3/3NP3/2N5/PPP2PPP/R1BQKB1R w KQkq - 1 6");
std::vector<Move>* moves = new std::vector<Move>();

void performanceTest(int i) {
    std::cout << "Test " << i << std::endl;
    std::chrono::steady_clock::time_point begin = std::chrono::steady_clock::now();
    for (int i = 0; i < 1000000; i++) {
        moves->clear();
        board.GetMoves(moves);
        for (auto move : *moves) {

            if (board.IsLegalMove(move)) {
                board.Play(move);
                board.UndoMove(move);
            }
        }
    }
    std::chrono::steady_clock::time_point end = std::chrono::steady_clock::now();
    std::cout << "Time difference = " << std::chrono::duration_cast<std::chrono::milliseconds> (end - begin).count() << "[milli s]" << std::endl;
}

int main()
{
    for (size_t i = 0; i < 10; i++) {
        performanceTest(i);
    }
    delete moves;
    delete[] board.bytes;
}



//Board board = BoardFactory::LoadBoardFromFen();
//
////auto move = board.FindMove(E2, E4);
//
//board.Play(E2, E4);
//std::cout << ChessOutput::AsciiBoard(board) << std::endl;
//
//board.Play(E7, E5);
//std::cout << ChessOutput::AsciiBoard(board) << std::endl;
//
//board.Play(D1, H5);
//std::cout << ChessOutput::AsciiBoard(board) << std::endl;
//
//board.Play(D8, G5);
//std::cout << ChessOutput::AsciiBoard(board) << std::endl;
//
//board.Play(H5, G5);
//std::cout << ChessOutput::AsciiBoard(board) << std::endl;

// Run program: Ctrl + F5 or Debug > Start Without Debugging menu
// Debug program: F5 or Debug > Start Debugging menu

// Tips for Getting Started: 
//   1. Use the Solution Explorer window to add/manage files
//   2. Use the Team Explorer window to connect to source control
//   3. Use the Output window to see build output and other messages
//   4. Use the Error List window to view errors
//   5. Go to Project > Add New Item to create new code files, or Project > Add Existing Item to add existing code files to the project
//   6. In the future, to open this project again, go to File > Open > Project and select the .sln file
