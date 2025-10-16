using Core.Entities;

namespace Core.Specifications;
public class ProductSpecification : BaseSpecification<Product>
{
    public ProductSpecification(ProductSpecParams productParams) : base(p =>
        (string.IsNullOrEmpty(productParams.SearchTerm) || p.Name.ToLower().Contains(productParams.SearchTerm)) &&
        (!productParams.Brands.Any() || productParams.Brands.Contains(p.Brand)) &&
        (!productParams.Types.Any() || productParams.Types.Contains(p.Type))
    )
    {
        ApplyPaging(productParams.PageSize * (productParams.PageIndex - 1), productParams.PageSize);

        switch (productParams.Sort)
        {
            case "priceAsc":
                AddOrderBy(p => p.Price);
                break;
            case "priceDesc":
                AddOrderByDescending(p => p.Price);
                break;
            default:
                AddOrderBy(p => p.Name);
                break;
        }
    }
}
