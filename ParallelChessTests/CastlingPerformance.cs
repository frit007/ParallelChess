using NUnit.Framework;
using ParallelChess;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ParallelChessTests {
    class CastlingPerformance {



        CastlingBits canCastleLookup(int fromPosition) {
            CastlingBits castleBits = CastlingBits.CAN_ALL;

            castleBits = (CastlingBits) ((int)castleBits & (int)CastlingHelper.castleLookup[fromPosition]);

            return castleBits;
        }

        CastlingBits canCastleIf(int fromPosition) {
            CastlingBits castling = CastlingBits.CAN_ALL;

            if (fromPosition == BoardOffset.A1) {
                castling = castling & CastlingBits.CAN_NOT_BQ;
            } else if (fromPosition == BoardOffset.E1) {
                castling = castling & CastlingBits.CAN_NOT_B;
            } else if (fromPosition == BoardOffset.H1) {
                castling = castling & CastlingBits.CAN_NOT_BK;
            } else if (fromPosition == BoardOffset.A8) {
                castling = castling & CastlingBits.CAN_NOT_WQ;
            } else if (fromPosition == BoardOffset.E8) {
                castling = castling & CastlingBits.CAN_NOT_W;
            } else if (fromPosition == BoardOffset.H8) {
                castling = castling & CastlingBits.CAN_NOT_WK;
            }
            return castling;
        }

        CastlingBits canCastleSwitch(int fromPosition) {
            CastlingBits castling = CastlingBits.CAN_ALL;
            switch (fromPosition) {
                case BoardOffset.A1:
                    castling = castling & CastlingBits.CAN_NOT_BQ;
                    break;
                case BoardOffset.E1:
                    castling = castling & CastlingBits.CAN_NOT_B;
                    break;
                case BoardOffset.H1:
                    castling = castling & CastlingBits.CAN_NOT_BK;
                    break;
                case BoardOffset.A8:
                    castling = castling & CastlingBits.CAN_NOT_WQ;
                    break;
                case BoardOffset.E8:
                    castling = castling & CastlingBits.CAN_NOT_W;
                    break;
                case BoardOffset.H8:
                    castling = castling & CastlingBits.CAN_NOT_WK;
                    break;
            }
            return castling;
        }

        [Test]
        public void testPerformanceLookup() {
            for (int i = 0; i < 1000000; i++) {
                // brug i % 64 til at checke alle positioner på brættet
                canCastleLookup(i % 64);
            }
        }

        [Test]
        public void testPerformanceIf() {
            for (int i = 0; i < 1000000; i++) {
                canCastleIf(i % 64);
            }
        }

        [Test]
        public void testPerformanceSwitch() {
            for (int i = 0; i < 1000000; i++) {
                canCastleSwitch(i%64);
            }
        }
    }
}
