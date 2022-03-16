using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalBox.Test
{
    public static class AsyncEnumerableExtensions
    {
        public static async IAsyncEnumerable<T> ToAsyncEnumerable<T>(this IEnumerable<T> collection)
        {
            if (collection is null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            foreach (var i in collection)
            {
                yield return i;
            }

            await Task.CompletedTask; // to make the compiler warning go away
        }
    }
}
