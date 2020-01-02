using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParallelChess {
    public static class Helper {
        public static T RandomElement<T>(this IEnumerable<T> enumerable) {
            return enumerable.RandomElementUsing<T>(new Random());
        }

        public static T RandomElementUsing<T>(this IEnumerable<T> enumerable, Random rand) {
            int count = enumerable.Count();
            if (count != 0) {
                int index = rand.Next(0, count);
                return enumerable.ElementAt(index);
            }
            return default(T);
        }
    }
}
