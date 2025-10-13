using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Concretes;
public class GenericRepository<T>(StoreContext context) : IGenericRepository<T> where T : BaseEntity
{

    public void Add(T entity)
    {
        context.Set<T>().Add(entity);
    }

    public async Task<Int32> CountAsync(CancellationToken cancellationToken)
    {
        return await context.Set<T>().CountAsync(cancellationToken);
    }

    public void Remove(T entity)
    {
        context.Set<T>().Remove(entity);
    }

    public Boolean Exists(Guid id)
    {
        return context.Set<T>().Any(e => e.Id == id);
    }

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await context.Set<T>().FindAsync(id, cancellationToken);
    }

    public async Task<IReadOnlyList<T>> ListAllAsync(CancellationToken cancellationToken)
    {
        return await context.Set<T>().ToListAsync(cancellationToken);
    }

    public async Task<Boolean> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await context.SaveChangesAsync(cancellationToken) > 0;
    }

    public void Update(T entity)
    {
        context.Set<T>().Attach(entity);
        context.Entry(entity).State = EntityState.Modified;
    }

    public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec, CancellationToken cancellationToken)
    {
        return await ApplySpecification(spec).ToListAsync(cancellationToken);
    }

    public async Task<T?> GetEntityWithSpecAsync(ISpecification<T> spec, CancellationToken cancellationToken)
    {
        return await ApplySpecification(spec).FirstOrDefaultAsync(cancellationToken);
    }

    private IQueryable<T> ApplySpecification(ISpecification<T> spec)
    {
        return SpecificationEvaluator<T>.GetQuery(context.Set<T>().AsQueryable(), spec);
    }
    public async Task<IReadOnlyList<TResult>> ListAsync<TResult>(ISpecification<T, TResult> spec, CancellationToken cancellationToken)
    {        
        return await ApplySpecification(spec).ToListAsync(cancellationToken);
    }

    public async Task<TResult?> GetEntityWithSpecAsync<TResult>(ISpecification<T, TResult> spec, CancellationToken cancellationToken)
    {
        return await ApplySpecification(spec).FirstOrDefaultAsync(cancellationToken);
    }
    private IQueryable<TResult> ApplySpecification<TResult>(ISpecification<T, TResult> spec)
    {
        return SpecificationEvaluator<T>.GetQuery<T, TResult>(context.Set<T>().AsQueryable(), spec);
    }
}
