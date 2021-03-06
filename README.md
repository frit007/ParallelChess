![Unit tests](https://github.com/frit007/ParallelChess/workflows/Unit%20tests/badge.svg)

# ParallelChess
The project was developped in visual studio 2019 using dotnet core 3.1

# Structure
The project is split into multiple solution each trying to solve a small problem. 

## ParallelChess
ParallelChess is the core chess program which tries to be as fast as c# allows. [read more here](./ParallelChess/README.md)

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

## ChessDisplay
A angular application using the ChessApi to play against the AI. it is planned that this could also play against a web assembly version, for offline play.

## ParrallelChessPerformance
A messy library containing performance tests

## AsciiCommentGenerator
A internal cmd line program which makes it a bit easier to generate comments for unit tests that visualize which moves where played.

## ParallelChessC
A version the core of ParallelChess written in C++. This was primarily done to see how C# vs C++ would perform(it was slightly faster). And might allow webassembly, since the C# version didn't convert well to wasm.  
