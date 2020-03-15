using System;
using System.Collections.Generic;

namespace Candles.Domain.Extensions
{
    public static class SortedListExtensions
    {
        public static int SearchEqualOrGreater<TValue>(this SortedList<DateTime, TValue> sortedList, DateTime value)
        {
            return sortedList.SearchEqualOrGreater(value, (v1, v2) =>
            {
                if (v1 < v2)
                    return -1;

                return !(v1 > v2) ? 0 : 1;
            });
        }

        private static int SearchEqualOrGreater<TKey, TValue>(this SortedList<TKey, TValue> sortedList, TKey value,
            Func<TKey, TKey, int> comparator)
        {
            if (sortedList.Count == 0)
                return -1;

            var key1 = value;

            var keys = sortedList.Keys;

            var key2 = keys[^1];

            if (comparator(key1, key2) > 0)
                return sortedList.Count;

            var index1 = 0;

            var index2 = sortedList.Count - 1;

            var index3 = index2 / 2;

            do
            {
                var num = comparator(sortedList.Keys[index3], value);

                if (num == 0)
                    return index3;
                if (num < 0)
                    index1 = index3;
                else
                    index2 = index3;

                index3 = index1 + (index2 - index1) / 2;
            } while (index2 - index1 > 1);

            var num1 = comparator(sortedList.Keys[index1], value);

            return num1 == 0 || comparator(sortedList.Keys[index2], value) != 0 && num1 > 0 ? index1 : index2;
        }
    }
}
