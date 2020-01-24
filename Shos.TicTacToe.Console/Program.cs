using System;

namespace Shos.TicTacToe.Console
{
    using Shos.TicTacToe.Core;
    using Shos.TicTacToe.Players;

    class Program
    {
        static void Main()
        {
            const int gameTimes = 10;
            gameTimes.Times(RunGame);
        }

        static void RunGame()
        {
            var game = new Game();
            game.SetPlayer(0, new MLPlayer());
            game.SetPlayer(1, new MLPlayer());

            game.Started += () => OnGameStart(game);
            game.Update  += stone => OnGameUpdate(game, stone);
            game.Ended   += OnGameEnd;
            game.Run();
        }

        static void OnGameStart(Game game)
        {
            System.Console.WriteLine("Start!");
            ShowGame(game);
        }

        static void OnGameUpdate(Game game, Stone stone)
        {
            System.Console.WriteLine($"{ToString(stone)}'s turn:");
            ShowGame(game);
        }

        static void OnGameEnd(Stone stone)
            => System.Console.WriteLine($"Won by {ToString(stone)}.\n");

        static void ShowGame(Game game)
        {
            for (var row = 0; row < Board.Size; row++) {
                for (var column = 0; column < Board.Size; column++)
                    System.Console.Write(ToString(game.Board[new TableIndex { Row = row, Column = column }]));
                System.Console.WriteLine();
            }
            System.Console.WriteLine();
        }

        static string ToString(Stone stone)
            => stone switch {
                   Stone.None   => ".",
                   Stone.Circle => "O",
                   Stone.Cross  => "X",
                   _ => throw new InvalidOperationException()
               };
    }
}
