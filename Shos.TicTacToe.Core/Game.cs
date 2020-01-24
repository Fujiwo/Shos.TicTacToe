using System;

namespace Shos.TicTacToe.Core
{
    public class Game
    {
        public event Action? Started;
        public event Action<Stone>? Update;
        public event Action<Stone>? Ended;

        Player[] players = new Player[2] { new RandomPlayer { Stone = Stone.Circle }, new RandomPlayer { Stone = Stone.Cross } };
        int playerIndex = 0;

        public void SetPlayer(int index, Player player)
        {
            players[index] = player;
            player.Stone = index == 0 ? Stone.Circle : Stone.Cross;
        }

        public Board Board { get; } = new Board();

        public void Run()
        {
            Start();
            while (Step())
                ;
        }

        public void Start()
        {
            playerIndex = 0;
            Started?.Invoke();
        }

        public bool Step()
        {
            var validHands = Board.ValidHands;
            if (validHands.Count == 0) {
                Ended?.Invoke(Stone.None);
                return false;
            }
            var hand = players[playerIndex].GetHand(Board, Board.ValidHands);
            Board[hand] = players[playerIndex].Stone;
            Update?.Invoke(players[playerIndex].Stone);
            var stone = Board.Judge();
            if (stone != Stone.None) {
                Ended?.Invoke(stone);
                return false;
            }
            playerIndex = (playerIndex + 1) % 2;
            return true;
        }
    }
}
