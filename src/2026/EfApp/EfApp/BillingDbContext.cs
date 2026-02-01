using Microsoft.EntityFrameworkCore;

namespace EfApp;

public class BillingDbContext : DbContext
{
    public DbSet<Payment> Payments { get; set; }

    public DbSet<Refund> Refunds { get; set; }
}