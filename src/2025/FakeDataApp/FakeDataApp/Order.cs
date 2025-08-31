namespace FakeDataApp;

    public sealed class Order
    {
        public Guid Id { get; set; }
        
        public List<OrderItem> Items { get; set; } = [];

        public decimal Total => Items.Sum(item => item.Amount);

        public DateTimeOffset CreatedAt { get; set; }
    }

    public sealed class OrderItem
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public decimal Amount { get; set; }
    }