using System;
using System.Collections.Generic;
using System.Linq;

namespace SignalBox.Core
{
#nullable enable
    public static class LinqExtensions
    {
        public static IEnumerable<IEnumerable<TSource>> Batch<TSource>(
                  this IEnumerable<TSource> source, int size)
        {
            TSource[]? bucket = null;
            var count = 0;

            foreach (var item in source)
            {
                if (bucket == null)
                    bucket = new TSource[size];

                bucket[count++] = item;
                if (count != size)
                    continue;

                yield return bucket;

                bucket = null;
                count = 0;
            }

            if (bucket != null && count > 0)
                yield return bucket.Take(count).ToArray();
        }

        private static readonly Random rng = new Random();

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static Dictionary<T, int> ValueCounts<T>(this IEnumerable<T> list)
        {
            var result = new Dictionary<T, int>();
            foreach (var item in list)
            {
                if (!result.ContainsKey(item))
                {
                    result[item] = 0;
                }

                result[item] += 1;
            }
            return result;
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T>? collection)
        {
            return collection == null || !collection.Any();
        }
    }
}