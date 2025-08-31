using Bogus;
using Bogus.DataSets;
using FakeDataApp;

static IReadOnlyCollection<Order> CreateOrders()
{
    var productNames = new[]
    {
        "Wireless Headphones",
        "Bluetooth Speaker",
        "Coffee Mug",
        "Laptop Stand",
        "Phone Charger",
        "Notebook",
        "Desk Lamp",
        "Water Bottle",
        "Keyboard",
        "Mouse Pad",
        "USB Cable",
        "Power Bank",
        "Tablet Case",
        "Screen Cleaner",
        "Pen Set",
        "Backpack",
        "Travel Adapter",
        "Webcam",
        "Monitor",
        "Printer"
    };

    var descriptions = new[]
    {
        "High-quality product designed for everyday use",
        "Perfect for professional environments",
        "Innovative design meets functionality",
        "Built to last with premium materials",
        "Streamlined for maximum efficiency",
        "User-friendly with modern aesthetics",
        "Essential tool for productivity",
        "Crafted with attention to detail",
        "Versatile solution for multiple needs",
        "Engineered for optimal performance",
        "Sleek design with robust construction",
        "Reliable and long-lasting quality",
        "Advanced features in a compact form",
        "Professional grade with consumer appeal",
        "Thoughtfully designed for comfort and utility"
    };

    var orders = new List<Order>();

    for (var index = 0; index < 100; index++)
    {
        var order = new Order
        {
            Id = Guid.CreateVersion7(),
            CreatedAt = DateTimeOffset.UtcNow.AddHours(-Random.Shared.Next(0, 72))
        };

        for (var itemIndex = 0; itemIndex < Random.Shared.Next(1, 100); itemIndex++)
        {
            var orderItem = new OrderItem
            {
                Id = Guid.CreateVersion7(),
                Name = productNames[Random.Shared.Next(productNames.Length - 1)],
                Description = descriptions[Random.Shared.Next(descriptions.Length - 1)],
                Amount =  (decimal)(Random.Shared.NextDouble() * 999.99)
            };

            order.Items.Add(orderItem);
        }

        orders.Add(order);
    }

    return orders;
}

static IReadOnlyCollection<Order> CreateOrdersByDataset()
{
    var orders = new List<Order>();
    var lorem = new Lorem();
    var date = new Date();
    var commerce = new Commerce();
    var amount = new Finance();

    for (var index = 0; index < 100; index++)
    {
        var order = new Order
        {
            Id = Guid.CreateVersion7(),
            CreatedAt = date.FutureOffset()
        };

        for (var itemIndex = 0; itemIndex < Random.Shared.Next(1, 100); itemIndex++)
        {
            var orderItem = new OrderItem
            {
                Id = Guid.CreateVersion7(),
                Name = commerce.ProductName(),
                Description = lorem.Sentence(),
                Amount = amount.Amount()
            };

            order.Items.Add(orderItem);
        }

        orders.Add(order);
    }

    return orders;
}

static IReadOnlyCollection<Order> CreateOrdersByFaker()
{
    var orders = new List<Order>();
    var faker = new Faker();

    for (var index = 0; index < 100; index++)
    {
        var order = new Order
        {
            Id = Guid.CreateVersion7(),
            CreatedAt = faker.Date.FutureOffset()
        };

        for (var itemIndex = 0; itemIndex < Random.Shared.Next(1, 100); itemIndex++)
        {
            var orderItem = new OrderItem
            {
                Id = Guid.CreateVersion7(),
                Name = faker.Commerce.ProductName(),
                Description = faker.Lorem.Sentence(),
                Amount = faker.Finance.Amount()
            };

            order.Items.Add(orderItem);
        }

        orders.Add(order);
    }

    return orders;
}

static IReadOnlyCollection<Order> CreateOrderByTypedFaker()
{
    var itemFaker = new Faker<OrderItem>()
        .RuleFor(item => item.Id, _ => Guid.CreateVersion7())
        .RuleFor(item => item.Name, faker => faker.Commerce.ProductName())
        .RuleFor(item => item.Description, faker => faker.Lorem.Sentence())
        .RuleFor(item => item.Amount, faker => faker.Finance.Amount());

    var orderFaker = new Faker<Order>()
        .RuleFor(order => order.Id, _ => Guid.CreateVersion7())
        .RuleFor(order => order.Items, _ => itemFaker.Generate(Random.Shared.Next(1, 100)))
        .RuleFor(order => order.CreatedAt, faker => faker.Date.FutureOffset());

    return orderFaker.Generate(1000);
}

static IReadOnlyCollection<Order> CreateOrderByFakeFactoryOrder()
{
    var fakeFactoryOrder = new FakeFactoryOrder();

    return fakeFactoryOrder.Generate(1000);
}

sealed class FakeFactoryOrderItem : Faker<OrderItem>
{
    public FakeFactoryOrderItem()
    {
        RuleFor(item => item.Id, _ => Guid.CreateVersion7());
        RuleFor(item => item.Name, faker => faker.Commerce.ProductName());
        RuleFor(item => item.Description, faker => faker.Lorem.Sentence());
        RuleFor(item => item.Amount, faker => faker.Finance.Amount());
    }
}

sealed class FakeFactoryOrder : Faker<Order>
{
    private static readonly FakeFactoryOrderItem _factoryOrderItem = new();

    public FakeFactoryOrder()
    {
        RuleFor(order => order.Id, _ => Guid.CreateVersion7());
        RuleFor(order => order.Items, _ => _factoryOrderItem.Generate(Random.Shared.Next(1, 100)));
        RuleFor(order => order.CreatedAt, faker => faker.Date.FutureOffset());
    }
}