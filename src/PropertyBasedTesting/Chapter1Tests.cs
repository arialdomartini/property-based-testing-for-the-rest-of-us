using FsCheck;
using FsCheck.Xunit;
using Xunit;
using PropertyBasedTesting.Resources;
using static PropertyBasedTesting.AssertExtensions;
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


    int Sum(int a, int b) => a + b;

    [Property]
    bool sum_is_commutative(int a, int b) =>
        a + b == b + a;

    [Property]
    bool adding_zero_does_not_change_the_result(int a) =>
        a + 0 == a;

    void generates_strings_from_generator_of_chars()
    {
        Gen<char> chars = Arb.Generate<char>();
        Gen<string> strings = Gen.ListOf(chars).Select(string.Concat);
    }

    void generates_tuples_of_strings_from_generator_of_strings()
    {
        Gen<string> strings = Arb.Generate<string>();
        Gen<Tuple<string, string>> tuples = Gen.Two(strings);
    }

    void generates_random_Products()
    {
        Gen<Product> products =
            from id in Arb.Generate<Guid>()
            from name in Arb.Generate<string>()
            from price in Arb.Generate<decimal>()
            from category in Arb.Generate<Category>()
            select new Product(Id: id, Name: name, Price: price, Category: category);
    }

    void example_of_generators()
    {
        Gen<bool> equallyDistributedBooleans =
            Arb.Generate<bool>();

        Gen<bool> tenMoreTrueValuesThanFalseOnes =
            Gen.Frequency(new Tuple<int, Gen<bool>>[]
            {
                new(10, Gen.Constant(true)),
                new(1, Gen.Constant(false))
            });
        
        var tuplesWithDifferentElements =
            Gen.Two(Gen.Choose(1, 100))
                .Where(t => t.Item1 != t.Item2);
    }

    record User(int Id, string FirstName, string LastName);
    
    void generating_users()
    {
        Gen<User> users =
            from firstName in Gen.Elements("Don", "Henrik", null)
            from secondName in Gen.Elements("Syme", "Feldt")
            from id in Gen.Choose(0, 1000)
            select new User(id, firstName, secondName);
    }
}
