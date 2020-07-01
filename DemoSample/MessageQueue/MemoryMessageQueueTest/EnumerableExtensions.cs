using System.Collections.Generic;

namespace MemoryMessageQueueTest
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T[]> BatchesOf<T>(this IEnumerable<T> sequence, int batchSize)
        {
            var iteratorVariable0 = new List<T>(batchSize);
            foreach (var iteratorVariable1 in sequence)
            {
                iteratorVariable0.Add(iteratorVariable1);
                if (iteratorVariable0.Count < batchSize) continue;
                yield return iteratorVariable0.ToArray();
                iteratorVariable0.Clear();
            }

            if (iteratorVariable0.Count <= 0) yield break;
            yield return iteratorVariable0.ToArray();
            iteratorVariable0.Clear();
        }
    }
}