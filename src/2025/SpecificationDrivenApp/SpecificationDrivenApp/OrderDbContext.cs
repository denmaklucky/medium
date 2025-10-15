using Microsoft.EntityFrameworkCore;

namespace SpecificationDrivenApp;

public sealed class OrderDbContext : DbContext
{
    public DbSet<Order> Orders { get; set; }

    public DbSet<OrderItem> OrderItems { get; set; }
}