using API.RequestHelpers;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BaseApiController : ControllerBase
{
    protected async Task<ActionResult> CreatePagedResult<T>(IGenericRepository<T> repository, ISpecification<T> spec,
        int pageIndex, int pageSize, CancellationToken cancellationToken) where T : BaseEntity
    {
        var items = await repository.ListAsync(spec, cancellationToken);
        var count = await repository.CountAsync(spec, cancellationToken);
        var pagination = new Pagination<T>(pageIndex, pageSize, count, items);
        
        return Ok(pagination);
    }
}
