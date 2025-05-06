using FluentValidation;

namespace ValidationMinimalApi;

public sealed class ValidationFilter<TRequest>(IValidator<TRequest> validator) : IEndpointFilter
    where TRequest : class
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var argument = context.Arguments.SingleOrDefault(argument => argument?.GetType() == typeof(TRequest));

        if (argument is not TRequest request)
        {
            throw new ArgumentException($"Could not find request with the following type `{typeof(TRequest)}`");
        }

        var validationResult = await validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            return Results.BadRequest(validationResult.Errors.Select(error => error.ErrorMessage));
        }

        return await next(context);
    }
}