using System;
using System.Collections.Generic;

namespace OAST.Project1.Common.Extensions
{
    public static class Extensions
    {
        public static T RemoveAndGet<T>(this IList<T> list, int index)
        {
            lock (list)
            {
                T value = list[index];
                list.RemoveAt(index);
                return value;
            }
        }

        public static bool IsPositiveAndLessThanOne(float value, bool shouldBeLessThanOne)
        {
            return value > 0 & (!shouldBeLessThanOne || value < 1);
        }

        static long CalculateBinomialCoefficient(long n, long k)
        {
            long r = 1;
            long d;
            if (k > n) return 0;
            for (d = 1; d <= k; d++)
            {
                r *= n--;
                r /= d;
            }
            return r;
        }
    }
}
