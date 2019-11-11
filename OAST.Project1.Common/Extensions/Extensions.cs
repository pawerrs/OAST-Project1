using System;
using System.Collections.Generic;

namespace OAST.Project1.Common.Extensions
{
    public static class Extensions
    {
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

        public static long CalculateCombinationsWithRepetitionCount(long n, long k)
        {
            var numerator = Factorial(n + k - 1);
            var denominator = Factorial(k) * Factorial(n - 1);

            return numerator / denominator;
        }

        public static long Factorial(long f)
        {
            if (f == 0)
            {
                return 1;
            }

            return f * Factorial(f - 1);
        }
    }
}
