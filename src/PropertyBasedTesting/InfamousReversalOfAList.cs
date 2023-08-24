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
}
