using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Shos.TicTacToe.GameRunner
{
    using Shos.CsvHelper;
    using Shos.TicTacToe.Core;
    using Shos.TicTacToe.Players;

    class GameStep
    {
        int[] stones = new int[Board.Size * Board.Size];
        public int Stone1 { get => stones[0]; set => stones[0] = value; }
        public int Stone2 { get => stones[1]; set => stones[1] = value; }
        public int Stone3 { get => stones[2]; set => stones[2] = value; }
        public int Stone4 { get => stones[3]; set => stones[3] = value; }
        public int Stone5 { get => stones[4]; set => stones[4] = value; }
        public int Stone6 { get => stones[5]; set => stones[5] = value; }
        public int Stone7 { get => stones[6]; set => stones[6] = value; }
        public int Stone8 { get => stones[7]; set => stones[7] = value; }
        public int Stone9 { get => stones[8]; set => stones[8] = value; }

        public int HasWon { get; set; }

        [CsvIgnore()]
        public Stone MyStone { get; set; }

        public GameStep(Board board, Stone myStone)
        {
            MyStone = myStone;
            var tableIndexes = board.AllHands.ToArray();
            tableIndexes.ForEachWithIndex(
                (index, tableIndex)
                => stones[index] = board[tableIndex] == Stone.None ? 0
                                                                   : board[tableIndex] == myStone ?  1
                                                                                                  : -1
            );
        }
    }

    class Program
    {
        enum GamePlayersMode
        {
            /*RandomPlayers,*/ MLandRandomPlayers, RandomandMLPlayers, MLPlayers
        }

        static readonly GamePlayersMode[] gamePlayersModes = new[] {
            /*GamePlayersMode.RandomPlayers,*/ GamePlayersMode.MLandRandomPlayers, GamePlayersMode.RandomandMLPlayers, GamePlayersMode.MLPlayers
        };

        static List<GameStep> gameStepList = new List<GameStep>();
        static bool isFirst = true;
        static int gameCount        = 0;
        static int wonByCircleCount = 0;
        static int wonByCrossCount  = 0;

        static Stopwatch stopwatch = new Stopwatch();

        //const int gameTimes = 10000000;
        const int gameTimes = 4000000;
        static int AllGameTimes => gameTimes * gamePlayersModes.Length;

        static string CsvFileName => $"gameresult.{DateTime.Now.ToString().Replace(' ', '.').Replace('/', '.').Replace(':', '.')}.csv";

        static Stream? stream = null;

        static void Main()
        {
            stopwatch.Start();
            using (stream = new FileStream(CsvFileName, FileMode.Create)) {
                gamePlayersModes.ForEach(gamePlayersMode => gameTimes.Times(() => RunGame(gamePlayersMode)));
            }
            stopwatch.Stop();
            System.Console.WriteLine($"wonByCircleCount: {wonByCircleCount}, wonByCrossCount: {wonByCrossCount}, draw: {AllGameTimes - wonByCircleCount - wonByCrossCount}, time: {stopwatch.Elapsed}");
        }

        static MLPlayer mlPlayer1 = new MLPlayer();
        static MLPlayer mlPlayer2 = new MLPlayer();

        static void RunGame(GamePlayersMode gamePlayersMode)
        {
            var game = new Game();
            switch (gamePlayersMode) {
                //case GamePlayersMode.RandomPlayers     :
                //    break;
                case GamePlayersMode.MLandRandomPlayers:
                    game.SetPlayer(0, mlPlayer1);
                    break;
                case GamePlayersMode.RandomandMLPlayers:
                    game.SetPlayer(1, mlPlayer2);
                    break;
                case GamePlayersMode.MLPlayers         :
                    game.SetPlayer(0, mlPlayer1);
                    game.SetPlayer(1, mlPlayer2);
                    break;
            }

            game.Started += () => {};
            game.Update  += stone => OnGameUpdate(game, stone);
            game.Ended   += async stone => await OnGameEnd(stone);
            game.Run();
        }

        static void OnGameUpdate(Game game, Stone stone)
        {
            var gameStep = new GameStep(game.Board, stone);
            gameStepList.Add(gameStep);
        }

        static async Task OnGameEnd(Stone stone)
        {
            switch (stone) {
                case Stone.Circle: wonByCircleCount++; break;
                case Stone.Cross : wonByCrossCount++ ; break;
            };

            gameStepList.ForEach(gameStep => gameStep.HasWon = stone == Stone.None ? 0
                                                                                   : gameStep.MyStone == stone ?  1
                                                                                                               : -1);
            await gameStepList.WriteCsvAsync(stream: stream, bufferSize: 1024, leaveOpen: true, hasHeader: isFirst);
            gameStepList.Clear();
            isFirst = false;
            Console.WriteLine($"game: {++gameCount}/{AllGameTimes}");
        }
    }
}
