namespace PropertyBasedTesting.Resources;

internal interface IRepository
{
    Product LoadById(Guid productId);
    void Save(Product product);
}
