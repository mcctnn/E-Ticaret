using Core.Interfaces;
using System.Linq.Expressions;

namespace Core.Specifications;
public class BaseSpecification<T>(Expression<Func<T, Boolean>>? criteria) : ISpecification<T>
{
    protected BaseSpecification() : this(null) { }
    public Expression<Func<T, Boolean>>? Criteria => criteria;

    public Expression<Func<T, Object>>? OrderBy { get; private set; }

    public Expression<Func<T, Object>>? OrderByDescending { get; private set; }

    public Boolean IsDistinct { get; private set; }

    protected void AddOrderBy(Expression<Func<T, Object>> orderByExpression) => OrderBy = orderByExpression;
    protected void AddOrderByDescending(Expression<Func<T, Object>> orderByDescExpression) => OrderByDescending = orderByDescExpression;
    protected void ApplyDistinct() => IsDistinct = true;
}

public class BaseSpecification<T, TResult>(Expression<Func<T, Boolean>>? criteria) : BaseSpecification<T>(criteria), ISpecification<T, TResult>
{
    protected BaseSpecification() : this(null) { }
    public Expression<Func<T, TResult>>? Select { get; private set; }
    protected void AddSelect(Expression<Func<T, TResult>> selectExpression) => Select = selectExpression;
}
