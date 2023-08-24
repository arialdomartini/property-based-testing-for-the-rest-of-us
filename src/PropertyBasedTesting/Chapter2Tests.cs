using static PropertyBasedTesting.Resources.Serializer;

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
    
    [Property]
    Property square_of_numbers_are_non_negative_as_a_one_liner() => 
        ForAll(Arb.From<int>(), n => n * n >= 0);
    
    [Property]
    Property serialization_deserialization_roundtrip()
    {
        Arbitrary<Product> products = Arb.From<Product>();

        bool roundtripLooseNoInformation(Product product)
        {
            var afterRoundTrip = Deserialize<Product>(Serialize(product));
                
            return afterRoundTrip == product;
        }

        return Prop.ForAll(products, roundtripLooseNoInformation);
    }
        
        
}
