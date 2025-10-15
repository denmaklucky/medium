using System.Linq.Expressions;

namespace SpecificationDrivenApp;

internal static class Queries
{
    private static readonly DateTimeOffset discountStartedAt = new DateTimeOffset(2025, 12, 10, 0 , 0 , 0, TimeSpan.Zero);

    public static Expression<Func<Order, bool>> WinterDiscount
        => order => order.CreatedAt > discountStartedAt;
}