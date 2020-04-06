using System;
using System.Collections.Generic;
using System.Linq;

public static class EnumerableExtension {

    public static T PickRandom<T>(this IEnumerable<T> source) {
        if(source.Count() <= 0) {
            return default;
        }
        return source.PickRandom(1).Single();
    }

    public static IEnumerable<T> PickRandom<T>(this IEnumerable<T> source, int count) {
        return source.Shuffle().Take(count);
    }

    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source) {
        return source.OrderBy(x => Guid.NewGuid());
    }
}
