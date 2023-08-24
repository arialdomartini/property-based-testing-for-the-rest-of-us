using FsCheck;
using FsCheck.Xunit;
using Xunit;
using static PropertyBasedTesting.Resources.Category;
using static PropertyBasedTesting.Resources.Countries;

namespace PropertyBasedTesting;

public class Chapter3Tests
{
    private bool Ship(Product product, object france) => true;
    
    [Fact]
    void books_can_be_shipped_internationally()
    {
        var product = new Product(
             Id: Guid.NewGuid(),
            Name: "The Little Schemer", 
            Category: Books, 
            Price: 16.50M);
    
   
        var canBeShipped = Ship(product, France);
	
        Assert.True(canBeShipped);
    }
    
    [Property]
    Property all_books_can_be_shipped_to_France()
    {
        var books = Arb.From(
            Arb.Generate<Product>()
                .Where(p => p.Category == Books));

        bool canBeShippedToFrance(Product product) =>
            Ship(product, France) == true;

        return Prop.ForAll(books, canBeShippedToFrance);
    }
}
