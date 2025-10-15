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