namespace PropertyBasedTesting;

using FsCheck;
using FsCheck.Xunit;
using static FsCheck.Prop;

public class Chapter2Tests
{
    [Property]
    Property square_of_numbers_are_non_negative()
    {
        Arbitrary<int> numbers = Arb.From<int>();

        int square(int n) => n * n;

        bool squareIsNotNegative(int n) => square(n) >= 0;

        return ForAll(numbers, squareIsNotNegative);
    }
}
