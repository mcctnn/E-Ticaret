using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Concretes;
public class ProductRepository(StoreContext context) : IProductRepository
{

    public void AddProduct(Product product)
    {
        context.Products.Add(product);
    }

    public void DeleteProduct(Product product)
    {
        context.Products.Remove(product);
    }

    public async Task<IReadOnlyList<String>> GetProductBrandsAsync(CancellationToken cancellationToken)
    {
        return await context.Products.Select(p => p.Brand).Distinct().ToListAsync(cancellationToken);
    }

    public async Task<Product?> GetProductByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var product = await context.Products.FindAsync(id, cancellationToken);
        return product;
    }

    public async Task<IReadOnlyList<Product>> GetProductsAsync(string? brand,
        string? type, string? sort, CancellationToken cancellationToken)
    {
        var query = context.Products.AsQueryable();

        if (!string.IsNullOrWhiteSpace(brand))
            query = query.Where(p => p.Brand == brand);

        if (!string.IsNullOrWhiteSpace(type))
            query = query.Where(p => p.Type == type);

        query = sort switch
        {
            "priceAsc" => query.OrderBy(p => p.Price),
            "priceDesc" => query.OrderByDescending(p => p.Price),
            _ => query.OrderBy(p => p.Name)
        };

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<String>> GetProductTypesAsync(CancellationToken cancellationToken)
    {
        return await context.Products.Select(p => p.Type).Distinct().ToListAsync(cancellationToken);
    }

    public Boolean IsExists(Guid id)
    {
        return context.Products.Any(p => p.Id == id);
    }

    public Task<Boolean> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return context.SaveChangesAsync(cancellationToken).ContinueWith(t => t.Result > 0, cancellationToken);
    }

    public void UpdateProduct(Product product)
    {
        context.Entry(product).State = EntityState.Modified;
    }
}
