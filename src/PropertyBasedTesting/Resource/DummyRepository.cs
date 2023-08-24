namespace PropertyBasedTesting.Resource;

internal class DummyRepository : IRepository
{
    private readonly Dictionary<Guid, Product> _products = new();

    Product IRepository.LoadById(Guid productId) => _products[productId];

    void IRepository.Save(Product product)
    {
        _products.Add(product.Id, product);
    }
}
