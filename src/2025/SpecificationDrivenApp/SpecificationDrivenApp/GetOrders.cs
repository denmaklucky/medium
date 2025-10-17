using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace SpecificationDrivenApp;

public sealed class GetOrders(OrderDbContext dbContext)
{
    public async Task<IReadOnlyList<Order>> InvokeAsync(CancellationToken cancellationToken)
    {
        var orders = await dbContext.Orders
            .Where(Queries.WinterDiscount)
            .ToListAsync(cancellationToken);
        
        return orders;
    }
}

internal static class Queries
{
    private static readonly DateTimeOffset discountStartedAt = new (2025, 12, 10, 0 , 0 , 0, TimeSpan.Zero);
    
    public static Expression<Func<Order, bool>> WinterDiscount
        => order => order.CreatedAt > discountStartedAt;
}