using System;
using System.Collections.Generic;
using System.Linq;

namespace DmitryAdventure
{
    public static class CollectionExtensions
    {
        public static IEnumerable<TValue> RandomValues<TKey, TValue>(IDictionary<TKey, TValue> dict)
        {
            var random = new Random();
            var values = dict.Values.ToList();
            var size = dict.Count;
            while (true)
            {
                yield return values[random.Next(size)];
            }
        }
    }
}