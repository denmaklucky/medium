using System.Text.Json;
using DiscriminatedUnions;

var options = new JsonSerializerOptions { WriteIndented = false };

// var result = new PaymentResult(new Approved("", 100));
//
// var json = JsonSerializer.Serialize(result);
//
// Console.WriteLine(json);
//
// var deserializedResult = JsonSerializer.Deserialize<PaymentResult>(json, options);
//
// Console.WriteLine();

var t = new Result(new Approved("", 100));

var js = JsonSerializer.Serialize(t);

var t1 = JsonSerializer.Deserialize<Result>(js, options);

public sealed record Approved(string TransactionId, decimal Amount);

public sealed record Declined(string Reason);

public sealed record Pending(TimeSpan RetryAfter);

[DiscriminatedUnion(typeof(Approved), typeof(Declined), typeof(Pending))]
public partial class PaymentResult;

union Result(Approved, Declined);
