using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews
{
    public static class ExtensionsTask
    {
        public static double Median(this IEnumerable<double> items)
        {
            var array = items.OrderBy(x => x).ToArray();
            if (array.Length == 0)
                throw new InvalidOperationException();
                
            var middle = array.Length / 2;

            return array.Length % 2 == 0 ? (array[middle] + array[middle - 1]) / 2 : array[middle];
        }

        public static IEnumerable<Tuple<T, T>> Bigrams<T>(this IEnumerable<T> items)
        {
            var previousItem = default(T);
            var isFirst = true;

            foreach (var item in items)
            {
                if (isFirst)
                    isFirst = false;
                else
                    yield return Tuple.Create(previousItem, item);

                previousItem = item;
            }
        }
    }
}