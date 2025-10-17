using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ProductsController(IGenericRepository<Product> repository) : BaseApiController
{

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(
        [FromQuery] ProductSpecParams productParams, CancellationToken cancellationToken)
    {
        var spec = new ProductSpecification(productParams);

        return await CreatePagedResult(repository, spec, productParams.PageIndex, productParams.PageSize, cancellationToken);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Product>> GetProduct(Guid id, CancellationToken cancellationToken)
    {
        var product = await repository.GetByIdAsync(id, cancellationToken);

        if (product == null) return NotFound();

        return Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product, CancellationToken cancellationToken)
    {
        repository.Add(product);

        if (await repository.SaveChangesAsync(cancellationToken))
        {
            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        return BadRequest("Failed to creating product");
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteProduct(Guid id, CancellationToken cancellationToken)
    {
        var product = await repository.GetByIdAsync(id, cancellationToken);

        if (product == null) return NotFound();

        repository.Remove(product);

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

        repository.Update(product);
        if (await repository.SaveChangesAsync(cancellationToken))
        {
            return NoContent();
        }

        return BadRequest("Failed to updating product");
    }

    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetProductBrands(CancellationToken cancellationToken)
    {
        var spec = new BrandListSpecification();
        return Ok(await repository.ListAsync(spec, cancellationToken));
    }

    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetProductTypes(CancellationToken cancellationToken)
    {
        var spec = new TypeListSpecification();
        return Ok(await repository.ListAsync(spec, cancellationToken));
    }
    private bool ProductExists(Guid id) => repository.Exists(id);
}
