using System.Collections.Generic;
using System.Linq;

namespace Shos.TicTacToe.Core
{
    public enum Stone
    {
        None, Circle, Cross
    }

    public class Board
    {
        public const int Size = 3;

        Stone[,] stones = new Stone[Size, Size];

        static readonly List<IList<TableIndex>> judgementTable = new List<IList<TableIndex>>();

        public Board Clone {
            get {
                var clone = new Board();
                clone.stones = (Stone[,])stones.Clone();
                return clone;
            }
        }

        static Board()
        {
            TableIndex.MaximumRowNumber = TableIndex.MaximumColumnNumber = Size;

            judgementTable.AddRange(
                Enumerable.Range(0, Size)
                          .Select(row => Enumerable.Range(0, Size)
                                                   .Select(column => new TableIndex { Row = row, Column = column })
                                                   .ToArray()
                          )
                          );
            judgementTable.AddRange(
                Enumerable.Range(0, Size)
                          .Select(column => Enumerable.Range(0, Size)
                                                      .Select(row => new TableIndex { Row = row, Column = column })
                                                      .ToArray()
                          )
                          );
            judgementTable.Add(
                Enumerable.Range(0, Size)
                          .Select(index => new TableIndex { Row = index, Column = index })
                          .ToArray()
                          );
            judgementTable.Add(
                Enumerable.Range(0, Size)
                          .Select(index => new TableIndex { Row = index, Column = Size - 1 - index })
                          .ToArray()
                          );
        }

        bool HasWonBy(Stone stone)
            => judgementTable.Any(judgementLine => judgementLine.All(index => stones.Get(index) == stone));

        public Stone Judge()
            => HasWonBy(Stone.Circle) ? Stone.Circle
                                      : HasWonBy(Stone.Cross) ? Stone.Cross : Stone.None;

        public IList<TableIndex> ValidHands
            => stones.AllIndexes().Where(index => stones.Get(index) == Stone.None).ToArray();

        public IEnumerable<TableIndex> AllHands => stones.AllIndexes();

        public Stone this[TableIndex index]
        {
            get => stones.Get(index);
            set => stones.Set(index, value);
        }
    }
}
