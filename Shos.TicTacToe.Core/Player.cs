using System;
using System.Collections.Generic;

namespace Shos.TicTacToe.Core
{
    public abstract class Player
    {
        public Stone Stone { get; set; } = Stone.Circle;
        public abstract TableIndex GetHand(Board board, IList<TableIndex> validHands);
    }

    public class RandomPlayer : Player
    {
        static Random random = new Random();

        public override TableIndex GetHand(Board board, IList<TableIndex> validHands)
            => validHands[random.Next(validHands.Count)];
    }
}
