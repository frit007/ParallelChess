﻿using System;
using System.Collections.Generic;
using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Environments;
using ParallelChess;

namespace ParallelChessPerformance {
    [SimpleJob()]
    public class CastlingPerformance {
        CastlingBits CanCastleLookup(int fromPosition) {
            CastlingBits castleBits = CastlingBits.CAN_ALL;

            castleBits = (CastlingBits)((int)castleBits & (int)CastlingHelper.castleLookup[fromPosition]);

            return castleBits;
        }

        CastlingBits CanCastleIf(int fromPosition) {
            CastlingBits castling = CastlingBits.CAN_ALL;

            if (fromPosition == BoardStateOffset.A1) {
                castling = castling & CastlingBits.CAN_NOT_BQ;
            } else if (fromPosition == BoardStateOffset.E1) {
                castling = castling & CastlingBits.CAN_NOT_B;
            } else if (fromPosition == BoardStateOffset.H1) {
                castling = castling & CastlingBits.CAN_NOT_BK;
            } else if (fromPosition == BoardStateOffset.A8) {
                castling = castling & CastlingBits.CAN_NOT_WQ;
            } else if (fromPosition == BoardStateOffset.E8) {
                castling = castling & CastlingBits.CAN_NOT_W;
            } else if (fromPosition == BoardStateOffset.H8) {
                castling = castling & CastlingBits.CAN_NOT_WK;
            }
            return castling;
        }

        CastlingBits CanCastleSwitch(int fromPosition) {
            CastlingBits castling = CastlingBits.CAN_ALL;
            switch (fromPosition) {
                case BoardStateOffset.A1:
                    castling = castling & CastlingBits.CAN_NOT_BQ;
                    break;
                case BoardStateOffset.E1:
                    castling = castling & CastlingBits.CAN_NOT_B;
                    break;
                case BoardStateOffset.H1:
                    castling = castling & CastlingBits.CAN_NOT_BK;
                    break;
                case BoardStateOffset.A8:
                    castling = castling & CastlingBits.CAN_NOT_WQ;
                    break;
                case BoardStateOffset.E8:
                    castling = castling & CastlingBits.CAN_NOT_W;
                    break;
                case BoardStateOffset.H8:
                    castling = castling & CastlingBits.CAN_NOT_WK;
                    break;
            }
            return castling;
        }

        [Benchmark]
        public void TestPerformanceLookup() {
            for (int i = 0; i < 1000000000; i++) {
                // brug i % 64 til at checke alle positioner på brættet
                CanCastleLookup(i % 64);
            }
        }

        [Benchmark]
        public void TestPerformanceIf() {
            for (int i = 0; i < 1000000000; i++) {
                CanCastleIf(i % 64);
            }
        }

        [Benchmark]
        public void TestPerformanceSwitch() {
            for (int i = 0; i < 1000000000; i++) {
                CanCastleSwitch(i % 64);
            }
        }
    }
}