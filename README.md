# ParallelChess
The project was developped in visual studio 2019 using dotnet core 3.1

# Structure
The project is split into multiple parts 

## ParallelChess
ParallelChess is the core chess program which tries to be as fast c# allows.

## ParrallelChessTests
Unit tests for libraries

## AI
contains a interface declaring how AI should be structed to be recognized

## MinMaxAI
Implementation of minimax using alpha beta pruning

## FightEval
Used for testing the AI using a command line interface

## ChessApi
A ASP.NET Core server allowing people to challenge the AI via a rest api

## ParrallelChessPerformance
A messy library containing performance tests

## AsciiCommentGenerator
A internal cmd line program which makes it a bit easier to generate comments for unit tests that visualize which moves where played.
