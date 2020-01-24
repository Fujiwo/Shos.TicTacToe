using System;

namespace Shos.TicTacToe.Core
{
    public struct TableIndex
    {
        public static int MaximumRowNumber = 3;
        public static int MaximumColumnNumber = MaximumRowNumber;

        public int Row;
        public int Column;

        public bool IsValid => 0 <= Row && Row < MaximumRowNumber && 0 <= Column && Column < MaximumColumnNumber;

        public int LinearIndex => Row * MaximumColumnNumber + Column;
        public static TableIndex FromLinearIndex(int linearIndex) => new TableIndex { Row = linearIndex / MaximumColumnNumber, Column = linearIndex % MaximumColumnNumber };

        public override bool Equals(object obj) => Row.Equals(((TableIndex)obj).Row) && Column.Equals(((TableIndex)obj).Column);
        public override int GetHashCode() => LinearIndex;

        public static bool operator ==(TableIndex index1, TableIndex index2) => index1.Equals(index2);
        public static bool operator !=(TableIndex index1, TableIndex index2) => !(index1 == index2);
    }
}
