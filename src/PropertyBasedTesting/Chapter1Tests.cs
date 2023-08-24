using FsCheck.Xunit;
using PropertyBasedTesting.Resource;
using Xunit;
using static PropertyBasedTesting.Resource.Category;

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
}
