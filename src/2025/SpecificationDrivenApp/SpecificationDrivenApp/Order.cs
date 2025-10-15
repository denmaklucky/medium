namespace SpecificationDrivenApp;

public sealed class Order
{
    public Guid Id { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public decimal Total => Items.Sum(item => item.Amount);

    public OrderItem[] Items { get; set; } = [];
}

public sealed class OrderItem
{
    public Guid Id { get; set; }

    public Guid OrderId { get; set; }

    public string Name { get; set; } = null!;

    public decimal Amount { get; set; }
}