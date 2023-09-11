using FsCheck;
using FsCheck.Xunit;

namespace PropertyBasedTesting;

internal static class PrimeNumberTestExtensions
{
    private static bool IsPrime(this int n) =>
        Enumerable.Range(1, n)
            .Where(i => !(i == 1 || i == n))
            .All(i => n.CannotBeDividedBy(i));

    private static bool CannotBeDividedBy(this int n, int i) =>
        n % i != 0;

    internal static bool AreAllPrime(this IEnumerable<int> xs) =>
        xs.All(IsPrime);

    internal static int Multiplied(this IEnumerable<int> xs) =>
        xs.Aggregate(1, (product, i) => product * i);
}

public class Chapter4Tests
{
    IEnumerable<int> factorsOf(int n)
    {
        var remainder = n;
        var divisor = 2;
        var factors = new List<int>();
        while (remainder > 1)
        {
            while (remainder % divisor == 0)
            {
                factors.Add(divisor);
                remainder /= divisor;
            }

            divisor++;
        }

        return factors;
    }

    [Property]
    bool boolean_factorization_in_prime_numbers(PositiveInt positiveNumber)
    {
        var n = positiveNumber.Item;

        var factors = factorsOf(n);

        return factors.AreAllPrime() && factors.Multiplied() == n;
    }
}
