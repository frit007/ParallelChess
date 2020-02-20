using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using ParallelChess;
using System.IO;

namespace ParallelChessTests.BaseChess {
    class PgnTests {
        [Test]
        public void parseHeaderFormat() {
            var format = PGNParser.parseHeaderFormat("[Event \"No event\"]");

            Assert.AreEqual("Event", format.key);
            Assert.AreEqual("No event", format.value);
        }

        [Test]
        public void parseHeaders() {
            var pgn = @"[Event ""Rated Blitz game""]
[Site ""https://lichess.org/kglGFcVo""]
[Date ""????.??.??""]
[Round ""?""]
[White ""awser44""]
[Black ""9804808497""]
[Result ""0-1""]
[BlackElo ""1360""]
[BlackRatingDiff ""+17""]
[ECO ""A20""]
[FEN ""r2q1rk1/ppp1bppp/2n5/3p4/2PP2P1/1P2P3/1P1Q1PBP/R1B1K2R w KQ - 3 16""]
[Opening ""English Opening: King's English Variation, Nimzowitsch-Flohr Variation""]
[SetUp ""1""]
[Termination ""Time forfeit""]
[TimeControl ""420+0""]
[UTCDate ""2014.04.27""]
[UTCTime ""10:59:44""]
[WhiteElo ""1466""]
[WhiteRatingDiff ""-14""]

16. Bxd5 Bb4 17. O-O Bxd2 18. Bxd2 0-1";

            var chess = PGNParser.parse(pgn);

            Assert.AreEqual("Rated Blitz game", chess.pgn.Event);
            Assert.AreEqual("https://lichess.org/kglGFcVo", chess.pgn.Site);
            Assert.AreEqual("????.??.??", chess.pgn.Date);
            Assert.AreEqual("?", chess.pgn.Round);
            Assert.AreEqual("awser44", chess.pgn.White);
            Assert.AreEqual("9804808497", chess.pgn.Black);
            Assert.AreEqual("0-1", chess.pgn.Result);
        }

        [Test]
        public void parseBody() {
            var pgn = @"[Event ""Rated Blitz game""]
[Site ""https://lichess.org/kglGFcVo""]
[Date ""????.??.??""]
[Round ""?""]
[White ""awser44""]
[Black ""9804808497""]
[Result ""0-1""]
[BlackElo ""1360""]
[BlackRatingDiff ""+17""]
[ECO ""A20""]
[FEN ""r2q1rk1/ppp1bppp/2n5/3p4/2PP2P1/1P2P3/1P1Q1PBP/R1B1K2R w KQ - 3 16""]
[Opening ""English Opening: King's English Variation, Nimzowitsch-Flohr Variation""]
[SetUp ""1""]
[Termination ""Time forfeit""]
[TimeControl ""420+0""]
[UTCDate ""2014.04.27""]
[UTCTime ""10:59:44""]
[WhiteElo ""1466""]
[WhiteRatingDiff ""-14""]

16. Bxd5 Bb4 17. O-O Bxd2 18. Bxd2 0-1";

            var chess = PGNParser.parse(pgn);

            chess.UndoAll();

            Assert.AreEqual("Bxd5", chess.Redo().san);
            Assert.AreEqual("Bb4", chess.Redo().san);
            Assert.AreEqual("O-O", chess.Redo().san);
            Assert.AreEqual("Bxd2", chess.Redo().san);
            Assert.AreEqual("Bxd2", chess.Redo().san);
        }


        [Test]
        public void CountRepeatedPositionsKeepBoardAlive1() {
            // if you want to see the game import the game on lichess.org
            var pgn = @"[Event ""Shamkir Chess""]
[Site ""chess24.com""]
[Date ""2019.03.31""]
[Round ""1""]
[White ""Anand, Viswanathan""]
[Black ""Navara, David""]
[Result ""1/2-1/2""]
[Board ""1""]
[WhiteElo ""2779""]
[WhiteTitle ""GM""]
[WhiteCountry ""IND""]
[WhiteFideId ""5000017""]
[WhiteEloChange ""-1""]
[BlackElo ""2739""]
[BlackTitle ""GM""]
[BlackCountry ""CZE""]
[BlackFideId ""309095""]
[BlackEloChange ""1""]

1. e4 {[%clk 1:59:57]} c5 {[%clk 1:59:56]} 2. Nf3 {[%clk 1:59:45]} d6 {[%clk
1:59:51]} 3. d4 {[%clk 1:59:39]} cxd4 {[%clk 1:59:45]} 4. Nxd4 {[%clk 1:59:34]}
Nf6 {[%clk 1:59:39]} 5. Nc3 {[%clk 1:59:26]} a6 {[%clk 1:59:35]} 6. Bd3 {[%clk
1:59:16]} g6 {[%clk 1:54:35]} 7. f3 {[%clk 1:58:42]} Bg7 {[%clk 1:29:57]} 8. Be3
{[%clk 1:57:45]} Nc6 {[%clk 1:29:16]} 9. Qd2 {[%clk 1:55:46]} Nxd4 {[%clk
1:19:30]} 10. Bxd4 {[%clk 1:55:22]} Be6 {[%clk 1:19:22]} 11. g4 {[%clk 1:36:58]}
b5 {[%clk 1:06:38]} 12. h4 {[%clk 1:28:04]} Qa5 {[%clk 0:56:17]} 13. a3 {[%clk
1:16:31]} h6 {[%clk 0:53:06]} 14. O-O-O {[%clk 1:10:07]} Rb8 {[%clk 0:51:16]} 15.
g5 {[%clk 1:06:02]} Nh5 {[%clk 0:50:45]} 16. Bxg7 {[%clk 1:05:53]} Nxg7 {[%clk
0:50:36]} 17. gxh6 {[%clk 1:05:48]} Nh5 {[%clk 0:50:27]} 18. Qg5 {[%clk 1:05:10]}
f6 {[%clk 0:46:26]} 19. Qxg6+ {[%clk 0:59:49]} Bf7 {[%clk 0:46:13]} 20. Qg1
{[%clk 0:59:27]} b4 {[%clk 0:45:51]} 21. axb4 {[%clk 0:50:08]} Qxb4 {[%clk
0:44:35]} 22. Kd2 {[%clk 0:48:00]} Nf4 {[%clk 0:34:46]} 23. Qe3 {[%clk 0:44:29]}
Nxd3 {[%clk 0:28:41]} 24. Qxd3 {[%clk 0:39:57]} Rxh6 {[%clk 0:28:18]} 25. Qxa6
{[%clk 0:35:14]} Kf8 {[%clk 0:16:27]} 26. Ra1 {[%clk 0:28:43]} d5 {[%clk
0:11:26]} 27. Ra4 {[%clk 0:24:41]} Qc5 {[%clk 0:11:18]} 28. exd5 {[%clk 0:21:08]}
Rxb2 {[%clk 0:08:00]} 29. Qa7 {[%clk 0:17:36]} Qd6 {[%clk 0:02:30]} 30. Qe3
{[%clk 0:17:04]} Rg6 {[%clk 0:02:08]} 31. Ra8+ {[%clk 0:16:02]} Kg7 {[%clk
0:01:40]} 32. Kc1 {[%clk 0:15:36]} Qb4 {[%clk 0:00:52]} 33. Ra4 {[%clk 0:10:47]}
Rb1+ {[%clk 0:00:51]} 34. Nxb1 {[%clk 0:10:37]} Qxa4 {[%clk 0:00:50]} 35. Qxe7
{[%clk 0:07:18]} Rg2 {[%clk 0:00:12]} 36. Qe4 {[%clk 0:05:27]} Qa7 {[%clk
0:00:10]} 37. Re1 {[%clk 0:03:31]} Rg1 {[%clk 0:00:09]} 38. Nc3 {[%clk 0:01:07]}
Qa1+ {[%clk 0:00:04]} 39. Kd2 {[%clk 0:00:49]} Rg2+ {[%clk 0:00:03]} 40. Re2
{[%clk 1:00:29]} Rg1 {[%clk 1:00:01]} 41. Qe7 {[%clk 0:50:10]} Rd1+ {[%clk
0:59:32]} 1/2-1/2";

            var game = PGNParser.parse(pgn);
            // replays the game
            Assert.AreEqual("8/4Qbk1/5p2/3P4/7P/2N2P2/2PKR3/q2r4 w - - 13 42", game.FEN);

            game.board.RepeatedPositions(game.moveHistory());
            Assert.AreEqual("8/4Qbk1/5p2/3P4/7P/2N2P2/2PKR3/q2r4 w - - 13 42", game.FEN);

            game.Winner();
            Assert.AreEqual("8/4Qbk1/5p2/3P4/7P/2N2P2/2PKR3/q2r4 w - - 13 42", game.FEN);


        }

        [Test]
        public void CountRepeatedPositionsKeepBoardAlive2() {
            // if you want to see the game import the game on lichess.org
            var pgn = @"[Event ""Shamkir Chess""]
[Site ""chess24.com""]
[Date ""2019.03.31""]
[Round ""1""]
[White ""Giri, Anish""]
[Black ""Topalov, Veselin""]
[Result ""1/2-1/2""]
[Board ""2""]
[WhiteElo ""2797""]
[WhiteTitle ""GM""]
[WhiteCountry ""NED""]
[WhiteFideId ""24116068""]
[WhiteEloChange ""-1""]
[BlackElo ""2740""]
[BlackTitle ""GM""]
[BlackCountry ""BUL""]
[BlackFideId ""2900084""]
[BlackEloChange ""1""]

1. e4 {[%clk 1:59:58]} e5 {[%clk 1:59:43]} 2. Nf3 {[%clk 1:59:54]} Nc6 {[%clk
1:59:37]} 3. Bb5 {[%clk 1:59:45]} Nf6 {[%clk 1:59:19]} 4. d3 {[%clk 1:59:31]} Bc5
{[%clk 1:58:47]} 5. Bxc6 {[%clk 1:59:22]} dxc6 {[%clk 1:58:41]} 6. O-O {[%clk
1:59:18]} Qe7 {[%clk 1:57:39]} 7. Nbd2 {[%clk 1:58:19]} Bg4 {[%clk 1:57:19]} 8.
h3 {[%clk 1:58:04]} Bh5 {[%clk 1:56:32]} 9. a3 {[%clk 1:56:44]} Nd7 {[%clk
1:53:59]} 10. b4 {[%clk 1:55:42]} Bd6 {[%clk 1:53:42]} 11. Nc4 {[%clk 1:55:23]}
f6 {[%clk 1:53:05]} 12. Ne3 {[%clk 1:53:44]} Nf8 {[%clk 1:51:29]} 13. Nf5 {[%clk
1:52:12]} Qd7 {[%clk 1:49:23]} 14. Be3 {[%clk 1:50:46]} Ne6 {[%clk 1:46:19]} 15.
c3 {[%clk 1:50:31]} O-O-O {[%clk 1:38:42]} 16. Ng3 {[%clk 1:50:19]} Bxf3 {[%clk
1:25:42]} 17. Qxf3 {[%clk 1:50:06]} Kb8 {[%clk 1:25:34]} 18. Rfd1 {[%clk
1:29:02]} g6 {[%clk 1:22:23]} 19. d4 {[%clk 1:23:18]} exd4 {[%clk 1:19:35]} 20.
cxd4 {[%clk 1:23:10]} Rhf8 {[%clk 1:14:50]} 21. Bh6 {[%clk 1:09:27]} Rf7 {[%clk
1:11:02]} 22. d5 {[%clk 1:07:42]} cxd5 {[%clk 1:05:20]} 23. Rxd5 {[%clk 1:07:01]}
Qe8 {[%clk 0:58:09]} 24. Rad1 {[%clk 1:05:52]} Rc8 {[%clk 0:57:15]} 25. Qg4
{[%clk 0:54:55]} Bf8 {[%clk 0:51:09]} 26. Bxf8 {[%clk 0:51:19]} Nxf8 {[%clk
0:49:40]} 27. Ne2 {[%clk 0:43:27]} h5 {[%clk 0:47:58]} 28. Qf3 {[%clk 0:42:55]}
Nd7 {[%clk 0:46:56]} 29. Nc3 {[%clk 0:42:18]} Ne5 {[%clk 0:38:52]} 30. Qe2 {[%clk
0:40:33]} Re7 {[%clk 0:36:43]} 31. f4 {[%clk 0:25:30]} Nf7 {[%clk 0:36:35]} 32.
R5d4 {[%clk 0:25:18]} Nd6 {[%clk 0:33:50]} 33. Qd3 {[%clk 0:25:06]} Re6 {[%clk
0:30:22]} 34. b5 {[%clk 0:18:57]} b6 {[%clk 0:25:08]} 35. a4 {[%clk 0:18:24]} g5
{[%clk 0:18:50]} 36. f5 {[%clk 0:17:03]} Re5 {[%clk 0:18:46]} 37. Rf1 {[%clk
0:13:10]} Rd8 {[%clk 0:12:28]} 38. Rd1 {[%clk 0:11:01]} Rc8 {[%clk 0:09:15]} 39.
Ra1 {[%clk 0:02:56]} g4 {[%clk 0:06:41]} 40. hxg4 {[%clk 1:02:06]} hxg4 {[%clk
1:05:57]} 41. Rd1 {[%clk 0:50:11]} Kb7 {[%clk 0:58:25]} 42. Rd5 {[%clk 0:47:24]}
Rxd5 {[%clk 0:46:58]} 43. Qxd5+ {[%clk 0:36:21]} Kb8 {[%clk 0:46:53]} 44. Qd4
{[%clk 0:29:45]} Qh5 {[%clk 0:44:15]} 45. Nd5 {[%clk 0:23:11]} g3 {[%clk
0:31:10]} 46. Nxf6 {[%clk 0:23:06]} Qh2+ {[%clk 0:30:58]} 47. Kf1 {[%clk
0:23:04]} Qh1+ {[%clk 0:30:44]} 48. Qg1 {[%clk 0:22:55]} Qh4 {[%clk 0:27:30]} 49.
Qd4 {[%clk 0:22:47]} Qh1+ {[%clk 0:25:44]} 50. Qg1 {[%clk 0:22:43]} 1/2-1/2";

            var game = PGNParser.parse(pgn);
            // replays the game
            Assert.AreEqual("1kr5/p1p5/1p1n1N2/1P3P2/P3P3/6p1/6P1/3R1KQq b - - 8 50", game.FEN);

            game.board.RepeatedPositions(game.moveHistory());
            Assert.AreEqual("1kr5/p1p5/1p1n1N2/1P3P2/P3P3/6p1/6P1/3R1KQq b - - 8 50", game.FEN);

            game.Winner();
            Assert.AreEqual("1kr5/p1p5/1p1n1N2/1P3P2/P3P3/6p1/6P1/3R1KQq b - - 8 50", game.FEN);

        }


        [Test]
        public void CountRepeatedPositionsKeepBoardAlive3() {
            // if you want to see the game import the game on lichess.org
            var pgn = @"[Event ""Shamkir Chess""]
[Site ""chess24.com""]
[Date ""2019.03.31""]
[Round ""1""]
[White ""Radjabov, Teimour""]
[Black ""Carlsen, Magnus""]
[Result ""1/2-1/2""]
[Board ""3""]
[WhiteElo ""2756""]
[WhiteTitle ""GM""]
[WhiteCountry ""AZE""]
[WhiteFideId ""13400924""]
[WhiteEloChange ""1""]
[BlackElo ""2845""]
[BlackTitle ""GM""]
[BlackCountry ""NOR""]
[BlackFideId ""1503014""]
[BlackEloChange ""-1""]

1. e4 {[%clk 1:59:59]} e5 {[%clk 1:59:52]} 2. Nf3 {[%clk 1:59:52]} Nc6 {[%clk
1:59:45]} 3. Bc4 {[%clk 1:58:32]} Nf6 {[%clk 1:59:34]} 4. d3 {[%clk 1:58:28]} Be7
{[%clk 1:59:07]} 5. O-O {[%clk 1:56:56]} O-O {[%clk 1:58:53]} 6. h3 {[%clk
1:55:28]} d6 {[%clk 1:53:14]} 7. a4 {[%clk 1:54:55]} a5 {[%clk 1:43:32]} 8. Nbd2
{[%clk 1:51:21]} Nd7 {[%clk 1:35:24]} 9. Re1 {[%clk 1:49:21]} Nb6 {[%clk
1:29:28]} 10. Bb3 {[%clk 1:48:09]} Kh8 {[%clk 1:28:33]} 11. c3 {[%clk 1:41:55]}
f5 {[%clk 1:27:42]} 12. exf5 {[%clk 1:35:09]} Bxf5 {[%clk 1:26:55]} 13. Nf1
{[%clk 1:32:50]} Bg6 {[%clk 1:17:04]} 14. Ng3 {[%clk 1:30:26]} Bf6 {[%clk
1:11:04]} 15. Ne4 {[%clk 1:24:11]} d5 {[%clk 1:03:35]} 16. Nxf6 {[%clk 1:19:52]}
Qxf6 {[%clk 1:03:20]} 17. Bg5 {[%clk 1:18:14]} Qf5 {[%clk 0:48:04]} 18. Qd2
{[%clk 1:16:51]} Rae8 {[%clk 0:45:32]} 19. Be3 {[%clk 1:07:42]} Bh5 {[%clk
0:41:21]} 20. Bd1 {[%clk 1:01:25]} Qd7 {[%clk 0:36:20]} 21. Bxb6 {[%clk 0:54:49]}
cxb6 {[%clk 0:36:11]} 22. Qg5 {[%clk 0:52:58]} Qf7 {[%clk 0:33:17]} 23. Qh4
{[%clk 0:49:06]} Bg6 {[%clk 0:32:15]} 24. Bb3 {[%clk 0:43:29]} Qd7 {[%clk
0:20:26]} 25. Qg3 {[%clk 0:42:18]} d4 {[%clk 0:16:01]} 26. Nxe5 {[%clk 0:37:09]}
Nxe5 {[%clk 0:15:48]} 27. Rxe5 {[%clk 0:37:06]} dxc3 {[%clk 0:15:09]} 28. Rxe8
{[%clk 0:35:28]} Rxe8 {[%clk 0:14:58]} 29. bxc3 {[%clk 0:35:26]} Qxd3 {[%clk
0:14:41]} 30. Qxd3 {[%clk 0:28:05]} Bxd3 {[%clk 0:14:39]} 31. Rd1 {[%clk
0:27:52]} Be4 {[%clk 0:13:31]} 32. Rd6 {[%clk 0:26:02]} Bc6 {[%clk 0:13:04]} 33.
Bd5 {[%clk 0:24:31]} Bxd5 {[%clk 0:12:58]} 34. Rxd5 {[%clk 0:24:29]} Re1+ {[%clk
0:12:47]} 35. Kh2 {[%clk 0:24:27]} h6 {[%clk 0:12:37]} 36. Rd7 {[%clk 0:23:42]}
Rc1 {[%clk 0:12:18]} 37. Rxb7 {[%clk 0:23:37]} Rxc3 {[%clk 0:12:14]} 38. Rxb6
{[%clk 0:23:33]} Rc4 {[%clk 0:12:08]} 39. Ra6 {[%clk 0:23:07]} Rxa4 {[%clk
0:12:04]} 40. f4 {[%clk 1:23:00]} Rxf4 {[%clk 1:11:54]} 41. Rxa5 {[%clk 1:22:57]}
1/2-1/2";

            var game = PGNParser.parse(pgn);
            // replays the game
            Assert.AreEqual("7k/6p1/7p/R7/5r2/7P/6PK/8 b - - 0 41", game.FEN);

            game.board.RepeatedPositions(game.moveHistory());
            Assert.AreEqual("7k/6p1/7p/R7/5r2/7P/6PK/8 b - - 0 41", game.FEN);

            game.Winner();
            Assert.AreEqual("7k/6p1/7p/R7/5r2/7P/6PK/8 b - - 0 41", game.FEN);

        }

        [Test]
        public void CountRepeatedPositionsKeepBoardAlive4() {
            // if you want to see the game import the game on lichess.org
            var pgn = @"[Event ""Shamkir Chess""]
[Site ""chess24.com""]
[Date ""2019.03.31""]
[Round ""1""]
[White ""Karjakin, Sergey""]
[Black ""Ding, Liren""]
[Result ""1/2-1/2""]
[Board ""4""]
[WhiteElo ""2753""]
[WhiteTitle ""GM""]
[WhiteCountry ""RUS""]
[WhiteFideId ""14109603""]
[WhiteEloChange ""1""]
[BlackElo ""2812""]
[BlackTitle ""GM""]
[BlackCountry ""CHN""]
[BlackFideId ""8603677""]
[BlackEloChange ""-1""]

1. e4 {[%clk 1:59:58]} e5 {[%clk 1:59:54]} 2. Nf3 {[%clk 1:59:49]} Nc6 {[%clk
1:59:51]} 3. Bc4 {[%clk 1:59:43]} Nf6 {[%clk 1:59:33]} 4. d3 {[%clk 1:59:38]} Bc5
{[%clk 1:59:29]} 5. c3 {[%clk 1:59:34]} d6 {[%clk 1:58:38]} 6. O-O {[%clk
1:59:25]} O-O {[%clk 1:58:26]} 7. Re1 {[%clk 1:59:10]} a5 {[%clk 1:56:39]} 8. Bg5
{[%clk 1:58:31]} h6 {[%clk 1:55:56]} 9. Bh4 {[%clk 1:58:23]} g5 {[%clk 1:55:53]}
10. Bg3 {[%clk 1:58:18]} Nh7 {[%clk 1:55:23]} 11. d4 {[%clk 1:57:57]} Bb6 {[%clk
1:55:17]} 12. dxe5 {[%clk 1:57:48]} h5 {[%clk 1:55:12]} 13. h4 {[%clk 1:57:35]}
Bg4 {[%clk 1:55:06]} 14. Nbd2 {[%clk 1:57:04]} Nxe5 {[%clk 1:54:32]} 15. Be2
{[%clk 1:56:57]} Nxf3+ {[%clk 1:50:45]} 16. Nxf3 {[%clk 1:56:49]} Re8 {[%clk
1:49:28]} 17. Qd2 {[%clk 1:53:42]} Bxf3 {[%clk 1:47:42]} 18. Bxf3 {[%clk
1:53:21]} gxh4 {[%clk 1:47:38]} 19. Bf4 {[%clk 1:53:13]} Qf6 {[%clk 1:46:36]} 20.
Bxh5 {[%clk 1:52:36]} Qg7 {[%clk 1:46:27]} 21. Bh6 {[%clk 1:45:22]} Qf6 {[%clk
1:44:12]} 22. Bf4 {[%clk 1:41:49]} Qg7 {[%clk 1:44:05]} 23. Bh6 {[%clk 1:38:02]}
Qf6 {[%clk 1:43:58]} 24. Bf4 {[%clk 1:37:56]} Qg7 {[%clk 1:43:52]} 1/2-1/2";

            var game = PGNParser.parse(pgn);
            // replays the game
            Assert.AreEqual("r3r1k1/1pp2pqn/1b1p4/p6B/4PB1p/2P5/PP1Q1PP1/R3R1K1 w - - 9 25", game.FEN);

            game.board.RepeatedPositions(game.moveHistory());
            Assert.AreEqual("r3r1k1/1pp2pqn/1b1p4/p6B/4PB1p/2P5/PP1Q1PP1/R3R1K1 w - - 9 25", game.FEN);

            game.Winner();
            Assert.AreEqual("r3r1k1/1pp2pqn/1b1p4/p6B/4PB1p/2P5/PP1Q1PP1/R3R1K1 w - - 9 25", game.FEN);

        }
    }
}
