using FsCheck;
using FsCheck.Xunit;
using Xunit;

using PropertyBasedTesting.Resources;
using static PropertyBasedTesting.Resources.Category;

namespace PropertyBasedTesting;
 
record Product(Guid Id, string Name, Category Category, decimal Price);


public class Chapter1Tests
{
    private readonly IRepository _repository = new DummyRepository();

    // An ordinary (dummy) example-based test
    [Fact]
    void products_can_be_persisted()
    {
        var product = new Product(
            Id: Guid.NewGuid(),
            Name: "The Little Schemer", 
            Category: Books, 
            Price: 16.50M);
    
        _repository.Save(product);

        var found = _repository.LoadById(product.Id);

        Assert.Equal(found, product);
    }

    // The same test, as a Property
    [Property]
    bool all_products_can_be_persisted(Product product)
    {
        _repository.Save(product);

        var found = _repository.LoadById(product.Id);

        return found == product;
    }
    
    string fizzbuzz(int i) =>
        i switch
        {
            _ when i % 15 == 0 => "fizzbuzz",
            _ when i % 3 == 0 => "fizz",
            _ when i % 5 == 0 => "buzz",
            _ => i.ToString()
        };
    
    [Theory]
    [InlineData(15)]
    [InlineData(30)]
    [InlineData(45)]
    [InlineData(60)]
    void multiples_of_15_return_fizzbuzz(int multipleOf15)
    {
        Assert.Equal("fizzbuzz", fizzbuzz(multipleOf15));
    }
    
    // The same, as a property (in different styles)
    [Property]
    Property all_the_multiples_of_15_return_fizzbuzz()
    {
        var multiplesOf15 = Arb.From( 
            Arb.Generate<int>()
                .Select(i => i * 15));

        return Prop.ForAll(multiplesOf15, n => fizzbuzz(n) == "fizzbuzz");
    }

    [Property]
    Property all_the_multiples_of_15_return_fizzbuzz_with_Linq()
    {
        var multiplesOf15 = Arb.From(
            from n in Arb.Generate<int>()
            let nn = n * 15
            select nn);
        
        return Prop.ForAll(
            multiplesOf15, 
            n => fizzbuzz(n) == "fizzbuzz");
    }
    
    [Property]
    bool all_the_multiples_of_15_return_fizzbuzz_as_a_predicate(int n) => 
        fizzbuzz(n * 15) == "fizzbuzz";

    [Fact]
    void all_the_multiples_of_15_return_fizzbuzz_with_Linq_as_a_fact()
    {
        var multiplesOf15 = Arb.From(
            from n in Arb.Generate<int>()
            let nn = n * 15
            select nn);

        var property = Prop.ForAll(
            multiplesOf15, 
            n => fizzbuzz(n) == "fizzbuzz");
        
        Check.QuickThrowOnFailure(property);
    }
}
