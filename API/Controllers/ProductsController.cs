using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly StoreContext _context;

    public ProductsController(StoreContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts(CancellationToken cancellationToken)
    {
        var products = await _context.Products.ToListAsync(cancellationToken);

        return Ok(products);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Product>> GetProduct(Guid id, CancellationToken cancellationToken)
    {
        var product = await _context.Products.FindAsync(id, cancellationToken);

        if (product == null) return NotFound();

        return Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product, CancellationToken cancellationToken)
    {
        _context.Products.Add(product);

        await _context.SaveChangesAsync(cancellationToken);

        return product;
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteProduct(Guid id, CancellationToken cancellationToken)
    {
        var product = await _context.Products.FindAsync(id, cancellationToken);

        if (product == null) return NotFound();

        _context.Products.Remove(product);
        await _context.SaveChangesAsync(cancellationToken);

        return NoContent();
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult> UpdateProduct(Guid id, Product product,CancellationToken cancellationToken)
    {
        if (product.Id != id || !IsExists(id)) return BadRequest("Cannot Update This Product");

        _context.Entry(product).State = EntityState.Modified;

        await _context.SaveChangesAsync(cancellationToken);
        return NoContent();
    }

    private bool IsExists(Guid id)
    {
        return _context.Products.Any(x => x.Id == id);
    }
}
