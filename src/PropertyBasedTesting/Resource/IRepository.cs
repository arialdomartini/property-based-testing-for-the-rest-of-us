namespace PropertyBasedTesting.Resource;

internal interface IRepository
{
    Product LoadById(Guid productId);
    void Save(Product product);
}
