using System.Collections.Immutable;
using FsCheck.Xunit;
using Xunit;

namespace PropertyBasedTesting;

public class InfamousReversalOfAList
{
    IEnumerable<string> Reverse(IEnumerable<string> xs) => xs.Reverse();


    [Property]
    void invariant_of_reversal(ImmutableArray<string> xs) =>
        Assert.Equal(xs, (Reverse(Reverse(xs))));


    IEnumerable<string> WrongReverse(IEnumerable<string> xs) => xs;

    [Property]
    void collateral_properties_can_be_deceiving(ImmutableArray<string> xs) =>
        Assert.Equal(xs, (WrongReverse(WrongReverse(xs))));

    [Property]
    bool specification_of_reversal(List<string> xs)
    {
        var reversed = Reverse(xs).ToList();

        var eachItemHasBeenReversed =
            Enumerable.Range(0, xs.Count)
                .All(i => xs[i] == reversed[xs.Count - i - 1]);

        return eachItemHasBeenReversed;
    }

    [Property]
    bool specification_of_reversal_with_a_for_cycle(List<string> xs)
    {
        var reversed = Reverse(xs).ToList();

        for(var i = 0; i < xs.Count; i++)
        {
            if (xs[i] != reversed[xs.Count - i - 1])
                return false;
        }

        return true;
    }
}
