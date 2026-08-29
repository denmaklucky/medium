using System.Text.Json;
using DiscriminatedUnions;

var options = new JsonSerializerOptions { WriteIndented = false, PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

var paymentResult = new PaymentResult(new Approved(Guid.CreateVersion7().ToString(), 100));

var paymentResultAsJson = JsonSerializer.Serialize(paymentResult, options);

Console.WriteLine(paymentResultAsJson);

var deserializedPaymentResult = JsonSerializer.Deserialize<PaymentResult>(paymentResultAsJson, options);

Console.WriteLine();

public sealed record Approved(string TransactionId, decimal Amount);

public sealed record Declined(string Reason);

public sealed record Pending(TimeSpan RetryAfter);

[DiscriminatedUnion(typeof(Approved), typeof(Declined), typeof(Pending))]
public partial class PaymentResult;

public sealed record NotFound;

public sealed record Created(Guid Id);

[DiscriminatedUnion(typeof(NotFound), typeof(Created))]
public partial class CreatePostResult;
