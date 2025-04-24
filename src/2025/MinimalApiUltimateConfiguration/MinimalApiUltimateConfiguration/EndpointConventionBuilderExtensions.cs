using FluentValidation;

namespace MinimalApiUltimateConfiguration;

public static class EndpointConventionBuilderExtensions
{
    public static IEndpointConventionBuilder WithValidator<TRequest>(
        this IEndpointConventionBuilder routeBuilder,
        Action<InlineValidator<TRequest>> configure)
        where TRequest : class
    {
        var validator = new InlineValidator<TRequest>();
        configure(validator);

        return routeBuilder.AddEndpointFilter(new ValidationFilter<TRequest>(validator));
    }

    private sealed class ValidationFilter<TRequest>(IValidator<TRequest> validator) : IEndpointFilter
        where TRequest : class
    {
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var argument = context.Arguments.SingleOrDefault(argument => argument?.GetType() == typeof(TRequest));

            if (argument is not TRequest request)
            {
                throw new ArgumentException($"Could not find request with the following type `{typeof(TRequest)}`.");
            }

            var validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                return Results.BadRequest(validationResult.Errors.Select(error => error.ErrorMessage));
            }

            return await next(context);
        }
    }
}