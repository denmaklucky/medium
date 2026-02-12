using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;
using StripeIntegrationApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<StripeOptions>(builder.Configuration.GetSection("Stripe"));
builder.Services.AddScoped<SessionService>(_ => new SessionService());

var app = builder.Build();

app.MapPost("/payments", async (
    [FromServices] SessionService sessionService,
    [FromServices] IOptions<StripeOptions> options,
    MakePaymentRequest request,
    CancellationToken cancellationToken) =>
{
    const string returnUrl = "http://localhost:5005/payments/confirm";

    var sessionOptions = new SessionCreateOptions
    {
        Mode = "payment",
        UiMode = "hosted",
        SuccessUrl = returnUrl + "?sessionId={CHECKOUT_SESSION_ID}",
        LineItems =
        [
            new()
            {
                Quantity = 1,
                PriceData = new SessionLineItemPriceDataOptions
                {
                    Currency = "EUR",
                    UnitAmount = ToCents(request.Amount),
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = "Test item",
                        Description = "Test item description"
                    }
                }
            }
        ]
    };

    var requestOptions = new RequestOptions
    {
        ApiKey = options.Value.SecretKey,
        IdempotencyKey = Guid.NewGuid().ToString("N")
    };

    var session = await sessionService.CreateAsync(sessionOptions, requestOptions, cancellationToken: cancellationToken);

    return TypedResults.Ok(new { redirectTo = session.Url });

    static int ToCents(decimal amount)
    {
        const int hundred = 100;

        var value = amount * hundred;

        return (int)value;
    }
});

app.MapGet("/payments/confirm", async (
    [FromServices] SessionService sessionService,
    [FromServices] IOptions<StripeOptions> options,
    [FromQuery] string sessionId,
    CancellationToken cancellationToken) =>
{
    var requestOptions = new RequestOptions
    {
        ApiKey = options.Value.SecretKey,
        IdempotencyKey = Guid.NewGuid().ToString("N")
    };

    var session = await sessionService.GetAsync(sessionId, requestOptions: requestOptions, cancellationToken: cancellationToken);

    var isPaymentCompleted = string.Equals(session.PaymentStatus, "paid", StringComparison.OrdinalIgnoreCase);

    return isPaymentCompleted
        ? TypedResults.Ok(new { status  = "completed" })
        : TypedResults.Ok(new { status = "incompleted" });
});

app.Run();