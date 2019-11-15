using OAST.Project1.Common.Enums;

namespace OAST.Project1.Common.Extensions
{
    public static class Extensions
    {
        public static bool IsPositiveAndLessThanOne(float value, bool shouldBeLessThanOne)
        {
            return value > 0 & (!shouldBeLessThanOne || value < 1);
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

        public static string GetFileName(FileName fileName)
        {
            return $"{fileName.ToString().ToLower()}.txt";
        }
    }
}
