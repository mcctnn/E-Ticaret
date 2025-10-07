using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IProductRepository repository) : ControllerBase
{

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(string? brand, string? type,
        string? sort, CancellationToken cancellationToken)
    {
        var products = await repository.GetProductsAsync(brand, type, sort, cancellationToken);

        return Ok(products);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Product>> GetProduct(Guid id, CancellationToken cancellationToken)
    {
        var product = await repository.GetProductByIdAsync(id, cancellationToken);

        if (product == null) return NotFound();

        return Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product, CancellationToken cancellationToken)
    {
        repository.AddProduct(product);

        if (await repository.SaveChangesAsync(cancellationToken))
        {
            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        return BadRequest("Failed to creating product");
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteProduct(Guid id, CancellationToken cancellationToken)
    {
        var product = await repository.GetProductByIdAsync(id, cancellationToken);

        if (product == null) return NotFound();

        repository.DeleteProduct(product);

        if (await repository.SaveChangesAsync(cancellationToken))
        {
            return NoContent();
        }

        return BadRequest("Failed to deleting product");
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult> UpdateProduct(Guid id, Product product, CancellationToken cancellationToken)
    {
        if (product.Id != id || !ProductExists(id)) return BadRequest("Cannot Update This Product");

        repository.UpdateProduct(product);
        if (await repository.SaveChangesAsync(cancellationToken))
        {
            return NoContent();
        }

        return BadRequest("Failed to updating product");
    }

    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetProductBrands(CancellationToken cancellationToken)
    {
        var brands = await repository.GetProductBrandsAsync(cancellationToken);
        return Ok(brands);
    }

    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetProductTypes(CancellationToken cancellationToken)
    {
        var types = await repository.GetProductTypesAsync(cancellationToken);
        return Ok(types);
    }
    private bool ProductExists(Guid id) => repository.IsExists(id);
}
