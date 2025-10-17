using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace SpecificationDrivenApp;

public sealed class GetOrdersOne(OrderDbContext dbContext)
{
    public async Task<IReadOnlyList<OrderSummary>> InvokeAsync(CancellationToken cancellationToken)
    {
        var from = new DateTimeOffset(2025, 12, 10, 0, 0, 0, TimeSpan.Zero);
        var to   = new DateTimeOffset(2025, 12, 31, 0, 0, 0, TimeSpan.Zero);

        var orders = await dbContext.Orders
            .Where(o => o.CreatedAt >= from && o.CreatedAt < to)
            .Where(o => dbContext.OrderItems
                .Where(i => i.OrderId == o.Id)
                .Sum(i => (decimal?)i.Amount) >= 500)
            .Where(o => dbContext.OrderItems
                .Any(i => i.OrderId == o.Id && i.Name.Contains("Gift")))
            .OrderByDescending(o => o.CreatedAt)
            .Select(o => new OrderSummary(
                o.Id,
                o.CreatedAt,
                dbContext.OrderItems.Count(i => i.OrderId == o.Id),
                dbContext.OrderItems.Where(i => i.OrderId == o.Id).Sum(i => (decimal?)i.Amount) ?? 0m,
                dbContext.OrderItems.Where(i => i.OrderId == o.Id).Max(i => (decimal?)i.Amount) ?? 0m,
                0m,
                dbContext.OrderItems.Any(i => i.OrderId == o.Id && i.Amount > 1000m)
            ))
            .ToListAsync(cancellationToken);
        
        return orders;
    }

    public async Task<IReadOnlyList<OrderSummary>> InvokeOneAsync(CancellationToken cancellationToken)
    {
        var orders = await dbContext.Orders
            .Where(QueriesOne.CreatedInWinter2025)
            .Where(QueriesOne.WithMinTotal(500))
            .Where(QueriesOne.ContainingGiftItem)
            .Select(QueriesOne.ToSummary)
            .ToListAsync(cancellationToken);

        return orders;
    }

    public sealed record OrderSummary(
        Guid OrderId,
        DateTimeOffset CreatedAt,
        int ItemCount,
        decimal Total,
        decimal MaxItem,
        decimal AveragePerItem,
        bool HasExpensiveItem);
}

public static class QueriesOne
{
    public static Expression<Func<Order, bool>> CreatedInWinter2025 =>
        o => o.CreatedAt >= new DateTimeOffset(2025, 12, 10, 0, 0, 0, TimeSpan.Zero) &&
             o.CreatedAt <  new DateTimeOffset(2025, 12, 31, 0, 0, 0, TimeSpan.Zero);

    public static Expression<Func<Order, bool>> WithMinTotal(decimal min) =>
        o => o.Items.Sum(i => (decimal?)i.Amount) >= min;

    public static Expression<Func<Order, bool>> ContainingGiftItem =>
        o => o.Items.Any(i => i.Name.Contains("Gift"));

    public static Expression<Func<Order, GetOrdersOne.OrderSummary>> ToSummary =>
        o => new GetOrdersOne.OrderSummary(
            o.Id,
            o.CreatedAt,
            o.Items.Count(),
            o.Items.Sum(i => (decimal?)i.Amount) ?? 0m,
            o.Items.Max(i => (decimal?)i.Amount) ?? 0m,
            o.Items.Average(i => (decimal?)i.Amount) ?? 0m,
            o.Items.Any(i => i.Amount > 1000m)
        );
}