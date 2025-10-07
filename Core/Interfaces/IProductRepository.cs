using Core.Entities;

namespace Core.Interfaces;
public interface IProductRepository
{
    Task<IReadOnlyList<Product>> GetProductsAsync(string? brand, string? type,
        string? sort, CancellationToken cancellationToken);
    Task<Product?> GetProductByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyList<string>> GetProductBrandsAsync(CancellationToken cancellationToken);
    Task<IReadOnlyList<string>> GetProductTypesAsync(CancellationToken cancellationToken);
    void AddProduct(Product product);
    void UpdateProduct(Product product);
    void DeleteProduct(Product product);
    bool IsExists(Guid id);
    Task<bool> SaveChangesAsync(CancellationToken cancellationToken);
}
