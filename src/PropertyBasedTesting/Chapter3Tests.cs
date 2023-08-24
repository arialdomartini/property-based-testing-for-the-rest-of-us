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
}
