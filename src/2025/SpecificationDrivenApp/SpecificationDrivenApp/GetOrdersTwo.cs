using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace SpecificationDrivenApp;

public sealed class GetOrdersTwo(OrderDbContext dbContext)
{
    public async Task<IReadOnlyList<Order>> InvokeOneAsync(CancellationToken cancellationToken)
    {
        var orders = await dbContext.Orders
            .Specify(new CreatedInWinter2025())
            .Specify(new WithMinTotal(500))
            .Specify(new ContainingGiftItem())
            .ToListAsync(cancellationToken);
        
        return orders;
    }
}

public interface ISpecification<T>
{
    Expression<Func<T, bool>> Criteria { get; }
    
    List<Expression<Func<T, object>>> Includes { get; }
}

public abstract class BaseSpecification<T> : ISpecification<T>
{
    public Expression<Func<T, bool>> Criteria { get; set; } = null!;

    public List<Expression<Func<T, object>>> Includes { get; } = new ();
 
    protected void AddInclude(Expression<Func<T, object>> includeExpression)
    {
        Includes.Add(includeExpression);
    }
}

public sealed class CreatedInWinter2025 : BaseSpecification<Order>
{
    public CreatedInWinter2025()
    {
        Criteria = o => o.CreatedAt >= new DateTimeOffset(2025, 12, 10, 0, 0, 0, TimeSpan.Zero) &&
                        o.CreatedAt <  new DateTimeOffset(2025, 12, 31, 0, 0, 0, TimeSpan.Zero);
    }
}

public sealed class WithMinTotal : BaseSpecification<Order>
{
    public WithMinTotal(decimal min)
    {
        Criteria = o => o.Items.Sum(i => (decimal?)i.Amount) >= min;

        AddInclude(o => o.Items);
    }
}

public sealed class ContainingGiftItem : BaseSpecification<Order>
{
    public ContainingGiftItem()
    {
        Criteria = o => o.Items.Any(i => i.Name.Contains("Gift"));

        AddInclude(o => o.Items);
    }
}