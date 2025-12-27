using API.RequestHelpers;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ProductsController(IUnitOfWork unitOfWork) : BaseApiController
{
    [Cache(600)]
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(
        [FromQuery] ProductSpecParams productParams, CancellationToken cancellationToken)
    {
        var spec = new ProductSpecification(productParams);

        return await CreatePagedResult(unitOfWork.Repository<Product>(), spec, productParams.PageIndex, productParams.PageSize, cancellationToken);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Product>> GetProduct(Guid id, CancellationToken cancellationToken)
    {
        var product = await unitOfWork.Repository<Product>().GetByIdAsync(id, cancellationToken);

        if (product == null) return NotFound();

        return Ok(product);
    }

    [InvalidateCache("api/products|")]
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product, CancellationToken cancellationToken)
    {
        unitOfWork.Repository<Product>().Add(product);

        if (await unitOfWork.Complete())
        {
            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        return BadRequest("Failed to creating product");
    }

    [InvalidateCache("api/products|")]
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteProduct(Guid id, CancellationToken cancellationToken)
    {
        var product = await unitOfWork.Repository<Product>().GetByIdAsync(id, cancellationToken);

        if (product == null) return NotFound();

        unitOfWork.Repository<Product>().Remove(product);

        if (await unitOfWork.Complete())
        {
            return NoContent();
        }

        return BadRequest("Failed to deleting product");
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:guid}")]
    public async Task<ActionResult> UpdateProduct(Guid id, Product product, CancellationToken cancellationToken)
    {
        if (product.Id != id || !ProductExists(id)) return BadRequest("Cannot Update This Product");

        unitOfWork.Repository<Product>().Update(product);
        if (await unitOfWork.Complete())
        {
            return NoContent();
        }

        return BadRequest("Failed to updating product");
    }

    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetProductBrands(CancellationToken cancellationToken)
    {
        var spec = new BrandListSpecification();
        return Ok(await unitOfWork.Repository<Product>().ListAsync(spec, cancellationToken));
    }

    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetProductTypes(CancellationToken cancellationToken)
    {
        var spec = new TypeListSpecification();
        return Ok(await unitOfWork.Repository<Product>().ListAsync(spec, cancellationToken));
    }
    private bool ProductExists(Guid id) => unitOfWork.Repository<Product>().Exists(id);
}
