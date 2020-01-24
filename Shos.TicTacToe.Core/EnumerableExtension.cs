using System;
using System.Collections.Generic;

namespace Shos.TicTacToe.Core
{
    public static class EnumerableExtension
    {
        public static void ForEach<T>(this IEnumerable<T> @this, Action action)
        {
            foreach (var item in @this)
                action();
        }

        public static void ForEach<T>(this IEnumerable<T> @this, Action<T> action)
        {
            foreach (var item in @this)
                action(item);
        }

        public static void ForEachWithIndex<T>(this IEnumerable<T> @this, Action<int, T> action)
        {
            var index = 0;
            foreach (var item in @this)
                action(index++, item);
        }

        public static void Times(this int @this, Action action)
        {
            for (var count = 0; count < @this; count++)
                action();
        }
    }
}
